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
        public static string[] roleName = new string[] { "READ ONLY", "Administration", "No Wait Agent", "Lead/Mentor", "Agents", "TPL", "Manager" };
        public static IEnumerable PositiveScenarios
        {
        get
            {
                foreach (var agentRole in roleName)
                {
                    CsAgentModel csAgent = CsAgentController.GenerateRandomAgent();
                    csAgent.STATUS = AgentStatus.Active;              
                    csAgent.EMAILADDRESS = csAgent.EMAILADDRESS.ToUpper();
                    bool updateAgent = false;
                    yield return new TestCaseData(csAgent, agentRole, updateAgent).SetName($"Add CS Agent Positive - Role : [{agentRole}]");
                }

                foreach (var agentRole in roleName)
                {
                    CsAgentModel csAgent = CsAgentController.GenerateRandomAgent();
                    csAgent.STATUS = AgentStatus.Active;
                    csAgent.EMAILADDRESS = csAgent.EMAILADDRESS.ToUpper();
                    bool updateAgent = true;
                    yield return new TestCaseData(csAgent, agentRole, updateAgent).SetName($"Update CS Agent Positive - Role : [{agentRole}]");
                }
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
