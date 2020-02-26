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
    public class HertzAwardLoyaltyCurrencyTestData
    {
        public static decimal[] points = new decimal[] { 100, -100 };
        public static IEnumerable PositiveScenarios
        {
            get
            {
                foreach (IHertzProgram program in HertzLoyalty.Programs)
                { 
                    if (program.EarningPreference == "DX" ) continue;
                    foreach (var awardPoints in points)
                    {
                        MemberModel member = MemberController.GenerateRandomMember(program.Tiers.First());
                        member.MemberDetails.A_COUNTRY = "US";
                        string pointeventname = "AwardNotApplied-CSAdjustment";
                        bool useRanum = true;
                        yield return new TestCaseData(member, program, pointeventname, awardPoints, useRanum)

                    .SetName($"Hertz Award Loyalty Currency Positive - Program:[{program.Tiers.First().ParentProgram.Name}] Tier:[{program.Tiers.First().Name}] Points:[{awardPoints}] With Ranum {useRanum}");
                    }
                    foreach (var awardPoints in points)
                    {
                        MemberModel member = MemberController.GenerateRandomMember(program.Tiers.First());
                        member.MemberDetails.A_COUNTRY = "US";
                        string pointeventname = "AwardNotApplied-CSAdjustment";
                        bool useRanum = false;
                        yield return new TestCaseData(member, program, pointeventname, awardPoints, useRanum)

                    .SetName($"Hertz Award Loyalty Currency Positive - Program:[{program.Tiers.First().ParentProgram.Name}] Tier:[{program.Tiers.First().Name}] Points:[{awardPoints}] With Ranum {useRanum}");
                    }

                }
            }

        }

    }
}
 