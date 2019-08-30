using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brierley.TestAutomation.Core.Utilities;
using HertzNetFramework.DataModels;
using NUnit.Framework;

namespace HertzNetFramework.Tests.BonusTestData
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
        
        public static readonly IHertzProgram[] ValidPrograms = new IHertzProgram[] { HertzProgram.GoldPointsRewards };
        public static readonly IHertzTier[] ValidTiers = new IHertzTier[] { GPR.Tier.RegularGold, GPR.Tier.FiveStar, GPR.Tier.PresidentsCircle };
        public static TimeSpan ValidRentalLength = TimeSpan.FromDays(1);
        public static ExpectedPointEvent[] ExpectedPointEvents = new ExpectedPointEvent[] { new ExpectedPointEvent(TriplePointEventName, BaseTxnAmount*2),
                                                                                            new ExpectedPointEvent(PointEventName, PointEventAmount) };
        
        public static IEnumerable PositiveScenarios
        {
            get
            {
                foreach (string acquisitionMethod in ValidAcquisitionMethodTypeCodes)
                {
                    yield return new TestCaseData(
                        Member.GenerateRandom(MemberStyle.ProjectOne, ValidPrograms[0].Set(ValidTiers[0].Code, "SpecificTier"))
                                                                                     .Set(ValidRSDNCTRYCDs[0], "MemberDetails.A_COUNTRY")
                                                                                     .Set(ValidAcquisitionMethodTypeCodes[0], "MemberDetails.A_ACQUISITIONMETHODTYPECODE")
                        ,
                        new TxnHeader[] {
                            TxnHeader.Generate("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                        checkOutDate:DateTime.Now.Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: ValidPrograms[0].Set(ValidTiers[0].Code,"SpecificTier"),
                                        RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, qualifyingAmount: BaseTxnAmount,
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
                    yield return new TestCaseData(
                        Member.GenerateRandom(MemberStyle.ProjectOne, ValidPrograms[0].Set(ValidTiers[0].Code, "SpecificTier"))
                                                                                     .Set(rsdnCtryCode, "MemberDetails.A_COUNTRY")
                                                                                     .Set(ValidAcquisitionMethodTypeCodes[0], "MemberDetails.A_ACQUISITIONMETHODTYPECODE")
                        ,
                        new TxnHeader[] {
                            TxnHeader.Generate("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                        checkOutDate:DateTime.Now.Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: ValidPrograms[0].Set(ValidTiers[0].Code,"SpecificTier"),
                                        RSDNCTRYCD: rsdnCtryCode, HODIndicator: 0, qualifyingAmount: BaseTxnAmount,
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
                    yield return new TestCaseData(
                        Member.GenerateRandom(MemberStyle.ProjectOne, ValidPrograms[0].Set(ValidTiers[0].Code, "SpecificTier"))
                                                                                     .Set(ValidRSDNCTRYCDs[0], "MemberDetails.A_COUNTRY")
                                                                                     .Set(ValidAcquisitionMethodTypeCodes[0], "MemberDetails.A_ACQUISITIONMETHODTYPECODE")
                        ,
                        new TxnHeader[] {
                            TxnHeader.Generate("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                        checkOutDate:DateTime.Now.Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: ValidPrograms[0].Set(ValidTiers[0].Code,"SpecificTier"),
                                        RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, qualifyingAmount: BaseTxnAmount,
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
                        yield return new TestCaseData(
                            Member.GenerateRandom(MemberStyle.ProjectOne, validProgram.Set(validTier.Code, "SpecificTier"))
                                                                                            .Set(ValidRSDNCTRYCDs[0], "MemberDetails.A_COUNTRY")
                                                                                            .Set(ValidAcquisitionMethodTypeCodes[0], "MemberDetails.A_ACQUISITIONMETHODTYPECODE")
                            ,
                            new TxnHeader[] {
                                    TxnHeader.Generate("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                    checkOutDate:DateTime.Now.Comparable(),
                                    bookDate:DateTime.Now.Comparable(),
                                    program: validProgram.Set(validTier.Code,"SpecificTier"),
                                    RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, qualifyingAmount: BaseTxnAmount,
                                    contractTypeCode: ContractTypeCode, checkoutWorldWideISO: ValidCHKWORLDWIDECTRYCDs[0])
                            },
                            ExpectedPointEvents,
                            new string[] { }
                        ).SetName($"{PointEventName}. EarningPref={validProgram.EarningPreference}, Tier={validTier.Code}, AcquistionMethodTypeCode = {ValidAcquisitionMethodTypeCodes[0]}, RSDNCTRYCODE = {ValidRSDNCTRYCDs[0]}, WWISOCTRYCODE = {ValidCHKWORLDWIDECTRYCDs[0]}")
                         .SetCategory(BonusTestCategory.Regression)
                         .SetCategory(BonusTestCategory.Positive)
                         .SetCategory(BonusTestCategory.Smoke);
                    }
                }
            }
        }
    }
}
