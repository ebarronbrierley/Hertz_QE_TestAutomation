﻿using System;
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

        public static IEnumerable NegativeScenarios
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
                        yield return new TestCaseData(gprMember2, HertzLoyalty.Programs.ToList().Find(x => x.Name != program.Name), errorCode2, errorMessage2).SetName($"Add Member Reward Negative - Redeeming a Reward of the wrong Program - Program: [{program.Name}] Tier: [{tier.Name}]");
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
                        if (program.EarningPreference == HertzLoyalty.GoldPointsRewards.EarningPreference)
                        {
                            decimal pointsToEarn = 471000M;
                            //string[] typeCodes = { "101041", "101043", "101045", "101047", "101033", "101035", "101037", "101039", "101049", "101051" };
                            string[] typeCodes = { "205275", "205229", "205271", "205272", "205267", "205276", "205257", "205237", "205269", 
                                "205274", "205277", "205261", "205234", "205268", "205256", "205235", "205270", "205263", "205231", "205287",
                                "205291", "205252", "205278", "205255", "205232", "205241", "205248", "205246", "205243", "205284", "205230",
                                "205286", "205285", "205233", "205282", "205279", "205283", "205289", "205266", "205280", "205265", "205244",
                                "205281", "205288", "205290", "205247", "205245", "205240", "205242", "205239", "205258", "205260", "205264",
                                "205259", "205254", "205250", "205251", "205253" };
                            yield return new TestCaseData(gprMember, typeCodes, pointsToEarn, program).SetName($"Add Member Reward FlashSale - Program: [{program.Name}] Tier: [{tier.Name}]");
                        }
                    }
                }
            }
        }
    }

    
}
