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
                    IHertzTier tier = program.Tiers.First();
                    // if (program.EarningPreference == "DX" ) continue;
                    foreach (var awardPoints in points)
                    {
                       
                        MemberModel member = MemberController.GenerateRandomMember(tier);
                        member.MemberDetails.A_COUNTRY = "US";
                        string pointeventname = "AwardNotApplied-CSAdjustment";
                        bool useRanum = true;
                        yield return new TestCaseData(member, tier, pointeventname, awardPoints, useRanum)
                    .SetName($"Hertz Award Loyalty Currency Positive - Program:[{program.Name}] Tier:[{tier.Name}] Points:[{awardPoints}] With Ranum {useRanum}");
                    }
                    foreach (var awardPoints in points)
                    {
                        MemberModel member = MemberController.GenerateRandomMember(program.Tiers.First());
                        member.MemberDetails.A_COUNTRY = "US";
                        string pointeventname = "AwardNotApplied-CSAdjustment";
                        bool useRanum = false;
                        yield return new TestCaseData(member, tier, pointeventname, awardPoints, useRanum)
                    .SetName($"Hertz Award Loyalty Currency Positive - Program:[{program.Tiers.First().ParentProgram.Name}] Tier:[{program.Tiers.First().Name}] Points:[{awardPoints}] With Ranum {useRanum}");
                    }

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
                    yield return new TestCaseData(member,errorCode, errorMessage.ToString(), 0m, "csadmin", null, null, null).SetName($"Hertz Award Loyalty Currency  Negative - Program:[{program.Name}] Points to be awarded are equal to zero");

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
}
 