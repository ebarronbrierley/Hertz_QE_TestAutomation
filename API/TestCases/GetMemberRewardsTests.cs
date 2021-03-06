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
using System.Collections;

namespace Hertz.API.TestCases
{
    class GetMemberRewardsTestData
    {
        public static IEnumerable PositiveScenarios
        {
            get
            {
                MemberModel createMember = MemberController.GenerateRandomMember(HertzLoyalty.GoldPointsRewards.RegularGold);
                yield return new TestCaseData(createMember, HertzLoyalty.GoldPointsRewards);
                MemberModel createMember2 = MemberController.GenerateRandomMember(HertzLoyalty.DollarExpressRenters.DefaultTier);
                yield return new TestCaseData(createMember2, HertzLoyalty.DollarExpressRenters);
                MemberModel createMember3 = MemberController.GenerateRandomMember(HertzLoyalty.ThriftyBlueChip.DefaultTier);
                yield return new TestCaseData(createMember3, HertzLoyalty.ThriftyBlueChip);
            }
        }

        public static IEnumerable NegativeScenarios
        {
            get
            {
                int errorCode = 3362;
                string errorMessage = "No member rewards found";
                MemberModel createMember = MemberController.GenerateRandomMember(HertzLoyalty.GoldPointsRewards.RegularGold);
                yield return new TestCaseData(createMember, HertzLoyalty.GoldPointsRewards, errorCode, errorMessage);
                MemberModel createMember2 = MemberController.GenerateRandomMember(HertzLoyalty.DollarExpressRenters.DefaultTier);
                yield return new TestCaseData(createMember2, HertzLoyalty.DollarExpressRenters, errorCode, errorMessage);
                MemberModel createMember3 = MemberController.GenerateRandomMember(HertzLoyalty.ThriftyBlueChip.DefaultTier);
                yield return new TestCaseData(createMember3, HertzLoyalty.ThriftyBlueChip, errorCode, errorMessage);
            }
        }
    }

    class GetMemberRewardsTests:BrierleyTestFixture
    {
        [TestCaseSource(typeof(GetMemberRewardsTestData), "PositiveScenarios")]
        public void GetMemberRewards_Positive(MemberModel testMember, IHertzProgram program)
        {
            MemberController memController = new MemberController(Database, TestStep);
            List<TxnHeaderModel> txnList = new List<TxnHeaderModel>();
            int daysAfterCheckOut = 1;
            DateTime checkOutDt = new DateTime(2020, 03, 12);
            DateTime checkInDt = checkOutDt.AddDays(daysAfterCheckOut);
            DateTime origBkDt = checkOutDt;
            RewardController rewardController = new RewardController(Database, TestStep);
            RewardDefModel reward = rewardController.GetRandomRewardDef(program);
            decimal points = reward.HOWMANYPOINTSTOEARN;
            try
            {
                TestStep.Start("Assing Member unique LoyaltyIds for each virtual card", "Unique LoyaltyIds should be assigned");
                testMember = memController.AssignUniqueLIDs(testMember);
                TestStep.Pass("Unique LoyaltyIds assigned", testMember.VirtualCards.ReportDetail());

                string loyaltyID = testMember.VirtualCards[0].LOYALTYIDNUMBER;
                string alternateID = testMember.ALTERNATEID;
                string vckey = testMember.VirtualCards[0].VCKEY.ToString();

                TestStep.Start($"Make AddMember Call", $"Member with LID {loyaltyID} should be added successfully and member object should be returned");
                MemberModel memberOut = memController.AddMember(testMember);
                AssertModels.AreEqualOnly(testMember, memberOut, MemberModel.BaseVerify);
                TestStep.Pass("Member was added successfully and member object was returned", memberOut.ReportDetail());

                TestStep.Start($"Make AwardLoyaltyCurrency Call", $"Member should be updated successfully and earn {points} points");
                AwardLoyaltyCurrencyResponseModel currencyOut = memController.AwardLoyaltyCurrency(loyaltyID, points);
                decimal pointsOut = memController.GetPointSumFromDB(loyaltyID);
                Assert.AreEqual(points, pointsOut, "Expected points and pointsOut values to be equal, but the points awarded to the member and the point summary taken from the DB are not equal");
                Assert.AreEqual(currencyOut.CurrencyBalance, points, "Expected point value put into AwardLoyaltyCurrency API Call to be equal to the member's current balance, but the point values are not equal");
                TestStep.Pass("Points are successfully awarded");

                TestStep.Start("Make AddMemberReward Call", "AddMemberReward Call is successfully made");
                AddMemberRewardsResponseModel rewardResponse = memController.AddMemberReward(loyaltyID, reward.CERTIFICATETYPECODE, testMember.MemberDetails.A_EARNINGPREFERENCE);
                Assert.IsNotNull(rewardResponse, "Expected populated AddMemberRewardsResponse object from AddMemberRewards call, but AddMemberRewardsResponse object returned was null");
                TestStep.Pass("Reward is added.");

                TestStep.Start("Validate that Member has Reward in DB", "Member is found with Reward");
                IEnumerable<MemberRewardsModel> membersRewardOut = memController.GetMemberRewardsFromDB(memberOut.IPCODE, null);
                Assert.IsNotNull(membersRewardOut, "Expected populated MemberRewards object from query, but MemberRewards object returned was null");
                //Assert.Greater(membersRewardOut.Count(), 0, "Expected at least one MemberReward to be returned from query");
                TestStep.Pass("Member Reward Found");

                TestStep.Start("GetMemberRewards API is Called", "GetMemberRewards API Sucessfully Returns");
                List<GetMemberRewardsResponseModel> getMROut = memController.GetMemberRewards(loyaltyID);
                Assert.AreEqual(membersRewardOut.Count(), getMROut.Count);
                TestStep.Pass("GetMemberRewards Return is Validated");

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

        [TestCaseSource(typeof(GetMemberRewardsTestData), "NegativeScenarios")]
        public void GetMemberRewards_Negative(MemberModel testMember, IHertzProgram program, int errorCode, string errorMessage)
        {
            MemberController memController = new MemberController(Database, TestStep);
            List<TxnHeaderModel> txnList = new List<TxnHeaderModel>();
            int daysAfterCheckOut = 1;
            DateTime checkOutDt = new DateTime(2020, 03, 12);
            DateTime checkInDt = checkOutDt.AddDays(daysAfterCheckOut);
            DateTime origBkDt = checkOutDt;
            RewardController rewardController = new RewardController(Database, TestStep);
            RewardDefModel reward = rewardController.GetRandomRewardDef(program);
            decimal points = reward.HOWMANYPOINTSTOEARN;
            try
            {
                TestStep.Start("Assing Member unique LoyaltyIds for each virtual card", "Unique LoyaltyIds should be assigned");
                testMember = memController.AssignUniqueLIDs(testMember);
                TestStep.Pass("Unique LoyaltyIds assigned", testMember.VirtualCards.ReportDetail());

                string loyaltyID = testMember.VirtualCards[0].LOYALTYIDNUMBER;
                string alternateID = testMember.ALTERNATEID;
                string vckey = testMember.VirtualCards[0].VCKEY.ToString();

                TestStep.Start($"Make AddMember Call", $"Member with LID {loyaltyID} should be added successfully and member object should be returned");
                MemberModel memberOut = memController.AddMember(testMember);
                AssertModels.AreEqualOnly(testMember, memberOut, MemberModel.BaseVerify);
                TestStep.Pass("Member was added successfully and member object was returned", memberOut.ReportDetail());

                TestStep.Start("GetMemberRewards API is Called", "GetMemberRewards API Unsucessfully Returns");
                LWServiceException exception = Assert.Throws<LWServiceException>(() => memController.GetMemberRewards(loyaltyID), "Excepted LWServiceException, exception was not thrown.");
                Assert.AreEqual(errorCode, exception.ErrorCode);
                Assert.IsTrue(exception.Message.Contains(errorMessage));
                TestStep.Pass("GetMemberRewards Return is Validated");

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
