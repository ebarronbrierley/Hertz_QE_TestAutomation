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


namespace Hertz.API.TestCases
{
    [TestFixture]
    public class GetMemberPromotionsCount : BrierleyTestFixture
    {
        [TestCaseSource(typeof(GetMemberPromotionCountTestData), "NegativeScenarios")]
        public void GetMemberPromotionCount_Negative(int errorCode, string errorMessage)
        {
            MemberController memController = new MemberController(Database, TestStep);
            try
            {
                TestStep.Start("Get Existing Member from the database", "Existing member should be found");
                MemberModel dbMember = memController.GetRandomFromDB(MemberModel.Status.Active);
                Assert.IsNotNull(dbMember, "Member could not be retrieved from DB");
                TestStep.Pass("Existing member was found", dbMember.ReportDetail());

                TestStep.Start("Make GetMembers Call", $"GetPromotionCount call should throw exception with error code = {errorCode}");
                LWServiceException exception = Assert.Throws<LWServiceException>(() => memController.GetMemberPromotionCount(dbMember.VirtualCards.First().LOYALTYIDNUMBER),
                                                                                 "Expected LWServiceException, exception was not thrown.");
                Assert.AreEqual(errorCode, exception.ErrorCode, $"Expected Error Code: {errorCode}");
                Assert.IsTrue(exception.Message.Contains(errorMessage), $"Expected Error Message to contain: {errorMessage}");
                TestStep.Pass("GetPromotionCount call threw expected exception", exception.ReportDetail());
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
