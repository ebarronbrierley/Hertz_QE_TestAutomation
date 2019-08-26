using System;
using Brierley.TestAutomation.Core.Utilities;
using HertzNetFramework.DataModels;

namespace HertzNetFramework.Tests.BonusTestData
{
    public class HorizonCardPointsActivity
    {
        static object[] PositiveScenarios =
        {
            new object[]
            {
                "HorizonCardPointsActivity [GPR Regular Gold]  CDP = 867695, MemberDetails.A_COUNTRY = FR",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier"))
                                                                                            .Set(867695M,"MemberDetails.A_CDPNUMBER")
                                                                                            .Set("FR","MemberDetails.A_COUNTRY"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.AddDays(2).Comparable(),
                                        checkOutDate:DateTime.Now.AddDays(1).Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier"),
                                        RSDNCTRYCD: "FR", HODIndicator: 0, qualifyingAmount: 25M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("GPRGoldRental", 25M),
                                            new ExpectedPointEvent("HorizonCardPointsActivity",12000M) },
                new string[]{ }
            },
            new object[]
            {
                "HorizonCardPointsActivity [GPR Regular Gold]  CDP = 867695, MemberDetails.A_COUNTRY = FR, Enroll Date = 01/01/2018",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier"))
                                                                                            .Set(867695M,"MemberDetails.A_CDPNUMBER")
                                                                                            .Set("FR","MemberDetails.A_COUNTRY")
                                                                                            .Set(DateTime.Parse("01/01/2018"),"MemberDetails.A_ENROLLDATE"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.AddDays(2).Comparable(),
                                        checkOutDate:DateTime.Now.AddDays(1).Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier"),
                                        RSDNCTRYCD: "FR", HODIndicator: 0, qualifyingAmount: 25M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("GPRGoldRental", 25M),
                                            new ExpectedPointEvent("HorizonCardPointsActivity",12000M) },
                new string[]{ }
            }
        };

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
