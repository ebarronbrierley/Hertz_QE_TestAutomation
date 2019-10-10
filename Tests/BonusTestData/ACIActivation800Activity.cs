using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brierley.TestAutomation.Core.Utilities;
using HertzNetFramework.DataModels;
using NUnit.Framework;

namespace HertzNetFramework.Tests.BonusTestData
{
    public class ACIActivation800Activity
    {
        public static readonly decimal[] ValidCDPs = new decimal[] { 664920M, 664924M };
        public const string ValidContractTypeCode = "AAAA";     //TxnHeader.A_CONTRACTTYPECD
        public const string ValidContractSegment = "DISC";      //MemberDetails.A_CONTRACTSEGMENTTYPE
        public static readonly string[] ValidRSDNCTRYCDs = new string[] { "US", "BM", "CA", "MX", "AS", "GU", "MP", "PR", "VI",
                                                                        "BE","FR","DE","IT","LU","NL","ES","CH","GB","IE",
                                                                        "AI","AW","BB","BS","VG","KY","DM","DO","GD","GP","HT","JM","MQ","MS","AN","SM","KN","LC","VC","TT","TC",
                                                                        "AR","BO","BR","BZ","CL","CO","CR","EC","FK","GF","GT","GY","HN","NI","PA","PE","PY","SR","SV","UY","VE",
                                                                        "AU","NZ","PW"};
        public const string PointEventName = "ACIActivation800Activity";
        public const decimal PointEventAmount = 800M;
        public const string ValidFTPTNRNUM = "ZE1";
        public static IHertzProgram[] ValidPrograms = new IHertzProgram[] { HertzProgram.GoldPointsRewards };
        public static IHertzTier[] ValidTiers = new IHertzTier[] { GPR.Tier.RegularGold, GPR.Tier.FiveStar, GPR.Tier.PresidentsCircle, GPR.Tier.Platinum };
        public const decimal BaseTxnAmount = 25M;
        public static ExpectedPointEvent[] ExpectedPointEvents = new ExpectedPointEvent[] { new ExpectedPointEvent(PointEventName, PointEventAmount) };
        public static TimeSpan ValidRentalLength = TimeSpan.FromDays(1);
        public static readonly DateTime StartDate = DateTime.Parse("08/01/2018");
        public static readonly DateTime EndDate = DateTime.Parse("08/01/2099");


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
                            Member.GenerateRandom(MemberStyle.ProjectOne, validProgram.Set(validTier.Code, "SpecificTier"))
                                                                                          .Set(ValidRSDNCTRYCDs[0], "MemberDetails.A_COUNTRY")
                                                                                          .Set(ValidCDPs[0], "MemberDetails.A_CDPNUMBER")
                                                                                          .Set(ValidContractSegment, "MemberDetails.A_CONTRACTSEGMENTTYPE")
                            ,
                            new TxnHeader[] {
                                TxnHeader.Generate("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                checkOutDate:DateTime.Now.Comparable(),
                                bookDate:DateTime.Now.Comparable(),
                                program: validProgram.Set(validTier.Code,"SpecificTier"), CDP: ValidCDPs[0],
                                RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, rentalCharges: BaseTxnAmount,
                                contractTypeCode: ValidContractTypeCode)
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
                    yield return new TestCaseData(
                            Member.GenerateRandom(MemberStyle.ProjectOne, ValidPrograms[0].Set(ValidTiers[0].Code, "SpecificTier"))
                                                                                          .Set(ValidRSDNCTRYCDs[0], "MemberDetails.A_COUNTRY")
                                                                                          .Set(validCDP, "MemberDetails.A_CDPNUMBER")
                                                                                          .Set(ValidContractSegment, "MemberDetails.A_CONTRACTSEGMENTTYPE")
                            ,
                            new TxnHeader[] {
                                TxnHeader.Generate("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                checkOutDate:DateTime.Now.Comparable(),
                                bookDate:DateTime.Now.Comparable(),
                                program: ValidPrograms[0].Set(ValidTiers[0].Code,"SpecificTier"), CDP: validCDP,
                                RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, rentalCharges: BaseTxnAmount,
                                contractTypeCode: ValidContractTypeCode)
                            },
                            ExpectedPointEvents,
                            new string[] { }
                        ).SetName($"{PointEventName}. CDP = {validCDP}, EarningPref={ValidPrograms[0].EarningPreference}, Tier={ValidTiers[0].Code}, RSDNCTRYCODE = {ValidRSDNCTRYCDs[0]}")
                         .SetCategory(BonusTestCategory.Regression)
                         .SetCategory(BonusTestCategory.Positive)
                         .SetCategory(BonusTestCategory.Smoke);
                }
                foreach (string validRSDN in ValidRSDNCTRYCDs)
                {
                    yield return new TestCaseData(
                            Member.GenerateRandom(MemberStyle.ProjectOne, ValidPrograms[0].Set(ValidTiers[0].Code, "SpecificTier"))
                                                                                          .Set(validRSDN, "MemberDetails.A_COUNTRY")
                                                                                          .Set(ValidCDPs[0], "MemberDetails.A_CDPNUMBER")
                                                                                          .Set(ValidContractSegment, "MemberDetails.A_CONTRACTSEGMENTTYPE")
                            ,
                            new TxnHeader[] {
                                TxnHeader.Generate("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                checkOutDate:DateTime.Now.Comparable(),
                                bookDate:DateTime.Now.Comparable(),
                                program: ValidPrograms[0].Set(ValidTiers[0].Code,"SpecificTier"), CDP: ValidCDPs[0],
                                RSDNCTRYCD: validRSDN, HODIndicator: 0, rentalCharges: BaseTxnAmount,
                                contractTypeCode: ValidContractTypeCode)
                            },
                            ExpectedPointEvents,
                            new string[] { }
                        ).SetName($"{PointEventName}. RSDNCTRYCODE = {validRSDN}, EarningPref={ValidPrograms[0].EarningPreference}, Tier={ValidTiers[0].Code}, CDP = {ValidCDPs[0]}")
                         .SetCategory(BonusTestCategory.Regression)
                         .SetCategory(BonusTestCategory.Positive);
                }
            }
        }




        public static IHertzProgram[] InvalidPrograms = new IHertzProgram[]{ HertzProgram.DollarExpressRenters, HertzProgram.ThriftyBlueChip };
        public static decimal[] InvalidCDPs = new decimal[] { 1234567M };
        public static decimal[] InvalidBaseTxnAmount = new decimal[] { 10M, 24M };
        public static IEnumerable NegativeScenarios
        {
            get
            {
                foreach (IHertzProgram invalidProgram in InvalidPrograms)
                {
                    yield return new TestCaseData(
                        Member.GenerateRandom(MemberStyle.ProjectOne, invalidProgram)
                                                                                              .Set(ValidRSDNCTRYCDs[0], "MemberDetails.A_COUNTRY")
                                                                                              .Set(ValidCDPs[0], "MemberDetails.A_CDPNUMBER")
                                                                                              .Set(ValidContractSegment, "MemberDetails.A_CONTRACTSEGMENTTYPE")
                            ,
                            new TxnHeader[] {
                            TxnHeader.Generate("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                            checkOutDate:DateTime.Now.Comparable(),
                            bookDate:DateTime.Now.Comparable(),
                            program: invalidProgram, CDP: ValidCDPs[0],
                            RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, rentalCharges: BaseTxnAmount,
                            contractTypeCode: ValidContractTypeCode)
                            },
                            ExpectedPointEvents,
                            new string[] { }
                        ).SetName($"{PointEventName}. Invalid EarningPref={invalidProgram.EarningPreference}, CDP = {ValidCDPs[0]},  RSDNCTRYCODE = {ValidRSDNCTRYCDs[0]}")
                            .SetCategory(BonusTestCategory.Regression)
                            .SetCategory(BonusTestCategory.Negative)
                            .SetCategory(BonusTestCategory.Smoke);
                }
                foreach (decimal invalidCDP in InvalidCDPs)
                {
                    yield return new TestCaseData(
                                Member.GenerateRandom(MemberStyle.ProjectOne, ValidPrograms[0].Set(ValidTiers[0].Code, "SpecificTier"))
                                                                                              .Set(ValidRSDNCTRYCDs[0], "MemberDetails.A_COUNTRY")
                                                                                              .Set(invalidCDP, "MemberDetails.A_CDPNUMBER")
                                                                                              .Set(ValidContractSegment, "MemberDetails.A_CONTRACTSEGMENTTYPE")
                                ,
                                new TxnHeader[] {
                                TxnHeader.Generate("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                checkOutDate:DateTime.Now.Comparable(),
                                bookDate:DateTime.Now.Comparable(),
                                program: ValidPrograms[0].Set(ValidTiers[0].Code,"SpecificTier"), CDP: invalidCDP,
                                RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, rentalCharges: BaseTxnAmount,
                                contractTypeCode: ValidContractTypeCode)
                                },
                                ExpectedPointEvents,
                                new string[] { }
                            ).SetName($"{PointEventName}. Invalid CDP = {invalidCDP}, EarningPref={ValidPrograms[0].EarningPreference}, Tier={ValidTiers[0].Code}, RSDNCTRYCODE = {ValidRSDNCTRYCDs[0]}")
                             .SetCategory(BonusTestCategory.Regression)
                             .SetCategory(BonusTestCategory.Positive)
                             .SetCategory(BonusTestCategory.Smoke);
                }
                foreach(decimal invalidTxnAmount in InvalidBaseTxnAmount)
                {

                }
            }
        }
    }
}
