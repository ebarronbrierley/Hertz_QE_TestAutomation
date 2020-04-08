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
    public class GetMemberRewardSummaryByIdTestData
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
                        yield return new TestCaseData(createMember, program).SetName($"GetMemberRewardSummaryById Positive - Program: [{program.Name}] Tier: [{tier.Name}]");

                    }
                }
            }
        }
        public static IEnumerable NegativeScenarios
        {
            get
            {
                long memberRewardId = 0;
                string language = null;
                int errorCode = 3347;
                string errorMessage = $"No reward found with id {memberRewardId}";
                yield return new TestCaseData(memberRewardId, language, errorCode, errorMessage)
                    .SetName($"GetMemberRewardSummaryById Negative - Error Code:[{errorCode}]");

                memberRewardId = 0;
                language = "LanguageMoreThan20Characters";
                errorCode = 2002;
                errorMessage = "Parm Language of GetMemberRewardSummaryByIdIn cannot be more than 20 characters";
                yield return new TestCaseData(memberRewardId, language, errorCode, errorMessage)
                    .SetName($"GetMemberRewardSummaryById Negative - Error Code:[{errorCode}]");
            }
        }
    }
}
