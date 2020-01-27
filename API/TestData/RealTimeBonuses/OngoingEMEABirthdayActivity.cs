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
    public class OngoingEMEABirthdayActivity
    {
        public static readonly string[] ValidRSDNCTRYCDs = new string[] { "BE", "FR", "DE", "IT", "LU", "NL", "ES", "CH", "GB", "IE", "SE", "NO", "DK", "FI",
                                                                          "CN", "JP", "SG", "KR"};
        public const string PointEventName = "OngoingEMEABirthdayActivity";
        public const decimal PointEventAmount = 400M;
        public const string ValidFTPTNRNUM = "ZE1";
        public static IHertzProgram[] ValidPrograms = new IHertzProgram[] { HertzLoyalty.GoldPointsRewards };
        public static IHertzTier[] ValidTiers = new IHertzTier[] { HertzLoyalty.GoldPointsRewards.RegularGold, HertzLoyalty.GoldPointsRewards.FiveStar, HertzLoyalty.GoldPointsRewards.PresidentsCircle, HertzLoyalty.GoldPointsRewards.Platinum, HertzLoyalty.GoldPointsRewards.PlatinumSelect, HertzLoyalty.GoldPointsRewards.PlatinumVIP };
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

                        MemberModel member = MemberController.GenerateRandomMember(validTier);
                        member.MemberDetails.A_COUNTRY = ValidRSDNCTRYCDs[0];

                        yield return new TestCaseData(
                            member,
                            new TxnHeaderModel[] {
                                TxnHeaderController.GenerateTransaction("", checkInDate: DateTime.Now.AddTicks(ValidBookingDateFromEnroll.Ticks).AddTicks(ValidRentalLength.Ticks).Comparable(),
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

                        member = MemberController.GenerateRandomMember(validTier);
                        member.MemberDetails.A_COUNTRY = ValidRSDNCTRYCDs[0];
                        yield return new TestCaseData(
                          member,
                          new TxnHeaderModel[] {
                                TxnHeaderController.GenerateTransaction("", checkInDate: DateTime.Now.AddTicks(ValidBookingDateFromEnroll.Ticks).AddTicks(ValidRentalLength.Ticks).Comparable(),
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
                    MemberModel member = MemberController.GenerateRandomMember(ValidTiers[0]);
                    member.MemberDetails.A_COUNTRY = validRSDN;
                    yield return new TestCaseData(
                            member,
                            new TxnHeaderModel[] {
                                TxnHeaderController.GenerateTransaction("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
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
