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
using System.Collections;

namespace Hertz.API.TestCases
{
    public class MultiThreadingTestsData
    {
        public static IEnumerable PositiveScenarios
        {
            get
            {
                MemberModel member1 = MemberController.GenerateRandomMember();
                MemberModel member2 = MemberController.GenerateRandomMember();
                yield return new TestCaseData(member1, member2);
            }
        }
    }
    [TestFixture]
    class MultiThreadingTests:BrierleyTestFixture
    {
        [TestCaseSource(typeof(MultiThreadingTestsData), "PositiveScenarios")]
        public void MultiThreadingTestsPositive(MemberModel member1, MemberModel member2)
        {
            MemberController memController = new MemberController(Database, TestStep);
            TxnHeaderController txnController = new TxnHeaderController(Database, TestStep);
            List<TxnHeaderModel> txnList1 = new List<TxnHeaderModel>();
            List<TxnHeaderModel> txnList2 = new List<TxnHeaderModel>();
            int daysAfterCheckOut = 1;
            DateTime checkOutDt = new DateTime(2020, 01, 31);
            DateTime checkInDt = checkOutDt.AddDays(daysAfterCheckOut);
            DateTime origBkDt = checkOutDt;
            IHertzProgram program = HertzLoyalty.GoldPointsRewards;
            decimal points = -5;
            try
            {
                TestStep.Start("Assign Members unique LoyaltyIds for each virtual card", "Unique LoyaltyIds should be assigned");
                member1 = memController.AssignUniqueLIDs(member1);
                member2 = memController.AssignUniqueLIDs(member2);
                TestStep.Pass("Unique LoyaltyIds assigned", member1.VirtualCards.ReportDetail());

                TestStep.Start($"Make AddMember Call", "Member should be added successfully and member object should be returned");
                MemberModel memberOut1 = memController.AddMember(member1);
                MemberModel memberOut2 = memController.AddMember(member2);
                AssertModels.AreEqualOnly(member1, memberOut1, MemberModel.BaseVerify);
                AssertModels.AreEqualOnly(member2, memberOut2, MemberModel.BaseVerify);
                TestStep.Pass("Member was added successfully and member object was returned", memberOut1.ReportDetail());

                string loyaltyID1 = member1.VirtualCards[0].LOYALTYIDNUMBER;
                string loyaltyID2 = member2.VirtualCards[0].LOYALTYIDNUMBER;

                for (int x = 0; x < 1; x++)
                {
                    TestStep.Start($"Make UpdateMember Call", $"Member should be updated successfully and earn {points} points");
                    TxnHeaderModel txn1 = TxnHeaderController.GenerateTransaction(loyaltyID1, checkInDt, checkOutDt, origBkDt, null, program, null, "US", points, null, null, "N", "US", null);
                    txnList1.Add(txn1);
                    member1.VirtualCards[0].Transactions = txnList1;
                    TxnHeaderModel txn2 = TxnHeaderController.GenerateTransaction(loyaltyID2, checkInDt, checkOutDt, origBkDt, null, program, null, "US", points, null, null, "N", "US", null);
                    txnList2.Add(txn2);
                    member2.VirtualCards[0].Transactions = txnList2;
                    MemberModel updatedMember = memController.UpdateMember(member1);
                    MemberModel updatedMember2 = memController.UpdateMember(member2);
                    txnList1.Clear();
                    txnList2.Clear();
                    TestStep.Pass("Member was successfully updated");
                }

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
    }
}
