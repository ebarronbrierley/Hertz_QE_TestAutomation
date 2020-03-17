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
        public void HertzAddUpdateCsAgent_Positive(CsAgentModel csAgent)
        {
            CsAgentController CsAgentController = new CsAgentController(Database, TestStep);       
            try
            {
                TestStep.Start($"Make HertzAddUpdateCSAgent Call", "CsAgent should be added successfully");
                CsAgentModel csAgentOut = CsAgentController.AddCsAgent(csAgent);
                AssertModels.AreEqualWithAttribute(csAgent, csAgentOut);             
                TestStep.Pass("CsAgent was added successfully and CsAgent object was returned", csAgentOut.ReportDetail());

                TestStep.Start($"Verify CsAgent in {CsAgentModel.TableName} table", "Database CsAgent should match created CsAgent");
                CsAgentModel dbCsAgent = CsAgentController.GetFromDB(csAgentOut.USERNAME);               
                AssertModels.AreEqualWithAttribute(csAgentOut, dbCsAgent);               
                TestStep.Pass("CsAgent created matches CsAgent in database", dbCsAgent.ReportDetail());
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
