using System;
using Brierley.TestAutomation.Core.Utilities;
using HertzNetFramework.DataModels;

namespace HertzNetFramework.Tests.BonusTestData
{
    public class TopGolf_2019_GPR2XBonus
    {
        static object[] PositiveScenarios =
        {
            new object[]
            {
                "TopGolf_2019_GPR2XBonus [GPR Regular Gold]  CDP = 2152921, Residence = US, Check Out Country = US",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier")).Set(2152921M,"MemberDetails.A_CDPNUMBER"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.Comparable(),
                                       checkOutDate:DateTime.Now.AddDays(-2).Comparable(),
                                       bookDate:DateTime.Now.AddDays(-2).Comparable(),
                                       CDP: 2152921M, program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier"),
                                       RSDNCTRYCD: "US", HODIndicator: 0, qualifyingAmount: 80M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("GPRGoldRental", 80M),
                                           new ExpectedPointEvent("TopGolf_2019_GPR2XBonus",80M) }
            },
            new object[]
            {
                "TopGolf_2019_GPR2XBonus [GPR Five Star]  CDP = 2152921, Residence = US, Check Out Country = US",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.FiveStar.Code,"SpecificTier")).Set(2152921M,"MemberDetails.A_CDPNUMBER"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.Comparable(),
                                       checkOutDate:DateTime.Now.AddDays(-2).Comparable(),
                                       bookDate:DateTime.Now.AddDays(-2).Comparable(),
                                       CDP: 2152921M, program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.FiveStar.Code,"SpecificTier"),
                                       RSDNCTRYCD: "US", HODIndicator: 0, qualifyingAmount: 80M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("GPRGoldRental", 80M),
                                           new ExpectedPointEvent("GPRTierBonus_FS",80M*GPR.Tier.FiveStar.EarningRateModifier),
                                           new ExpectedPointEvent("TopGolf_2019_GPR2XBonus",80M) }
            },
            new object[]
            {
                "TopGolf_2019_GPR2XBonus [GPR Presidents Circle]  CDP = 2152921, Residence = US, Check Out Country = US",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.PresidentsCircle.Code,"SpecificTier")).Set(2152921M,"MemberDetails.A_CDPNUMBER"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.Comparable(),
                                       checkOutDate:DateTime.Now.AddDays(-2).Comparable(),
                                       bookDate:DateTime.Now.AddDays(-2).Comparable(),
                                       CDP: 2152921M, program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.PresidentsCircle.Code,"SpecificTier"),
                                       RSDNCTRYCD: "US", HODIndicator: 0, qualifyingAmount: 80M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("GPRGoldRental", 80M),
                                           new ExpectedPointEvent("GPRTierBonus_PC_PL",80M*GPR.Tier.PresidentsCircle.EarningRateModifier),
                                           new ExpectedPointEvent("TopGolf_2019_GPR2XBonus",80M) }
            },
            new object[]
            {
                "TopGolf_2019_GPR2XBonus [GPR Platinum] CDP = 2152921, Residence = US, Check Out Country = US",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.Platinum.Code,"SpecificTier")).Set(2152921M,"MemberDetails.A_CDPNUMBER"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.Comparable(),
                                       checkOutDate:DateTime.Now.AddDays(-2).Comparable(),
                                       bookDate:DateTime.Now.AddDays(-2).Comparable(),
                                       CDP: 2152921M, program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.Platinum.Code,"SpecificTier"),
                                       RSDNCTRYCD: "US", HODIndicator: 0, qualifyingAmount: 80M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("GPRGoldRental", 80M),
                                           new ExpectedPointEvent("GPRTierBonus_PC_PL",80M*GPR.Tier.Platinum.EarningRateModifier),
                                           new ExpectedPointEvent("TopGolf_2019_GPR2XBonus",80M) }
            },
            new object[]
            {
                "TopGolf_2019_GPR2XBonus [GPR Platinum Select] CDP = 2152921, Residence = US, Check Out Country = US",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.PlatinumSelect.Code,"SpecificTier")).Set(2152921M,"MemberDetails.A_CDPNUMBER"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.Comparable(),
                                       checkOutDate:DateTime.Now.AddDays(-2).Comparable(),
                                       bookDate:DateTime.Now.AddDays(-2).Comparable(),
                                       CDP: 2152921M, program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.PlatinumSelect.Code,"SpecificTier"),
                                       RSDNCTRYCD: "US", HODIndicator: 0, qualifyingAmount: 80M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("GPRGoldRental", 80M),
                                           new ExpectedPointEvent("GPRTierBonus_PC_PL",80M*GPR.Tier.PlatinumSelect.EarningRateModifier),
                                           new ExpectedPointEvent("TopGolf_2019_GPR2XBonus",80M) }
            },
            new object[]
            {
                "TopGolf_2019_GPR2XBonus [GPR Platinum VIP] CDP = 2152921, Residence = US, Check Out Country = US",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.PlatinumVIP.Code,"SpecificTier")).Set(2152921M,"MemberDetails.A_CDPNUMBER"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.Comparable(),
                                       checkOutDate:DateTime.Now.AddDays(-2).Comparable(),
                                       bookDate:DateTime.Now.AddDays(-2).Comparable(),
                                       CDP: 2152921M, program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.PlatinumVIP.Code,"SpecificTier"),
                                       RSDNCTRYCD: "US", HODIndicator: 0, qualifyingAmount: 80M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("GPRGoldRental", 80M),
                                           new ExpectedPointEvent("GPRTierBonus_PC_PL",80M*GPR.Tier.PlatinumVIP.EarningRateModifier),
                                           new ExpectedPointEvent("TopGolf_2019_GPR2XBonus",80M) }
            },
            new object[]
            {
                "TopGolf_2019_GPR2XBonus [GPR Regular Gold] CDP = 2152921, Residence = US, Check Out Country = CA",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier")).Set(2152921M,"MemberDetails.A_CDPNUMBER"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.Comparable(),
                                       checkOutDate:DateTime.Now.AddDays(-2).Comparable(),
                                       bookDate:DateTime.Now.AddDays(-2).Comparable(),
                                       CDP: 2152921M, program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier"),
                                       RSDNCTRYCD: "CA", HODIndicator: 0, qualifyingAmount: 80M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("GPRGoldRental", 80M),
                                           new ExpectedPointEvent("TopGolf_2019_GPR2XBonus",80M) }
            },
            new object[]
            {
                "TopGolf_2019_GPR2XBonus [GPR Regular Gold] CDP = 2152921, Residence = US, Check Out Country = PR",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier")).Set(2152921M,"MemberDetails.A_CDPNUMBER"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.Comparable(),
                                       checkOutDate:DateTime.Now.AddDays(-2).Comparable(),
                                       bookDate:DateTime.Now.AddDays(-2).Comparable(),
                                       CDP: 2152921M, program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier"),
                                       RSDNCTRYCD: "PR", HODIndicator: 0, qualifyingAmount: 80M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("GPRGoldRental", 80M),
                                           new ExpectedPointEvent("TopGolf_2019_GPR2XBonus",80M) }
            },
            new object[]
            {
                "TopGolf_2019_GPR2XBonus [GPR Regular Gold] CDP = 2152921, Residence = US, Check Out Country = VI",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier")).Set(2152921M,"MemberDetails.A_CDPNUMBER"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.Comparable(),
                                       checkOutDate:DateTime.Now.AddDays(-2).Comparable(),
                                       bookDate:DateTime.Now.AddDays(-2).Comparable(),
                                       CDP: 2152921M, program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier"),
                                       RSDNCTRYCD: "VI", HODIndicator: 0, qualifyingAmount: 80M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("GPRGoldRental", 80M),
                                           new ExpectedPointEvent("TopGolf_2019_GPR2XBonus",80M) }
            },
        };
        static object[] NegativeScenarios =
        {
            new object[]
            {
                "TopGolf_2019_GPR2XBonus [GPR Regular Gold] Invalid CDP = 1234567, Residence = US, Check Out Country = US",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier")).Set(1234567M,"MemberDetails.A_CDPNUMBER"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.Comparable(),
                                       checkOutDate:DateTime.Now.AddDays(-2).Comparable(),
                                       bookDate:DateTime.Now.AddDays(-2).Comparable(),
                                       CDP: 1234567M, program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier"),
                                       RSDNCTRYCD: "US", HODIndicator: 0, qualifyingAmount: 80M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("TopGolf_2019_GPR2XBonus",80M) }
            },
            new object[]
            {
                "TopGolf_2019_GPR2XBonus [GPR Regular Gold] CDP = 2152921, Residence = BE, Check Out Country = BE",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier")).Set(2152921M,"MemberDetails.A_CDPNUMBER"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.Comparable(),
                                       checkOutDate:DateTime.Now.AddDays(-2).Comparable(),
                                       bookDate:DateTime.Now.AddDays(-2).Comparable(),
                                       CDP: 2152921M, program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier"),
                                       RSDNCTRYCD: "BE", HODIndicator: 0, qualifyingAmount: 80M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("TopGolf_2019_GPR2XBonus",80M) }
            },
            new object[]
            {
                "TopGolf_2019_GPR2XBonus [Dollar (DX)] CDP = 2152921, Residence = US, Check Out Country = US",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.DollarExpressRenters).Set(2152921M,"MemberDetails.A_CDPNUMBER"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.Comparable(),
                                       checkOutDate:DateTime.Now.AddDays(-2).Comparable(),
                                       bookDate:DateTime.Now.AddDays(-2).Comparable(),
                                       CDP: 2152921M, program: HertzProgram.DollarExpressRenters,
                                       RSDNCTRYCD: "US", HODIndicator: 0, qualifyingAmount: 80M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("TopGolf_2019_GPR2XBonus",80M) }
            },
            new object[]
            {
                "TopGolf_2019_GPR2XBonus [Thrifty (BC)] CDP = 2152921, Residence = US, Check Out Country = US",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.ThriftyBlueChip).Set(2152921M,"MemberDetails.A_CDPNUMBER"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.Comparable(),
                                       checkOutDate:DateTime.Now.AddDays(-2).Comparable(),
                                       bookDate:DateTime.Now.AddDays(-2).Comparable(),
                                       CDP: 2152921M, program: HertzProgram.ThriftyBlueChip,
                                       RSDNCTRYCD: "US", HODIndicator: 0, qualifyingAmount: 80M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("TopGolf_2019_GPR2XBonus",80M) }
            }

        };
    }
}
