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
    public class AddMemberTests : BrierleyTestFixture
    {
        [TestCaseSource(typeof(AddMemberTestData), "PositiveScenarios")]
        public void AddMember_Positive(MemberModel createMember)
        {
            MemberController memController = new MemberController(Database, TestStep);

            try
            {
                //Generate unique LIDs for each virtual card in the member
                createMember = memController.AssignUniqueLIDs(createMember);

                TestStep.Start($"Make AddMember Call", "Member should be added successfully and member object should be returned");
                MemberModel memberOut = memController.AddMember(createMember);
                AssertModels.AreEqualOnly(createMember, memberOut, MemberModel.BaseVerify);
                TestStep.Pass("Member was added successfully and member object was returned", memberOut.ReportDetail());

                TestStep.Start($"Verify Member in {MemberModel.TableName} table", "Database member should match created member");
                MemberModel dbMember = memController.GetFromDB(memberOut.IPCODE);
                AssertModels.AreEqualOnly(createMember, dbMember, MemberModel.BaseVerify);
                TestStep.Pass("Member created matches member in database", dbMember.ReportDetail());

                TestStep.Start($"Verify MemberDetails in {MemberDetailsModel.TableName} table", "Database member details should match created member");
                AssertModels.AreEqualOnly(createMember.MemberDetails, dbMember.MemberDetails, MemberDetailsModel.BaseVerify);
                TestStep.Pass("Member details in database match created member details", dbMember.MemberDetails.ReportDetail());

                TestStep.Start($"Verify MemberPreferences in {MemberPreferencesModel.TableName} table", "Database member preferences should match created member");
                AssertModels.AreEqualWithAttribute(createMember.MemberPreferences, dbMember.MemberPreferences);
                TestStep.Pass("Member preferences created matches member preferences in database", dbMember.MemberPreferences.ReportDetail());

                TestStep.Start($"Verify VirtualCard in {VirtualCardModel.TableName} table", "Database VirtualCard should API member VirtualCard");
                AssertModels.AreEqualWithAttribute(memberOut.VirtualCards.First(), dbMember.VirtualCards.First());
                TestStep.Pass("API Member VirtualCard matches database member VirtualCard", dbMember.VirtualCards.First().ReportDetail());

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

        [TestCaseSource(typeof(AddMemberTestData), "NegativeScenarios")]
        public void AddMember_Negative(MemberModel member, int errorCode, string errorMessage)
        {
            MemberController memController = new MemberController(Database, TestStep);
            try
            {
                TestStep.Start($"Make AddMember Call", $"Add member call should throw exception with error code = {errorCode}");
                LWServiceException exception = Assert.Throws<LWServiceException>(() => memController.AddMember(member), "Excepted LWServiceException, exception was not thrown.");
                Assert.AreEqual(errorCode, exception.ErrorCode);
                Assert.IsTrue(exception.Message.Contains(errorMessage));
                TestStep.Pass("Add Member call threw expected exception", exception.ReportDetail());
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
