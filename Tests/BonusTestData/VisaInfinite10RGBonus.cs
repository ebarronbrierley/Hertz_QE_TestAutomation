using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brierley.TestAutomation.Core.Utilities;
using HertzNetFramework.DataModels;
using NUnit.Framework;

namespace HertzNetFramework.Tests.BonusTestData
{
    public class VisaInfinite10RGBonusActivity
    {
        public const string PointEventName = "VisaInfinite10RGBonusActivity";
        public const decimal BaseTxnAmount = 100M;
        public const decimal PointEventAmount = BaseTxnAmount;
        public static readonly DateTime StartDate = DateTime.Parse("03/01/19 12:00:01 AM");
        public static readonly DateTime EndDate = DateTime.Parse("02/28/23 12:00:00 AM");

        public static readonly string[] ValidRSDNCTRYCDs = new string[] { "US", "BM", "CA", "MX", "AS", "GU", "MP", "PR", "VI",
                                                                          "BE", "FR", "DE", "IT", "LU", "NL", "ES", "CH", "GB", "IE", "SE", "NO", "DK", "FI", "PL", "PT",
                                                                          "AI", "AW", "BB", "BS", "VG", "KY","DM","DO","GD","GP","HT","JM","MQ","MS","AN","SM","KN","LC","VC","TT","TC",
                                                                          "AR","BO","BR","BZ","CL","CO","CR","EC","FK","GF","GT","GY","HN","NI","PA","PE","PY","SR","SV","UY","VE",
                                                                          "AU","NZ","PW","CN","SG","JP","KR" };
        public static readonly decimal[] ValidCDPs = new decimal[] { 2150933M };
        public static readonly IHertzProgram[] ValidPrograms = new IHertzProgram[] { HertzProgram.GoldPointsRewards };
        public static readonly IHertzTier[] ValidTiers = new IHertzTier[] { GPR.Tier.RegularGold };
        public static TimeSpan ValidRentalLength = TimeSpan.FromDays(1);
        public static ExpectedPointEvent[] ExpectedPointEvents = new ExpectedPointEvent[] { new ExpectedPointEvent(PointEventName, BaseTxnAmount*0.1M) };


        public static IEnumerable PositiveScenarios
        {
            get
            {
                foreach (string validRSDNCTRY in ValidRSDNCTRYCDs)
                {
                    yield return new TestCaseData(
                        Member.GenerateRandom(MemberStyle.ProjectOne, ValidPrograms[0].Set(ValidTiers[0].Code, "SpecificTier"))
                                                                                      .Set(validRSDNCTRY, "MemberDetails.A_COUNTRY")
                                                                                      .Set(ValidCDPs[0], "MemberDetails.A_CDPNUMBER")
                        ,
                        new TxnHeader[] {
                            TxnHeader.Generate("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                        checkOutDate:DateTime.Now.Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: ValidPrograms[0].Set(ValidTiers[0].Code,"SpecificTier"), CDP: ValidCDPs[0],
                                        RSDNCTRYCD: validRSDNCTRY, HODIndicator: 0, qualifyingAmount: BaseTxnAmount)
                        },
                        ExpectedPointEvents,
                        new string[] { }
                    ).SetName($"{PointEventName}. RSDNCTRYCD = {validRSDNCTRY}, EarningPref={ValidPrograms[0].EarningPreference}, Tier={ValidTiers[0].Code}, CDP = {ValidCDPs[0]}")
                     .SetCategory(BonusTestCategory.Regression)
                     .SetCategory(BonusTestCategory.Positive);
                }
                foreach (decimal validCDP in ValidCDPs)
                {
                    yield return new TestCaseData(
                        Member.GenerateRandom(MemberStyle.ProjectOne, ValidPrograms[0].Set(ValidTiers[0].Code, "SpecificTier"))
                                                                                      .Set(ValidRSDNCTRYCDs[0], "MemberDetails.A_COUNTRY")
                                                                                      .Set(validCDP, "MemberDetails.A_CDPNUMBER")
                        ,
                        new TxnHeader[] {
                            TxnHeader.Generate("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                        checkOutDate:DateTime.Now.Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: ValidPrograms[0].Set(ValidTiers[0].Code,"SpecificTier"), CDP: validCDP,
                                        RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, qualifyingAmount: BaseTxnAmount)
                        },
                        ExpectedPointEvents,
                        new string[] { }
                    ).SetName($"{PointEventName}. CDP = {validCDP},  RSDNCTRYCD = {ValidRSDNCTRYCDs[0]}, EarningPref={ValidPrograms[0].EarningPreference}, Tier={ValidTiers[0].Code}")
                     .SetCategory(BonusTestCategory.Regression)
                     .SetCategory(BonusTestCategory.Positive);
                }
                foreach (IHertzProgram validProgram in ValidPrograms)
                {
                    foreach (IHertzTier validTier in validProgram.Tiers)
                    {
                        if (!ValidTiers.ToList().Any(x => x.Name.Equals(validTier.Name))) continue;

                        yield return new TestCaseData(
                            Member.GenerateRandom(MemberStyle.ProjectOne, validProgram.Set(validTier.Code, "SpecificTier"))
                                                                                          .Set(ValidRSDNCTRYCDs[0], "MemberDetails.A_COUNTRY")
                                                                                          .Set(ValidCDPs[0], "MemberDetails.A_CDPNUMBER")
                            ,
                            new TxnHeader[] {
                                TxnHeader.Generate("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                checkOutDate:DateTime.Now.Comparable(),
                                bookDate:DateTime.Now.Comparable(),
                                program: validProgram.Set(validTier.Code,"SpecificTier"), CDP: ValidCDPs[0],
                                RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, qualifyingAmount: BaseTxnAmount)
                            },
                            ExpectedPointEvents,
                            new string[] { }
                        ).SetName($"{PointEventName}. EarningPref={validProgram.EarningPreference}, Tier={validTier.Code}, CDP = {ValidCDPs[0]},  RSDNCTRYCODE = {ValidRSDNCTRYCDs[0]}")
                         .SetCategory(BonusTestCategory.Regression)
                         .SetCategory(BonusTestCategory.Positive)
                         .SetCategory(BonusTestCategory.Smoke);
                    }
                }
            }
        }

      
        static object[] NegativeScenarios =
        {
            new object[]
            {
                "VisaInfinite10RGBonus [GPR Platinum] CDP = 2150933, Residence = US, Check Out Country = US",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.Platinum.Code,"SpecificTier")).Set(2150933M,"MemberDetails.A_CDPNUMBER"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.Comparable(),
                                       checkOutDate:DateTime.Now.AddDays(-2).Comparable(),
                                       bookDate:DateTime.Now.AddDays(-2).Comparable(),
                                       CDP: 2150933M, program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.Platinum.Code,"SpecificTier"),
                                       RSDNCTRYCD: "US", HODIndicator: 0, qualifyingAmount: 80M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("VisaInfinite10RGBonusActivity", 80*0.1M) },
                new string[] { }
            },
            new object[]
            {
                "VisaInfinite10RGBonus [GPR Regular Gold] Invalid CDP = 1234567, Residence = US, Check Out Country = US",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier")).Set(1234567M,"MemberDetails.A_CDPNUMBER"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.Comparable(),
                                       checkOutDate:DateTime.Now.AddDays(-2).Comparable(),
                                       bookDate:DateTime.Now.AddDays(-2).Comparable(),
                                       CDP: 1234567M, program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier"),
                                       RSDNCTRYCD: "US", HODIndicator: 0, qualifyingAmount: 80M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("VisaInfinite10RGBonusActivity", 80*0.1M) },
                new string[] { }
            },
            new object[]
            {
                "VisaInfinite10RGBonus [Dollar (DX)] CDP = 2150933, Residence = US, Check Out Country = US",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.DollarExpressRenters).Set(2150933M,"MemberDetails.A_CDPNUMBER"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.Comparable(),
                                       checkOutDate:DateTime.Now.AddDays(-2).Comparable(),
                                       bookDate:DateTime.Now.AddDays(-2).Comparable(),
                                       CDP: 2150933M, program: HertzProgram.DollarExpressRenters,
                                       RSDNCTRYCD: "US", HODIndicator: 0, qualifyingAmount: 80M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("VisaInfinite10RGBonusActivity", 80*0.1M) },
                new string[] { }
            },
            new object[]
            {
                "VisaInfinite10RGBonus [Thrifty (BC)] CDP = 2150933, Residence = US, Check Out Country = US",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.ThriftyBlueChip).Set(2150933M,"MemberDetails.A_CDPNUMBER"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.Comparable(),
                                       checkOutDate:DateTime.Now.AddDays(-2).Comparable(),
                                       bookDate:DateTime.Now.AddDays(-2).Comparable(),
                                       CDP: 2150933M, program: HertzProgram.ThriftyBlueChip,
                                       RSDNCTRYCD: "US", HODIndicator: 0, qualifyingAmount: 80M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("VisaInfinite10RGBonusActivity", 80*0.1M) },
                new string[] { }
            }
        };
    }
}
