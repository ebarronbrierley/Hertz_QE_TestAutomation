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
    public class AddMemberRewardsData
    {
        public static IEnumerable PositiveScenarios
        {
            get
            {
                foreach (IHertzProgram program in HertzLoyalty.Programs)
                {
                    foreach (IHertzTier tier in program.Tiers)
                    {
                        MemberModel gprMember = MemberController.GenerateRandomMember(tier);
                        if(program.EarningPreference == HertzLoyalty.GoldPointsRewards.EarningPreference)
                        {
                            string certificatetypecode = "166703";
                            decimal pointsToEarn = 900M;
                            yield return new TestCaseData(gprMember, certificatetypecode, pointsToEarn, program).SetName($"Add Member Reward Positive - Program: [{program.Name}] Tier: [{tier.Name}] Reward: [{certificatetypecode}]");
                        }else if(program.EarningPreference == HertzLoyalty.DollarExpressRenters.EarningPreference)
                        {
                            string certificatetypecode = "101033";
                            decimal pointsToEarn = 625M;
                            yield return new TestCaseData(gprMember, certificatetypecode, pointsToEarn, program).SetName($"Add Member Reward Positive - Program: [{program.Name}] Tier: [{tier.Name}] Reward: [{certificatetypecode}]");
                        }
                        else if(program.EarningPreference == HertzLoyalty.ThriftyBlueChip.EarningPreference)
                        {
                            string certificatetypecode = "101034";
                            decimal pointsToEarn = 625M;
                            yield return new TestCaseData(gprMember, certificatetypecode, pointsToEarn, program).SetName($"Add Member Reward Positive - Program: [{program.Name}] Tier: [{tier.Name}] Reward: [{certificatetypecode}]");
                        }
                    }
                }
            }
        }
    }
    [TestFixture]
    public class AddMemberRewards : BrierleyTestFixture
    {
        [TestCaseSource(typeof(AddMemberRewardsData), "PositiveScenarios")]
        public void AddMemberRewards_Positive(MemberModel createMember, string certificateTypeCode, decimal points, IHertzProgram program)
        {
            MemberController memController = new MemberController(Database, TestStep);
            TxnHeaderController txnController = new TxnHeaderController(Database, TestStep);
            List<TxnHeaderModel> txnList = new List<TxnHeaderModel>();
            int daysAfterCheckOut = 1;
            DateTime checkOutDt = new DateTime(2020, 01, 31);
            DateTime checkInDt = checkOutDt.AddDays(daysAfterCheckOut);
            DateTime origBkDt = checkOutDt;
            try
            {
                TestStep.Start("Assing Member unique LoyaltyIds for each virtual card", "Unique LoyaltyIds should be assigned");
                createMember = memController.AssignUniqueLIDs(createMember);
                TestStep.Pass("Unique LoyaltyIds assigned", createMember.VirtualCards.ReportDetail());

                string loyaltyID = createMember.VirtualCards[0].LOYALTYIDNUMBER;
                string alternateID = createMember.ALTERNATEID;

                TestStep.Start($"Make AddMember Call", "Member should be added successfully and member object should be returned");
                MemberModel memberOut = memController.AddMember(createMember);
                AssertModels.AreEqualOnly(createMember, memberOut, MemberModel.BaseVerify);
                TestStep.Pass("Member was added successfully and member object was returned", memberOut.ReportDetail());

                TestStep.Start($"Make UpdateMember Call", $"Member should be updated successfully and earn {points} points");
                TxnHeaderModel txn = TxnHeaderController.GenerateTransaction(loyaltyID, checkInDt, checkOutDt, origBkDt, null, program, null, "US", points, null, null, "N", "US", null);
                txnList.Add(txn);
                createMember.VirtualCards[0].Transactions = txnList;
                MemberModel updatedMember = memController.UpdateMember(createMember);
                txnList.Clear();
                Assert.IsNotNull(updatedMember, "Expected non null Member object to be returned");
                TestStep.Pass("Member was successfully updated");

                TestStep.Start("Make AddMemberReward Call", "AddMemberReward Call is successfully made");
                AddMemberRewardsResponseModel rewardResponse =  memController.AddMemberReward(alternateID, certificateTypeCode);
                Assert.IsNotNull(rewardResponse, "Expected populated AddMemberRewardsResponse object from AddMemberRewards call, but AddMemberRewardsResponse object returned was null");
                TestStep.Pass("Reward is added.");

                TestStep.Start("Validate that Member has Reward", "Member is found with Reward");
                IEnumerable<MemberRewardsModel> membersReward = memController.GetMemberRewardsFromDB(memberOut.IPCODE, rewardResponse.MemberRewardID);
                Assert.IsNotNull(membersReward, "Expected populated MemberRewards object from query, but MemberRewards object returned was null");
                Assert.Greater(membersReward.Count(), 0, "Expected at least one MemberReward to be returned from query");
                TestStep.Pass("Member Reward Found");

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
