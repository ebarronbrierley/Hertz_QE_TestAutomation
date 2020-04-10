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
                    yield return new TestCaseData(csAgent, agentRole, updateAgent).SetName($"Add CS Agent Positive - Role : [{agentRole}]  Status : [{csAgent.STATUS}]");
                }

                //foreach (var agentRole in roleName)
                //{
                //    CsAgentModel csAgent = CsAgentController.GenerateRandomAgent();
                //    csAgent.STATUS = AgentStatus.Inactive;
                //    csAgent.EMAILADDRESS = csAgent.EMAILADDRESS.ToUpper();
                //    bool updateAgent = false;
                //    yield return new TestCaseData(csAgent, agentRole, updateAgent).SetName($"Add CS Agent Positive - Role : [{agentRole}]  Status : [{csAgent.STATUS}]");
                //}

                //foreach (var agentRole in roleName)
                //{
                //    CsAgentModel csAgent = CsAgentController.GenerateRandomAgent();
                //    csAgent.STATUS = AgentStatus.Locked;
                //    csAgent.EMAILADDRESS = csAgent.EMAILADDRESS.ToUpper();
                //    bool updateAgent = false;
                //    yield return new TestCaseData(csAgent, agentRole, updateAgent).SetName($"Add CS Agent Positive - Role : [{agentRole}]  Status : [{csAgent.STATUS}]");
                //}


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
                StringBuilder errorMessage = new StringBuilder();

                CsAgentModel csAgent = CsAgentController.GenerateRandomAgent();
                csAgent.USERNAME = null;
                csAgent.STATUS = AgentStatus.Active;
                csAgent.EMAILADDRESS = csAgent.EMAILADDRESS.ToUpper();
                string agentRole = "Administration";
                int errorCode = 200;
                errorMessage.Clear().Append("Agent Username must be populated");
                yield return new TestCaseData(csAgent, agentRole, errorCode, errorMessage.ToString()).SetName($"Hertz Add Update CSAgent Negative - Agent Username must be populated");

                csAgent = CsAgentController.GenerateRandomAgent();
                csAgent.FIRSTNAME = null;
                csAgent.STATUS = AgentStatus.Active;
                csAgent.EMAILADDRESS = csAgent.EMAILADDRESS.ToUpper();
                agentRole = "Administration";
                errorCode = 204;
                errorMessage.Clear().Append("First Name must be populated");
                yield return new TestCaseData(csAgent, agentRole, errorCode, errorMessage.ToString()).SetName($"Hertz Add Update CSAgent Negative - First Name must be populated");

                csAgent = CsAgentController.GenerateRandomAgent();
                csAgent.LASTNAME = null;
                csAgent.STATUS = AgentStatus.Active;
                csAgent.EMAILADDRESS = csAgent.EMAILADDRESS.ToUpper();
                agentRole = "Administration";
                errorCode = 205;
                errorMessage.Clear().Append("Last Name must be populated");
                yield return new TestCaseData(csAgent, agentRole, errorCode, errorMessage.ToString()).SetName($"Hertz Add Update CSAgent Negative - Last Name must be populated");

                csAgent = CsAgentController.GenerateRandomAgent();               
                csAgent.STATUS = AgentStatus.Active;
                csAgent.EMAILADDRESS = csAgent.EMAILADDRESS.ToUpper();
                agentRole = "";
                errorCode = 203;
                errorMessage.Clear().Append("Invalid Agent Role Id");
                yield return new TestCaseData(csAgent, agentRole, errorCode, errorMessage.ToString()).SetName($"Hertz Add Update CSAgent Negative - Invalid Agent Role Id");

                csAgent = CsAgentController.GenerateRandomAgent();
                csAgent.STATUS = AgentStatus.Invalid;          
                csAgent.EMAILADDRESS = csAgent.EMAILADDRESS.ToUpper();
                agentRole = "Administration";
                errorCode = 206;
                errorMessage.Clear().Append("Invalid Status");
                yield return new TestCaseData(csAgent, agentRole, errorCode, errorMessage.ToString()).SetName($"Hertz Add Update CSAgent Negative - Invalid Status");
            }
        }

    }
}

