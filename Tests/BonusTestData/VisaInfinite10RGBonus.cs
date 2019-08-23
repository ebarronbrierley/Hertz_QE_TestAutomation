using System;
using Brierley.TestAutomation.Core.Utilities;
using HertzNetFramework.DataModels;

namespace HertzNetFramework.Tests.BonusTestData
{
    public class VisaInfinite10RGBonus
    {
        static object[] PositiveScenarios =
       {
            new object[]
            {
                "VisaInfinite10RGBonus [GPR Regular Gold]  CDP = 2150933, Residence = US, Check Out Country = US",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier")).Set(2150933M,"MemberDetails.A_CDPNUMBER"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.Comparable(),
                                       checkOutDate:DateTime.Now.AddDays(-2).Comparable(),
                                       bookDate:DateTime.Now.AddDays(-2).Comparable(),
                                       CDP: 2150933M, program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier"),
                                       RSDNCTRYCD: "US", HODIndicator: 0, qualifyingAmount: 10M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("GPRGoldRental", 10M),
                                           new ExpectedPointEvent("VisaInfinite10RGBonusActivity",10*0.1M) },
                new string[] { }
            },
            new object[]
            {
                "VisaInfinite10RGBonus [GPR Regular Gold]  CDP = 2150933, Residence = BE, Check Out Country = BE",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier")).Set(2150933M,"MemberDetails.A_CDPNUMBER"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.Comparable(),
                                       checkOutDate:DateTime.Now.AddDays(-2).Comparable(),
                                       bookDate:DateTime.Now.AddDays(-2).Comparable(),
                                       CDP: 2150933M, program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier"),
                                       RSDNCTRYCD: "BE", HODIndicator: 0, qualifyingAmount: 10M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("GPRGoldRental", 10M),
                                           new ExpectedPointEvent("VisaInfinite10RGBonusActivity",10*0.1M) },
                new string[] { }
            },
            new object[]
            {
                "VisaInfinite10RGBonus [GPR Regular Gold]  CDP = 2150933, Residence = EC, Check Out Country = GU",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier")).Set(2150933M,"MemberDetails.A_CDPNUMBER"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.Comparable(),
                                       checkOutDate:DateTime.Now.AddDays(-2).Comparable(),
                                       bookDate:DateTime.Now.AddDays(-2).Comparable(),
                                       CDP: 2150933M, program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier"),
                                       RSDNCTRYCD: "GU", HODIndicator: 0, qualifyingAmount: 10M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("GPRGoldRental", 10M),
                                           new ExpectedPointEvent("VisaInfinite10RGBonusActivity",10*0.1M) },
                new string[] { }
            },
            new object[]
            {
                "VisaInfinite10RGBonus [GPR Regular Gold]  CDP = 2150933, Residence = EC, Check Out Country = EC",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier")).Set(2150933M,"MemberDetails.A_CDPNUMBER"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.Comparable(),
                                       checkOutDate:DateTime.Now.AddDays(-2).Comparable(),
                                       bookDate:DateTime.Now.AddDays(-2).Comparable(),
                                       CDP: 2150933M, program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier"),
                                       RSDNCTRYCD: "EC", HODIndicator: 0, qualifyingAmount: 10M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("GPRGoldRental", 10M),
                                           new ExpectedPointEvent("VisaInfinite10RGBonusActivity",10*0.1M) },
                new string[] { }
            }
        };
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
