using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brierley.TestAutomation.Core.Utilities;
using HertzNetFramework.DataModels;
using NUnit.Framework;

namespace HertzNetFramework.Tests.BonusTestData
{
    public class EUCorp800Points_OngoingActivity
    {
        public static readonly DateTime StartDate = DateTime.Parse("01/01/2017");
        public static readonly DateTime EndDate = DateTime.Parse("08/01/2099");

        public const string PointEventName = "EUCorp800Points_OngoingActivity";
        public const string ContractTypeCode = "COMM";
        public const decimal PointEventAmount = 800M;
        public const decimal BaseTxnAmount = 25M;
        public static readonly decimal[] ValidCDPs = new decimal[] { 841369M, 843852M, 843853M, 843854M, 843855M, 843859M, 843860M };
        public static readonly string[] ValidRSDNCTRYCDs = new string[] { "BE", "FR", "DE", "IT", "LU", "NL", "ES", "CH", "GB", "IE", "SE", "NO", "DK", "FI", 
                                                                          "AU","NZ","PW","CN","SG","JP","KR" };
        public static readonly string[] ValidCHKWORLDWIDECTRYCDs = new string[] {"US", "CA", "PR", "VI",
                                                                                 "BE", "FR", "DE", "IT", "LU", "NL","ES","CH","GB","IE","SE","NO", "DK","FI", 
                                                                                 "BR",
                                                                                 "AU","NZ", "PW"};

        public static readonly IHertzProgram[] ValidPrograms = new IHertzProgram[] { HertzProgram.GoldPointsRewards };
        public static readonly IHertzTier[] ValidTiers = new IHertzTier[] { GPR.Tier.RegularGold, GPR.Tier.FiveStar, GPR.Tier.PresidentsCircle };
        public static TimeSpan ValidRentalLength = TimeSpan.FromDays(2);
        public static ExpectedPointEvent[] ExpectedPointEvents = new ExpectedPointEvent[] { new ExpectedPointEvent(PointEventName, PointEventAmount) };
        public const string ChkOutLocId = "00004";

        public static IEnumerable PositiveScenarios
        {
            get
            {
                foreach (decimal validCDP in ValidCDPs)
                {
                    yield return new TestCaseData(
                        Member.GenerateRandom(MemberStyle.PreProjectOne, ValidPrograms[0].Set(ValidTiers[0].Code, "SpecificTier"))
                                                                                     .Set(ValidRSDNCTRYCDs[0], "MemberDetails.A_COUNTRY")
                                                                                     .Set(validCDP, "MemberDetails.A_CDPNUMBER")
                        ,
                        new TxnHeader[] {
                            TxnHeader.Generate("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                        checkOutDate:DateTime.Now.Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: ValidPrograms[0].Set(ValidTiers[0].Code,"SpecificTier"),
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
                    yield return new TestCaseData(
                        Member.GenerateRandom(MemberStyle.PreProjectOne, ValidPrograms[0].Set(ValidTiers[0].Code, "SpecificTier"))
                                                                                     .Set(rsdnCtryCode, "MemberDetails.A_COUNTRY")
                                                                                     .Set(ValidCDPs[0], "MemberDetails.A_CDPNUMBER")
                        ,
                        new TxnHeader[] {
                            TxnHeader.Generate("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                        checkOutDate:DateTime.Now.Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: ValidPrograms[0].Set(ValidTiers[0].Code,"SpecificTier"),
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
                    yield return new TestCaseData(
                        Member.GenerateRandom(MemberStyle.PreProjectOne, ValidPrograms[0].Set(ValidTiers[0].Code, "SpecificTier"))
                                                                                     .Set(ValidRSDNCTRYCDs[0], "MemberDetails.A_COUNTRY")
                                                                                      .Set(ValidCDPs[0], "MemberDetails.A_CDPNUMBER")
                        ,
                        new TxnHeader[] {
                            TxnHeader.Generate("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                        checkOutDate:DateTime.Now.Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: ValidPrograms[0].Set(ValidTiers[0].Code,"SpecificTier"),
                                        RSDNCTRYCD: ValidRSDNCTRYCDs[1], HODIndicator: 0, rentalCharges: BaseTxnAmount, sacCode: "Y",
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
                        yield return new TestCaseData(
                            Member.GenerateRandom(MemberStyle.PreProjectOne, validProgram.Set(validTier.Code, "SpecificTier"))
                                                                                            .Set(ValidRSDNCTRYCDs[0], "MemberDetails.A_COUNTRY")
                                                                                            .Set(ValidCDPs[0], "MemberDetails.A_CDPNUMBER")
                            ,
                            new TxnHeader[] {
                                    TxnHeader.Generate("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                    checkOutDate:DateTime.Now.Comparable(),
                                    bookDate:DateTime.Now.Comparable(),
                                    program: validProgram.Set(validTier.Code,"SpecificTier"),
                                    RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, rentalCharges: BaseTxnAmount, sacCode: "Y",
                                    contractTypeCode: ContractTypeCode, CDP:ValidCDPs[0], checkoutWorldWideISO: ValidCHKWORLDWIDECTRYCDs[0])
                            },
                            ExpectedPointEvents,
                            new string[] { }
                        ).SetName($"{PointEventName}. EarningPref={validProgram.EarningPreference}, Tier={validTier.Code}, CDP = {ValidCDPs[0]}, RSDNCTRYCODE = {ValidRSDNCTRYCDs[0]}, CHKOUTWORLDWIDERGNCTRYISO = {ValidCHKWORLDWIDECTRYCDs[0]}")
                         .SetCategory(BonusTestCategory.Regression)
                         .SetCategory(BonusTestCategory.Positive)
                         .SetCategory(BonusTestCategory.Smoke);
                        yield return new TestCaseData(
                       Member.GenerateRandom(MemberStyle.PreProjectOne, validProgram.Set(validTier.Code, "SpecificTier"))
                                                                                       .Set(ValidRSDNCTRYCDs[0], "MemberDetails.A_COUNTRY")
                                                                                       .Set(ValidCDPs[0], "MemberDetails.A_CDPNUMBER")
                       ,
                       new TxnHeader[] {
                                    TxnHeader.Generate("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                    checkOutDate:DateTime.Now.Comparable(),
                                    bookDate:DateTime.Now.Comparable(),
                                    program: validProgram.Set(validTier.Code,"SpecificTier"),
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
