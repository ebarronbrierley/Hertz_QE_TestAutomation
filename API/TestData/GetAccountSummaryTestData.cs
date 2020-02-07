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
    public class GetAccountSummaryTestData
    {
        public static IEnumerable PositiveScenarios
        {
            get
            {
                yield return new TestCaseData(null, null);

                foreach (IHertzProgram program in HertzLoyalty.Programs)
                {
                    foreach (IHertzTier tier in program.Tiers)
                    {
                        yield return new TestCaseData(program, tier).SetName($"GetAccountSymmary Positive - Program: [{program.Name}] Tier: [{tier.Name}]");
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
                StringBuilder errorMessage = new StringBuilder("MemberIdentity of GetAccountSummaryIn is a required property.  Please provide a valid value");
                yield return new TestCaseData(loyaltyId, errorCode, errorMessage.ToString()).
                    SetName($"GetAccountSymmary Negative - Error Code:[{errorCode}]");

                loyaltyId = "12345677889900";
                errorCode = 3302;
                errorMessage = new StringBuilder($"Unable to find member with identity = {loyaltyId}");
                yield return new TestCaseData(loyaltyId, errorCode, errorMessage.ToString()).
                    SetName($"GetAccountSymmary Negative - Error Code:[{errorCode}]");
            }
        }
    }
}
