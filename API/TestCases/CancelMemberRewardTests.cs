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
using Hertz.API.TestData;
using Hertz.API.Utilities;
using Hertz.API.Controllers;
using Hertz.API.DataModels;

namespace Hertz.API.TestCases
{
    [TestFixture]
    public class CancelMemberRewardTests : BrierleyTestFixture
    {
        [TestCaseSource(typeof(CancelMemberRewardsTestData), "PositiveScenarios")]
        public void CancelMemberReward_Positive(MemberModel createMember, IHertzProgram program)
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

                TestStep.Start("Validate that Member has Reward", "Member is found with Reward");
                IEnumerable<MemberRewardsModel> membersRewardOut = memberController.GetMemberRewardsFromDB(memberOut.IPCODE, rewardResponse.MemberRewardID);
                Assert.IsNotNull(membersRewardOut, "Expected populated MemberRewards object from query, but MemberRewards object returned was null");
                Assert.Greater(membersRewardOut.Count(), 0, "Expected at least one MemberReward to be returned from query");
                TestStep.Pass("Member Reward Found");

                var memberRewardId = rewardResponse.MemberRewardID.ToString();
                TestStep.Start("Call CancelMemberreward API", "Member reward is cancelled");
                var currencyBalance = memberController.CancelMemberReward(memberRewardId, null, null, null, null, null, null, null);
                Assert.IsNotNull(currencyBalance, "Expected Member Point Balance");
                TestStep.Pass("Successfully cancelled member reward");

                TestStep.Start("Get Total points from PointTransaction table in DB", "Expected Total points for the member");
                pointsOutDb = memberController.GetPointSumFromDB(loyaltyID);
                Assert.IsNotNull(pointsOutDb, "Expected non null point value");
                Assert.AreEqual(currencyBalance, pointsOutDb, "Expected API Point Balance after cancellation to be equal to sum of points from DB");
                TestStep.Pass("Point successfully credited to member");
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

        [TestCaseSource(typeof(CancelMemberRewardsTestData), "NegativeScenarios")]
        public void CancelMemberReward_Negative(string memberRewardId, int errorCode, string errorMessage)
        {
            try
            {
                MemberController memberController = new MemberController(Database, TestStep);
                RewardController rewardController = new RewardController(Database, TestStep);
                if (memberRewardId == null)
                {
                    TestStep.Start("Make CancelMemberReward Call", $"CancelMemberReward call should throw exception with error code = {errorCode}");
                    LWServiceException exception = Assert.Throws<LWServiceException>(() => memberController.CancelMemberReward(memberRewardId, null, null, null, null, null, null, null),
                                                                 "Expected LWServiceException, exception was not thrown.");
                    Assert.AreEqual(errorCode, exception.ErrorCode, $"Expected Error Code: {errorCode}");
                    Assert.IsTrue(exception.Message.Contains(errorMessage), $"Expected Error Message to contain: {errorMessage}");
                    TestStep.Pass("CancelMemberReward call threw expected exception", exception.ReportDetail());
                }
                else
                {
                    memberRewardId = rewardController.GetMemberRewardIdFormDB(MemberRewardsModel.OrderStatus.Cancelled);
                    errorMessage = $"Reward with id {memberRewardId} has already been cancelled";
                    TestStep.Start("Make CancelMemberReward Call", $"CancelMemberReward call should throw exception with error code = {errorCode}");
                    LWServiceException exception = Assert.Throws<LWServiceException>(() => memberController.CancelMemberReward(memberRewardId, null, null, null, null, null, null, null),
                                                                 "Expected LWServiceException, exception was not thrown.");
                    Assert.AreEqual(errorCode, exception.ErrorCode, $"Expected Error Code: {errorCode}");
                    Assert.IsTrue(exception.Message.Contains(errorMessage), $"Expected Error Message to contain: {errorMessage}");
                    TestStep.Pass("CancelMemberReward call threw expected exception", exception.ReportDetail());
                }
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
