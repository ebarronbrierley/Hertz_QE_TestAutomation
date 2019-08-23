using System;
using Brierley.TestAutomation.Core.Utilities;
using HertzNetFramework.DataModels;


namespace HertzNetFramework.Tests.BonusTestData
{
    public class OngoingEMEABirthdayActivity
    {
        static object[] PositiveScenarios =
        {
            new object[]
            {
                "OngoingEMEABirthday [GPR Regular Gold]  CDP = 2150933, Residence = BE, Check Out Country = BE",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier")).Set(2150933M,"MemberDetails.A_CDPNUMBER"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.Comparable(),
                                        checkOutDate:DateTime.Now.AddDays(-2).Comparable(),
                                        bookDate:DateTime.Now.AddDays(-2).Comparable(),
                                        CDP: 2150933M, program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier"),
                                        RSDNCTRYCD: "BE", HODIndicator: 0, qualifyingAmount: 25M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("GPRGoldRental", 25M),
                                            new ExpectedPointEvent("OngoingEMEABirthdayActivity",400M) }
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
                new ExpectedPointEvent[] { new ExpectedPointEvent("VisaInfinite10RGBonusActivity", 80*0.1M) }
            }
        };
    }
}
