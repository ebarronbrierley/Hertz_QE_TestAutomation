using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;
using Brierley.TestAutomation.Core.Reporting;
using Brierley.TestAutomation.Core.Utilities;
using HertzNetFramework.DataModels;

namespace HertzNetFramework.Tests.SOAP
{
    [TestFixture]
    public class Bonuses : BrierleyTestFixture
    {
        [Category("Bonus_Positive")]
        [Category("Bonus")]
        [TestCaseSource("PositiveScenarios")]
        public void Bonus_Positive(string name, MemberStyle memberStyle, Member member, TxnHeader transaction, string[] expectedPointEvents)
        {
            try
            {
                Member createMember = member;
                BPTest.Start<TestStep>($"Make AddMember Call for {memberStyle} Member with {name}", "Member should be added successfully and member object should be returned");
                Member memberOut = Member.AddMember(createMember);
                Assert.IsNotNull(memberOut, "Expected populated member object, but member object returned was null");
                BPTest.Pass<TestStep>("Member was added successfully and member object was returned", memberOut.ReportDetail());

                var memberVCKEY = memberOut.VirtualCards.First().VCKEY;
                var memberLID = memberOut.VirtualCards.First().LOYALTYIDNUMBER;
                transaction.A_LYLTYMEMBERNUM = memberLID;

                BPTest.Start<TestStep>($"Add random transaction to members virtual card with VCKEY = {memberVCKEY}", "Transaction should be added to members virtual card");
                memberOut.AddTransaction(transaction,memberVCKEY);
                Assert.IsTrue(memberOut.VirtualCards.First().TxnHeaders.Count > 0, "Expected 1 TxnHeader to be present in members vitual card");
                BPTest.Pass<TestStep>("Transaction is added to members virtual card", memberOut.VirtualCards.First().TxnHeaders.First().ReportDetail());

                TxnHeader expectedTransaction = memberOut.VirtualCards.First().TxnHeaders.First();

                BPTest.Start<TestStep>("Update Existing Member with added transaction", "Member object should be returned from UpdateMember call");
                Member updatedMember = Member.UpdateMember(memberOut);
                Assert.IsNotNull(updatedMember, "Expected non null Member object to be returned");
                BPTest.Pass<TestStep>("Member object returned from UpdateMember API call", updatedMember.ReportDetail());

                BPTest.Start<TestStep>($"Verify TxnHeader in DB with RANUM = {expectedTransaction.A_RANUM}, VCKEY = {memberVCKEY}", "TxnHeader from database should match expected TxnHeader");
                IEnumerable<TxnHeader> dbTxn = TxnHeader.GetFromDB(Database, memberVCKEY, expectedTransaction.A_RANUM);
                Assert.IsNotNull(dbTxn, "Expected transcation to be returned from database query");
                Assert.AreEqual(1, dbTxn.Count(), "Expected 1 transaction to be returned from database query");
                Assert.AreEqual(expectedTransaction.A_RANUM, dbTxn.First().A_RANUM, "RANUM does not match expected");
                Assert.AreEqual(memberVCKEY, dbTxn.First().A_VCKEY, "VCKEY does not match expected");
                // AssertModels.AreEqualWithAttribute(expectedTransaction, dbTxn.First());
                BPTest.Pass<TestStep>("TxnHeader from database matches expected TxnHeader", dbTxn.ReportDetail());

                BPTest.Start<TestStep>($"Verify point transaction(s) exist for member VCKEY = {memberVCKEY}", "Point transaction(s) should exists for member");
                IEnumerable<PointTransaction> ptTransactions = PointTransaction.GetFromDB(Database, memberVCKEY);
                Assert.NotNull(ptTransactions, "Expected database query to return point transactions");
                Assert.IsTrue(ptTransactions.Count() >= 1, "Expected at least one point transaction to be found");
                BPTest.Pass<TestStep>("Point event(s) exists for member", ptTransactions.ReportDetail());

                BPTest.Start<TestStep>($"Verify point event(s) match expected {String.Join(",", expectedPointEvents)}", "Point transaction should contain expected point event(s)");
                IEnumerable<PointEvent> pointEvents = PointEvent.GetFromDB(Database, ptTransactions.Select(x => x.POINTEVENTID).ToArray());
                List<string> pointEventNames = pointEvents.Select(x => x.NAME).ToList();
                Assert.IsTrue(pointEventNames.Intersect(expectedPointEvents).Count() == expectedPointEvents.Length, $"Expected point events {String.Join(",", expectedPointEvents)} - Actual {String.Join(",",pointEventNames)}");
                BPTest.Pass<TestStep>("Point event(s) exists for member", pointEventNames.ToArray());

            }
            catch (LWServiceException ex)
            {
                BPTest.Fail<TestStep>(ex.Message, new string[] { $"Error Code: {ex.ErrorCode}", "Error Message: {ex.Message}" });
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
        static object[] PositiveScenarios =
        {
            new object[]
            {
                "EUSchneider3x2019BonusActivity CDP = 830063, Residence = BE, Check Out Country = BE , Booking Date 8 days before Checkout",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne).Set(830063M,"MemberDetails.A_CDPNUMBER"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.Comparable(), 
                                       checkOutDate:DateTime.Now.AddDays(-7).Comparable(), 
                                       bookDate:DateTime.Now.AddDays(-15).Comparable(), 
                                       CDP: 830063M, program: HertzProgram.GoldPointsRewards.Set(GPR.TierCode.FiveStar,"SpecificTier"),
                                       RSDNCTRYCD: "BE", HODIndicator: 0),
                new string[] { "GPRGoldRental","EUSchneider3x2019BonusActivity" }
            }
        };
    }

    
}
