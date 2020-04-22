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
using Hertz.API.DataModels;
using Hertz.API.Controllers;

namespace Hertz.API.TestData
{
    public class HertzGetAccountActivitySummaryTestData
    {
        public static IEnumerable PositiveScenarios
        {
            get
            {
                foreach (IHertzProgram program in HertzLoyalty.Programs)
                {
                    foreach (IHertzTier tier in program.Tiers)
                    {
                        int[] numberOfTransactions = {1, 5};
                        for(int i = 0; i < numberOfTransactions.Length; i++)
                        {
                            MemberModel gprMember = MemberController.GenerateRandomMember(tier);
                            yield return new TestCaseData(gprMember, program, numberOfTransactions[i], true)
                                .SetName($"HertzGetAccountActivitySummary Positive - Program: [{program.Name}] Tier: [{tier.Name}] Number of Txns: [{numberOfTransactions[i]}] Display Points: True");
                            yield return new TestCaseData(gprMember, program, numberOfTransactions[i], false)
                                .SetName($"HertzGetAccountActivitySummary Positive - Program: [{program.Name}] Tier: [{tier.Name}] Number of Txns: [{numberOfTransactions[i]}] Display Points: False");
                        }                  
                    }
                }
            }
        }
        public static IEnumerable NegativeScenarios
        {
            get
            {
                string loyaltyId = null;
                int errorCode = 2003;
                StringBuilder errorMessage = new StringBuilder("MemberIdentity of HertzGetAccountActivitySummaryIn is a required property.  Please provide a valid value");
                yield return new TestCaseData(loyaltyId, errorCode, errorMessage.ToString()).
                    SetName($"HertzGetAccountActivitySummary Negative - Error Code:[{errorCode}]");

                loyaltyId = "12345677889900";
                errorCode = 3302;
                errorMessage = new StringBuilder($"Unable to find member with identity = {loyaltyId}");
                yield return new TestCaseData(loyaltyId, errorCode, errorMessage.ToString()).
                    SetName($"HertzGetAccountActivitySummary Negative - Error Code:[{errorCode}]");
            }
        }
    }
}
