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
                StringBuilder errorMessage = new StringBuilder();

                foreach (IHertzProgram program in HertzLoyalty.Programs)
                {
                    IHertzTier tier = program.Tiers.First();
                    MemberModel member = MemberController.GenerateRandomMember(tier);
                    int errorCode = 101;
                    errorMessage.Clear().Append("Invalid Loyalty Member Id");
                    yield return new TestCaseData(member, errorCode, errorMessage.ToString(), 100m, "csadmin", "invalidLoyaltyID", null, null).SetName($"Hertz Award Loyalty Currency  Negative - Program:[{program.Name}] Invalid Loyalty Member Id");

                    member = MemberController.GenerateRandomMember(tier);
                    errorCode = 601;
                    errorMessage.Clear().Append("Invalid Point Event Id");
                    yield return new TestCaseData(member, errorCode, errorMessage.ToString(), 100m, "csadmin", null, null, 1234567m).SetName($"Hertz Award Loyalty Currency  Negative - Program:[{program.Name}] Invalid Point Event Id");

                    member = MemberController.GenerateRandomMember(tier);
                    errorCode = 602;
                    errorMessage.Clear().Append("Invalid Rental Agreement Number");
                    yield return new TestCaseData(member, errorCode, errorMessage.ToString(), 100m, "csadmin", null, "inavlidranum", null).SetName($"Hertz Award Loyalty Currency  Negative - Program:[{program.Name}]  Invalid Rental Agreement Number");

                    member = MemberController.GenerateRandomMember(tier);
                    errorCode = 606;
                    errorMessage.Clear().Append("Points to be awarded are equal to zero");
                    yield return new TestCaseData(member, errorCode, errorMessage.ToString(), 0m, "csadmin", null, null, null).SetName($"Hertz Award Loyalty Currency  Negative - Program:[{program.Name}] Points to be awarded are equal to zero");

                    member = MemberController.GenerateRandomMember(tier);
                    errorCode = 607;
                    errorMessage.Clear().Append("Balance cannot go below zero, current balance is less than points to deduct");
                    yield return new TestCaseData(member, errorCode, errorMessage.ToString(), -100m, "csadmin", null, null, null).SetName($"Hertz Award Loyalty Currency  Negative - Program:[{program.Name}] Balance cannot go below zero, current balance is less than points to deduct");

                    member = MemberController.GenerateRandomMember(tier);
                    errorCode = 608;
                    errorMessage.Clear().Append("Point value must be a whole number (no decimals)");
                    yield return new TestCaseData(member, errorCode, errorMessage.ToString(), 50.5m, "csadmin", null, null, null).SetName($"Hertz Award Loyalty Currency  Negative - Program:[{program.Name}]  Point value must be a whole number (no decimals)");

                    member = MemberController.GenerateRandomMember(tier);
                    errorCode = 202;
                    errorMessage.Clear().Append("CS Agent does not have the permissions to issue this number of points");
                    yield return new TestCaseData(member, errorCode, errorMessage.ToString(), 3001m, "autotest", null, null, null).SetName($"Hertz Award Loyalty Currency  Negative - Program:[{program.Name}] CS Agent does not have the permissions to issue this number of points");
                }
            }
    }
}
