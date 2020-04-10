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
    public class HertzValidateTokenTests : BrierleyTestFixture
    {
      
        [TestCaseSource(typeof(HertzValidateTokenTestData), "NegativeScenarios")]
        public void HertzAwardLoyaltyCurrency_Negative(MemberModel member, string token,int errorCode, string errorMessage, string memLoyaltyID = null)
        {
            MemberController memController = new MemberController(Database, TestStep);      

            try
            {
                //Generate unique LIDs for each virtual card in the member
                member = memController.AssignUniqueLIDs(member);                

                TestStep.Start($"Make AddMember Call", "Member should be added successfully");
                MemberModel memberOut = memController.AddMember(member);
                AssertModels.AreEqualOnly(member, memberOut, MemberModel.BaseVerify);
                TestStep.Pass("Member was added successfully and member object was returned", memberOut.ReportDetail());               
       
                var loyaltyId = memLoyaltyID ?? memberOut.VirtualCards.First().LOYALTYIDNUMBER;
                TestStep.Start($"Make HertzValidateToken Call", $"Add member call should throw exception with error code = {errorCode}");               
                LWServiceException exception = Assert.Throws<LWServiceException>(() => memController.HertzValidateToken(loyaltyId,token), "Expected LWServiceException, exception was not thrown.");
                Assert.AreEqual(errorCode, exception.ErrorCode);
                Assert.IsTrue(exception.Message.Contains(errorMessage));
                Assert.AreEqual(errorCode, exception.ErrorCode);
                Assert.IsTrue(exception.Message.Contains(errorMessage));
                TestStep.Pass("HertzValidateToken call threw expected exception", exception.ReportDetail());
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
