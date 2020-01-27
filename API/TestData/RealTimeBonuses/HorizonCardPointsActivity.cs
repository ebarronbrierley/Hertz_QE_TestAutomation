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
    public class HorizonCardPointsActivity
    {
        public static readonly decimal[] ValidCDPs = new decimal[] { 867695M };
        public static readonly string[] ValidRSDNCTRYCDs = new string[] { "FR"};
        public const string PointEventName = "HorizonCardPointsActivity";
        public const decimal PointEventAmount = 12000M;
        public const string ValidFTPTNRNUM = "ZE1";
        public static IHertzProgram[] ValidPrograms = new IHertzProgram[] { HertzLoyalty.GoldPointsRewards };
        public static IHertzTier[] ValidTiers = new IHertzTier[] { HertzLoyalty.GoldPointsRewards.RegularGold };
        public const decimal BaseTxnAmount = 25M;
        public static ExpectedPointEvent[] ExpectedPointEvents = new ExpectedPointEvent[] { new ExpectedPointEvent(PointEventName, PointEventAmount) };
        public static TimeSpan ValidRentalLength = TimeSpan.FromDays(1);
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
                        member.MemberDetails.A_CDPNUMBER = $"{ValidCDPs[0]}";
                        yield return new TestCaseData(
                            member,
                            new TxnHeaderModel[] {
                                TxnHeaderController.GenerateTransaction("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                checkOutDate:DateTime.Now.Comparable(),
                                bookDate:DateTime.Now.Comparable(),
                                program: validProgram.Set(validTier.Code,"SpecificTier"), CDP: ValidCDPs[0],
                                RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, rentalCharges: BaseTxnAmount)
                            },
                            ExpectedPointEvents,
                            new string[] { }
                        ).SetName($"{PointEventName}. EarningPref={validProgram.EarningPreference}, Tier={validTier.Code}, CDP = {ValidCDPs[0]},  RSDNCTRYCODE = {ValidRSDNCTRYCDs[0]}")
                         .SetCategory(BonusTestCategory.Regression)
                         .SetCategory(BonusTestCategory.Positive)
                         .SetCategory(BonusTestCategory.Smoke);
                    }
                }
                foreach (decimal validCDP in ValidCDPs)
                {
                    MemberModel member = MemberController.GenerateRandomMember(ValidTiers[0]);
                    member.MemberDetails.A_COUNTRY = ValidRSDNCTRYCDs[0];
                    member.MemberDetails.A_CDPNUMBER = $"{validCDP}";
                    yield return new TestCaseData(
                            member,
                            new TxnHeaderModel[] {
                                TxnHeaderController.GenerateTransaction("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                checkOutDate:DateTime.Now.Comparable(),
                                bookDate:DateTime.Now.Comparable(),
                                program: ValidPrograms[0].Set(ValidTiers[0].Code,"SpecificTier"), CDP: validCDP,
                                RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, rentalCharges: BaseTxnAmount)
                            },
                            ExpectedPointEvents,
                            new string[] { }
                        ).SetName($"{PointEventName}. CDP = {validCDP}, EarningPref={ValidPrograms[0].EarningPreference}, Tier={ValidTiers[0].Code}, RSDNCTRYCODE = {ValidRSDNCTRYCDs[0]}")
                         .SetCategory(BonusTestCategory.Regression)
                         .SetCategory(BonusTestCategory.Positive)
                         .SetCategory(BonusTestCategory.Smoke);



                    member = MemberController.GenerateRandomMember(ValidTiers[0]);
                    member.MemberDetails.A_COUNTRY = ValidRSDNCTRYCDs[0];
                    member.MemberDetails.A_CDPNUMBER = $"{validCDP}";
                    yield return new TestCaseData(
                           member,
                           new TxnHeaderModel[] {
                                TxnHeaderController.GenerateTransaction("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                checkOutDate:DateTime.Now.Comparable(),
                                bookDate:DateTime.Now.Comparable(),
                                program: ValidPrograms[0].Set(ValidTiers[0].Code,"SpecificTier"), CDP: validCDP,
                                RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, rentalCharges: BaseTxnAmount,chkoutlocnum:null,chkoutareanum:null,chkoutlocid:ChkOutLocId)
                           },
                           ExpectedPointEvents,
                           new string[] { }
                       ).SetName($"{PointEventName}. CDP = {validCDP}, CHKOUTLOCID = {ChkOutLocId},EarningPref ={ValidPrograms[0].EarningPreference}, Tier={ValidTiers[0].Code}, RSDNCTRYCODE = {ValidRSDNCTRYCDs[0]}")
                        .SetCategory(BonusTestCategory.Regression)
                        .SetCategory(BonusTestCategory.Positive)
                        .SetCategory(BonusTestCategory.Smoke);
                }
                foreach (string validRSDN in ValidRSDNCTRYCDs)
                {
                    MemberModel member = MemberController.GenerateRandomMember(ValidTiers[0]);
                    member.MemberDetails.A_COUNTRY = validRSDN;
                    member.MemberDetails.A_CDPNUMBER = $"{ValidCDPs[0]}";
                    yield return new TestCaseData(
                           member,
                            new TxnHeaderModel[] {
                                TxnHeaderController.GenerateTransaction("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                checkOutDate:DateTime.Now.Comparable(),
                                bookDate:DateTime.Now.Comparable(),
                                program: ValidPrograms[0].Set(ValidTiers[0].Code,"SpecificTier"), CDP: ValidCDPs[0],
                                RSDNCTRYCD: validRSDN, HODIndicator: 0, rentalCharges: BaseTxnAmount)
                            },
                            ExpectedPointEvents,
                            new string[] { }
                        ).SetName($"{PointEventName}. RSDNCTRYCODE = {validRSDN}, EarningPref={ValidPrograms[0].EarningPreference}, Tier={ValidTiers[0].Code}, CDP = {ValidCDPs[0]}")
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
