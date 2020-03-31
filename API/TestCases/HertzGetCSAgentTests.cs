using Brierley.TestAutomation.Core.Utilities;
using Hertz.API.Controllers;
using Hertz.API.DataModels;
using Hertz.API.TestData;
using Hertz.API.Utilities;
using NUnit.Framework;
using System;

namespace Hertz.API.TestCases
{
    [TestFixture]
    public class HertzGetCSAgentTests: BrierleyTestFixture
    {
        [TestCaseSource(typeof(HertzGetCSAgentTestData), "PositiveScenarios")]
        public void HertzGetCSAgent_Positive(string csAgent)
        {
            LWController lwController = new LWController(Database, TestStep);
            try
            {
                TestStep.Start($"Make HertzGetCSAgent Call", "HertzGetCSAgent call should return HertzGetCSAgentResponseModel object");
                HertzGetCSAgentResponseModel hertzGetCSAgentResponse = lwController.HertzGetCSAgent(csAgent);
                Assert.IsNotNull(hertzGetCSAgentResponse, "Expected populated HertzGetCSAgentResponseModel object, but HertzUpdateTierResponseModel object returned was null");
                TestStep.Pass("HertzGetCSAgentResponseModel object was returned", hertzGetCSAgentResponse.AgentUserName);

                TestStep.Start($"Verify CSAgent values", "CSAgent values returned should be correct");
                Assert.AreEqual(csAgent.ToUpper(), hertzGetCSAgentResponse.AgentUserName.ToUpper());
                TestStep.Pass("New Tier values response is as expcted", hertzGetCSAgentResponse.AgentUserName);

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


        [TestCaseSource(typeof(HertzGetCSAgentTestData), "NegativeScenarios")]
        public void HTZUpdateTier_Negative(string csAgent, int errorCode, string errorMessage)
        {
            LWController lwController = new LWController(Database, TestStep);
            try
            {
               TestStep.Start($"Make HertzGetCSAgent Call", $"HertzGetCSAgent call should throw exception with error code = {errorCode}");
                LWServiceException exception =
                    Assert.Throws<LWServiceException>(() =>
                        lwController.HertzGetCSAgent(csAgent),
                        "Expected LWServiceException, exception was not thrown.");
                Assert.AreEqual(errorCode, exception.ErrorCode);
                Assert.IsTrue(exception.Message.Contains(errorMessage));
                TestStep.Pass("HertzGetCSAgent call threw expected exception", exception.ReportDetail());

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
