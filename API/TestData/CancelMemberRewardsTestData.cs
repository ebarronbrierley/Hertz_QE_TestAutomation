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

namespace Hertz.API.TestData
{
    public class CancelMemberRewardsTestData
    {
        public static IEnumerable PositiveScenarios
        {
            get
            {
                yield return new TestCaseData();
            }
        }
        public static IEnumerable NegativeScenarios
        {
            get
            {
                decimal? memberRewardId = null;
                int errorCode = 3350;
                string errorMessage = "Invalid member reward id";
                yield return new TestCaseData(memberRewardId, errorCode, errorMessage)
                    .SetName($"Cancel Member Rewards Negative - Error Code:[{errorCode}]");

                memberRewardId = 0;
                errorCode = 6013;
                errorMessage = "Reward with id has already been cancelled";
                yield return new TestCaseData(memberRewardId, errorCode, errorMessage)
                    .SetName($"Cancel Member Rewards Negative - Error Code:[{errorCode}]");
            }
        }
    }
}
