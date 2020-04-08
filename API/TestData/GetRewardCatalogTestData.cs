using NUnit.Framework;
using System.Collections;

namespace Hertz.API.TestData
{
    public class GetRewardCatalogTestData
    {
        public static IEnumerable PositiveScenarios
        {
            get
            {
                yield return new TestCaseData(null, 100, null, null)
                   .SetName($"Get Reward Catalog Positive - All rewards");
                yield return new TestCaseData(true,100, null, null)
                   .SetName($"Get Reward Catalog Positive - Only active rewards");
                yield return new TestCaseData(true, 2,  null, null)
                   .SetName($"Get Reward Catalog Positive - Batch Size");
                yield return new TestCaseData(true, 10,  new long?(200), null)
                   .SetName($"Get Reward Catalog Positive - CurrencyToEarnLow greater than 200");
                yield return new TestCaseData(true, 100, null, new long?(5000))
                   .SetName($"Get Reward Catalog Positive - CurrencyToEarnHigh lower than 5000");
                yield return new TestCaseData(null, 10, new long?(500), new long?(5000))
                   .SetName($"Get Reward Catalog Positive - CurrencyToEarn between 500 and 5000");
            }

        }

    }
}
