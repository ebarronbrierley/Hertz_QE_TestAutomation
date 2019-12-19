using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brierley.TestAutomation.Core.Utilities;
using HertzNetFramework.DataModels;
using NUnit.Framework;

namespace HertzNetFramework.Tests.BonusTestData
{
    public class OngoingEMEABirthdayActivity
    {
        public static readonly string[] ValidRSDNCTRYCDs = new string[] { "BE", "FR", "DE", "IT", "LU", "NL", "ES", "CH", "GB", "IE", "SE", "NO", "DK", "FI",
                                                                          "CN", "JP", "SG", "KR"};
        public const string PointEventName = "OngoingEMEABirthdayActivity";
        public const decimal PointEventAmount = 400M;
        public const string ValidFTPTNRNUM = "ZE1";
        public static IHertzProgram[] ValidPrograms = new IHertzProgram[] { HertzProgram.GoldPointsRewards };
        public static IHertzTier[] ValidTiers = new IHertzTier[] { GPR.Tier.RegularGold, GPR.Tier.FiveStar, GPR.Tier.PresidentsCircle, GPR.Tier.Platinum, GPR.Tier.PlatinumSelect, GPR.Tier.PlatinumVIP };
        public const decimal BaseTxnAmount = 25M;
        public static ExpectedPointEvent[] ExpectedPointEvents = new ExpectedPointEvent[] { new ExpectedPointEvent(PointEventName, PointEventAmount) };
        public static TimeSpan ValidRentalLength = TimeSpan.FromDays(1);
        public static TimeSpan ValidBookingDateFromEnroll = TimeSpan.FromDays(60);
        public static readonly DateTime StartDate = DateTime.Parse("01/01/2018");
        public static readonly DateTime EndDate = DateTime.Parse("01/01/2099");
        public const string ChkOutLocId = "00004";


        public static IEnumerable PositiveScenarios
        {
            get
            {
                foreach (IHertzProgram validProgram in ValidPrograms)
                {
                    foreach (IHertzTier validTier in validProgram.Tiers)
                    {
                        if (!ValidTiers.ToList().Any(x => x.Name.Equals(validTier.Name))) continue;

                        yield return new TestCaseData(
                            Member.GenerateRandom(MemberStyle.PreProjectOne, validProgram.Set(validTier.Code, "SpecificTier"))
                                                                                          .Set(ValidRSDNCTRYCDs[0], "MemberDetails.A_COUNTRY")
                            ,
                            new TxnHeader[] {
                                TxnHeader.Generate("", checkInDate: DateTime.Now.AddTicks(ValidBookingDateFromEnroll.Ticks).AddTicks(ValidRentalLength.Ticks).Comparable(),
                                checkOutDate:DateTime.Now.AddTicks(ValidBookingDateFromEnroll.Ticks).Comparable(),
                                bookDate:DateTime.Now.AddTicks(ValidBookingDateFromEnroll.Ticks).Comparable(),
                                program: validProgram.Set(validTier.Code,"SpecificTier"),
                                RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, rentalCharges: BaseTxnAmount)
                            },
                            ExpectedPointEvents,
                            new string[] { "EMEA60DayBirthdayEM" }
                        ).SetName($"{PointEventName}. EarningPref={validProgram.EarningPreference}, Tier={validTier.Code}, RSDNCTRYCODE = {ValidRSDNCTRYCDs[0]}")
                         .SetCategory(BonusTestCategory.Regression)
                         .SetCategory(BonusTestCategory.Positive)
                         .SetCategory(BonusTestCategory.Smoke);
                        yield return new TestCaseData(
                          Member.GenerateRandom(MemberStyle.PreProjectOne, validProgram.Set(validTier.Code, "SpecificTier"))
                                                                                        .Set(ValidRSDNCTRYCDs[0], "MemberDetails.A_COUNTRY")
                          ,
                          new TxnHeader[] {
                                TxnHeader.Generate("", checkInDate: DateTime.Now.AddTicks(ValidBookingDateFromEnroll.Ticks).AddTicks(ValidRentalLength.Ticks).Comparable(),
                                checkOutDate:DateTime.Now.AddTicks(ValidBookingDateFromEnroll.Ticks).Comparable(),
                                bookDate:DateTime.Now.AddTicks(ValidBookingDateFromEnroll.Ticks).Comparable(),
                                program: validProgram.Set(validTier.Code,"SpecificTier"),
                                RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, rentalCharges: BaseTxnAmount,chkoutlocnum:null,chkoutareanum:null,chkoutlocid: ChkOutLocId)
                          },
                          ExpectedPointEvents,
                          new string[] { "EMEA60DayBirthdayEM" }
                      ).SetName($"{PointEventName}. EarningPref={validProgram.EarningPreference}, ChkOutLocId ={ChkOutLocId}, Tier={validTier.Code}, RSDNCTRYCODE = {ValidRSDNCTRYCDs[0]}")
                       .SetCategory(BonusTestCategory.Regression)
                       .SetCategory(BonusTestCategory.Positive)
                       .SetCategory(BonusTestCategory.Smoke);
                    }
                }
                foreach (string validRSDN in ValidRSDNCTRYCDs)
                {
                    yield return new TestCaseData(
                            Member.GenerateRandom(MemberStyle.PreProjectOne, ValidPrograms[0].Set(ValidTiers[0].Code, "SpecificTier"))
                                                                                          .Set(validRSDN, "MemberDetails.A_COUNTRY")
                            ,
                            new TxnHeader[] {
                                TxnHeader.Generate("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                checkOutDate:DateTime.Now.Comparable(),
                                bookDate:DateTime.Now.Comparable(),
                                program: ValidPrograms[0].Set(ValidTiers[0].Code,"SpecificTier"),
                                RSDNCTRYCD: validRSDN, HODIndicator: 0, rentalCharges: BaseTxnAmount)
                            },
                            ExpectedPointEvents,
                            new string[] { "EMEA60DayBirthdayEM" }
                        ).SetName($"{PointEventName}. RSDNCTRYCODE = {validRSDN}, EarningPref={ValidPrograms[0].EarningPreference}, Tier={ValidTiers[0].Code}")
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
