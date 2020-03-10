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
    public class GetMemberTests : BrierleyTestFixture
    {
        [Test]
        public void GetMembers_Positive()
        {
            MemberController memController = new MemberController(Database, TestStep);
            string[] searchType = { "CardID" };
            string[] searchValue = { String.Empty };

            try
            {
                TestStep.Start("Get Existing Member from the database", "Existing member should be found");
                MemberModel dbMember = memController.GetRandomFromDB(MemberModel.Status.Active);
                Assert.IsNotNull(dbMember, "Member could not be retrieved from DB");
                TestStep.Pass("Existing member was found", dbMember.ReportDetail());

                searchValue = new string[] { dbMember.VirtualCards.First().LOYALTYIDNUMBER };

                TestStep.Start($"Make GetMembers Call by SearchType = {searchType}, SearchValue = {searchValue}", "GetMembers should return one member");
                IEnumerable<MemberModel> getMembersOut = memController.GetMembers(searchType, searchValue , null, null, String.Empty);
                Assert.AreEqual(1, getMembersOut.Count());
                TestStep.Pass("GetMembers returned one member", getMembersOut.ReportDetail());

                MemberModel getMember = getMembersOut.First();

                TestStep.Start($"Verify GetMembers returned member matches DB Member", "API member should match existing member found from DB");
                AssertModels.AreEqualOnly(dbMember, getMember, MemberModel.BaseVerify);
                TestStep.Pass("API member matches existing member found from DB");

                TestStep.Start($"Verify MemberDetails in {MemberDetailsModel.TableName} table", "Database member details should match API member");
                AssertModels.AreEqualWithAttribute(getMember.MemberDetails, dbMember.MemberDetails);
                TestStep.Pass("API Member details matches member details in database", dbMember.MemberDetails.ReportDetail());

                TestStep.Start($"Verify MemberPreferences in {MemberPreferencesModel.TableName} table", "Database member preferences should match API member");
                AssertModels.AreEqualWithAttribute(getMember.MemberPreferences, dbMember.MemberPreferences);
                TestStep.Pass("API Member preferences created matches member preferences in database", dbMember.MemberPreferences.ReportDetail());
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

        [TestCaseSource(typeof(GetMembersTestData), "NegativeScenarios")]
        public void GetMembers_Negative(string[] searchTypes, string[] searchValues, int? startIdx, int? batchSize, bool validSearchValue, int errorCode, string errorMessage)
        {
            MemberController memController = new MemberController(Database, TestStep);
            try
            {
                TestStep.Start("Get Existing Member from the database", "Existing member should be found");
                MemberModel dbMember = memController.GetRandomFromDB(MemberModel.Status.Active);
                Assert.IsNotNull(dbMember, "Member could not be retrieved from DB");
                TestStep.Pass("Existing member was found", dbMember.ReportDetail());

                if (validSearchValue)
                    searchValues = new string[] { dbMember.VirtualCards.First().LOYALTYIDNUMBER };

                TestStep.Start($"Make GetMembers Call", $"GetMembers call should throw exception with error code = {errorCode}");
                LWServiceException exception = Assert.Throws<LWServiceException>(() => memController.GetMembers(searchTypes, searchValues, startIdx, batchSize, String.Empty),
                                                                                 "Expected LWServiceException, exception was not thrown.");
                Assert.AreEqual(errorCode, exception.ErrorCode, $"Expected Error Code: {errorCode}");
                Assert.IsTrue(exception.Message.Contains(errorMessage), $"Expected Error Message to contain: {errorMessage}");
                TestStep.Pass("GetMembers call threw expected exception", exception.ReportDetail());

            }
            catch(AssertionException ex)
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
