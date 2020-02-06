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
    public class GetAccountSummaryTests : BrierleyTestFixture
    {
        [TestCaseSource(typeof(GetAccountSummaryTestData), "PositiveScenarios")]
        public void GetAccountSummaryTestsTest_Positive(IHertzProgram program, IHertzTier tier)
        {
            MemberController memController = new MemberController(Database, TestStep);
            try
            {
                TestStep.Start("Get Existing Member from the database", "Existing member should be found");
                MemberModel dbMember = memController.GetRandomFromDB(MemberModel.Status.Active, tier);
                Assert.IsNotNull(dbMember, "Member could not be retrieved from DB");
                TestStep.Pass("Existing member was found", dbMember.ReportDetail());

                var vckey = dbMember.VirtualCards.First().VCKEY.ToString();
                TestStep.Start("Get Account Summary from DB", "Account Summary retrieved from DB");
                MemberAccountSummaryModel memberAccountSummaryOutDb = memController.GetMemberAccountSummaryFromDB(vckey);
                Assert.IsNotNull(memberAccountSummaryOutDb, "Account Summary could not be retrieved from DB");
                TestStep.Pass("Existing member was found", dbMember.ReportDetail());

                var loyaltyId = dbMember.VirtualCards.First().LOYALTYIDNUMBER;
                var programCode = program != null ? program.EarningPreference : null;
                TestStep.Start("GetAccountSummary API call", "Account Summary retruned from API");
                MemberAccountSummaryModel memberAccountSummaryOut = memController.GetAccountSummary(loyaltyId, programCode, null);
                Assert.IsNotNull(memberAccountSummaryOut, "Account Summary not returned from API");
                TestStep.Pass("Account Summary was returned from API", dbMember.ReportDetail());

                TestStep.Start("Compare Account Summary between DB and API", "Account Summary matches");
                AssertModels.AreEqualWithAttribute(memberAccountSummaryOutDb, memberAccountSummaryOut);
                TestStep.Pass("Account Summary matches between DB and API", dbMember.ReportDetail());

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

        [TestCaseSource(typeof(GetAccountSummaryTestData), "NegativeScenarios")]
        public void GetAccountSummaryTestsTest_Negative(string loyaltyId, int errorCode, string errorMessage)
        {
            MemberController memController = new MemberController(Database, TestStep);
            try
            {
                if (loyaltyId == null)
                {
                    TestStep.Start("Make GetAccountSummary Call", $"GetAccountSummary call should throw exception with error code = {errorCode}");
                    LWServiceException exception = Assert.Throws<LWServiceException>(() => memController.GetAccountSummary(loyaltyId, null, null),
                                                                 "Expected LWServiceException, exception was not thrown.");
                    Assert.AreEqual(errorCode, exception.ErrorCode, $"Expected Error Code: {errorCode}");
                    Assert.IsTrue(exception.Message.Contains(errorMessage), $"Expected Error Message to contain: {errorMessage}");
                    TestStep.Pass("GetAccountSummary call threw expected exception", exception.ReportDetail());
                }
                else 
                {
                    TestStep.Start("Make GetAccountSummary Call", $"GetAccountSummary call should throw exception with error code = {errorCode}");
                    LWServiceException exception = Assert.Throws<LWServiceException>(() => memController.GetAccountSummary(loyaltyId, null, null),
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
                TestStep.Abort(ex.Message);
                Assert.Fail();
            }
        }
    }
}
