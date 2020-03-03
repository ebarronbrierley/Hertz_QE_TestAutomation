using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brierley.TestAutomation.Core.Reporting;
using Brierley.TestAutomation.Core.Database;
using Brierley.TestAutomation.Core.Utilities;
using Hertz.API.Controllers;
using Hertz.API.DataModels;
using Hertz.API.Utilities;
using Hertz.API.TestData;
using NUnit.Framework;
using System.Collections;

namespace Hertz.API.TestData
{
    public class AddMemberRewardsData
    {
        public static IEnumerable PositiveScenarios
        {
            get
            {
                foreach (IHertzProgram program in HertzLoyalty.Programs)
                {
                    foreach (IHertzTier tier in program.Tiers)
                    {
                        MemberModel createMember = MemberController.GenerateRandomMember(tier);
                        yield return new TestCaseData(createMember, program).SetName($"Add Member Reward Positive - Program: [{program.Name}] Tier: [{tier.Name}]");

                    }
                }
            }
        }

        //public static IEnumerable NegativeScenarios
        //{
        //    get
        //    {
        //        foreach (IHertzProgram program in HertzLoyalty.Programs)
        //        {
        //            foreach (IHertzTier tier in program.Tiers)
        //            {
        //                MemberModel gprMember = MemberController.GenerateRandomMember(tier);
        //                MemberModel gprMember2 = MemberController.GenerateRandomMember(tier);
        //                if (program.EarningPreference == HertzLoyalty.GoldPointsRewards.EarningPreference)
        //                {
        //                    string certificatetypecode = "166703";
        //                    string dxcertificatetypecode = "101033";
        //                    decimal pointsToEarn = 15M;
        //                    int errorCode1 = 3354;
        //                    string errorMessage1 = "Total points needed to fulfill this order are";
        //                    int errorCode2 = 1234;
        //                    string errorMessage2 = "Incorrect program code and reward combination.";
        //                    yield return new TestCaseData(gprMember, certificatetypecode, pointsToEarn, program, errorCode1, errorMessage1).SetName($"Add Member Reward Negative - Not Enough Points to Redeem Reward - Program: [{program.Name}] Tier: [{tier.Name}] Reward: [{certificatetypecode}]");
        //                    yield return new TestCaseData(gprMember2, dxcertificatetypecode, pointsToEarn, program, errorCode2, errorMessage2).SetName($"Add Member Reward Negative - GPR Redeeming a DX Reward - Program: [{program.Name}] Tier: [{tier.Name}] Reward: [{certificatetypecode}]");
        //                }
        //                else if (program.EarningPreference == HertzLoyalty.DollarExpressRenters.EarningPreference)
        //                {
        //                    string certificatetypecode = "101033";
        //                    string gprcertificatetypecode = "166703";
        //                    decimal pointsToEarn = 15M;
        //                    int errorCode1 = 3354;
        //                    string errorMessage1 = "Total points needed to fulfill this order are";
        //                    int errorCode2 = 1234;
        //                    string errorMessage2 = "Incorrect program code and reward combination.";
        //                    yield return new TestCaseData(gprMember, certificatetypecode, pointsToEarn, program, errorCode1, errorMessage1).SetName($"Add Member Reward Negative - Not Enough Points to Redeem Reward - Program: [{program.Name}] Tier: [{tier.Name}] Reward: [{certificatetypecode}]");
        //                    yield return new TestCaseData(gprMember2, gprcertificatetypecode, pointsToEarn, program, errorCode2, errorMessage2).SetName($"Add Member Reward Negative - DX Redeeming a GPR Reward - Program: [{program.Name}] Tier: [{tier.Name}] Reward: [{certificatetypecode}]");
        //                }
        //                else if (program.EarningPreference == HertzLoyalty.ThriftyBlueChip.EarningPreference)
        //                {
        //                    string certificatetypecode = "101034";
        //                    string gprcertificatetypecode = "166703";
        //                    decimal pointsToEarn = 15M;
        //                    int errorCode1 = 3354;
        //                    string errorMessage1 = "Total points needed to fulfill this order are";
        //                    int errorCode2 = 1234;
        //                    string errorMessage2 = "Incorrect program code and reward combination.";
        //                    yield return new TestCaseData(gprMember, certificatetypecode, pointsToEarn, program, errorCode1, errorMessage1).SetName($"Add Member Reward Negative - Not Enough Points to Redeem Reward - Program: [{program.Name}] Tier: [{tier.Name}] Reward: [{certificatetypecode}]");
        //                    yield return new TestCaseData(gprMember2, gprcertificatetypecode, pointsToEarn, program, errorCode2, errorMessage2).SetName($"Add Member Reward Negative - Thrifty Redeeming a GPR Reward - Program: [{program.Name}] Tier: [{tier.Name}] Reward: [{certificatetypecode}]");
        //                }
        //            }
        //        }
        //    }
        //}



        public static IEnumerable NegativeScenarios2
        {
            get
            {
                foreach (IHertzProgram program in HertzLoyalty.Programs)
                {
                    foreach (IHertzTier tier in program.Tiers)
                    {
                        MemberModel gprMember = MemberController.GenerateRandomMember(tier);
                        MemberModel gprMember2 = MemberController.GenerateRandomMember(tier);
                        int errorCode1 = 3354;
                        string errorMessage1 = "Total points needed to fulfill this order are";
                        int errorCode2 = 1234;
                        string errorMessage2 = "Incorrect program code and reward combination.";
                        yield return new TestCaseData(gprMember, program, errorCode1, errorMessage1).SetName($"Add Member Reward Negative - Not Enough Points to Redeem Reward - Program: [{program.Name}] Tier: [{tier.Name}]");
                        yield return new TestCaseData(gprMember2, HertzLoyalty.Programs.ToList().Find(x => x.Name != program.Name), errorCode2, errorMessage2).SetName($"Add Member Reward Negative - DX Redeeming a GPR Reward - Program: [{program.Name}] Tier: [{tier.Name}]");
                    }
                }
            }
        }

        public static IEnumerable FlashSaleScenarios
        {
            get
            {
                foreach (IHertzProgram program in HertzLoyalty.Programs)
                {
                    foreach (IHertzTier tier in program.Tiers)
                    {
                        MemberModel gprMember = MemberController.GenerateRandomMember(tier);
                        if (program.EarningPreference == HertzLoyalty.DollarExpressRenters.EarningPreference)
                        {
                            decimal pointsToEarn = 15040M;
                            string[] typeCodes = { "101041", "101043", "101045", "101047", "101033", "101035", "101037", "101039", "101049", "101051" };
                            yield return new TestCaseData(gprMember, typeCodes, pointsToEarn, program).SetName($"Add Member Reward FlashSale - Program: [{program.Name}] Tier: [{tier.Name}]");
                        }
                    }
                }
            }
        }
    }

    
}
