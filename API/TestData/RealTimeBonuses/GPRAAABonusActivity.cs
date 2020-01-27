using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brierley.TestAutomation.Core.Utilities;
using Hertz.API.Controllers;
using Hertz.API.DataModels;
using NUnit.Framework;

namespace Hertz.API.TestData.RealTimeBonuses
{
    public class GPRAAABonusActivity
    {
        public const string PointEventName = "GPRAAABonusActivity";
        public const decimal BaseAmount = 25;
        public static readonly decimal[] ValidCDPs = new decimal[]{ 01805452M, 00000001M, 00000002M, 00000004M, 00000005M, 00000006M, 00000007M,
                                                                    00000008M, 00000014M, 00000018M, 00000020M, 00000023M, 00000033M, 00000036M,
                                                                    00000042M, 00000045M, 00000049M, 00000057M, 00000066M, 00000069M, 00000071M,
                                                                    00000072M, 00000074M, 00000075M, 00000080M, 00000083M, 00000084M, 00000097M,
                                                                    00000101M, 00000104M, 00000111M, 00000113M, 00000115M, 00000130M, 00000133M,
                                                                    00000135M, 00000137M, 00000151M, 00000154M, 00000160M, 00000164M, 00000176M,
                                                                    00000177M, 00000188M, 00000195M, 00000212M, 00000215M, 00000216M, 00000217M,
                                                                    00000219M, 00000222M, 00000227M, 00000238M, 00000240M, 00000241M, 00000243M,
                                                                    00000252M, 00000258M, 00000260M, 00000270M, 00011845M, 00068965M, 01571971M };
        public static readonly string[] ValidRSDNCTRYCDs = new string[]{ "US", "PR" };
        public static readonly IHertzProgram[] ValidPrograms = new IHertzProgram[] { HertzLoyalty.GoldPointsRewards };
        public static readonly IHertzTier[] ValidTiers = new IHertzTier[] { HertzLoyalty.GoldPointsRewards.RegularGold, HertzLoyalty.GoldPointsRewards.FiveStar, HertzLoyalty.GoldPointsRewards.PresidentsCircle, HertzLoyalty.GoldPointsRewards.Platinum };
        public const string ChkOutLocId = "00004";

        public static IEnumerable PositiveScenarios 
        {
            get
            {
                foreach (decimal validCDP in ValidCDPs)
                {
                    MemberModel member = MemberController.GenerateRandomMember(ValidTiers[0]);
                    member.MemberDetails.A_CDPNUMBER = $"{validCDP}";
                    member.MemberDetails.A_COUNTRY = ValidRSDNCTRYCDs[0];

                    yield return new TestCaseData(
                        member,
                        GenerateTxns(2, validCDP, ValidTiers[0]),
                        GenerateExpectedPoints(2, ValidTiers[0]),
                        new string[] { }
                    ).SetName($"{PointEventName}. CDP = {validCDP}, EarningPref ={ValidPrograms[0].EarningPreference}, Tier ={ValidTiers[0].Code}, RSDNCTRYCD = {ValidRSDNCTRYCDs[0]}")
                    .SetCategory(BonusTestCategory.Regression)
                    .SetCategory(BonusTestCategory.Positive);
                }
                foreach (string rsdnCtry in ValidRSDNCTRYCDs)
                {
                    MemberModel member = MemberController.GenerateRandomMember(ValidTiers[0]);
                    member.MemberDetails.A_CDPNUMBER = $"{ValidCDPs[0]}";
                    member.MemberDetails.A_COUNTRY = rsdnCtry;

                    yield return new TestCaseData(
                        member,
                        GenerateTxns(2, ValidCDPs[0], ValidTiers[0], rsdnCtry),
                        GenerateExpectedPoints(2, ValidTiers[0]),
                        new string[] { }
                    ).SetName($"{PointEventName}. RSDNCTRYCD = {rsdnCtry}, EarningPref ={ValidPrograms[0].EarningPreference}, Tier ={ValidTiers[0].Code}, CDP = {ValidCDPs[0]}")
                     .SetCategory(BonusTestCategory.Regression)
                     .SetCategory(BonusTestCategory.Positive);
                }
                foreach(IHertzProgram validProgram in ValidPrograms)
                {
                    foreach(IHertzTier validTier in validProgram.Tiers)
                    {
                        if (!ValidTiers.ToList().Any(x => x.Name.Equals(validTier.Name))) continue;

                        for (int transactionCount = 2; transactionCount < 7; transactionCount++)
                        {
                            MemberModel member = MemberController.GenerateRandomMember(validTier);
                            member.MemberDetails.A_CDPNUMBER = $"{ValidCDPs[0]}";
                            member.MemberDetails.A_COUNTRY = ValidRSDNCTRYCDs[0];

                            yield return new TestCaseData(
                                member,
                                GenerateTxns(transactionCount, ValidCDPs[0], validTier),
                                GenerateExpectedPoints(transactionCount, validTier),
                                new string[] { }
                            ).SetName($"{PointEventName}. {transactionCount} Transactions EarningPref ={validProgram.EarningPreference}, Tier ={validTier.Code}, CDP = {ValidCDPs[0]}, RSDNCTRYCD = {ValidRSDNCTRYCDs[0]}");
                        }

                        for (int transactionCount = 2; transactionCount < 7; transactionCount++)
                        {
                            MemberModel member = MemberController.GenerateRandomMember(validTier);
                            member.MemberDetails.A_CDPNUMBER = $"{ValidCDPs[0]}";
                            member.MemberDetails.A_COUNTRY = ValidRSDNCTRYCDs[0];
                            yield return new TestCaseData(
                              member,
                              GenerateTxns(transactionCount, ValidCDPs[0], validTier, chkOutLocNum: null, chkOutAreaNum: null, chkoutLocId: ChkOutLocId),
                              GenerateExpectedPoints(transactionCount, validTier),
                              new string[] { }
                          ).SetName($"{PointEventName}. {transactionCount} Transactions EarningPref ={validProgram.EarningPreference}, Tier ={validTier.Code}, CDP = {ValidCDPs[0]}, RSDNCTRYCD = {ValidRSDNCTRYCDs[0]},CHKOUTLOCID:{ChkOutLocId}")
                          .SetCategory(BonusTestCategory.Smoke)
                          .SetCategory(BonusTestCategory.Regression)
                          .SetCategory(BonusTestCategory.Positive);
                        }
                    }
                }
            }
        }
        public static IEnumerable NegativeScenarios
        {
            get
            {
                yield return new TestCaseData();
            }
        }

        private static decimal Amount(int num)
        {
            return (BaseAmount + (BaseAmount * num));
        }
        private static TxnHeaderModel[] GenerateTxns(int num, decimal cdp, IHertzTier tierIn, string resCountry = "US", string chkOutLocNum = "06", string chkOutAreaNum ="01474", string chkoutLocId = null)
        {
            List<TxnHeaderModel> output = new List<TxnHeaderModel>();
            for (int i = 0; i < num; i++)
            {
                output.Add(TxnHeaderController.GenerateTransaction("", checkInDate: DateTime.Now.AddDays(2).Comparable(),
                                        checkOutDate: DateTime.Now.AddDays(1).Comparable(),
                                        bookDate: DateTime.Now.Comparable(),
                                        program: tierIn.ParentProgram,
                                        RSDNCTRYCD: resCountry, HODIndicator: 0, rentalCharges: Amount(i), CDP: cdp,chkoutlocnum: chkOutLocNum, chkoutareanum: chkOutAreaNum, chkoutlocid: chkoutLocId));
            }
            return output.ToArray();
        }
        private static ExpectedPointEvent[] GenerateExpectedPoints(int num, IHertzTier tier)
        {
            List<ExpectedPointEvent> output = new List<ExpectedPointEvent>();
            if (tier.Code == HertzLoyalty.GoldPointsRewards.RegularGold.Code) output.AddRange(RegularGold(num));
            else if (tier.Code == HertzLoyalty.GoldPointsRewards.FiveStar.Code) output.AddRange(FiveStar(num));
            else if (tier.Code == HertzLoyalty.GoldPointsRewards.PresidentsCircle.Code) output.AddRange(PresidentsCircle(num));
            else if (tier.Code == HertzLoyalty.GoldPointsRewards.Platinum.Code) output.AddRange(Platinum(num));
            return output.ToArray();
        }
        private static List<ExpectedPointEvent> RegularGold(int num)
        {
            List<ExpectedPointEvent> output = new List<ExpectedPointEvent>();
            for (int i = 0; i < num; i++)
            {
                if (i == 0) output.Add(new ExpectedPointEvent("GPRGoldRental", Amount(i) * (1+ HertzLoyalty.GoldPointsRewards.RegularGold.EarningRateModifier)));
                else if (i >= 1 && i <= 2)
                {
                    output.Add(new ExpectedPointEvent("GPRGoldRental", Amount(i) * (1 + HertzLoyalty.GoldPointsRewards.RegularGold.EarningRateModifier)));
                    output.Add(new ExpectedPointEvent("GPRAAABonusActivity", Math.Round(Amount(i) * (0.1M - HertzLoyalty.GoldPointsRewards.RegularGold.EarningRateModifier), MidpointRounding.AwayFromZero)));
                }
                else if (i >= 3 && i <= 4)
                {
                    output.Add(new ExpectedPointEvent("GPRGoldRental", Amount(i) * (1 + HertzLoyalty.GoldPointsRewards.RegularGold.EarningRateModifier)));
                    output.Add(new ExpectedPointEvent("GPRAAABonusActivity", Math.Round(Amount(i) * (0.15M - HertzLoyalty.GoldPointsRewards.RegularGold.EarningRateModifier), MidpointRounding.AwayFromZero)));
                }
                else if (i >= 5)
                {
                    output.Add(new ExpectedPointEvent("GPRGoldRental", Amount(i) * (1 + HertzLoyalty.GoldPointsRewards.RegularGold.EarningRateModifier)));
                    output.Add(new ExpectedPointEvent("GPRAAABonusActivity", Math.Round(Amount(i) * (0.25M - HertzLoyalty.GoldPointsRewards.RegularGold.EarningRateModifier), MidpointRounding.AwayFromZero)));
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
                    output.Add(new ExpectedPointEvent("GPRTierBonus_FS", Math.Round(Amount(i) * HertzLoyalty.GoldPointsRewards.FiveStar.EarningRateModifier, MidpointRounding.AwayFromZero)));
                }
                else if (i >= 3 && i <= 4)
                {
                    output.Add(new ExpectedPointEvent("GPRGoldRental", Amount(i)));
                    output.Add(new ExpectedPointEvent("GPRTierBonus_FS", Math.Round(Amount(i) * HertzLoyalty.GoldPointsRewards.FiveStar.EarningRateModifier, MidpointRounding.AwayFromZero)));
                    output.Add(new ExpectedPointEvent("GPRAAABonusActivity", Math.Round(Amount(i) * 0.05M , MidpointRounding.AwayFromZero)));
                }
                else if (i >= 5)
                {
                    output.Add(new ExpectedPointEvent("GPRGoldRental", Amount(i)));
                    output.Add(new ExpectedPointEvent("GPRTierBonus_FS", Math.Round(Amount(i) * HertzLoyalty.GoldPointsRewards.FiveStar.EarningRateModifier, MidpointRounding.AwayFromZero)));
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
                output.Add(new ExpectedPointEvent("GPRTierBonus_PC_PL", Math.Round(Amount(i) * HertzLoyalty.GoldPointsRewards.PresidentsCircle.EarningRateModifier, MidpointRounding.AwayFromZero)));
            }
            return output;
        }
        private static List<ExpectedPointEvent> Platinum(int num)
        {
            List<ExpectedPointEvent> output = new List<ExpectedPointEvent>();
            for (int i = 0; i < num; i++)
            {
                output.Add(new ExpectedPointEvent("GPRGoldRental", Amount(i)));
                output.Add(new ExpectedPointEvent("GPRTierBonus_PC_PL", Math.Round(Amount(i) * HertzLoyalty.GoldPointsRewards.Platinum.EarningRateModifier, MidpointRounding.AwayFromZero)));
            }
            return output;
        }
    }
}
