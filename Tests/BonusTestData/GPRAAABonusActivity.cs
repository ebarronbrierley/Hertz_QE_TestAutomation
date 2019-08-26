using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brierley.TestAutomation.Core.Utilities;
using HertzNetFramework.DataModels;
using NUnit.Framework;

namespace HertzNetFramework.Tests.BonusTestData
{
    public class GPRAAABonusActivity
    {
        private const decimal BaseAmount = 25;

        public static IEnumerable PositiveScenarios 
        {
            get
            {
                yield return new TestCaseData(
                    "GPRAAABonusActivity [GPR Regular Gold] 2 Transactions  CDP = 1805452, MemberDetails.A_COUNTRY = US",
                    MemberStyle.ProjectOne,
                    Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code, "SpecificTier"))
                                                                                                .Set(1805452M, "MemberDetails.A_CDPNUMBER")
                                                                                                .Set("US", "MemberDetails.A_COUNTRY"),
                    GenerateTxns(2, 1805452M, HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code, "SpecificTier")),
                    GenerateExpectedPoints(2, GPR.Tier.RegularGold),
                    new string[] { }
                ).SetName("GPRAAABonusActivity [GPR Regular Gold] 2 Transactions  CDP = 1805452, MemberDetails.A_COUNTRY = US");

                yield return new TestCaseData(
                    "GPRAAABonusActivity [GPR Regular Gold] 3 Transactions CDP = 1805452, MemberDetails.A_COUNTRY = US",
                    MemberStyle.ProjectOne,
                    Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code, "SpecificTier"))
                                                                                                .Set(1805452M, "MemberDetails.A_CDPNUMBER")
                                                                                                .Set("US", "MemberDetails.A_COUNTRY"),
                    GenerateTxns(3, 1805452M, HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code, "SpecificTier")),
                    GenerateExpectedPoints(3, GPR.Tier.RegularGold),
                    new string[] { }
                ).SetName("GPRAAABonusActivity [GPR Regular Gold] 3 Transactions CDP = 1805452, MemberDetails.A_COUNTRY = US");

                yield return new TestCaseData(
                    "GPRAAABonusActivity [GPR Regular Gold] 4 Transactions CDP = 1805452, MemberDetails.A_COUNTRY = US",
                    MemberStyle.ProjectOne,
                    Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code, "SpecificTier"))
                                                                                                .Set(1805452M, "MemberDetails.A_CDPNUMBER")
                                                                                                .Set("US", "MemberDetails.A_COUNTRY"),
                    GenerateTxns(4, 1805452M, HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code, "SpecificTier")),
                    GenerateExpectedPoints(4, GPR.Tier.RegularGold),
                    new string[] { }
                ).SetName("GPRAAABonusActivity [GPR Regular Gold] 4 Transactions CDP = 1805452, MemberDetails.A_COUNTRY = US");

                yield return new TestCaseData(
                    "GPRAAABonusActivity [GPR Regular Gold] 5 Transactions CDP = 1805452, MemberDetails.A_COUNTRY = US",
                    MemberStyle.ProjectOne,
                    Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code, "SpecificTier"))
                                                                                                .Set(1805452M, "MemberDetails.A_CDPNUMBER")
                                                                                                .Set("US", "MemberDetails.A_COUNTRY"),
                    GenerateTxns(5, 1805452M, HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code, "SpecificTier")),
                    GenerateExpectedPoints(5, GPR.Tier.RegularGold),
                    new string[] { }
                ).SetName("GPRAAABonusActivity [GPR Regular Gold] 5 Transactions CDP = 1805452, MemberDetails.A_COUNTRY = US");

                yield return new TestCaseData(
                    "GPRAAABonusActivity [GPR Regular Gold] 6 Transactions CDP = 1805452, MemberDetails.A_COUNTRY = US",
                    MemberStyle.ProjectOne,
                    Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code, "SpecificTier"))
                                                                                                .Set(1805452M, "MemberDetails.A_CDPNUMBER")
                                                                                                .Set("US", "MemberDetails.A_COUNTRY"),
                    GenerateTxns(6, 1805452M, HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code, "SpecificTier")),
                    GenerateExpectedPoints(6, GPR.Tier.RegularGold),
                    new string[] { }
                ).SetName("GPRAAABonusActivity [GPR Regular Gold] 6 Transactions CDP = 1805452, MemberDetails.A_COUNTRY = US");

                yield return new TestCaseData(
                    "GPRAAABonusActivity [GPR Five Star] 4 Transactions CDP = 1805452, MemberDetails.A_COUNTRY = US",
                    MemberStyle.ProjectOne,
                    Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.FiveStar.Code, "SpecificTier"))
                                                                                                .Set(1805452M, "MemberDetails.A_CDPNUMBER")
                                                                                                .Set("US", "MemberDetails.A_COUNTRY"),
                    GenerateTxns(4, 1805452M, HertzProgram.GoldPointsRewards.Set(GPR.Tier.FiveStar.Code, "SpecificTier")),
                    GenerateExpectedPoints(4, GPR.Tier.FiveStar),
                    new string[] { }
                ).SetName("GPRAAABonusActivity [GPR Five Star] 4 Transactions CDP = 1805452, MemberDetails.A_COUNTRY = US");

                yield return new TestCaseData(
                    "GPRAAABonusActivity [GPR Five Star] 5 Transactions CDP = 1805452, MemberDetails.A_COUNTRY = US",
                    MemberStyle.ProjectOne,
                    Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.FiveStar.Code, "SpecificTier"))
                                                                                                .Set(1805452M, "MemberDetails.A_CDPNUMBER")
                                                                                                .Set("US", "MemberDetails.A_COUNTRY"),

                    GenerateTxns(5, 1805452M, HertzProgram.GoldPointsRewards.Set(GPR.Tier.FiveStar.Code, "SpecificTier")),
                    GenerateExpectedPoints(5, GPR.Tier.FiveStar),
                    new string[] { }
                ).SetName("GPRAAABonusActivity [GPR Five Star] 5 Transactions CDP = 1805452, MemberDetails.A_COUNTRY = US");

                yield return new TestCaseData(
                    "GPRAAABonusActivity [GPR Five Star] 6 Transactions CDP = 1805452, MemberDetails.A_COUNTRY = US",
                    MemberStyle.ProjectOne,
                    Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.FiveStar.Code, "SpecificTier"))
                                                                                                .Set(1805452M, "MemberDetails.A_CDPNUMBER")
                                                                                                .Set("US", "MemberDetails.A_COUNTRY"),
                    GenerateTxns(6, 1805452M, HertzProgram.GoldPointsRewards.Set(GPR.Tier.FiveStar.Code, "SpecificTier")),
                    GenerateExpectedPoints(6, GPR.Tier.FiveStar),
                    new string[] { }
                ).SetName("GPRAAABonusActivity [GPR Five Star] 6 Transactions CDP = 1805452, MemberDetails.A_COUNTRY = US");

                yield return new TestCaseData(
                    "GPRAAABonusActivity [GPR Presidents Circle] 6 Transactions CDP = 1805452, MemberDetails.A_COUNTRY = US",
                    MemberStyle.ProjectOne,
                    Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.PresidentsCircle.Code, "SpecificTier"))
                                                                                                .Set(1805452M, "MemberDetails.A_CDPNUMBER")
                                                                                                .Set("US", "MemberDetails.A_COUNTRY"),
                    GenerateTxns(6, 1805452M, HertzProgram.GoldPointsRewards.Set(GPR.Tier.PresidentsCircle.Code, "SpecificTier")),
                    GenerateExpectedPoints(6, GPR.Tier.PresidentsCircle),
                    new string[] { }
                ).SetName("GPRAAABonusActivity [GPR Presidents Circle] 6 Transactions CDP = 1805452, MemberDetails.A_COUNTRY = US");

                yield return new TestCaseData(
                    "GPRAAABonusActivity [GPR Platinum] 6 Transactions CDP = 1805452, MemberDetails.A_COUNTRY = US",
                    MemberStyle.ProjectOne,
                    Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.Platinum.Code, "SpecificTier"))
                                                                                                .Set(1805452M, "MemberDetails.A_CDPNUMBER")
                                                                                                .Set("US", "MemberDetails.A_COUNTRY"),
                    GenerateTxns(6, 1805452M, HertzProgram.GoldPointsRewards.Set(GPR.Tier.Platinum.Code, "SpecificTier")),
                    GenerateExpectedPoints(6, GPR.Tier.Platinum),
                    new string[] { }
                ).SetName("GPRAAABonusActivity [GPR Platinum] 6 Transactions CDP = 1805452, MemberDetails.A_COUNTRY = US");
            }
        }

        static object[] NegativeScenarios =
        {
            new object[]
            {
                "ACIActivation800Activity [GPR Regular Gold]  Invalid CDP = 1234567, MemberDetails.A_COUNTRY = US",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier"))
                                                                                            .Set(1234567M,"MemberDetails.A_CDPNUMBER")
                                                                                            .Set("FR","MemberDetails.A_COUNTRY"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.AddDays(2).Comparable(),
                                        checkOutDate:DateTime.Now.AddDays(1).Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier"),
                                        RSDNCTRYCD: "US", HODIndicator: 0, qualifyingAmount: 25M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("ACIActivation800Activity",800M) },
                new string[]{ }
            },
            new object[]
            {
                "ACIActivation800Activity [GPR Regular Gold] Invalid QualifyingAmount = $24 CDP = 664920, MemberDetails.A_COUNTRY = US",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier"))
                                                                                            .Set(664920M,"MemberDetails.A_CDPNUMBER")
                                                                                            .Set("FR","MemberDetails.A_COUNTRY"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.AddDays(2).Comparable(),
                                        checkOutDate:DateTime.Now.AddDays(1).Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier"),
                                        RSDNCTRYCD: "US", HODIndicator: 0, qualifyingAmount: 24M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("ACIActivation800Activity",800M) },
                new string[]{ }
            }
        };


        private static decimal Amount(int num)
        {
            return (BaseAmount + (BaseAmount * num));
        }
        private static TxnHeader[] GenerateTxns(int num, decimal cdp, IHertzProgram programIn)
        {
            List<TxnHeader> output = new List<TxnHeader>();
            for (int i = 0; i < num; i++)
            {
                output.Add(TxnHeader.Generate("", checkInDate: DateTime.Now.AddDays(2).Comparable(),
                                        checkOutDate: DateTime.Now.AddDays(1).Comparable(),
                                        bookDate: DateTime.Now.Comparable(),
                                        program: programIn,
                                        RSDNCTRYCD: "US", HODIndicator: 0, qualifyingAmount: Amount(i), CDP: cdp));
            }
            return output.ToArray();
        }
        private static ExpectedPointEvent[] GenerateExpectedPoints(int num, IHertzTier tier)
        {
            List<ExpectedPointEvent> output = new List<ExpectedPointEvent>();
            if (tier.Code == GPR.Tier.RegularGold.Code) output.AddRange(RegularGold(num));
            else if (tier.Code == GPR.Tier.FiveStar.Code) output.AddRange(FiveStar(num));
            else if (tier.Code == GPR.Tier.PresidentsCircle.Code) output.AddRange(PresidentsCircle(num));
            else if (tier.Code == GPR.Tier.Platinum.Code) output.AddRange(Platinum(num));
            return output.ToArray();
        }
        private static List<ExpectedPointEvent> RegularGold(int num)
        {
            List<ExpectedPointEvent> output = new List<ExpectedPointEvent>();
            for (int i = 0; i < num; i++)
            {
                if (i == 0) output.Add(new ExpectedPointEvent("GPRGoldRental", Amount(i) * (1+GPR.Tier.RegularGold.EarningRateModifier)));
                else if (i >= 1 && i <= 2)
                {
                    output.Add(new ExpectedPointEvent("GPRGoldRental", Amount(i) * (1 + GPR.Tier.RegularGold.EarningRateModifier)));
                    output.Add(new ExpectedPointEvent("GPRAAABonusActivity", Math.Round(Amount(i) * (0.1M - GPR.Tier.RegularGold.EarningRateModifier), MidpointRounding.AwayFromZero)));
                }
                else if (i >= 3 && i <= 4)
                {
                    output.Add(new ExpectedPointEvent("GPRGoldRental", Amount(i) * (1 + GPR.Tier.RegularGold.EarningRateModifier)));
                    output.Add(new ExpectedPointEvent("GPRAAABonusActivity", Math.Round(Amount(i) * (0.15M - GPR.Tier.RegularGold.EarningRateModifier), MidpointRounding.AwayFromZero)));
                }
                else if (i >= 5)
                {
                    output.Add(new ExpectedPointEvent("GPRGoldRental", Amount(i) * (1 + GPR.Tier.RegularGold.EarningRateModifier)));
                    output.Add(new ExpectedPointEvent("GPRAAABonusActivity", Math.Round(Amount(i) * (0.25M - GPR.Tier.RegularGold.EarningRateModifier), MidpointRounding.AwayFromZero)));
                }
            }
            return output;
        }
        private static List<ExpectedPointEvent> FiveStar(int num)
        {
            List<ExpectedPointEvent> output = new List<ExpectedPointEvent>();
            for (int i = 0; i < num; i++)
            {
                if (i <= 2)
                {
                    output.Add(new ExpectedPointEvent("GPRGoldRental", Amount(i)));
                    output.Add(new ExpectedPointEvent("GPRTierBonus_FS", Math.Round(Amount(i) * GPR.Tier.FiveStar.EarningRateModifier, MidpointRounding.AwayFromZero)));
                }
                else if (i >= 3 && i <= 4)
                {
                    output.Add(new ExpectedPointEvent("GPRGoldRental", Amount(i)));
                    output.Add(new ExpectedPointEvent("GPRTierBonus_FS", Math.Round(Amount(i) * GPR.Tier.FiveStar.EarningRateModifier, MidpointRounding.AwayFromZero)));
                    output.Add(new ExpectedPointEvent("GPRAAABonusActivity", Math.Round(Amount(i) * 0.05M , MidpointRounding.AwayFromZero)));
                }
                else if (i >= 5)
                {
                    output.Add(new ExpectedPointEvent("GPRGoldRental", Amount(i)));
                    output.Add(new ExpectedPointEvent("GPRTierBonus_FS", Math.Round(Amount(i) * GPR.Tier.FiveStar.EarningRateModifier, MidpointRounding.AwayFromZero)));
                    output.Add(new ExpectedPointEvent("GPRAAABonusActivity", Math.Round(Amount(i) * 0.15M, MidpointRounding.AwayFromZero)));
                }
            }
            return output;
        }
        private static List<ExpectedPointEvent> PresidentsCircle(int num)
        {
            List<ExpectedPointEvent> output = new List<ExpectedPointEvent>();
            for (int i = 0; i < num; i++)
            {
                output.Add(new ExpectedPointEvent("GPRGoldRental", Amount(i)));
                output.Add(new ExpectedPointEvent("GPRTierBonus_PC_PL", Math.Round(Amount(i) * GPR.Tier.PresidentsCircle.EarningRateModifier, MidpointRounding.AwayFromZero)));
            }
            return output;
        }
        private static List<ExpectedPointEvent> Platinum(int num)
        {
            List<ExpectedPointEvent> output = new List<ExpectedPointEvent>();
            for (int i = 0; i < num; i++)
            {
                output.Add(new ExpectedPointEvent("GPRGoldRental", Amount(i)));
                output.Add(new ExpectedPointEvent("GPRTierBonus_PC_PL", Math.Round(Amount(i) * GPR.Tier.Platinum.EarningRateModifier, MidpointRounding.AwayFromZero)));
            }
            return output;
        }
    }
}
