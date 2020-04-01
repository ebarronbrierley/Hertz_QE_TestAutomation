using Brierley.TestAutomation.Core.Utilities;
using Hertz.API.Controllers;
using Hertz.API.DataModels;
using Hertz.API.TestData;
using Hertz.API.Utilities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hertz.API.TestCases
{
    [TestFixture]
    public class GetRewardCatalogTests : BrierleyTestFixture
    {
        [TestCaseSource(typeof(GetRewardCatalogTestData), "PositiveScenarios")]
        public void GetRewardCatalog_Positive(bool? activeOnly, int? batchSize,
            long? currencyToEarnLow, long? currencyToEarnHigh)
        {
            LWController lwController = new LWController(Database, TestStep);
            try
            {
                TestStep.Start($"Make GetRewardCatalog Call", "GetRewardCatalog call should return RewardCatalogSummaryResponseModel object");
                List<RewardCatalogSummaryResponseModel> rewardCatalogSummaryResponse = 
                    lwController.GetRewardCatalog(activeOnly, 
                    "", "", null, null, null, 1,batchSize,
                    currencyToEarnLow, currencyToEarnHigh);
                Assert.IsNotNull(rewardCatalogSummaryResponse, "Expected populated RewardCatalogSummaryResponseModel object, but RewardCatalogSummaryResponseModel object returned was null");
                Assert.AreNotEqual(0, rewardCatalogSummaryResponse.Count);
                TestStep.Pass("RewardCatalogSummaryResponseModel object was returned", rewardCatalogSummaryResponse.Count.ToString());

                TestStep.Start($"Verify Reward Catalog values", "Reward Catalog values returned should be correct");
                if (batchSize != null)
                    Assert.IsTrue(rewardCatalogSummaryResponse.Count <= batchSize);

                if(activeOnly!=null && activeOnly == true)
                {
                    int inactiveRewards = rewardCatalogSummaryResponse
                        .Where(x => x.CatalogEndDate.Date < DateTime.Now.Date)
                        .Count();
                    Assert.AreEqual(0, inactiveRewards);
                }
                if (currencyToEarnLow != null && currencyToEarnLow>0)
                {
                    int rewardsWithLowerValue = rewardCatalogSummaryResponse
                        .Where(x => x.CurrencyToEarn<currencyToEarnLow)
                        .Count();
                    Assert.AreEqual(0, rewardsWithLowerValue);
                }
                if (currencyToEarnHigh != null && currencyToEarnHigh > 0)
                {
                    int rewardsWithHigherValue = rewardCatalogSummaryResponse
                        .Where(x => x.CurrencyToEarn > currencyToEarnHigh)
                        .Count();
                    Assert.AreEqual(0, rewardsWithHigherValue);
                }
               
                TestStep.Pass("RewardCatalog response is as expcted", rewardCatalogSummaryResponse.Count.ToString());

            }
            catch (AssertionException ex)
            {
                TestStep.Fail(ex.Message);
                Assert.Fail();
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
