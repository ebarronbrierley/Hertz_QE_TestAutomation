using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brierley.TestAutomation.Core.Utilities;
using Hertz.API.Controllers;
using Hertz.API.DataModels;
using NUnit.Framework;

namespace Hertz.API.TestData.RealTimeBonuses
{
    public class VisaInfinite10RGBonusActivity
    {
        public const string PointEventName = "VisaInfinite10RGBonusActivity";
        public const decimal BaseTxnAmount = 100M;
        public const decimal PointEventAmount = BaseTxnAmount;
        public static readonly DateTime StartDate = DateTime.Parse("03/01/19 12:00:01 AM");
        public static readonly DateTime EndDate = DateTime.Parse("02/28/23 12:00:00 AM");

        public static readonly DateTime[] ValidChkOuts = { StartDate.Comparable(), StartDate.AddYears(1).Comparable(), StartDate.AddYears(3).Comparable(), EndDate.AddDays(-1).Comparable() };
        public static readonly DateTime[] ValidChkIns = { StartDate.AddDays(1).Comparable(), StartDate.AddYears(1).AddDays(1).Comparable(), StartDate.AddYears(3).AddDays(1).Comparable(), EndDate.Comparable() };

        public static readonly string[] ValidRSDNCTRYCDs = new string[] { "US", "BM", "CA", "MX", "AS", "GU", "MP", "PR", "VI",
                                                                          "BE", "FR", "DE", "IT", "LU", "NL", "ES", "CH", "GB", "IE", "SE", "NO", "DK", "FI", "PL", "PT",
                                                                          "AI", "AW", "BB", "BS", "VG", "KY","DM","DO","GD","GP","HT","JM","MQ","MS","AN","SM","KN","LC","VC","TT","TC",
                                                                          "AR","BO","BR","BZ","CL","CO","CR","EC","FK","GF","GT","GY","HN","NI","PA","PE","PY","SR","SV","UY","VE",
                                                                          "AU","NZ","PW","CN","SG","JP","KR" };
        public static readonly decimal[] ValidCDPs = new decimal[] { 2150933M };
        public static readonly IHertzProgram[] ValidPrograms = new IHertzProgram[] { HertzLoyalty.GoldPointsRewards };
        public static readonly IHertzTier[] ValidTiers = new IHertzTier[] { HertzLoyalty.GoldPointsRewards.RegularGold };
        public static TimeSpan ValidRentalLength = TimeSpan.FromDays(1);
        public static ExpectedPointEvent[] ExpectedPointEvents = new ExpectedPointEvent[] { new ExpectedPointEvent(PointEventName, BaseTxnAmount*0.1M) };


        public static IEnumerable PositiveScenarios
        {
            get
            {
                foreach (string validRSDNCTRY in ValidRSDNCTRYCDs)
                {
                    MemberModel member = MemberController.GenerateRandomMember(ValidTiers[0]);
                    member.MemberDetails.A_COUNTRY = validRSDNCTRY;
                    member.MemberDetails.A_CDPNUMBER = $"{ValidCDPs[0]}";

                    yield return new TestCaseData(
                        member,
                        new TxnHeaderModel[] {
                            TxnHeaderController.GenerateTransaction("", checkInDate: ValidChkIns[0],
                                        checkOutDate:ValidChkOuts[0],
                                        bookDate:ValidChkOuts[0],
                                        program: ValidPrograms[0], CDP: ValidCDPs[0],
                                        RSDNCTRYCD: validRSDNCTRY, HODIndicator: 0, rentalCharges: BaseTxnAmount)
                        },
                        ExpectedPointEvents,
                        new string[] { }
                    ).SetName($"{PointEventName}. RSDNCTRYCD = {validRSDNCTRY}, EarningPref={ValidPrograms[0].EarningPreference}, Tier={ValidTiers[0].Code}, CDP = {ValidCDPs[0]}, ChkOut = {ValidChkOuts[0].ToShortDateString()}, ChkIn = {ValidChkIns[0].ToShortDateString()}")
                     .SetCategory(BonusTestCategory.Regression)
                     .SetCategory(BonusTestCategory.Positive);
                }
                foreach (decimal validCDP in ValidCDPs)
                {
                    MemberModel member = MemberController.GenerateRandomMember(ValidTiers[0]);
                    member.MemberDetails.A_COUNTRY = ValidRSDNCTRYCDs[0];
                    member.MemberDetails.A_CDPNUMBER = $"{validCDP}";
                    yield return new TestCaseData(
                        member,
                        new TxnHeaderModel[] {
                            TxnHeaderController.GenerateTransaction("", checkInDate: ValidChkIns[0],
                                        checkOutDate:ValidChkOuts[0],
                                        bookDate:ValidChkOuts[0],
                                        program: ValidPrograms[0], CDP: validCDP,
                                        RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, rentalCharges: BaseTxnAmount)
                        },
                        ExpectedPointEvents,
                        new string[] { }
                    ).SetName($"{PointEventName}. CDP = {validCDP},  RSDNCTRYCD = {ValidRSDNCTRYCDs[0]}, EarningPref={ValidPrograms[0].EarningPreference}, Tier={ValidTiers[0].Code}, ChkOut = {ValidChkOuts[0].ToShortDateString()}, ChkIn = {ValidChkIns[0].ToShortDateString()}")
                     .SetCategory(BonusTestCategory.Regression)
                     .SetCategory(BonusTestCategory.Positive);
                }
                foreach (IHertzProgram validProgram in ValidPrograms)
                {
                    foreach (IHertzTier validTier in validProgram.Tiers)
                    {
                        if (!ValidTiers.ToList().Any(x => x.Name.Equals(validTier.Name))) continue;

                        MemberModel member = MemberController.GenerateRandomMember(validTier);
                        member.MemberDetails.A_COUNTRY = ValidRSDNCTRYCDs[0];
                        member.MemberDetails.A_CDPNUMBER = $"{ValidCDPs[0]}";

                        yield return new TestCaseData(
                            member,
                            new TxnHeaderModel[] {
                                TxnHeaderController.GenerateTransaction("", checkInDate: ValidChkIns[0],
                                checkOutDate:ValidChkOuts[0],
                                bookDate:ValidChkOuts[0],
                                program: validProgram, CDP: ValidCDPs[0],
                                RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, rentalCharges: BaseTxnAmount)
                            },
                            ExpectedPointEvents,
                            new string[] { }
                        ).SetName($"{PointEventName}. EarningPref={validProgram.EarningPreference}, Tier={validTier.Code}, CDP = {ValidCDPs[0]},  RSDNCTRYCODE = {ValidRSDNCTRYCDs[0]}, ChkOut = {ValidChkOuts[0].ToShortDateString()}, ChkIn = {ValidChkIns[0].ToShortDateString()}")
                         .SetCategory(BonusTestCategory.Regression)
                         .SetCategory(BonusTestCategory.Positive)
                         .SetCategory(BonusTestCategory.Smoke);
                    }
                }
                foreach(DateTime chkout in ValidChkOuts)
                {
                    MemberModel member = MemberController.GenerateRandomMember(ValidTiers[0]);
                    member.MemberDetails.A_COUNTRY = ValidRSDNCTRYCDs[0];
                    member.MemberDetails.A_CDPNUMBER = $"{ValidCDPs[0]}";

                    yield return new TestCaseData(
                        member,
                        new TxnHeaderModel[] {
                            TxnHeaderController.GenerateTransaction("", checkInDate: chkout.AddDays(1).Comparable(),
                                        checkOutDate:chkout,
                                        bookDate:chkout,
                                        program: ValidPrograms[0], CDP: ValidCDPs[0],
                                        RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, rentalCharges: BaseTxnAmount)
                        },
                        ExpectedPointEvents,
                        new string[] { }
                    ).SetName($"{PointEventName}. RSDNCTRYCD = {ValidRSDNCTRYCDs[0]}, EarningPref={ValidPrograms[0].EarningPreference}, Tier={ValidTiers[0].Code}, CDP = {ValidCDPs[0]}, ChkOut = {chkout.ToShortDateString()}, ChkIn = {chkout.AddDays(1).ToShortDateString()}")
                     .SetCategory(BonusTestCategory.Regression)
                     .SetCategory(BonusTestCategory.Positive);
                }
                foreach(DateTime chkin in ValidChkIns)
                {
                    MemberModel member = MemberController.GenerateRandomMember(ValidTiers[0]);
                    member.MemberDetails.A_COUNTRY = ValidRSDNCTRYCDs[0];
                    member.MemberDetails.A_CDPNUMBER = $"{ValidCDPs[0]}";

                    yield return new TestCaseData(
                        member,
                        new TxnHeaderModel[] {
                            TxnHeaderController.GenerateTransaction("", checkInDate: chkin,
                                        checkOutDate:chkin.AddDays(-1),
                                        bookDate:chkin.AddDays(-1),
                                        program: ValidPrograms[0], CDP: ValidCDPs[0],
                                        RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, rentalCharges: BaseTxnAmount)
                        },
                        ExpectedPointEvents,
                        new string[] { }
                    ).SetName($"{PointEventName}. RSDNCTRYCD = {ValidRSDNCTRYCDs[0]}, EarningPref={ValidPrograms[0].EarningPreference}, Tier={ValidTiers[0].Code}, CDP = {ValidCDPs[0]}, ChkOut = {chkin.AddDays(-1).ToShortDateString()}, ChkIn = {chkin.ToShortDateString()}")
                     .SetCategory(BonusTestCategory.Regression)
                     .SetCategory(BonusTestCategory.Positive);
                }
            }
        }

        public static IEnumerable NegativeScenarios
        {
            get
            {
                yield return new TestCaseData();
            }
        }
    }
}
