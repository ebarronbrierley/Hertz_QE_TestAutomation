using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Brierley.TestAutomation.Core.Reporting;
using Brierley.TestAutomation.Core.Database;
using Brierley.TestAutomation.Core.Utilities;
using Brierley.TestAutomation.Core.SFTP;
using Brierley.TestAutomation.Core.WebUI;
using Hertz.API.Controllers;
using Hertz.API.DataModels;

namespace Hertz.API.TestData
{
    public class HertzAddUpdateCsAgentTestData
    {
        public static IEnumerable PositiveScenarios
        {       
            get
            {
                CsAgentModel csAgent = CsAgentController.GenerateRandomAgent();
                csAgent.STATUS = AgentStatus.Active;
                csAgent.ROLEID = 11;
                csAgent.EMAILADDRESS = csAgent.EMAILADDRESS.ToUpper();
                yield return new TestCaseData(csAgent).SetName($"Add CS Agent Positive ");
            }

        }

        public static IEnumerable NegativeScenarios
        {
            get
            {
                yield return new TestCaseData();
            }
        }
    }
}
