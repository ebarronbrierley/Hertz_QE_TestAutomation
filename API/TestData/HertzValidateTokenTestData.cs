using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brierley.TestAutomation.Core.Utilities;
using Hertz.API.Controllers;
using Hertz.API.DataModels;
using NUnit.Framework;

namespace Hertz.API.TestData
{
    public class HertzValidateTokenTestData
    {
       public static IEnumerable NegativeScenarios
        {
            get
            {
                StringBuilder errorMessage = new StringBuilder();
                foreach (IHertzProgram program in HertzLoyalty.Programs)
                {
                    IHertzTier tier = program.Tiers.First();
                    MemberModel member = MemberController.GenerateRandomMember(tier);
                    int errorCode = 811;
                    string token = "invalid";
                    errorMessage.Clear().Append("Hertz Member Token Validation failed");
                    yield return new TestCaseData(member, token, errorCode, errorMessage.ToString(), null).SetName($"Hertz Validate Token Negative - Program:[{program.Name}] Hertz Member Token Validation failed");

                    tier = program.Tiers.First();
                    member = MemberController.GenerateRandomMember(tier);
                    errorCode = 2003;
                    token = "";
                    errorMessage.Clear().Append("Token of HertzValidateTokenIn is a required property");
                    yield return new TestCaseData(member, token, errorCode, errorMessage.ToString(), null).SetName($"Hertz Validate Token Negative - Program:[{program.Name}] Token of HertzValidateTokenIn is a required property");

                    tier = program.Tiers.First();
                    member = MemberController.GenerateRandomMember(tier);
                    errorCode = 2003;
                    token = "";
                    errorMessage.Clear().Append("CardID of HertzValidateTokenIn is a required property.  Please provide a valid value.");
                    yield return new TestCaseData(member, token, errorCode, errorMessage.ToString(), "").SetName($"Hertz Validate Token Negative - Program:[{program.Name}] CardID of HertzValidateTokenIn is a required property");

                    tier = program.Tiers.First();
                    member = MemberController.GenerateRandomMember(tier);
                    errorCode = 810;
                    token = "invalid";
                    errorMessage.Clear().Append("Invalid CardID");
                    yield return new TestCaseData(member, token, errorCode, errorMessage.ToString(),"878787878787").SetName($"Hertz Validate Token Negative - Program:[{program.Name}] Invalid CardID");
                }

            }
        }

    }
}
 