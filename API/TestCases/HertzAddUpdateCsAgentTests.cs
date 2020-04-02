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
using Hertz.API.DataModels;
using Hertz.API.Controllers;
using System.Threading.Tasks;
using Hertz.API.Utilities;

namespace Hertz.API.TestCases
{
   [TestFixture]
    public class HertzAddUpdateCsAgentTests : BrierleyTestFixture
    {
        [TestCaseSource(typeof(HertzAddUpdateCsAgentTestData), "PositiveScenarios")]
        public void HertzAddUpdateCsAgent_Positive(CsAgentModel csAgent,string agentRoleName, bool UpdateCsAgent)
        {
            CsAgentController CsAgentController = new CsAgentController(Database, TestStep);
            CsAgentModel csAgentOut;
            try
            {
                CsAgentRoleModel csAgentRole = CsAgentController.GetFromDBRoleID(agentRoleName);               
                decimal caAgentRoleID = csAgentRole.ID;
                csAgent.ROLEID = caAgentRoleID;
                TestStep.Start($"Make HertzAddUpdateCSAgent Call", "CSAgent should be added successfully");
                csAgentOut = CsAgentController.AddCsAgent(csAgent);
                AssertModels.AreEqualWithAttribute(csAgent, csAgentOut);                    
                TestStep.Pass("CsAgent was added successfully and CsAgent object was returned", csAgentOut.ReportDetail());

                TestStep.Start($"Verify CsAgent in {CsAgentModel.TableName} table", "Database CsAgent should match created CsAgent");
                CsAgentModel dbCsAgent = CsAgentController.GetFromDB(csAgentOut.USERNAME);               
                AssertModels.AreEqualWithAttribute(csAgentOut, dbCsAgent);               
                TestStep.Pass("CsAgent created matches CsAgent in database", dbCsAgent.ReportDetail());
                if(UpdateCsAgent)
                {
                    csAgentOut.LASTNAME = csAgentOut.LASTNAME + "Updated";
                    csAgentOut.EMAILADDRESS = "UPDATEDEMAIL@AUTOMATION.COM";
                    TestStep.Start($"Make HertzAddUpdateCSAgent Call for updating Last Name and Email", "CSAgent should be updated successfully");
                    CsAgentModel csAgentOutUpdated = CsAgentController.AddCsAgent(csAgentOut);
                    AssertModels.AreEqualWithAttribute(csAgentOutUpdated, csAgentOut);
                    TestStep.Pass("CsAgent was added successfully and CsAgent object was returned", csAgentOutUpdated.ReportDetail());

                    TestStep.Start($"Verify CsAgent is updated in {CsAgentModel.TableName} table", "Database CsAgent should match updated CsAgent");
                    CsAgentModel dbCsAgentUpdated = CsAgentController.GetFromDB(csAgentOutUpdated.USERNAME);
                    AssertModels.AreEqualWithAttribute(csAgentOutUpdated, dbCsAgentUpdated);
                    TestStep.Pass("CsAgent created matches CsAgent in database", dbCsAgentUpdated.ReportDetail());

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
   

    [TestCaseSource(typeof(HertzAddUpdateCsAgentTestData), "NegativeScenarios")]
    public void HertzAddUpdateCsAgent_Negative(CsAgentModel csAgent, string agentRoleName,  int errorCode, string errorMessage)
    {
        CsAgentController CsAgentController = new CsAgentController(Database, TestStep);
        try
        {

            CsAgentRoleModel csAgentRole = CsAgentController.GetFromDBRoleID(agentRoleName);
            decimal caAgentRoleID = csAgentRole.ID;
            csAgent.ROLEID = caAgentRoleID;       
            TestStep.Start($"Make HertzAwardLoyaltyCurrency Call", $"Add member call should throw exception with error code = {errorCode}");
            LWServiceException exception = Assert.Throws<LWServiceException>(() =>  CsAgentController.AddCsAgent(csAgent));
            Assert.AreEqual(errorCode, exception.ErrorCode);
            Assert.IsTrue(exception.Message.Contains(errorMessage));
            TestStep.Pass("HertzAwardLoyaltyCurrency call threw expected exception", exception.ReportDetail());
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
