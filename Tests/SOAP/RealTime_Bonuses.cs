using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;
using Brierley.TestAutomation.Core.Reporting;
using Brierley.TestAutomation.Core.Utilities;
using HertzNetFramework.DataModels;
using HertzNetFramework.Tests.BonusTestData;
using System.Threading;

namespace HertzNetFramework.Tests.SOAP
{
    [TestFixture]
    public class Bonuses : BrierleyTestFixture
    {



        [Category("Bonus_Positive")]
        [Category("Bonus")]
        [TestCaseSource(typeof(CorpNewMember550PointsOngoingActivity), "PositiveScenarios")]
        [TestCaseSource(typeof(Corp550Points_2016Activity), "PositiveScenarios")]
        [TestCaseSource(typeof(EUSchneider3x2019Bonus), "PositiveScenarios")] 
        [TestCaseSource(typeof(GPRAAABonusActivity), "PositiveScenarios")]
        [TestCaseSource(typeof(ACIActivation800Activity), "PositiveScenarios")]
        [TestCaseSource(typeof(HorizonCardPointsActivity), "PositiveScenarios")]
        [TestCaseSource(typeof(OngoingEMEABirthdayActivity),"PositiveScenarios")]
        [TestCaseSource(typeof(TopGolf_2019_GPR2XBonus), "PositiveScenarios")]
        [TestCaseSource(typeof(VisaInfinite10RGBonusActivity), "PositiveScenarios")]
        [TestCaseSource(typeof(EUCorp800Points_OngoingActivity), "PositiveScenarios")]
        [TestCaseSource(typeof(LapsedOnGoingActivity), "PositiveScenarios")]
        public void RealTime_Bonus_Positive(Member member, TxnHeader[] transactions, ExpectedPointEvent[] expectedPointEvents, string[] requiredPromotionCodes)
        {

            
            try
            {
                Member createMember = member.MakeVirtualCardLIDsUnique(Database);
                BPTest.Start<TestStep>($"Make AddMember Call for {member.Style} Member", "Member should be added successfully and member object should be returned");
                Member memberOut = Member.AddMember(createMember);
                Assert.IsNotNull(memberOut, "Expected populated member object, but member object returned was null");
                BPTest.Pass<TestStep>("Member was added successfully and member object was returned", memberOut.ReportDetail());

                BPTest.Start<TestStep>($"Verify MemberDetails in AddMember output against member details passed", "API MemberDetails should match passed MemberDetails");
                AssertModels.AreEqualWithAttribute(createMember.GetMemberDetails(member.Style).First(), memberOut.GetMemberDetails(member.Style).First());
                BPTest.Pass<TestStep>("API MemberDetails match passed MemberDetails", memberOut.GetMemberDetails(member.Style).ReportDetail());

                var memberVCKEY = memberOut.VirtualCards.First().VCKEY;
                var memberLID = memberOut.VirtualCards.First().LOYALTYIDNUMBER;

                if (requiredPromotionCodes != null)
                {
                    foreach (string promoCode in requiredPromotionCodes)
                    {
                        BPTest.Start<TestStep>($"Make AddMemberPromotion call to add required promotion {promoCode} to member", "AddMemberPromotion call should return MemberPromotion object");
                        MemberPromotion memberPromo = memberOut.AddPromotion(memberLID, promoCode, null, null, false, null, null, false);
                        Assert.NotNull(memberPromo, "Expected AddMemberPromotion to return MemberPromotion object, but returned object was null");
                        BPTest.Pass<TestStep>("AddMemberPromotion call returned MemberPromotion object", memberPromo.ReportDetail());
                    }
                }

                BPTest.Start<TestStep>($"Add {transactions.Length} transaction(s) to members virtual card with VCKEY = {memberVCKEY}", "Transaction(s) should be added to members virtual card");
                foreach (TxnHeader transaction in transactions)
                {
                    transaction.A_LYLTYMEMBERNUM = memberLID;
                    memberOut.AddTransaction(transaction, memberVCKEY);
                }
                Assert.AreEqual(transactions.Length, memberOut.VirtualCards.First().TxnHeaders.Count, $"Expected {transactions.Length} TxnHeader(s) to be present in members vitual card");
                BPTest.Pass<TestStep>("Transaction(s) added to members virtual card", memberOut.VirtualCards.First().TxnHeaders.ReportDetail());


                BPTest.Start<TestStep>("Update Existing Member with added transaction(s)", "Member object should be returned from UpdateMember call");
                Member updatedMember = Member.UpdateMember(memberOut);
                Assert.IsNotNull(updatedMember, "Expected non null Member object to be returned");
                BPTest.Pass<TestStep>("Member object returned from UpdateMember API call", updatedMember.ReportDetail());
                memberOut = updatedMember;

                Thread.Sleep(1000);


                BPTest.Start<TestStep>($"Verify TxnHeader(s) in DB with VCKEY = {memberVCKEY}", "TxnHeader(s) from database should match expected TxnHeader");
                List<TxnHeader> dbTxns = TxnHeader.GetFromDB(Database, memberVCKEY).ToList();
                Assert.IsNotNull(dbTxns, "Expected Database query to return TxnHeaders");
                foreach (TxnHeader expectedTransaction in transactions)
                {
                    TxnHeader foundHeader = dbTxns.Find(x => x.A_RANUM == expectedTransaction.A_RANUM);
                    Assert.IsNotNull(foundHeader, $"Expected to find TxnHeader with RANUM = { expectedTransaction.A_RANUM}");
                }
                BPTest.Pass<TestStep>("TxnHeader(s) from database matches expected TxnHeader(s)", transactions.Select(x => x.A_RANUM).ToArray());


                BPTest.Start<TestStep>($"Verify point transaction(s) exist for member VCKEY = {memberVCKEY}", "Point transaction(s) should exist for member");
                IEnumerable<PointTransaction> ptTransactions = PointTransaction.GetFromDB(Database, memberVCKEY);
                Assert.NotNull(ptTransactions, "Expected database query to return point transactions");
                Assert.IsTrue(ptTransactions.Count() >= transactions.Length, $"Expected at least {transactions.Length} point transaction(s) to be found");
                BPTest.Pass<TestStep>("Point event(s) exists for member", ptTransactions.ReportDetail());


                BPTest.Start<TestStep>($"Verify point event(s) match expected {String.Join(",", expectedPointEvents.Select(x => x.PointEventName))}", "Point transaction should contain expected point event(s)");
                List<PointEvent> pointEvents = PointEvent.GetFromDB(Database, ptTransactions.Select(x => x.POINTEVENTID).ToArray()).ToList();
                List<string> pointEventNames = ptTransactions.Select(x => pointEvents.Find(y => y.POINTEVENTID == x.POINTEVENTID).NAME).ToList();
                List<string> expectedPointEventNames = expectedPointEvents.Select(x => x.PointEventName).ToList();
                Assert.IsTrue(!expectedPointEventNames.Except(pointEventNames).Any(), $"Expected point events {String.Join(",", expectedPointEventNames)} - Actual {String.Join(",", pointEventNames)}");
                BPTest.Pass<TestStep>("Point event(s) exists for member", pointEvents.ReportDetail());


                List<PointTransaction> ptTransCompare = ptTransactions.ToList();
                foreach (ExpectedPointEvent expectedEvent in expectedPointEvents)
                {
                    PointEvent ptEvent = pointEvents.Find(x => x.NAME.Equals(expectedEvent.PointEventName));
                    BPTest.Start<TestStep>($"Verify point event {expectedEvent.PointEventName} points =  {expectedEvent.PointAmount}", $"Points for event {expectedEvent.PointEventName} should be {expectedEvent.PointAmount}");
                    PointTransaction ptTransaction = ptTransCompare.Find(x => x.POINTEVENTID == ptEvent.POINTEVENTID && x.POINTS == expectedEvent.PointAmount);
                    Assert.NotNull(ptTransaction, $"Expected point transaction Name:{expectedEvent.PointEventName} Point Value: {expectedEvent.PointAmount} was not found");
                    Assert.AreEqual(expectedEvent.PointAmount, ptTransaction.POINTS, $"Expected points for {expectedEvent.PointEventName} do not match");
                    BPTest.Pass<TestStep>($"Points for event {expectedEvent.PointEventName} match expected: {expectedEvent.PointAmount}", ptTransaction.ReportDetail());
                    ptTransCompare.Remove(ptTransaction);
                }

            }
            catch (LWServiceException ex)
            {
                BPTest.Fail<TestStep>(ex.Message, new string[] { $"Error Code: {ex.ErrorCode}", $"Error Message: {ex.Message}" });
                Assert.Fail();
            }
            catch (AssertModelEqualityException ex)
            {
                BPTest.Fail<TestStep>(ex.Message, ex.ComparisonFailures);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                BPTest.Fail<TestStep>(ex.Message);
                Assert.Fail();
            }
        }

        [Category("Bonus_Negative")]
        [Category("Bonus")]
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
        public void RealTime_Bonus_Negative(Member member, TxnHeader[] transactions, ExpectedPointEvent[] pointEventsNotPresent, string[] requiredPromotionCodes = null)
        {
            try
            {
                Member createMember = member.MakeVirtualCardLIDsUnique(Database);
                BPTest.Start<TestStep>($"Make AddMember Call for {member.Style} Member", "Member should be added successfully and member object should be returned");
                Member memberOut = Member.AddMember(createMember);
                Assert.IsNotNull(memberOut, "Expected populated member object, but member object returned was null");
                BPTest.Pass<TestStep>("Member was added successfully and member object was returned", memberOut.ReportDetail());

                BPTest.Start<TestStep>($"Verify MemberDetails in AddMember output against member details passed", "API MemberDetails should match passed MemberDetails");
                AssertModels.AreEqualWithAttribute(createMember.GetMemberDetails(member.Style).First(), memberOut.GetMemberDetails(member.Style).First());
                BPTest.Pass<TestStep>("API MemberDetails match passed MemberDetails", memberOut.GetMemberDetails(member.Style).ReportDetail());

                var memberVCKEY = memberOut.VirtualCards.First().VCKEY;
                var memberLID = memberOut.VirtualCards.First().LOYALTYIDNUMBER;

                if (requiredPromotionCodes != null)
                {
                    foreach (string promoCode in requiredPromotionCodes)
                    {
                        BPTest.Start<TestStep>($"Make AddMemberPromotion call to add required promotion {promoCode} to member", "AddMemberPromotion call should return MemberPromotion object");
                        MemberPromotion memberPromo = memberOut.AddPromotion(memberLID, promoCode, null, null, false, null, null, false);
                        Assert.NotNull(memberPromo, "Expected AddMemberPromotion to return MemberPromotion object, but returned object was null");
                        BPTest.Pass<TestStep>("AddMemberPromotion call returned MemberPromotion object", memberPromo.ReportDetail());
                    }
                }

                BPTest.Start<TestStep>($"Add {transactions.Length} transaction(s) to members virtual card with VCKEY = {memberVCKEY}", "Transaction(s) should be added to members virtual card");
                foreach (TxnHeader transaction in transactions)
                {
                    transaction.A_LYLTYMEMBERNUM = memberLID;
                    memberOut.AddTransaction(transaction, memberVCKEY);
                }
                Assert.AreEqual(transactions.Length, memberOut.VirtualCards.First().TxnHeaders.Count, $"Expected {transactions.Length} TxnHeader(s) to be present in members vitual card");
                BPTest.Pass<TestStep>("Transaction(s) added to members virtual card", memberOut.VirtualCards.First().TxnHeaders.ReportDetail());


                BPTest.Start<TestStep>("Update Existing Member with added transaction(s)", "Member object should be returned from UpdateMember call");
                Member updatedMember = Member.UpdateMember(memberOut);
                Assert.IsNotNull(updatedMember, "Expected non null Member object to be returned");
                BPTest.Pass<TestStep>("Member object returned from UpdateMember API call", updatedMember.ReportDetail());
                memberOut = updatedMember;
                Thread.Sleep(1000);


                BPTest.Start<TestStep>($"Verify TxnHeader(s) in DB with VCKEY = {memberVCKEY}", "TxnHeader(s) from database should match expected TxnHeader");
                List<TxnHeader> dbTxns = TxnHeader.GetFromDB(Database, memberVCKEY).ToList();
                Assert.IsNotNull(dbTxns, "Expected Database query to return TxnHeaders");
                foreach (TxnHeader expectedTransaction in transactions)
                {
                    TxnHeader foundHeader = dbTxns.Find(x => x.A_RANUM == expectedTransaction.A_RANUM);
                    Assert.IsNotNull(foundHeader, $"Expected to find TxnHeader with RANUM = { expectedTransaction.A_RANUM}");
                }
                BPTest.Pass<TestStep>("TxnHeader(s) from database matches expected TxnHeader(s)", transactions.Select(x => x.A_RANUM).ToArray());


                BPTest.Start<TestStep>($"Verify point transaction(s) exist for member VCKEY = {memberVCKEY}", "Point transaction(s) should exist for member");
                IEnumerable<PointTransaction> ptTransactions = PointTransaction.GetFromDB(Database, memberVCKEY);
                Assert.NotNull(ptTransactions, "Expected database query to return point transactions");
                Assert.IsTrue(ptTransactions.Count() >= transactions.Length, $"Expected at least {transactions.Length} point transaction(s) to be found");
                BPTest.Pass<TestStep>("Point event(s) exists for member", ptTransactions.ReportDetail());


                BPTest.Start<TestStep>($"Verify point event(s) {String.Join(",", pointEventsNotPresent.Select(x => x.PointEventName))} are not present for member", "Point transaction should not contain point event(s)");
                IEnumerable<PointEvent> pointEvents = new List<PointEvent>();
                if (ptTransactions.Count() > 0) pointEvents = PointEvent.GetFromDB(Database, ptTransactions.Select(x => x.POINTEVENTID).ToArray());
                List<string> pointEventNames = pointEvents.Select(x => x.NAME).ToList();
                Assert.IsTrue(pointEventNames.Intersect(pointEventsNotPresent.Select(x => x.PointEventName)).Count() == 0, $"{String.Join(",", pointEventNames.Intersect(pointEventsNotPresent.Select(x => x.PointEventName)))} should not be present for member");
                BPTest.Pass<TestStep>("Point event(s) do not exist for member", pointEvents.ReportDetail());


            }
            catch (LWServiceException ex)
            {
                BPTest.Fail<TestStep>(ex.Message, new string[] { $"Error Code: {ex.ErrorCode}", $"Error Message: {ex.Message}" });
                Assert.Fail();
            }
            catch (AssertModelEqualityException ex)
            {
                BPTest.Fail<TestStep>(ex.Message, ex.ComparisonFailures);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                BPTest.Fail<TestStep>(ex.Message);
                Assert.Fail();
            }
        }
    }
}
