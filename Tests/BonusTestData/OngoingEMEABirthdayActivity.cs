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
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier")),
                TxnHeader.Generate("", checkInDate: DateTime.Now.AddDays(2).Comparable(),
                                        checkOutDate:DateTime.Now.AddDays(1).Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier"),
                                        RSDNCTRYCD: "BE", HODIndicator: 0, qualifyingAmount: 25M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("GPRGoldRental", 25M),
                                            new ExpectedPointEvent("OngoingEMEABirthdayActivity",400M) },
                new string[]{ "EMEA60DayBirthdayEM" }
            }
        };

        static object[] NegativeScenarios =
        {
            new object[]
            {
                "OngoingEMEABirthday [GPR Platinum] EMEA60DayBirthdayEM Promotion Code not added to member",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.Platinum.Code,"SpecificTier")).Set(2150933M,"MemberDetails.A_CDPNUMBER"),
                TxnHeader.Generate("", checkInDate: DateTime.Now.Comparable(),
                                        checkOutDate:DateTime.Now.AddDays(-2).Comparable(),
                                        bookDate:DateTime.Now.AddDays(-2).Comparable(),
                                        CDP: 2150933M, program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.Platinum.Code,"SpecificTier"),
                                        RSDNCTRYCD: "US", HODIndicator: 0, qualifyingAmount: 80M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("VisaInfinite10RGBonusActivity", 80*0.1M) },
                new string[] { }
            }
        };
    }
}
