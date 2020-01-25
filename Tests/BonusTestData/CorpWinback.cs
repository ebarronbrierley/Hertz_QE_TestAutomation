using Brierley.TestAutomation.Core.Utilities;
using HertzNetFramework.DataModels;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HertzNetFramework.Tests.BonusTestData
{
    public class CorpWinback
    {
        public const string PointEventName = "";
        public const decimal BaseAmount = 25;
        public static readonly string[] ValidRSDNCTRYCDs = new string[] { "GB", "IE", "BE", "LUX", "FR", "IT", "ES", "DE", "NL", "CHF", "CHG" };
        public static readonly IHertzProgram[] ValidPrograms = new IHertzProgram[] { HertzProgram.GoldPointsRewards };
        public static readonly IHertzTier[] ValidTiers = new IHertzTier[] { GPR.Tier.RegularGold, GPR.Tier.FiveStar, GPR.Tier.PresidentsCircle };
        public static object[] PosScenariosObject =
        {
            Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE").Set("RG", "MemberDetails.A_TIERCODE").Set("GB", "MemberDetails.A_COUNTRY"),
            new TxnHeader[]{TxnHeader.Generate("", new DateTime(2019, 9, 4), new DateTime(2019, 9, 3), new DateTime(2019, 9, 3), null, HertzProgram.GoldPointsRewards, null, "GB", 50, "AAAA", 246095, "N", "US", null) },
            GenerateExpectedPoints(7, GPR.Tier.RegularGold),
            new string[]{ "EUWinback2019R2FG1" }
        };

        public static IEnumerable PositiveScenarios2
        {
            get
            {
                TestCaseData posTCD = new TestCaseData(PosScenariosObject);
                yield return posTCD;
            }
        }

        public static IEnumerable PositiveScenarios
        {
            get
            {
                foreach (string rsdnCtry in ValidRSDNCTRYCDs)
                {
                    yield return new TestCaseData(
                        Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE")
                        .Set("", "MemberDetails.A_CDPNUMBER")
                        .Set("FG", "MemberDetails.A_TIERCODE")
                        .Set(rsdnCtry, "MemberDetails.A_COUNTRY"),
                        GenerateTxns(13, "", ValidPrograms[0].Set(ValidTiers[1].Code, "SpecificTier"), rsdnCtry),
                        GenerateExpectedPoints(2, ValidTiers[1]),
                        new string[] { "EUWinback2019R2FG1" }
                    ).SetName($"CorpWinbackPos{PointEventName}. RSDNCTRYCD = {rsdnCtry}, EarningPref ={ValidPrograms[0].EarningPreference}, Tier ={ValidTiers[0].Code}");
                }
                //foreach (IHertzProgram validProgram in ValidPrograms)
                //{
                //    foreach (IHertzTier validTier in validProgram.Tiers)
                //    {
                //        if (!ValidTiers.ToList().Any(x => x.Name.Equals(validTier.Name))) continue;
                //        for (int transactionCount = 2; transactionCount < 7; transactionCount++)
                //            yield return new TestCaseData(
                //                Member.GenerateRandom(MemberStyle.ProjectOne, validProgram.Set(validTier.Code, "SpecificTier"))
                //                                                                                            .Set("", "MemberDetails.A_CDPNUMBER")
                //                                                                                            .Set(ValidRSDNCTRYCDs[0], "MemberDetails.A_COUNTRY"),
                //                GenerateTxns(transactionCount, "", validProgram.Set(validTier.Code, "SpecificTier")),
                //                GenerateExpectedPoints(transactionCount, validTier),
                //                new string[] { }
                //            ).SetName($"CorpWinbackPos2{PointEventName}. {transactionCount} Transactions EarningPref ={validProgram.EarningPreference}, Tier ={validTier.Code}, RSDNCTRYCD = {ValidRSDNCTRYCDs[0]}");
                //    }
                //}
            }
        }

        private static TxnHeader[] GenerateTxns(int num, string cdp, IHertzProgram programIn, string resCountry = "US")
        {
            List<TxnHeader> output = new List<TxnHeader>();
            for (int i = 0; i < num; i++)
            {
                output.Add(TxnHeader.Generate("", checkInDate: DateTime.Now.AddDays(2).Comparable(),
                                        checkOutDate: DateTime.Now.AddDays(1).Comparable(),
                                        bookDate: DateTime.Now.Comparable(),
                                        program: programIn,
                                        RSDNCTRYCD: resCountry, HODIndicator: 0, rentalCharges: Amount(i)));
            }
            return output.ToArray();
        }
        private static ExpectedPointEvent[] GenerateExpectedPoints(int num, IHertzTier tier)
        {
            List<ExpectedPointEvent> output = new List<ExpectedPointEvent>();
            if (tier.Code == GPR.Tier.RegularGold.Code) output.AddRange(RegularGold(num));
            else if (tier.Code == GPR.Tier.FiveStar.Code) output.AddRange(FiveStar(num));
            else if (tier.Code == GPR.Tier.PresidentsCircle.Code) output.AddRange(PresidentsCircle(num));
            return output.ToArray();
        }
        private static decimal Amount(int num)
        {
            return (BaseAmount + (BaseAmount * num));
        }
        private static List<ExpectedPointEvent> RegularGold(int num)
        {
            List<ExpectedPointEvent> output = new List<ExpectedPointEvent>();
            for (int i = 0; i < num; i++)
            {
                if (i == 0) output.Add(new ExpectedPointEvent("GPRGoldRental", Amount(i) * (1 + GPR.Tier.RegularGold.EarningRateModifier)));
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
                    output.Add(new ExpectedPointEvent("GPRAAABonusActivity", Math.Round(Amount(i) * 0.05M, MidpointRounding.AwayFromZero)));
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

    }
}
