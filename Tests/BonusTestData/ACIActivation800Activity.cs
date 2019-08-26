using System;
using Brierley.TestAutomation.Core.Utilities;
using HertzNetFramework.DataModels;

namespace HertzNetFramework.Tests.BonusTestData
{
    public class ACIActivation800Activity
    {
        static object[] PositiveScenarios =
        {
            new object[]
            {
                "ACIActivation800Activity [GPR Regular Gold]  CDP = 664924, MemberDetails.A_COUNTRY = US",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier"))
                                                                                            .Set(664924M,"MemberDetails.A_CDPNUMBER")
                                                                                            .Set("US","MemberDetails.A_COUNTRY"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.AddDays(2).Comparable(),
                                        checkOutDate:DateTime.Now.AddDays(1).Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier"),
                                        RSDNCTRYCD: "US", HODIndicator: 0, qualifyingAmount: 25M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("GPRGoldRental", 25M),
                                            new ExpectedPointEvent("ACIActivation800Activity",800M) },
                new string[]{ }
            }
        };
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
    }
}
