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
using Hertz.API.Controllers;
using Hertz.API.Utilities;
using Hertz.API.TestData;

namespace Hertz.API.TestCases
{
    [TestFixture]
    public class GetLoyaltyEventNamesTestCase : BrierleyTestFixture
    {
        [TestCaseSource(typeof(GetLoyaltyEventNamesTestData), "NegativeScenarios")]
        public void GetLoyaltyEventNamesTestCaseTest(int errorCode, string errorMessage)
        {
            LWController lwController = new LWController(Database, TestStep);
            try
            {
                TestStep.Start("Make GetLoyaltyEventNames Call", $"GetLoyaltyEventNames call should throw exception with error code = {errorCode}");
                LWServiceException exception = Assert.Throws<LWServiceException>(() => lwController.GetLoyaltyEventNames(string.Empty),
                                                                                 "Expected LWServiceException, exception was not thrown.");
                Assert.AreEqual(errorCode, exception.ErrorCode, $"Expected Error Code: {errorCode}");
                Assert.IsTrue(exception.Message.Contains(errorMessage), $"Expected Error Message to contain: {errorMessage}");
                TestStep.Pass("GetLoyaltyEventNames call threw expected exception", exception.ReportDetail());
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
