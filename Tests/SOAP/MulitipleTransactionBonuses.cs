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
    public partial class Bonuses : BrierleyTestFixture
    {
        [Category("Bonus_Positive")]
        [Category("Bonus")]
        [TestCaseSource(typeof(GPRAAABonusActivity), "PositiveScenarios")]
        public void MultiTransaction_Bonus_Positive(string name, MemberStyle memberStyle, Member member, TxnHeader[] transactions, ExpectedPointEvent[] expectedPointEvents, string[] requiredPromotionCodes = null)
        {
            try
            {
                Member createMember = member.MakeVirtualCardLIDsUnique(Database);
                BPTest.Start<TestStep>($"Make AddMember Call for {memberStyle} Member with {name}", "Member should be added successfully and member object should be returned");
                Member memberOut = Member.AddMember(createMember);
                Assert.IsNotNull(memberOut, "Expected populated member object, but member object returned was null");
                BPTest.Pass<TestStep>("Member was added successfully and member object was returned", memberOut.ReportDetail());

                BPTest.Start<TestStep>($"Verify MemberDetails in AddMember output against member details passed", "API MemberDetails should match passed MemberDetails");
                AssertModels.AreEqualWithAttribute(createMember.GetMemberDetails(memberStyle).First(), memberOut.GetMemberDetails(memberStyle).First());
                BPTest.Pass<TestStep>("API MemberDetails match passed MemberDetails", memberOut.GetMemberDetails(memberStyle).ReportDetail());

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
                List<TxnHeader> expectedTransactions = new List<TxnHeader>();
                foreach (TxnHeader transaction in transactions)
                {
                    transaction.A_LYLTYMEMBERNUM = memberLID;

                    BPTest.Start<TestStep>($"Add transaction to members virtual card with VCKEY = {memberVCKEY}", "Transaction should be added to members virtual card");
                    memberOut.AddTransaction(transaction, memberVCKEY);
                    Assert.IsTrue(memberOut.VirtualCards.First().TxnHeaders.Count > 0, "Expected 1 TxnHeader to be present in members vitual card");
                    BPTest.Pass<TestStep>("Transaction is added to members virtual card", memberOut.VirtualCards.First().TxnHeaders.First().ReportDetail());
                    expectedTransactions.Add(transaction);


                    BPTest.Start<TestStep>("Update Existing Member with added transaction(s)", "Member object should be returned from UpdateMember call");
                    Member updatedMember = Member.UpdateMember(memberOut);
                    Assert.IsNotNull(updatedMember, "Expected non null Member object to be returned");
                    BPTest.Pass<TestStep>("Member object returned from UpdateMember API call", updatedMember.ReportDetail());
                    memberOut = updatedMember;
                    Thread.Sleep(1000);
                }
                foreach (TxnHeader expectedTransaction in expectedTransactions)
                {
                    BPTest.Start<TestStep>($"Verify TxnHeader in DB with RANUM = {expectedTransaction.A_RANUM}, VCKEY = {memberVCKEY}", "TxnHeader from database should match expected TxnHeader");
                    IEnumerable<TxnHeader> dbTxn = TxnHeader.GetFromDB(Database, memberVCKEY, expectedTransaction.A_RANUM);
                    Assert.IsNotNull(dbTxn, "Expected transcation to be returned from database query");
                    Assert.AreEqual(1, dbTxn.Count(), "Expected 1 transaction to be returned from database query");
                    Assert.AreEqual(expectedTransaction.A_RANUM, dbTxn.First().A_RANUM, "RANUM does not match expected");
                    Assert.AreEqual(memberVCKEY, dbTxn.First().A_VCKEY, "VCKEY does not match expected");
                    BPTest.Pass<TestStep>("TxnHeader from database matches expected TxnHeader", dbTxn.ReportDetail());
                }

                BPTest.Start<TestStep>($"Verify point transaction(s) exist for member VCKEY = {memberVCKEY}", "Point transaction(s) should exist for member");
                IEnumerable<PointTransaction> ptTransactions = PointTransaction.GetFromDB(Database, memberVCKEY);
                Assert.NotNull(ptTransactions, "Expected database query to return point transactions");
                Assert.IsTrue(ptTransactions.Count() >= expectedTransactions.Count, $"Expected at least {expectedTransactions.Count} point transaction(s) to be found");
                BPTest.Pass<TestStep>("Point event(s) exists for member", ptTransactions.ReportDetail());

                BPTest.Start<TestStep>($"Verify point event(s) match expected {String.Join(",", expectedPointEvents.Select(x => x.PointEventName))}", "Point transaction should contain expected point event(s)");
                IEnumerable<PointEvent> pointEvents = PointEvent.GetFromDB(Database, ptTransactions.Select(x => x.POINTEVENTID).ToArray());
                //List<string> pointEventNames = pointEvents.Select(x => x.NAME).ToList();
                List<string> pointEventNames = ptTransactions.Select(x => pointEvents.ToList().Find(y => y.POINTEVENTID == x.POINTEVENTID).NAME).ToList();
                List<string> expectedPointEventNames = expectedPointEvents.Select(x => x.PointEventName).ToList();
                Assert.IsTrue(!expectedPointEventNames.Except(pointEventNames).Any(), $"Expected point events {String.Join(",", expectedPointEventNames)} - Actual {String.Join(",", pointEventNames)}");
                BPTest.Pass<TestStep>("Point event(s) exists for member", pointEvents.ReportDetail());

                List<PointTransaction> ptTransCompare = ptTransactions.ToList();
                foreach (ExpectedPointEvent expectedEvent in expectedPointEvents)
                {
                    PointEvent ptEvent = pointEvents.ToList().Find(x => x.NAME.Equals(expectedEvent.PointEventName));
                    BPTest.Start<TestStep>($"Verify points for event {expectedEvent.PointEventName}", $"Points for event {expectedEvent.PointEventName} should be {expectedEvent.PointAmount}");
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
    }
}
