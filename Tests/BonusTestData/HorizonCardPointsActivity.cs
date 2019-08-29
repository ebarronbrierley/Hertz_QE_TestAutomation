using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brierley.TestAutomation.Core.Utilities;
using HertzNetFramework.DataModels;
using NUnit.Framework;

namespace HertzNetFramework.Tests.BonusTestData
{
    public class HorizonCardPointsActivity
    {
        public static readonly decimal[] ValidCDPs = new decimal[] { 867695M };
        public static readonly string[] ValidRSDNCTRYCDs = new string[] { "FR"};
        public const string PointEventName = "HorizonCardPointsActivity";
        public const decimal PointEventAmount = 12000M;
        public const string ValidFTPTNRNUM = "ZE1";
        public static IHertzProgram[] ValidPrograms = new IHertzProgram[] { HertzProgram.GoldPointsRewards };
        public static IHertzTier[] ValidTiers = new IHertzTier[] { GPR.Tier.RegularGold };
        public const decimal BaseTxnAmount = 25M;
        public static ExpectedPointEvent[] ExpectedPointEvents = new ExpectedPointEvent[] { new ExpectedPointEvent(PointEventName, PointEventAmount) };
        public static TimeSpan ValidRentalLength = TimeSpan.FromDays(1);
        public static readonly DateTime StartDate = DateTime.Parse("01/01/2018");
        public static readonly DateTime EndDate = DateTime.Parse("01/01/2099");

        public static IEnumerable PositiveScenarios
        {
            get
            {
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
                        ).SetName($"{PointEventName}. CDP = {validCDP}, EarningPref={ValidPrograms[0].EarningPreference}, Tier={ValidTiers[0].Code}, RSDNCTRYCODE = {ValidRSDNCTRYCDs[0]}")
                         .SetCategory(BonusTestCategory.Regression)
                         .SetCategory(BonusTestCategory.Positive)
                         .SetCategory(BonusTestCategory.Smoke);
                }
                foreach (string validRSDN in ValidRSDNCTRYCDs)
                {
                    yield return new TestCaseData(
                            Member.GenerateRandom(MemberStyle.ProjectOne, ValidPrograms[0].Set(ValidTiers[0].Code, "SpecificTier"))
                                                                                          .Set(validRSDN, "MemberDetails.A_COUNTRY")
                                                                                          .Set(ValidCDPs[0], "MemberDetails.A_CDPNUMBER")
                            ,
                            new TxnHeader[] {
                                TxnHeader.Generate("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                checkOutDate:DateTime.Now.Comparable(),
                                bookDate:DateTime.Now.Comparable(),
                                program: ValidPrograms[0].Set(ValidTiers[0].Code,"SpecificTier"), CDP: ValidCDPs[0],
                                RSDNCTRYCD: validRSDN, HODIndicator: 0, qualifyingAmount: BaseTxnAmount)
                            },
                            ExpectedPointEvents,
                            new string[] { }
                        ).SetName($"{PointEventName}. RSDNCTRYCODE = {validRSDN}, EarningPref={ValidPrograms[0].EarningPreference}, Tier={ValidTiers[0].Code}, CDP = {ValidCDPs[0]}")
                         .SetCategory(BonusTestCategory.Regression)
                         .SetCategory(BonusTestCategory.Positive);
                }
            }
        }

        static object[] NegativeScenarios =
        {
            new object[]
            {
                 "HorizonCardPointsActivity Invalid Tier [GPR Presidents Circle]  CDP = 867695, MemberDetails.A_COUNTRY = FR",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.PresidentsCircle.Code,"SpecificTier"))
                                                                                            .Set(867695M,"MemberDetails.A_CDPNUMBER")
                                                                                            .Set("FR","MemberDetails.A_COUNTRY"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.AddDays(2).Comparable(),
                                        checkOutDate:DateTime.Now.AddDays(1).Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.PresidentsCircle.Code,"SpecificTier"),
                                        RSDNCTRYCD: "FR", HODIndicator: 0, qualifyingAmount: 25M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("HorizonCardPointsActivity",12000M) },
                new string[]{ }
            },
            new object[]
            {
                 "HorizonCardPointsActivity Invalid Tier [GPR Platinum]  CDP = 867695, MemberDetails.A_COUNTRY = FR",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.Platinum.Code,"SpecificTier"))
                                                                                            .Set(867695M,"MemberDetails.A_CDPNUMBER")
                                                                                            .Set("FR","MemberDetails.A_COUNTRY"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.AddDays(2).Comparable(),
                                        checkOutDate:DateTime.Now.AddDays(1).Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.Platinum.Code,"SpecificTier"),
                                        RSDNCTRYCD: "FR", HODIndicator: 0, qualifyingAmount: 25M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("HorizonCardPointsActivity",12000M) },
                new string[]{ }
            },
            new object[]
            {
                 "HorizonCardPointsActivity Invalid Tier [GPR Platinum Select]  CDP = 867695, MemberDetails.A_COUNTRY = FR",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.PlatinumSelect.Code,"SpecificTier"))
                                                                                            .Set(867695M,"MemberDetails.A_CDPNUMBER")
                                                                                            .Set("FR","MemberDetails.A_COUNTRY"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.AddDays(2).Comparable(),
                                        checkOutDate:DateTime.Now.AddDays(1).Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.PlatinumSelect.Code,"SpecificTier"),
                                        RSDNCTRYCD: "FR", HODIndicator: 0, qualifyingAmount: 25M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("HorizonCardPointsActivity",12000M) },
                new string[]{ }
            },
            new object[]
            {
                 "HorizonCardPointsActivity Invalid Tier [GPR Platinum VIP]  CDP = 867695, MemberDetails.A_COUNTRY = FR",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.PlatinumVIP.Code,"SpecificTier"))
                                                                                            .Set(867695M,"MemberDetails.A_CDPNUMBER")
                                                                                            .Set("FR","MemberDetails.A_COUNTRY"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.AddDays(2).Comparable(),
                                        checkOutDate:DateTime.Now.AddDays(1).Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.PlatinumVIP.Code,"SpecificTier"),
                                        RSDNCTRYCD: "FR", HODIndicator: 0, qualifyingAmount: 25M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("HorizonCardPointsActivity",12000M) },
                new string[]{ }
            },
            new object[]
            {
                 "HorizonCardPointsActivity Invalid Tier [Dollar DX]  CDP = 867695, MemberDetails.A_COUNTRY = FR",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.DollarExpressRenters)
                                                                                            .Set(867695M,"MemberDetails.A_CDPNUMBER")
                                                                                            .Set("FR","MemberDetails.A_COUNTRY"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.AddDays(2).Comparable(),
                                        checkOutDate:DateTime.Now.AddDays(1).Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: HertzProgram.DollarExpressRenters,
                                        RSDNCTRYCD: "FR", HODIndicator: 0, qualifyingAmount: 25M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("HorizonCardPointsActivity",12000M) },
                new string[]{ }
            },
            new object[]
            {
                 "HorizonCardPointsActivity Invalid Tier [Thrifty BC]  CDP = 867695, MemberDetails.A_COUNTRY = FR",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.ThriftyBlueChip)
                                                                                            .Set(867695M,"MemberDetails.A_CDPNUMBER")
                                                                                            .Set("FR","MemberDetails.A_COUNTRY"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.AddDays(2).Comparable(),
                                        checkOutDate:DateTime.Now.AddDays(1).Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: HertzProgram.ThriftyBlueChip,
                                        RSDNCTRYCD: "FR", HODIndicator: 0, qualifyingAmount: 25M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("HorizonCardPointsActivity",12000M) },
                new string[]{ }
            },
            new object[]
            {
                 "HorizonCardPointsActivity [GPR Regular Gold]  CDP = 867695, Invalid MemberDetails.A_COUNTRY = US",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier"))
                                                                                            .Set(867695M,"MemberDetails.A_CDPNUMBER")
                                                                                            .Set("US","MemberDetails.A_COUNTRY"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.AddDays(2).Comparable(),
                                        checkOutDate:DateTime.Now.AddDays(1).Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier"),
                                        RSDNCTRYCD: "US", HODIndicator: 0, qualifyingAmount: 25M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("HorizonCardPointsActivity",12000M) },
                new string[]{ }
            },
            new object[]
            {
                 "HorizonCardPointsActivity [GPR Regular Gold] Invalid CDP = 1234567, MemberDetails.A_COUNTRY = FR",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier"))
                                                                                            .Set(1234567M,"MemberDetails.A_CDPNUMBER")
                                                                                            .Set("FR","MemberDetails.A_COUNTRY"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.AddDays(2).Comparable(),
                                        checkOutDate:DateTime.Now.AddDays(1).Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier"),
                                        RSDNCTRYCD: "FR", HODIndicator: 0, qualifyingAmount: 25M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("HorizonCardPointsActivity",12000M) },
                new string[]{ }
            },
            new object[]
            {
                 "HorizonCardPointsActivity [GPR Regular Gold] Invalid Enroll Date = 12/31/2017, CDP = 867695, MemberDetails.A_COUNTRY = FR",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier"))
                                                                                            .Set(867695M,"MemberDetails.A_CDPNUMBER")
                                                                                            .Set("FR","MemberDetails.A_COUNTRY")
                                                                                            .Set(DateTime.Parse("12/31/2017"),"MemberDetails.A_ENROLLDATE"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.AddDays(2).Comparable(),
                                        checkOutDate:DateTime.Now.AddDays(1).Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier"),
                                        RSDNCTRYCD: "FR", HODIndicator: 0, qualifyingAmount: 25M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("HorizonCardPointsActivity",12000M) },
                new string[]{ }
            }
        };
    }
}
