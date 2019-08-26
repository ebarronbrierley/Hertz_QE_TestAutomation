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
            },
            new object[]
            {
                "OngoingEMEABirthday [GPR Presidents Circle]  CDP = 2150933, Residence = CN, Check Out Country = CN",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.PresidentsCircle.Code,"SpecificTier")),
                TxnHeader.Generate("", checkInDate: DateTime.Now.AddDays(2).Comparable(),
                                        checkOutDate:DateTime.Now.AddDays(1).Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.PresidentsCircle.Code,"SpecificTier"),
                                        RSDNCTRYCD: "CN", HODIndicator: 0, qualifyingAmount: 25M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("GPRGoldRental", 25M),
                                            new ExpectedPointEvent("OngoingEMEABirthdayActivity",400M) },
                new string[]{ "EMEA60DayBirthdayEM" }
            },
            new object[]
            {
                "OngoingEMEABirthday [GPR Platinum]  CDP = 2150933, Residence = FR, Check Out Country = FR",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.Platinum.Code,"SpecificTier")),
                TxnHeader.Generate("", checkInDate: DateTime.Now.AddDays(2).Comparable(),
                                        checkOutDate:DateTime.Now.AddDays(1).Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.Platinum.Code,"SpecificTier"),
                                        RSDNCTRYCD: "FR", HODIndicator: 0, qualifyingAmount: 25M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("GPRGoldRental", 25M),
                                            new ExpectedPointEvent("OngoingEMEABirthdayActivity",400M) },
                new string[]{ "EMEA60DayBirthdayEM" }
            },
            new object[]
            {
                "OngoingEMEABirthday [GPR Platinum Select]  CDP = 2150933, Residence = DK, Check Out Country = DK",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.PlatinumSelect.Code,"SpecificTier")),
                TxnHeader.Generate("", checkInDate: DateTime.Now.AddDays(59).Comparable(),
                                        checkOutDate:DateTime.Now.AddDays(58).Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.PlatinumSelect.Code,"SpecificTier"),
                                        RSDNCTRYCD: "BE", HODIndicator: 0, qualifyingAmount: 25M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("GPRGoldRental", 25M),
                                            new ExpectedPointEvent("OngoingEMEABirthdayActivity",400M) },
                new string[]{ "EMEA60DayBirthdayEM" }
            },
            new object[]
            {
                "OngoingEMEABirthday [GPR Platinum VIP]  CDP = 2150933, Residence = JP, Check Out Country = JP",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.PlatinumVIP.Code,"SpecificTier")),
                TxnHeader.Generate("", checkInDate: DateTime.Now.AddDays(21).Comparable(),
                                        checkOutDate:DateTime.Now.AddDays(20).Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.PlatinumVIP.Code,"SpecificTier"),
                                        RSDNCTRYCD: "JP", HODIndicator: 0, qualifyingAmount: 25M),
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
                                        RSDNCTRYCD: "US", HODIndicator: 0, qualifyingAmount: 25M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("OngoingEMEABirthdayActivity", 400) },
                new string[] { }
            },
            new object[]
            {
                "OngoingEMEABirthday [GPR Platinum VIP] 61 Days after promo applied,  CDP = 2150933, Residence = JP, Check Out Country = JP",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.PlatinumVIP.Code,"SpecificTier")),
                TxnHeader.Generate("", checkInDate: DateTime.Now.AddDays(62).Comparable(),
                                        checkOutDate:DateTime.Now.AddDays(61).Comparable(),
                                        bookDate:DateTime.Now.AddDays(61).Comparable(),
                                        program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.PlatinumVIP.Code,"SpecificTier"),
                                        RSDNCTRYCD: "JP", HODIndicator: 0, qualifyingAmount: 25M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("OngoingEMEABirthdayActivity", 400M) },
                new string[]{ "EMEA60DayBirthdayEM" }
            },
            new object[]
            {
                "OngoingEMEABirthday [GPR Platinum VIP] CDP = 2150933, Invalid RSDNCTRYCD = US",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.PlatinumVIP.Code,"SpecificTier")),
                TxnHeader.Generate("", checkInDate: DateTime.Now.AddDays(2).Comparable(),
                                        checkOutDate:DateTime.Now.AddDays(1).Comparable(),
                                        bookDate:DateTime.Now.AddDays(1).Comparable(),
                                        program: HertzProgram.GoldPointsRewards.Set(GPR.Tier.PlatinumVIP.Code,"SpecificTier"),
                                        RSDNCTRYCD: "US", HODIndicator: 0, qualifyingAmount: 25M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("OngoingEMEABirthdayActivity", 400M) },
                new string[]{ "EMEA60DayBirthdayEM" }
            },
            new object[]
            {
                "OngoingEMEABirthday Invalid Earning Preference [Dollar Express DX] CDP = 2150933, Residence = JP, Check Out Country = JP",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.DollarExpressRenters),
                TxnHeader.Generate("", checkInDate: DateTime.Now.AddDays(2).Comparable(),
                                        checkOutDate:DateTime.Now.AddDays(1).Comparable(),
                                        bookDate:DateTime.Now.AddDays(1).Comparable(),
                                        program: HertzProgram.DollarExpressRenters,
                                        RSDNCTRYCD: "JP", HODIndicator: 0, qualifyingAmount: 25M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("OngoingEMEABirthdayActivity", 400M) },
                new string[]{ "EMEA60DayBirthdayEM" }
            },
            new object[]
            {
                "OngoingEMEABirthday Invalid Earning Preference [Thrifty Blue Chip BC] CDP = 2150933, Residence = JP, Check Out Country = JP",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.ThriftyBlueChip),
                TxnHeader.Generate("", checkInDate: DateTime.Now.AddDays(2).Comparable(),
                                        checkOutDate:DateTime.Now.AddDays(1).Comparable(),
                                        bookDate:DateTime.Now.AddDays(1).Comparable(),
                                        program: HertzProgram.ThriftyBlueChip,
                                        RSDNCTRYCD: "JP", HODIndicator: 0, qualifyingAmount: 25M),
                new ExpectedPointEvent[] { new ExpectedPointEvent("OngoingEMEABirthdayActivity", 400M) },
                new string[]{ "EMEA60DayBirthdayEM" }
            }
        };
    }
}
