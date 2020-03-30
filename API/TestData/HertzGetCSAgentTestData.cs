using NUnit.Framework;
using System.Collections;
using System.Text;

namespace Hertz.API.TestData
{
    public class HertzGetCSAgentTestData
    {
        public static IEnumerable PositiveScenarios
        {
            get
            {
                yield return new TestCaseData("csadmin")
                    .SetName($"HertzGetCSAgent Positive - Agent:[csadmin]");
            }

        }

        public static IEnumerable NegativeScenarios
        {
            get
            {
                StringBuilder errorMessage = new StringBuilder();
                int errorCode = 200;
                errorMessage.Clear().Append("Agent Username must be populated");
                yield return new TestCaseData(null, errorCode, errorMessage.ToString())
                    .SetName($"HertzGetCSAgent  Negative - Agent Username must be populated");

                errorCode = 201;
                errorMessage.Clear().Append("Invalid Agent Username");
                yield return new TestCaseData("testautm1", errorCode, errorMessage.ToString())
                    .SetName($"HertzGetCSAgent  Negative - Agent Username:[testautm1] Invalid Agent Username");


            }
        }
    }
}
