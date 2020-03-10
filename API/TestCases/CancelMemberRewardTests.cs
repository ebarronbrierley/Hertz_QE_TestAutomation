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
using Hertz.API.TestData;
using Hertz.API.Utilities;
using Hertz.API.Controllers;
using Hertz.API.DataModels;

namespace Hertz.API.TestCases
{
    [TestFixture]
    public class CancelMemberRewardTests : BrierleyTestFixture
    {
        [TestCaseSource(typeof(CancelMemberRewardsTestData), "NegativeScenarios")]
        public void CancelMemberRewardTestCaseTest(string memberRewardId, int errorCode, string errorMessage)
        {
            try
            {
                MemberController memberController = new MemberController(Database, TestStep);
                RewardController rewardController = new RewardController(Database, TestStep);
                if (memberRewardId == null)
                {
                    TestStep.Start("Make GetAccountSummary Call", $"GetAccountSummary call should throw exception with error code = {errorCode}");
                    LWServiceException exception = Assert.Throws<LWServiceException>(() => memberController.CancelMemberReward(memberRewardId, null, null, null, null, null, null, null),
                                                                 "Expected LWServiceException, exception was not thrown.");
                    Assert.AreEqual(errorCode, exception.ErrorCode, $"Expected Error Code: {errorCode}");
                    Assert.IsTrue(exception.Message.Contains(errorMessage), $"Expected Error Message to contain: {errorMessage}");
                    TestStep.Pass("GetAccountSummary call threw expected exception", exception.ReportDetail());
                }
                else
                {
                    memberRewardId = rewardController.GetMemberRewardIdFormDB(MemberRewardsModel.OrderStatus.Cancelled);
                    errorMessage = $"Reward with id {memberRewardId} has already been cancelled";
                    TestStep.Start("Make GetAccountSummary Call", $"GetAccountSummary call should throw exception with error code = {errorCode}");
                    LWServiceException exception = Assert.Throws<LWServiceException>(() => memberController.CancelMemberReward(memberRewardId, null, null, null, null, null, null, null),
                                                                 "Expected LWServiceException, exception was not thrown.");
                    Assert.AreEqual(errorCode, exception.ErrorCode, $"Expected Error Code: {errorCode}");
                    Assert.IsTrue(exception.Message.Contains(errorMessage), $"Expected Error Message to contain: {errorMessage}");
                    TestStep.Pass("GetAccountSummary call threw expected exception", exception.ReportDetail());
                }
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
