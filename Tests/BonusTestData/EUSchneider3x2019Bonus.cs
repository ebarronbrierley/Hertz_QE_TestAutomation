using System;
using Brierley.TestAutomation.Core.Utilities;
using HertzNetFramework.DataModels;

namespace HertzNetFramework.Tests.BonusTestData
{
    public class EUSchneider3x2019Bonus
    {
       static object[] PositiveScenarios =
       {
            new object[]
            {
                "GPR Five Star EUSchneider3x2019BonusActivity CDP = 830063, Residence = BE, Check Out Country = BE , Booking Date 8 days before Checkout",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.FiveStar.Code,"SpecificTier")).Set(830063M,"MemberDetails.A_CDPNUMBER"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.Comparable(),
                                       checkOutDate:DateTime.Now.AddDays(-7).Comparable(),
                                       bookDate:DateTime.Now.AddDays(-15).Comparable(),
                                       CDP: 830063M, program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.FiveStar.Code,"SpecificTier"),
                                       RSDNCTRYCD: "BE", HODIndicator: 0, qualifyingAmount: 80M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("GPRGoldRental", 80M),
                                           new ExpectedPointEvent("GPRTierBonus_FS",80M*GPR.Tier.FiveStar.EarningRateModifier),
                                           new ExpectedPointEvent("EUSchneider3x2019BonusActivity",80M*2) },
                new string[] { }
            },
            new object[]
            {
                "GPR Regular Gold EUSchneider3x2019BonusActivity CDP = 830063, Residence = BE, Check Out Country = BE , Booking Date 8 days before Checkout",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier")).Set(830063M,"MemberDetails.A_CDPNUMBER"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.Comparable(),
                                       checkOutDate:DateTime.Now.AddDays(-7).Comparable(),
                                       bookDate:DateTime.Now.AddDays(-15).Comparable(),
                                       CDP: 830063M, program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier"),
                                       RSDNCTRYCD: "BE", HODIndicator: 0, qualifyingAmount: 80M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("GPRGoldRental", 80M),
                                           new ExpectedPointEvent("EUSchneider3x2019BonusActivity",80M*2) },
                new string[] { }
            },
            new object[]
            {
                "GPR Presidents Circle EUSchneider3x2019BonusActivity CDP = 830063, Residence = BE, Check Out Country = BE , Booking Date 8 days before Checkout",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.PresidentsCircle.Code,"SpecificTier")).Set(830063M,"MemberDetails.A_CDPNUMBER"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.Comparable(),
                                       checkOutDate:DateTime.Now.AddDays(-7).Comparable(),
                                       bookDate:DateTime.Now.AddDays(-15).Comparable(),
                                       CDP: 830063M, program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.PresidentsCircle.Code,"SpecificTier"),
                                       RSDNCTRYCD: "BE", HODIndicator: 0, qualifyingAmount: 80M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("GPRGoldRental", 80M),
                                           new ExpectedPointEvent("GPRTierBonus_PC_PL",80M*GPR.Tier.PresidentsCircle.EarningRateModifier),
                                           new ExpectedPointEvent("EUSchneider3x2019BonusActivity",80M*2) },
                new string[] { }
            }
        };
    }
}
