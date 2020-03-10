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
    public class CorpNewMember550PointsOngoingActivity
    {
        public const string PointEventName = "CorpNewMember550PointsOngoingActivity";
        public const string TriplePointEventName = "CorpNewMemberTriplePoints_OngoingActivity";
        public const string ContractTypeCode = "COMM";
        public const decimal PointEventAmount = 550M;
        public const decimal BaseTxnAmount = 25M;
        public static readonly string[] ValidAcquisitionMethodTypeCodes = new string[] { "1898", "12091", "1882", "1627", "6730" };
        public static readonly string[] ValidRSDNCTRYCDs = new string[] { "US", "BM", "CA", "MX", "AS", "GU", "MP", "PR", "VI",
                                                                          "BE", "FR", "DE", "IT", "LU", "NL", "ES", "CH", "GB", "IE", "SE", "NO", "DK", "FI", "PT",
                                                                          "AI", "AW", "BB", "BS", "VG", "KY","DM","DO","GD","GP","HT","JM","MQ","MS","AN","SM","KN","LC","VC","TT","TC",
                                                                          "AR","BO","BR","BZ","CL","CO","CR","EC","FK","GF","GT","GY","HN","NI","PA","PE","PY","SR","SV","UY","VE",
                                                                          "AU","NZ","PW","CN","SG","JP","KR" };
        public static readonly string[] ValidCHKWORLDWIDECTRYCDs = new string[] {"US", "CA", "PR", "VI",
                                                                                 "BE", "FR", "DE", "IT", "LU", "NL","ES","CH","GB","IE","SE","NO", "DK","FI",
                                                                                 "BR",
                                                                                 "AU","NZ"};
        
        public static readonly IHertzProgram[] ValidPrograms = new IHertzProgram[] { HertzLoyalty.GoldPointsRewards };
        public static readonly IHertzTier[] ValidTiers = new IHertzTier[] { HertzLoyalty.GoldPointsRewards.RegularGold, HertzLoyalty.GoldPointsRewards.FiveStar, HertzLoyalty.GoldPointsRewards.PresidentsCircle };
        public static TimeSpan ValidRentalLength = TimeSpan.FromDays(1);
        public static ExpectedPointEvent[] ExpectedPointEvents = new ExpectedPointEvent[] { new ExpectedPointEvent(TriplePointEventName, BaseTxnAmount*2),

                                                                                            new ExpectedPointEvent(PointEventName, PointEventAmount) };
        public const string ChkOutLocId = "00004";

        public static IEnumerable PositiveScenarios
        {
            get
            {
                foreach (string acquisitionMethod in ValidAcquisitionMethodTypeCodes)
                {
                    MemberModel member = MemberController.GenerateRandomMember(ValidTiers[0]);
                    member.MemberDetails.A_COUNTRY = ValidRSDNCTRYCDs[0];
                    member.MemberDetails.A_ACQUISITIONMETHODTYPECODE = ValidAcquisitionMethodTypeCodes[0];

                    yield return new TestCaseData(
                        member,
                        new TxnHeaderModel[] {
                            TxnHeaderController.GenerateTransaction("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                        checkOutDate:DateTime.Now.Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: ValidPrograms[0],
                                        RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, rentalCharges: BaseTxnAmount,
                                        contractTypeCode: ContractTypeCode, checkoutWorldWideISO: ValidCHKWORLDWIDECTRYCDs[0])
                        },
                        ExpectedPointEvents,
                        new string[] { }
                    ).SetName($"{PointEventName}. EarningPref={ValidPrograms[0].EarningPreference}, Tier={ValidTiers[0].Code}, AcquistionMethodTypeCode = {acquisitionMethod}, RSDNCTRYCODE = {ValidRSDNCTRYCDs[0]}, WWISOCTRYCODE = {ValidCHKWORLDWIDECTRYCDs[0]}")
                     .SetCategory(BonusTestCategory.Regression)
                     .SetCategory(BonusTestCategory.Positive)
                     .SetCategory(BonusTestCategory.Smoke);
                }
                foreach (string rsdnCtryCode in ValidRSDNCTRYCDs)
                {
                    MemberModel member = MemberController.GenerateRandomMember(ValidTiers[0]);
                    member.MemberDetails.A_COUNTRY = rsdnCtryCode;
                    member.MemberDetails.A_ACQUISITIONMETHODTYPECODE = ValidAcquisitionMethodTypeCodes[0];

                    yield return new TestCaseData(
                        member,
                        new TxnHeaderModel[] {
                            TxnHeaderController.GenerateTransaction("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                        checkOutDate:DateTime.Now.Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: ValidPrograms[0],
                                        RSDNCTRYCD: rsdnCtryCode, HODIndicator: 0, rentalCharges: BaseTxnAmount,
                                        contractTypeCode: ContractTypeCode, checkoutWorldWideISO: ValidCHKWORLDWIDECTRYCDs[0])
                        },
                        ExpectedPointEvents,
                        new string[] { }
                    ).SetName($"{PointEventName}. EarningPref={ValidPrograms[0].EarningPreference}, Tier={ValidTiers[0].Code}, AcquistionMethodTypeCode = {ValidAcquisitionMethodTypeCodes[0]}, RSDNCTRYCODE = {rsdnCtryCode}, WWISOCTRYCODE = {ValidCHKWORLDWIDECTRYCDs[0]}")
                     .SetCategory(BonusTestCategory.Regression)
                     .SetCategory(BonusTestCategory.Positive);
                }
                foreach (string wwCheckout in ValidCHKWORLDWIDECTRYCDs)
                {
                    MemberModel member = MemberController.GenerateRandomMember(ValidTiers[0]);
                    member.MemberDetails.A_COUNTRY = ValidRSDNCTRYCDs[0];
                    member.MemberDetails.A_ACQUISITIONMETHODTYPECODE = ValidAcquisitionMethodTypeCodes[0];

                    yield return new TestCaseData(
                        member,
                        new TxnHeaderModel[] {
                            TxnHeaderController.GenerateTransaction("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                        checkOutDate:DateTime.Now.Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: ValidPrograms[0],
                                        RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, rentalCharges: BaseTxnAmount,
                                        contractTypeCode: ContractTypeCode, checkoutWorldWideISO: wwCheckout)
                        },
                        ExpectedPointEvents,
                        new string[] { }
                    ).SetName($"{PointEventName}. EarningPref={ValidPrograms[0].EarningPreference}, Tier={ValidTiers[0].Code}, AcquistionMethodTypeCode = {ValidAcquisitionMethodTypeCodes[0]}, RSDNCTRYCODE = {ValidRSDNCTRYCDs[0]}, WWISOCTRYCODE = {wwCheckout}")
                     .SetCategory(BonusTestCategory.Regression)
                     .SetCategory(BonusTestCategory.Positive);
                }
                foreach (IHertzProgram validProgram in ValidPrograms)
                {
                    foreach (IHertzTier validTier in validProgram.Tiers)
                    {
                        if (!ValidTiers.ToList().Any(x=>x.Name.Equals(validTier.Name))) continue;
                        MemberModel member = MemberController.GenerateRandomMember(validTier);
                        member.MemberDetails.A_COUNTRY = ValidRSDNCTRYCDs[0];
                        member.MemberDetails.A_ACQUISITIONMETHODTYPECODE = ValidAcquisitionMethodTypeCodes[0];

                        yield return new TestCaseData(
                            member,
                            new TxnHeaderModel[] {
                                    TxnHeaderController.GenerateTransaction("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                    checkOutDate:DateTime.Now.Comparable(),
                                    bookDate:DateTime.Now.Comparable(),
                                    program: validProgram,
                                    RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, rentalCharges: BaseTxnAmount,
                                    contractTypeCode: ContractTypeCode, checkoutWorldWideISO: ValidCHKWORLDWIDECTRYCDs[0])
                            },
                            ExpectedPointEvents,
                            new string[] { }
                        ).SetName($"{PointEventName}. EarningPref={validProgram.EarningPreference}, Tier={validTier.Code}, AcquistionMethodTypeCode = {ValidAcquisitionMethodTypeCodes[0]}, RSDNCTRYCODE = {ValidRSDNCTRYCDs[0]}, WWISOCTRYCODE = {ValidCHKWORLDWIDECTRYCDs[0]}")
                         .SetCategory(BonusTestCategory.Regression)
                         .SetCategory(BonusTestCategory.Positive)
                         .SetCategory(BonusTestCategory.Smoke);



                        member = MemberController.GenerateRandomMember(validTier);
                        member.MemberDetails.A_COUNTRY = ValidRSDNCTRYCDs[0];
                        member.MemberDetails.A_ACQUISITIONMETHODTYPECODE = ValidAcquisitionMethodTypeCodes[0];

                        yield return new TestCaseData(
                            member,
                            new TxnHeaderModel[] {
                                    TxnHeaderController.GenerateTransaction("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                    checkOutDate:DateTime.Now.Comparable(),
                                    bookDate:DateTime.Now.Comparable(),
                                    program: validProgram,
                                    RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, rentalCharges: BaseTxnAmount,
                                    contractTypeCode: ContractTypeCode, checkoutWorldWideISO: ValidCHKWORLDWIDECTRYCDs[0],chkoutlocnum: null,chkoutareanum: null,chkoutlocid: ChkOutLocId)
                            },
                            ExpectedPointEvents,
                            new string[] { }
                        ).SetName($"{PointEventName}. EarningPref={validProgram.EarningPreference},CHCOUTLOCID = {ChkOutLocId}, Tier={validTier.Code}, AcquistionMethodTypeCode = {ValidAcquisitionMethodTypeCodes[0]}, RSDNCTRYCODE = {ValidRSDNCTRYCDs[0]}, WWISOCTRYCODE = {ValidCHKWORLDWIDECTRYCDs[0]}")
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
