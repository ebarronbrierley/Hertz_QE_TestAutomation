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
    public class LapsedOnGoingActivity
    {
        public const string PointEventName = "LapsedOnGoingActivity";
        public const string RequiredPromotionCode = "GPRLapsedOnGoing";

        public const string ContractTypeCode = "COMM";
        public const decimal PointEventAmount = 800M;
        public const decimal BaseTxnAmount = 25M;
        //HTZ-13795 Removed NA,LAMC,EU RSNDCTRYCDs from the bonus
        public static readonly string[] ValidRSDNCTRYCDs = new string[] { "CN", "SG", "JP", "KR", "AU", "NZ", "PW" };
        public static readonly IHertzProgram[] ValidPrograms = new IHertzProgram[] { HertzLoyalty.GoldPointsRewards };
        public static readonly IHertzTier[] ValidTiers = new IHertzTier[] { HertzLoyalty.GoldPointsRewards.RegularGold, HertzLoyalty.GoldPointsRewards.FiveStar, HertzLoyalty.GoldPointsRewards.PresidentsCircle };
        public static TimeSpan ValidRentalLength = TimeSpan.FromDays(1);
        public static TimeSpan ValidTimeFromPromotion = TimeSpan.FromDays(90);
        public const string ChkOutLocId = "00004";

        public static IEnumerable PositiveScenarios
        {
            get
            {
                foreach (string rsdnCtryCd in ValidRSDNCTRYCDs)
                {
                    MemberModel member = MemberController.GenerateRandomMember(ValidTiers[0]);
                    member.MemberDetails.A_COUNTRY = rsdnCtryCd;

                    yield return new TestCaseData(
                        member,
                        new TxnHeaderModel[] {
                            TxnHeaderController.GenerateTransaction("", checkInDate: DateTime.Now.AddTicks(ValidTimeFromPromotion.Ticks).Comparable(),
                                        checkOutDate:DateTime.Now.AddTicks(ValidTimeFromPromotion.Ticks).Comparable(),
                                        bookDate:DateTime.Now.AddTicks(ValidTimeFromPromotion.Ticks).Comparable(),
                                        program: ValidPrograms[0],
                                        RSDNCTRYCD: rsdnCtryCd, HODIndicator: 0, rentalCharges: BaseTxnAmount,
                                        contractTypeCode: ContractTypeCode, checkoutWorldWideISO: rsdnCtryCd)
                        },
                        new ExpectedPointEvent[] { new ExpectedPointEvent(PointEventName, PointEventAmount) },
                        new string[] { RequiredPromotionCode }
                    ).SetName($"{PointEventName}. RSDNCTRYCODE = {rsdnCtryCd}, EarningPref={ValidPrograms[0].EarningPreference}, Tier={ValidTiers[0].Code}")
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
                        yield return new TestCaseData(
                            member,
                            new TxnHeaderModel[] {
                                    TxnHeaderController.GenerateTransaction("", checkInDate: DateTime.Now.AddDays(2).Comparable(),
                                    checkOutDate:DateTime.Now.AddDays(1).Comparable(),
                                    bookDate:DateTime.Now.AddDays(1).Comparable(),
                                    program: validProgram,
                                    RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, rentalCharges: BaseTxnAmount,
                                    contractTypeCode: ContractTypeCode, checkoutWorldWideISO: ValidRSDNCTRYCDs[0])
                            },
                             new ExpectedPointEvent[] { new ExpectedPointEvent(PointEventName, PointEventAmount) },
                            new string[] { "GPRLapsedOnGoing" }
                        ).SetName($"{PointEventName}. EarningPref={validProgram.EarningPreference}, Tier={validTier.Code}, RSDNCTRYCODE = {ValidRSDNCTRYCDs[0]}")
                         .SetCategory(BonusTestCategory.Regression)
                         .SetCategory(BonusTestCategory.Positive)
                         .SetCategory(BonusTestCategory.Smoke);


                        member = MemberController.GenerateRandomMember(validTier);
                        member.MemberDetails.A_COUNTRY = ValidRSDNCTRYCDs[0];
                        yield return new TestCaseData(
                           member,
                           new TxnHeaderModel[] {
                                    TxnHeaderController.GenerateTransaction("", checkInDate: DateTime.Now.AddDays(2).Comparable(),
                                    checkOutDate:DateTime.Now.AddDays(1).Comparable(),
                                    bookDate:DateTime.Now.AddDays(1).Comparable(),
                                    program: validProgram,
                                    RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, rentalCharges: BaseTxnAmount,
                                    contractTypeCode: ContractTypeCode, checkoutWorldWideISO: ValidRSDNCTRYCDs[0],chkoutlocnum:null,chkoutareanum:null,chkoutlocid: ChkOutLocId)
                           },
                            new ExpectedPointEvent[] { new ExpectedPointEvent(PointEventName, PointEventAmount) },
                           new string[] { "GPRLapsedOnGoing" }
                       ).SetName($"{PointEventName}. EarningPref={validProgram.EarningPreference}, Tier={validTier.Code}, RSDNCTRYCODE = {ValidRSDNCTRYCDs[0]},CHKOUTLOCID = {ChkOutLocId}")
                        .SetCategory(BonusTestCategory.Regression)
                        .SetCategory(BonusTestCategory.Positive)
                        .SetCategory(BonusTestCategory.Smoke);
                    }
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
