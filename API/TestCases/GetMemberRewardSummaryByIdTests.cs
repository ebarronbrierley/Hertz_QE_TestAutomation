using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Brierley.TestAutomation.Core.API;
using Brierley.TestAutomation.Core.Reporting;
using Brierley.TestAutomation.Core.Database;
using Brierley.TestAutomation.Core.Utilities;
using Brierley.TestAutomation.Core.SFTP;
using Brierley.TestAutomation.Core.WebUI;
using Hertz.API.Controllers;
using Hertz.API.Utilities;
using Hertz.API.TestData;
using Hertz.API.DataModels;

namespace Hertz.API.TestCases
{
    [TestFixture]
    public class GetMemberRewardSummaryByIdTests : BrierleyTestFixture
    {
        [TestCaseSource(typeof(GetMemberRewardSummaryByIdTestData), "PositiveScenarios")]
        public void GetMemberRewardSummaryByIdTest_Positive(MemberModel createMember, IHertzProgram program)
        {
            MemberController memberController = new MemberController(Database, TestStep);
            TxnHeaderController txnController = new TxnHeaderController(Database, TestStep);
            List<TxnHeaderModel> txnList = new List<TxnHeaderModel>();
            int daysAfterCheckOut = 1;
            DateTime checkOutDt = new DateTime(2020, 01, 31);
            DateTime checkInDt = checkOutDt.AddDays(daysAfterCheckOut);
            DateTime origBkDt = checkOutDt;
            RewardController rewardController = new RewardController(Database, TestStep);
            RewardDefModel reward = rewardController.GetRandomRewardDef(program);
            decimal points = reward.HOWMANYPOINTSTOEARN;
            decimal pointsOutDb = 0;
            try
            {
                TestStep.Start("Adding Member unique LoyaltyIds for each virtual card", "Unique LoyaltyIds should be assigned");
                createMember = memberController.AssignUniqueLIDs(createMember);
                TestStep.Pass("Unique LoyaltyIds assigned", createMember.VirtualCards.ReportDetail());

                string loyaltyID = createMember.VirtualCards[0].LOYALTYIDNUMBER;
                string alternateID = createMember.ALTERNATEID;
                string vckey = createMember.VirtualCards[0].VCKEY.ToString();

                TestStep.Start($"Make AddMember Call", $"Member with LID {loyaltyID} should be added successfully and member object should be returned");
                MemberModel memberOut = memberController.AddMember(createMember);
                AssertModels.AreEqualOnly(createMember, memberOut, MemberModel.BaseVerify);
                TestStep.Pass("Member was added successfully and member object was returned", memberOut.ReportDetail());

                if (program.EarningPreference == HertzLoyalty.GoldPointsRewards.EarningPreference)
                {
                    TestStep.Start($"Make UpdateMember Call", $"Member should be updated successfully and earn {points} points");
                    TxnHeaderModel txn = TxnHeaderController.GenerateTransaction(loyaltyID, checkInDt, checkOutDt, origBkDt, null, program, null, "US", points, null, null, "N", "US", null);
                    txnList.Add(txn);
                    createMember.VirtualCards[0].Transactions = txnList;
                    MemberModel updatedMember = memberController.UpdateMember(createMember);
                    txnList.Clear();
                    Assert.IsNotNull(updatedMember, "Expected non null Member object to be returned");
                    TestStep.Pass("Member was successfully updated");
                }
                else //Dollar and Thrifty Members Cannot be updated with the UpdateMember API so we use the HertzAwardLoyatlyCurrency API instead
                {
                    TestStep.Start($"Make AwardLoyaltyCurrency Call", $"Member should be updated successfully and earn {points} points");
                    AwardLoyaltyCurrencyResponseModel currencyOut = memberController.AwardLoyaltyCurrency(loyaltyID, points);
                    pointsOutDb = memberController.GetPointSumFromDB(loyaltyID);
                    Assert.AreEqual(points, pointsOutDb, "Expected points and pointsOut values to be equal, but the points awarded to the member and the point summary taken from the DB are not equal");
                    Assert.AreEqual(currencyOut.CurrencyBalance, points, "Expected point value put into AwardLoyaltyCurrency API Call to be equal to the member's current balance, but the point values are not equal");
                    TestStep.Pass("Points are successfully awarded");
                }

                TestStep.Start("Make AddMemberReward Call", "AddMemberReward Call is successfully made");
                AddMemberRewardsResponseModel rewardResponse = memberController.AddMemberReward(loyaltyID, reward.CERTIFICATETYPECODE, createMember.MemberDetails.A_EARNINGPREFERENCE);
                Assert.IsNotNull(rewardResponse, "Expected populated AddMemberRewardsResponse object from AddMemberRewards call, but AddMemberRewardsResponse object returned was null");
                TestStep.Pass("Reward is added.");

                var memberRewardId = rewardResponse.MemberRewardID;
                TestStep.Start("Call GetMemberRewardSummaryById API", "Member reward is returned");
                var memberRewardOut = memberController.GetMemberRewardSummaryById(memberRewardId, null, program.EarningPreference, null);
                Assert.IsNotNull(memberRewardOut, "Expected Member Reward");
                TestStep.Pass("Successfully retrieved member reward");

                TestStep.Start("Get MemberRewardSummary from DB", "Expected member reward");
                MemberRewardSummaryModel memberRewardSummaryDB = memberController.GetMemberRewardsummaryFromDB(memberOut.IPCODE, memberRewardId).First();
                Assert.IsNotNull(memberRewardSummaryDB, "Expected non null member reward from DB");
                Assert.AreEqual(memberRewardSummaryDB.POINTSCONSUMED, memberRewardOut.POINTSCONSUMED, "Expected Member Reward Summary PointConsumed from API to equal DB");
                Assert.AreEqual(memberRewardSummaryDB.AVAILABLEBALANCE, memberRewardOut.AVAILABLEBALANCE, "Expected Member Reward Summary Availablebalance from API to equal DB");
                Assert.AreEqual(memberRewardSummaryDB.REWARDNAME, memberRewardOut.REWARDNAME, "Expected Member Reward Summary RewardName from API to equal DB");                TestStep.Pass("Successfully retrieved member reward from DB and compared to API");
                TestStep.Pass("Successfully retrieved member reward summary and compared with DB");
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

        [TestCaseSource(typeof(GetMemberRewardSummaryByIdTestData), "NegativeScenarios")]
        public void GetMemberRewardSummaryByIdTest_Negative(long memberRewardId, string language, int errorCode, string errorMessage)
        {
            try
            {
                MemberController memberController = new MemberController(Database, TestStep);

                TestStep.Start("Make GetMemberRewardSummaryById Call", $"GetMemberRewardSummaryById call should throw exception with error code = {errorCode}");
                LWServiceException exception = Assert.Throws<LWServiceException>(() => memberController.GetMemberRewardSummaryById(memberRewardId, language, null, null),
                                                                "Expected LWServiceException, exception was not thrown.");
                Assert.AreEqual(errorCode, exception.ErrorCode, $"Expected Error Code: {errorCode}");
                Assert.IsTrue(exception.Message.Contains(errorMessage), $"Expected Error Message to contain: {errorMessage}");
                TestStep.Pass("GetMemberRewardSummaryById call threw expected exception", exception.ReportDetail());
            }
            catch (AssertionException ex)
            {
                TestStep.Fail(ex.Message);
                Assert.Fail();
            }
            catch (AssertModelEqualityException ex)
            {
                TestStep.Fail(ex.Message, ex.ComparisonFailures);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                TestStep.Abort(ex.Message, ex.StackTrace);
                Assert.Fail();
            }
        }
    }
}
