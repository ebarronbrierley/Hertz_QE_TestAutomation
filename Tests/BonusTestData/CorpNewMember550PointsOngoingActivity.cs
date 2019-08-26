using System;
using Brierley.TestAutomation.Core.Utilities;
using HertzNetFramework.DataModels;

namespace HertzNetFramework.Tests.BonusTestData
{
    public class CorpNewMember550PointsOngoingActivity
    {
        public static readonly string AccenturePC = "1898";
        public static readonly string EYPC = "12091";
        public static readonly string PwCPC = "1882";
        public static readonly string KPMGPC = "1627";
        public static readonly string BPPC = "6730";

        static object[] PositiveScenarios =
        {
            new object[]
            {
                "CorpNewMember550PointsOngoingActivity [GPR Regular Gold]  CDP = 252294, MemberDetails.A_COUNTRY = FR",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier"))
                                                                                            .Set(252294M,"MemberDetails.A_CDPNUMBER")
                                                                                            .Set("FR","MemberDetails.A_COUNTRY")
                                                                                            .Set(BPPC, "A_ACQUISITIONMETHODTYPECODE"),

                TxnHeader.Generate("", checkInDate: DateTime.Now.AddDays(2).Comparable(),
                                        checkOutDate:DateTime.Now.AddDays(1).Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier"),
                                        RSDNCTRYCD: "FR", HODIndicator: 0, qualifyingAmount: 25M, 
                                        contractTypeCode: "COMM", contractNumber: 83225M ,sacCode: "N"),
                new ExpectedPointEvent[] { new ExpectedPointEvent("GPRGoldRental", 25M),
                                            new ExpectedPointEvent("CorpNewMember550PointsOngoingActivity",550M) },
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
