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
    public class UpdateMemberTestData
    {
        public static IEnumerable PositiveScenarios
        {
            get
            {
                foreach (IHertzProgram program in HertzLoyalty.Programs)
                {
                    foreach (IHertzTier tier in program.Tiers)
                    {
                        yield return new TestCaseData(MemberModel.Status.Active, tier, 1)
                            .SetName($"Update Member with Transaction Positive - Program: [{program.Name}] Tier: [{tier.Name}]");
                    }
                }
            }
        }
    }
}
