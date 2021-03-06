﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brierley.TestAutomation.Core.Reporting;
using Brierley.TestAutomation.Core.Database;
using Brierley.TestAutomation.Core.Utilities;
using Hertz.API.Controllers;
using Hertz.API.DataModels;
using Hertz.API.Utilities;
using Hertz.API.TestData;
using NUnit.Framework;

namespace Hertz.API.TestCases
{
    [TestFixture]
    public class HertzAwardLoyaltyCurrencyTests : BrierleyTestFixture
    {
        [TestCaseSource(typeof(HertzAwardLoyaltyCurrencyTestData), "PositiveScenarios")]
        public void HertzAwardLoyaltyCurrency_Positive(MemberModel member, IHertzTier tier,string pointeventname,decimal points,bool useRanum)
        {           
            MemberController memController = new MemberController(Database, TestStep);
            PointController pointController = new PointController(Database, TestStep);        
            try
            {
                //Generate unique LIDs for each virtual card in the member
                member = memController.AssignUniqueLIDs(member);
                string ranum;

                TestStep.Start($"Make AddMember Call", "Member should be added successfully");
                MemberModel memberOut = memController.AddMember(member);
                AssertModels.AreEqualOnly(member, memberOut, MemberModel.BaseVerify);
                TestStep.Pass("Member was added successfully and member object was returned", memberOut.ReportDetail());

                VirtualCardModel vc = memberOut.VirtualCards.First();
                //Transactions are added to test the API with ranum and also to test the negative points scenario
                TestStep.Start($"Add Transaction to the member", "Transactions added successfully");      
                vc.Transactions = TxnHeaderController.GenerateRandomTransactions(vc, tier.ParentProgram, 1, 200);
                if (useRanum)
                {
                    ranum = vc.Transactions.Select(x => x.A_RANUM).First();
                }
                else
                {
                    ranum = null;
                }
                Assert.IsNotNull(vc.Transactions, "Expected populated transaction object, but transaction object returned was null");
                TestStep.Pass("Transaction added to the member", vc.Transactions.ReportDetail());

                TestStep.Start("Update Existing Member with added transaction", "Member object should be returned from UpdateMember call");
                MemberModel updatedMember = memController.UpdateMember(memberOut);
                Assert.IsNotNull(updatedMember, "Expected non null Member object to be returned");
                TestStep.Pass("Member object returned from UpdateMember API call", updatedMember.ReportDetail());

                TestStep.Start("Find PointEventId in database",$"PointEventId should be found {pointeventname}");
                IEnumerable<PointEventModel> pointEvent = pointController.GetPointEventIdsFromDb(pointeventname);
                decimal pointEventId = pointEvent.Select(x => x.POINTEVENTID).First();                             
                Assert.IsTrue(pointEvent.Any(x=>x.NAME.Equals(pointeventname)), "Expected pointevent name was not found in database");
                TestStep.Pass("Pointevent name was found in the Database", pointEvent.ReportDetail());

                var loyaltyId = memberOut.VirtualCards.First().LOYALTYIDNUMBER;
                TestStep.Start($"Make HertzAwardLoyaltyCurrency Call", "HertzAwardLoyaltyCurrency call should return HertzAwardLoyaltyCurrency object");
                HertzAwardLoyaltyCurrencyResponseModel memberAwardLoyaltyCurrency = memController.HertzAwardLoyaltyCurrency(loyaltyId, "csadmin", points, Convert.ToInt64(pointEventId), "automation", ranum);
                Assert.IsNotNull(memberAwardLoyaltyCurrency, "Expected populated AwardLoyaltyCurrency object, but AwardLoyaltyCurrency object returned was null");
                TestStep.Pass("HertzAwardLoyaltyCurrency object was returned", memberAwardLoyaltyCurrency.ReportDetail());

                var vckey = memberOut.VirtualCards.First().VCKEY.ToString();
                TestStep.Start("Get Account Summary from DB", "Account Summary retrieved from DB");
                MemberAccountSummaryModel memberAccountSummaryOutDb = memController.GetMemberAccountSummaryFromDB(vckey);
                Assert.IsNotNull(memberAccountSummaryOutDb, "Account Summary could not be retrieved from DB");
                TestStep.Pass("Existing member was found", memberAccountSummaryOutDb.ReportDetail());

                TestStep.Start("Verify Points awarded matches the points in DB", "Points awarded matches the points in DB");             
                IEnumerable<PointTransactionModel> dbPointTransaction = pointController.GetPointTransactionsFromDb(decimal.Parse(vckey)).ToList();                
                Assert.IsNotNull(dbPointTransaction, "Expected populated PointTransaction object from database query, but PointTransaction object returned was null");
                Assert.AreEqual(points, dbPointTransaction.Where(x => x.POINTEVENTID == pointEventId && x.POINTS == points).Select(x => x.POINTS).First(), $"Points awarded does not match database points{dbPointTransaction.Select(x => x.POINTS)} ");             
                TestStep.Pass("Points awarded are added to the DB", dbPointTransaction.ReportDetail());

                TestStep.Start($"Verify CurrencyBalance of HertzAwardLoyaltyCurrency", "CurrencyBalance returned should be correct");
                Assert.AreEqual(dbPointTransaction.Sum(x => x.POINTS), memberAwardLoyaltyCurrency.CURRENCYBALANCE, $"CurrencyBalance does not match database points{dbPointTransaction.Select(x => x.POINTS)} ");
                Assert.AreEqual(0,memberAwardLoyaltyCurrency.CURRENCYTONEXTTIER, "CurrencyToNextTier does not match 0");
                TestStep.Pass("CurrencyBalance response is as expcted", memberAwardLoyaltyCurrency.ReportDetail());

                TestStep.Start("Compare Account Summary between DB and API", "Account Summary matches");
                AssertModels.AreEqualWithAttribute(memberAccountSummaryOutDb, memberAwardLoyaltyCurrency);
                TestStep.Pass("Account Summary matches between DB and API", memberAccountSummaryOutDb.ReportDetail());

                TestStep.Start($"Verify response of HertzAwardLoyaltyCurrency for GPR", "Response returned should be correct");
                if (tier.ParentProgram.EarningPreference == "N1")
                {
                    Assert.AreEqual(tier.RentalsToNextTier - vc.Transactions.Count(), memberAwardLoyaltyCurrency.RENTALSTONEXTTIER, $"RentalsToNextTier does not match {tier.RentalsToNextTier - vc.Transactions.Count()}");
                    Assert.AreEqual(tier.RevenueToNextTier - vc.Transactions.First().A_GRSREVNAMT, memberAwardLoyaltyCurrency.REVENUETONEXTTIER, $"RentalsToNextTier does not match {tier.RevenueToNextTier - vc.Transactions.First().A_GRSREVNAMT}");
                    Assert.AreEqual(vc.Transactions.Count(), memberAwardLoyaltyCurrency.TOTALRENTALSYTD, $"TotalRentalsYTD does not match {memberAwardLoyaltyCurrency.TOTALRENTALSYTD}");
                    Assert.AreEqual(vc.Transactions.Sum(x => x.A_GRSREVNAMT), memberAwardLoyaltyCurrency.TOTALREVENUEYTD, $"TotalRentalsYTD does not match {memberAwardLoyaltyCurrency.TOTALREVENUEYTD}");
                    Assert.AreEqual("Gold", memberAwardLoyaltyCurrency.CURRENTTIERNAME, $"CurrentTierName does not match {memberAwardLoyaltyCurrency.CURRENCYBALANCE}");
                }
                TestStep.Pass("HertzAwardLoyaltyCurrency response is as expcted for GPR", memberAwardLoyaltyCurrency.ReportDetail());
            }
            catch(AssertionException ex)
            {
                TestStep.Fail(ex.Message);
                Assert.Fail();
            }
            catch (LWServiceException ex)
            {                                 
                TestStep.Fail(ex.Message, new[] { $"Error Code: {ex.ErrorCode}", $"Error Message: {ex.ErrorMessage}" });
                Assert.Fail();
            }
            catch (AssertModelEqualityException ex)
            {
                TestStep.Fail(ex.Message, ex.ComparisonFailures);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                TestStep.Abort(ex.Message);
                Assert.Fail();
            }
        }

        [TestCaseSource(typeof(HertzAwardLoyaltyCurrencyTestData), "NegativeScenarios")]
        public void HertzAwardLoyaltyCurrency_Negative(MemberModel member, int errorCode, string errorMessage, decimal points,string csagent,string memLoyaltyID = null, string txnRanum = null, decimal? txnPointEventID = null)
        {
            MemberController memController = new MemberController(Database, TestStep);
            PointController pointController = new PointController(Database, TestStep);   
            string pointeventname = "AwardNotApplied-CSAdjustment";
            try
            {
                //Generate unique LIDs for each virtual card in the member
                member = memController.AssignUniqueLIDs(member);                

                TestStep.Start($"Make AddMember Call", "Member should be added successfully");
                MemberModel memberOut = memController.AddMember(member);
                AssertModels.AreEqualOnly(member, memberOut, MemberModel.BaseVerify);
                TestStep.Pass("Member was added successfully and member object was returned", memberOut.ReportDetail());

                TestStep.Start("Find PointEventId in database", $"PointEventId should be found {pointeventname}");
                IEnumerable<PointEventModel> pointEvent = pointController.GetPointEventIdsFromDb(pointeventname);
                decimal pointEventId = txnPointEventID ?? pointEvent.Select(x => x.POINTEVENTID).First();
                Assert.IsTrue(pointEvent.Any(x => x.NAME.Equals(pointeventname)), "Expected pointevent name was not found in database");
                TestStep.Pass("Pointevent name was found in the Database", pointEvent.ReportDetail());
       
                var loyaltyId = memLoyaltyID ?? memberOut.VirtualCards.First().LOYALTYIDNUMBER;
                TestStep.Start($"Make HertzAwardLoyaltyCurrency Call", $"Add member call should throw exception with error code = {errorCode}");
                LWServiceException exception = Assert.Throws<LWServiceException>(() => memController.HertzAwardLoyaltyCurrency(loyaltyId, csagent, points, Convert.ToInt64(pointEventId), "automationNeg", txnRanum), "Expected LWServiceException, exception was not thrown.");
                Assert.AreEqual(errorCode, exception.ErrorCode);
                Assert.IsTrue(exception.Message.Contains(errorMessage));
                TestStep.Pass("HertzAwardLoyaltyCurrency call threw expected exception", exception.ReportDetail());
            }
            catch (AssertModelEqualityException ex)
            {
                TestStep.Fail(ex.Message, ex.ComparisonFailures);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                TestStep.Abort(ex.Message);
                Assert.Fail();
            }
        }
    }
}
