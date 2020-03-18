using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Brierley.TestAutomation.Core.Reporting;
using Brierley.TestAutomation.Core.Database;
using Brierley.TestAutomation.Core.Utilities;
using Brierley.TestAutomation.Core.SFTP;
using Brierley.TestAutomation.Core.WebUI;
using Hertz.API.DataModels;
using Hertz.API.Controllers;

namespace Hertz.API.TestData
{
    public class CancelMemberRewardsTestData
    {
        public static IEnumerable PositiveScenarios
        {
            get
            {
                foreach (IHertzProgram program in HertzLoyalty.Programs)
                {
                    foreach (IHertzTier tier in program.Tiers)
                    {
                        MemberModel createMember = MemberController.GenerateRandomMember(tier);
                        yield return new TestCaseData(createMember, program).SetName($"Cancel Member Reward Positive - Program: [{program.Name}] Tier: [{tier.Name}]");

                    }
                }
            }
        }
        public static IEnumerable NegativeScenarios
        {
            get
            {
                string memberRewardId = null;
                int errorCode = 3356;
                string errorMessage = "Invalid member reward id";
                yield return new TestCaseData(memberRewardId, errorCode, errorMessage)
                    .SetName($"Cancel Member Rewards Negative - Error Code:[{errorCode}]");
            }
        }
        public static IEnumerable NegativeCancelledScenarios
        {
            get
            {
                MemberModel createMember = null;
                string memberRewardId = null;
                int errorCode = 6013;
                string errorMessage = null;

                foreach (IHertzProgram program in HertzLoyalty.Programs)
                {
                    foreach (IHertzTier tier in program.Tiers)
                    {
                        createMember = MemberController.GenerateRandomMember(tier);
                        yield return new TestCaseData(createMember, program, memberRewardId, errorCode, errorMessage)
                            .SetName($"Cancel Member Rewards Negative - Error Code:[{errorCode}] Program: [{program.Name}] Tier: [{tier.Name}]");

                    }
                }
            }
        }
    }
}
