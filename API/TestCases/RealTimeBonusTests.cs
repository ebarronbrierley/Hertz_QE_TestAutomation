using System;
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
using Hertz.API.TestData.RealTimeBonuses;
using System.Threading;

namespace Hertz.API.TestCases
{
    [TestFixture]
    public class Bonuses : BrierleyTestFixture
    {
        [Category("Bonus_Positive")]
        [Category("Bonus")]
        [TestCaseSource(typeof(CorpNewMember550PointsOngoingActivity), "PositiveScenarios")]
        [TestCaseSource(typeof(EUSchneider3x2019Bonus), "PositiveScenarios")]
        [TestCaseSource(typeof(GPRAAABonusActivity), "PositiveScenarios")]
        [TestCaseSource(typeof(ACIActivation800Activity), "PositiveScenarios")]
        [TestCaseSource(typeof(HorizonCardPointsActivity), "PositiveScenarios")]
        [TestCaseSource(typeof(OngoingEMEABirthdayActivity), "PositiveScenarios")]
        [TestCaseSource(typeof(VisaInfinite10RGBonusActivity), "PositiveScenarios")]
        [TestCaseSource(typeof(EUCorp800Points_OngoingActivity), "PositiveScenarios")]
        [TestCaseSource(typeof(LapsedOnGoingActivity), "PositiveScenarios")]
        public void RealTime_Bonus_Positive(MemberModel member, TxnHeaderModel[] transactionsIn, ExpectedPointEvent[] expectedPointEvents, string[] requiredPromotionCodes)
        {
            //Currently there are no async rules for Hertz should be all ran by return of UpdateMember
            int ruleTriggerWaitMS = 1;
            MemberController memController = new MemberController(Database, TestStep);
            TxnHeaderController txnController = new TxnHeaderController(Database, TestStep);
            PointController pointController = new PointController(Database, TestStep);

            try
            {
                TestStep.Start("Assigning Member unique LoyaltyIds for each virtual card", "Unique LoyaltyIds should be assigned");
                member = memController.AssignUniqueLIDs(member);
                TestStep.Pass("Unique LoyaltyIds assigned", member.VirtualCards.ReportDetail());

                TestStep.Start($"Make AddMember Call", "Member should be added successfully and member object should be returned");
                MemberModel memberOut = memController.AddMember(member);
                Assert.IsNotNull(memberOut, "Expected populated member object, but member object returned was null");
                Assert.IsNotNull(memberOut.VirtualCards, "Expected member to have virtual cards");
                TestStep.Pass("Member was added successfully and member object was returned", memberOut.ReportDetail());

                TestStep.Start($"Verify MemberDetails in AddMember output against member details passed", "API MemberDetails should match passed MemberDetails");
                AssertModels.AreEqualWithAttribute(member.MemberDetails, memberOut.MemberDetails);
                TestStep.Pass("API MemberDetails match passed MemberDetails", memberOut.MemberDetails.ReportDetail());


                var memberVC = memberOut.VirtualCards.First();


                if (requiredPromotionCodes != null)
                {
                    foreach (string promoCode in requiredPromotionCodes)
                    {
                        TestStep.Start($"Make AddMemberPromotion call to add required promotion {promoCode} to member", "AddMemberPromotion call should return MemberPromotion object");
                        MemberPromotionModel memberPromo = memController.AddMemberPromotion(memberVC.LOYALTYIDNUMBER, promoCode, null, null, false, null, null, false);
                        Assert.NotNull(memberPromo, "Expected AddMemberPromotion to return MemberPromotion object, but returned object was null");
                        TestStep.Pass("AddMemberPromotion call returned MemberPromotion object", memberPromo.ReportDetail());
                    }
                }

                TestStep.Start($"Add {transactionsIn.Length} transaction(s) to members virtual card with VCKEY = {memberVC.VCKEY}", "Transaction(s) should be added to members virtual card");
                if (memberVC.Transactions == null) memberVC.Transactions = new List<TxnHeaderModel>();
                foreach (var transaction in transactionsIn)
                {
                    transaction.A_LYLTYMEMBERNUM = memberVC.LOYALTYIDNUMBER;
                    memberVC.Transactions.Add(transaction);
                }
                Assert.AreEqual(transactionsIn.Length, memberVC.Transactions.Count, $"Expected {transactionsIn.Length} TxnHeader(s) to be present in members vitual card");
                TestStep.Pass("Transaction(s) added to members virtual card", memberVC.Transactions.ReportDetail());


                TestStep.Start("Update Existing Member with added transaction(s)", "Member object should be returned from UpdateMember call");
                MemberModel updatedMember = memController.UpdateMember(memberOut);
                Assert.IsNotNull(updatedMember, "Expected non null Member object to be returned");
                TestStep.Pass("Member object returned from UpdateMember API call", updatedMember.ReportDetail());
                memberOut = updatedMember;


                Thread.Sleep(ruleTriggerWaitMS);


                TestStep.Start($"Verify TxnHeader(s) in DB with VCKEY = {memberVC.VCKEY}", "TxnHeader(s) from database should match expected TxnHeader");
                List<TxnHeaderModel> dbTxns = txnController.GetFromDB(memberVC.VCKEY).ToList();
                Assert.IsNotNull(dbTxns, "Expected Database query to return TxnHeaders");
                foreach (var expectedTransaction in transactionsIn)
                {
                    var foundHeader = dbTxns.Find(x => x.A_RANUM == expectedTransaction.A_RANUM);
                    Assert.IsNotNull(foundHeader, $"Expected to find TxnHeader with RANUM = { expectedTransaction.A_RANUM}");
                }
                TestStep.Pass("TxnHeader(s) from database matches expected TxnHeader(s)", transactionsIn.Select(x => x.A_RANUM).ToArray());


                TestStep.Start($"Verify point transaction(s) exist for member VCKEY = {memberVC.VCKEY}", "Point transaction(s) should exist for member");
                IEnumerable<PointTransactionModel> ptTransactions = pointController.GetPointTransactionsFromDb(memberVC.VCKEY);
                Assert.NotNull(ptTransactions, "Expected database query to return point transactions");
                Assert.IsTrue(ptTransactions.Count() >= transactionsIn.Length, $"Expected at least {transactionsIn.Length} point transaction(s) to be found");
                TestStep.Pass("Point event(s) exists for member", ptTransactions.ReportDetail());


                TestStep.Start($"Verify point event(s) match expected {String.Join(",", expectedPointEvents.Select(x => x.PointEventName))}", "Point transaction should contain expected point event(s)");
                List<PointEventModel> pointEvents = pointController.GetPointEventsFromDb(ptTransactions.Select(x => x.POINTEVENTID).ToArray()).ToList();
                List<string> pointEventNames = ptTransactions.Select(x => pointEvents.Find(y => y.POINTEVENTID == x.POINTEVENTID).NAME).ToList();
                List<string> expectedPointEventNames = expectedPointEvents.Select(x => x.PointEventName).ToList();
                Assert.IsTrue(!expectedPointEventNames.Except(pointEventNames).Any(), $"Expected point events {String.Join(",", expectedPointEventNames)} - Actual {String.Join(",", pointEventNames)}");
                TestStep.Pass("Point event(s) exists for member", pointEvents.ReportDetail());


                List<PointTransactionModel> ptTransCompare = ptTransactions.ToList();
                foreach (ExpectedPointEvent expectedEvent in expectedPointEvents)
                {
                    PointEventModel ptEvent = pointEvents.Find(x => x.NAME.Equals(expectedEvent.PointEventName));
                    TestStep.Start($"Verify point event {expectedEvent.PointEventName} points =  {expectedEvent.PointAmount}", $"Points for event {expectedEvent.PointEventName} should be {expectedEvent.PointAmount}");
                    PointTransactionModel ptTransaction = ptTransCompare.Find(x => x.POINTEVENTID == ptEvent.POINTEVENTID && x.POINTS == expectedEvent.PointAmount);
                    Assert.NotNull(ptTransaction, $"Expected point transaction Name:{expectedEvent.PointEventName} Point Value: {expectedEvent.PointAmount} was not found");
                    Assert.AreEqual(expectedEvent.PointAmount, ptTransaction.POINTS, $"Expected points for {expectedEvent.PointEventName} do not match");
                    TestStep.Pass($"Points for event {expectedEvent.PointEventName} match expected: {expectedEvent.PointAmount}", ptTransaction.ReportDetail());
                    ptTransCompare.Remove(ptTransaction);
                }

            }
            catch(AssertionException ex)
            {
                TestStep.Fail(ex.Message);
                Assert.Fail();
            }
            catch (LWServiceException ex)
            {
                TestStep.Fail(ex.Message, new string[] { $"Error Code: {ex.ErrorCode}", $"Error Message: {ex.Message}" });
                Assert.Fail();
            }
            catch (AssertModelEqualityException ex)
            {
                TestStep.Fail(ex.Message, ex.ComparisonFailures);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                TestStep.AddAttachment(new Attachment("StackTrace", ex.StackTrace, Attachment.Type.Text));
                TestStep.Abort(ex.Message);
                Assert.Fail();
            }
        }

        //[Category("Bonus_Negative")]
        //[Category("Bonus")]
        [TestCaseSource(typeof(CorpNewMember550PointsOngoingActivity), "NegativeScenarios")]
        [TestCaseSource(typeof(Corp550Points_2016Activity), "NegativeScenarios")]
        [TestCaseSource(typeof(EUSchneider3x2019Bonus), "NegativeScenarios")]
        [TestCaseSource(typeof(GPRAAABonusActivity), "NegativeScenarios")]
        [TestCaseSource(typeof(ACIActivation800Activity), "NegativeScenarios")]
        [TestCaseSource(typeof(HorizonCardPointsActivity), "NegativeScenarios")]
        [TestCaseSource(typeof(OngoingEMEABirthdayActivity), "NegativeScenarios")]
        [TestCaseSource(typeof(TopGolf_2019_GPR2XBonus), "NegativeScenarios")]
        [TestCaseSource(typeof(VisaInfinite10RGBonusActivity), "NegativeScenarios")]
        [TestCaseSource(typeof(EUCorp800Points_OngoingActivity), "NegativeScenarios")]
        [TestCaseSource(typeof(LapsedOnGoingActivity), "NegativeScenarios")]
        public void RealTime_Bonus_Negative(MemberModel member, TxnHeaderModel[] transactionsIn, ExpectedPointEvent[] pointEventsNotPresent, string[] requiredPromotionCodes = null)
        {
            int ruleTriggerWaitMS = 1000;
            MemberController memController = new MemberController(Database, TestStep);
            TxnHeaderController txnController = new TxnHeaderController(Database, TestStep);
            PointController pointController = new PointController(Database, TestStep);

            try
            {
                TestStep.Start("Assing Member unique LoyaltyIds for each virtual card", "Unique LoyaltyIds should be assigned");
                MemberModel createMember = memController.AssignUniqueLIDs(member);
                TestStep.Pass("Unique LoyaltyIds assigned", createMember.VirtualCards.ReportDetail());

                TestStep.Start($"Make AddMember Call", "Member should be added successfully and member object should be returned");
                MemberModel memberOut = memController.AddMember(createMember);
                Assert.IsNotNull(memberOut, "Expected populated member object, but member object returned was null");
                TestStep.Pass("Member was added successfully and member object was returned", memberOut.ReportDetail());

                TestStep.Start($"Verify MemberDetails in AddMember output against member details passed", "API MemberDetails should match passed MemberDetails");
                AssertModels.AreEqualWithAttribute(createMember.MemberDetails, memberOut.MemberDetails);
                TestStep.Pass("API MemberDetails match passed MemberDetails", memberOut.MemberDetails.ReportDetail());

                var memberVC = memberOut.VirtualCards.First();

                if (requiredPromotionCodes != null)
                {
                    foreach (string promoCode in requiredPromotionCodes)
                    {
                        TestStep.Start($"Make AddMemberPromotion call to add required promotion {promoCode} to member", "AddMemberPromotion call should return MemberPromotion object");
                        MemberPromotionModel memberPromo = memController.AddMemberPromotion(memberVC.LOYALTYIDNUMBER, promoCode, null, null, false, null, null, false);
                        Assert.NotNull(memberPromo, "Expected AddMemberPromotion to return MemberPromotion object, but returned object was null");
                        TestStep.Pass("AddMemberPromotion call returned MemberPromotion object", memberPromo.ReportDetail());
                    }
                }

                TestStep.Start($"Add {transactionsIn.Length} transaction(s) to members virtual card with VCKEY = {memberVC.VCKEY}", "Transaction(s) should be added to members virtual card");
                if (memberVC.Transactions == null) memberVC.Transactions = new List<TxnHeaderModel>();
                foreach (var transaction in transactionsIn)
                {
                    transaction.A_LYLTYMEMBERNUM = memberVC.LOYALTYIDNUMBER;
                    memberVC.Transactions.Add(transaction);
                }
                Assert.AreEqual(transactionsIn.Length, memberVC.Transactions.Count, $"Expected {transactionsIn.Length} TxnHeader(s) to be present in members vitual card");
                TestStep.Pass("Transaction(s) added to members virtual card", memberVC.Transactions.ReportDetail());


                TestStep.Start("Update Existing Member with added transaction(s)", "Member object should be returned from UpdateMember call");
                MemberModel updatedMember = memController.UpdateMember(memberOut);
                Assert.IsNotNull(updatedMember, "Expected non null Member object to be returned");
                TestStep.Pass("Member object returned from UpdateMember API call", updatedMember.ReportDetail());
                memberOut = updatedMember;

                Thread.Sleep(ruleTriggerWaitMS);


                TestStep.Start($"Verify TxnHeader(s) in DB with VCKEY = {memberVC.VCKEY}", "TxnHeader(s) from database should match expected TxnHeader");
                List<TxnHeaderModel> dbTxns = txnController.GetFromDB(memberVC.VCKEY).ToList();
                Assert.IsNotNull(dbTxns, "Expected Database query to return TxnHeaders");
                foreach (var expectedTransaction in transactionsIn)
                {
                    var foundHeader = dbTxns.Find(x => x.A_RANUM == expectedTransaction.A_RANUM);
                    Assert.IsNotNull(foundHeader, $"Expected to find TxnHeader with RANUM = { expectedTransaction.A_RANUM}");
                }
                TestStep.Pass("TxnHeader(s) from database matches expected TxnHeader(s)", transactionsIn.Select(x => x.A_RANUM).ToArray());


                TestStep.Start($"Verify point transaction(s) exist for member VCKEY = {memberVC.VCKEY}", "Point transaction(s) should exist for member");
                IEnumerable<PointTransactionModel> ptTransactions = pointController.GetPointTransactionsFromDb(memberVC.VCKEY);
                Assert.NotNull(ptTransactions, "Expected database query to return point transactions");
                Assert.IsTrue(ptTransactions.Count() >= transactionsIn.Length, $"Expected at least {transactionsIn.Length} point transaction(s) to be found");
                TestStep.Pass("Point event(s) exists for member", ptTransactions.ReportDetail());


                TestStep.Start($"Verify point event(s) {String.Join(",", pointEventsNotPresent.Select(x => x.PointEventName))} are not present for member", "Point transaction should not contain point event(s)");
                IEnumerable<PointEventModel> pointEvents = new List<PointEventModel>();
                if (ptTransactions.Count() > 0) pointEvents = pointController.GetPointEventsFromDb(ptTransactions.Select(x => x.POINTEVENTID).ToArray());
                List<string> pointEventNames = pointEvents.Select(x => x.NAME).ToList();
                Assert.IsTrue(pointEventNames.Intersect(pointEventsNotPresent.Select(x => x.PointEventName)).Count() == 0, $"{String.Join(",", pointEventNames.Intersect(pointEventsNotPresent.Select(x => x.PointEventName)))} should not be present for member");
                TestStep.Pass("Point event(s) do not exist for member", pointEvents.ReportDetail());
            }
            catch(AssertionException ex)
            {
                TestStep.Fail(ex.Message);
                Assert.Fail();
            }
            catch (LWServiceException ex)
            {
                TestStep.Fail(ex.Message, new string[] { $"Error Code: {ex.ErrorCode}", $"Error Message: {ex.Message}" });
                Assert.Fail();
            }
            catch (AssertModelEqualityException ex)
            {
                TestStep.Fail(ex.Message, ex.ComparisonFailures);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                TestStep.AddAttachment(new Attachment("StackTrace", ex.StackTrace, Attachment.Type.Text));
                TestStep.Abort(ex.Message, ex.StackTrace);
                Assert.Fail();
            }
        }
    }
}
