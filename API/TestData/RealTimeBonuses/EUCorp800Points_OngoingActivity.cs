using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brierley.TestAutomation.Core.Utilities;
using Hertz.API.DataModels;
using Hertz.API.Controllers;
using NUnit.Framework;

namespace Hertz.API.TestData.RealTimeBonuses
{
    public class EUCorp800Points_OngoingActivity
    {
        public static readonly DateTime StartDate = DateTime.Parse("01/01/2017");
        public static readonly DateTime EndDate = DateTime.Parse("08/01/2099");

        public const string PointEventName = "EUCorp800Points_OngoingActivity";
        public const string ContractTypeCode = "COMM";
        public const decimal PointEventAmount = 800M;
        public const decimal BaseTxnAmount = 25M;
        public static readonly decimal[] ValidCDPs = new decimal[] { 841369, 843852, 843853, 843854, 843855, 843859, 843860 };
        public static readonly string[] ValidRSDNCTRYCDs = new string[] { "BE", "FR", "DE", "IT", "LU", "NL", "ES", "CH", "GB", "IE", "SE", "NO", "DK", "FI", 
                                                                          "AU","NZ","PW","CN","SG","JP","KR" };
        public static readonly string[] ValidCHKWORLDWIDECTRYCDs = new string[] {"US", "CA", "PR", "VI",
                                                                                 "BE", "FR", "DE", "IT", "LU", "NL","ES","CH","GB","IE","SE","NO", "DK","FI", 
                                                                                 "BR",
                                                                                 "AU","NZ", "PW"};

        public static readonly IHertzProgram[] ValidPrograms = new IHertzProgram[] { HertzLoyalty.GoldPointsRewards };
        public static readonly IHertzTier[] ValidTiers = new IHertzTier[] { HertzLoyalty.GoldPointsRewards.RegularGold, HertzLoyalty.GoldPointsRewards.FiveStar, HertzLoyalty.GoldPointsRewards.PresidentsCircle };
        public static TimeSpan ValidRentalLength = TimeSpan.FromDays(2);
        public static ExpectedPointEvent[] ExpectedPointEvents = new ExpectedPointEvent[] { new ExpectedPointEvent(PointEventName, PointEventAmount) };
        public const string ChkOutLocId = "00004";

        public static IEnumerable PositiveScenarios
        {
            get
            {
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
                                        program: ValidPrograms[0],
                                        RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, rentalCharges: BaseTxnAmount,
                                        contractTypeCode: ContractTypeCode, CDP: validCDP, checkoutWorldWideISO: ValidCHKWORLDWIDECTRYCDs[0])
                        },
                        ExpectedPointEvents,
                        new string[] { }
                    ).SetName($"{PointEventName}. CDP = {validCDP}, EarningPref={ValidPrograms[0].EarningPreference}, Tier={ValidTiers[0].Code}, RSDNCTRYCODE = {ValidRSDNCTRYCDs[0]}, CHKOUTWORLDWIDERGNCTRYISO = {ValidCHKWORLDWIDECTRYCDs[0]}")
                     .SetCategory(BonusTestCategory.Regression)
                     .SetCategory(BonusTestCategory.Positive)
                     .SetCategory(BonusTestCategory.Smoke);
                }
                foreach (string rsdnCtryCode in ValidRSDNCTRYCDs)
                {
                    MemberModel member = MemberController.GenerateRandomMember(ValidTiers[0]);
                    member.MemberDetails.A_COUNTRY = rsdnCtryCode;
                    member.MemberDetails.A_CDPNUMBER = $"{ValidCDPs[0]}";

                    yield return new TestCaseData(
                        member,
                        new TxnHeaderModel[] {
                            TxnHeaderController.GenerateTransaction("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                        checkOutDate:DateTime.Now.Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: ValidPrograms[0],
                                        RSDNCTRYCD: rsdnCtryCode, HODIndicator: 0, rentalCharges: BaseTxnAmount, sacCode: "Y",
                                        contractTypeCode: ContractTypeCode, CDP: ValidCDPs[0], checkoutWorldWideISO: ValidCHKWORLDWIDECTRYCDs[0])
                        },
                        ExpectedPointEvents,
                        new string[] { }
                    ).SetName($"{PointEventName}. EarningPref={ValidPrograms[0].EarningPreference}, Tier={ValidTiers[0].Code}, CDP = {ValidCDPs[0]}, RSDNCTRYCODE = {rsdnCtryCode}, WWISOCTRYCODE = {ValidCHKWORLDWIDECTRYCDs[0]}")
                     .SetCategory(BonusTestCategory.Regression)
                     .SetCategory(BonusTestCategory.Positive);
                }
                foreach (string wwCheckout in ValidCHKWORLDWIDECTRYCDs)
                {
                    MemberModel member = MemberController.GenerateRandomMember(ValidTiers[0]);
                    member.MemberDetails.A_COUNTRY = ValidRSDNCTRYCDs[0];
                    member.MemberDetails.A_CDPNUMBER = $"{ValidCDPs[0]}";

                    yield return new TestCaseData(
                        member,
                        new TxnHeaderModel[] {
                            TxnHeaderController.GenerateTransaction("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                        checkOutDate:DateTime.Now.Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: ValidPrograms[0],
                                        RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, rentalCharges: BaseTxnAmount, sacCode: "Y",
                                        contractTypeCode: ContractTypeCode, CDP: ValidCDPs[0], checkoutWorldWideISO: wwCheckout)
                        },
                        ExpectedPointEvents,
                        new string[] { }
                    ).SetName($"{PointEventName}. CHKOUTWORLDWIDERGNCTRYISO = {wwCheckout}, EarningPref={ValidPrograms[0].EarningPreference}, Tier={ValidTiers[0].Code}, CDP = {ValidCDPs[0]}, RSDNCTRYCODE = {ValidRSDNCTRYCDs[1]}")
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
                                    TxnHeaderController.GenerateTransaction("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                    checkOutDate:DateTime.Now.Comparable(),
                                    bookDate:DateTime.Now.Comparable(),
                                    program: validProgram,
                                    RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, rentalCharges: BaseTxnAmount, sacCode: "Y",
                                    contractTypeCode: ContractTypeCode, CDP:ValidCDPs[0], checkoutWorldWideISO: ValidCHKWORLDWIDECTRYCDs[0])
                            },
                            ExpectedPointEvents,
                            new string[] { }
                        ).SetName($"{PointEventName}. EarningPref={validProgram.EarningPreference}, Tier={validTier.Code}, CDP = {ValidCDPs[0]}, RSDNCTRYCODE = {ValidRSDNCTRYCDs[0]}, CHKOUTWORLDWIDERGNCTRYISO = {ValidCHKWORLDWIDECTRYCDs[0]}")
                         .SetCategory(BonusTestCategory.Regression)
                         .SetCategory(BonusTestCategory.Positive)
                         .SetCategory(BonusTestCategory.Smoke);


                        member = MemberController.GenerateRandomMember(validTier);
                        member.MemberDetails.A_COUNTRY = ValidRSDNCTRYCDs[0];
                        member.MemberDetails.A_CDPNUMBER = $"{ValidCDPs[0]}";
                        yield return new TestCaseData(
                        member,
                        new TxnHeaderModel[] {
                                    TxnHeaderController.GenerateTransaction("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                    checkOutDate:DateTime.Now.Comparable(),
                                    bookDate:DateTime.Now.Comparable(),
                                    program: validProgram,
                                    RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, rentalCharges: BaseTxnAmount, sacCode: "Y",
                                    contractTypeCode: ContractTypeCode, CDP:ValidCDPs[0], checkoutWorldWideISO: ValidCHKWORLDWIDECTRYCDs[0],chkoutlocnum: null,chkoutareanum: null,chkoutlocid: ChkOutLocId)
                        },
                        ExpectedPointEvents,
                        new string[] { }
                   ).SetName($"{PointEventName}. EarningPref={validProgram.EarningPreference}, ChkOutLocId={ChkOutLocId}, Tier ={validTier.Code}, CDP = {ValidCDPs[0]}, RSDNCTRYCODE = {ValidRSDNCTRYCDs[0]}, CHKOUTWORLDWIDERGNCTRYISO = {ValidCHKWORLDWIDECTRYCDs[0]}")
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
