﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brierley.TestAutomation.Core.Utilities;
using Hertz.API.Controllers;
using Hertz.API.DataModels;
using NUnit.Framework;


namespace Hertz.API.TestData.RealTimeBonuses
{
    public class Corp550Points_2016Activity
    {
        public const string PointEventName = "Corp550Points_2016Activity";
        public const string TriplePointEventName = "CorpTriplePoints_2016Activity";
        public const string ContractTypeCode = "COMM";
        public const decimal PointEventAmount = 550M;
        public const decimal BaseTxnAmount = 25M;
        public static readonly decimal[] ValidContracts = new decimal[] { 102521M, 277740M, 376777M  };
        public static readonly string[] ValidRSDNCTRYCDs = new string[] { "US", "BM", "CA", "MX", "AS", "GU", "MP", "PR", "VI",
                                                                          "BE", "FR", "DE", "IT", "LU", "NL", "ES", "CH", "GB", "IE", "SE", "NO", "DK", "FI", "PT",
                                                                          "AI", "AW", "BB", "BS", "VG", "KY","DM","DO","GD","GP","HT","JM","MQ","MS","AN","SM","KN","LC","VC","TT","TC",
                                                                          "AR","BO","BR","BZ","CL","CO","CR","EC","FK","GF","GT","GY","HN","NI","PA","PE","PY","SR","SV","UY","VE",
                                                                          "AU","NZ","PW","CN","SG","JP","KR" };
        public static readonly string[] ValidCHKWORLDWIDECTRYCDs = new string[] {"US", "CA", "PR", "VI",
                                                                                 "BE", "FR", "DE", "IT", "LU", "NL","ES","CH","GB","IE","SE","NO", "DK","FI", "PT",
                                                                                 "BR",
                                                                                 "AU","NZ"};

        public static readonly IHertzProgram[] ValidPrograms = new IHertzProgram[] { HertzLoyalty.GoldPointsRewards };
        public static readonly IHertzTier[] ValidTiers = new IHertzTier[] { HertzLoyalty.GoldPointsRewards.RegularGold, HertzLoyalty.GoldPointsRewards.FiveStar, HertzLoyalty.GoldPointsRewards.PresidentsCircle };
        public static TimeSpan ValidRentalLength = TimeSpan.FromDays(1);
        public static ExpectedPointEvent[] ExpectedPointEvents = new ExpectedPointEvent[] { new ExpectedPointEvent(TriplePointEventName, BaseTxnAmount*2),
                                                                                            new ExpectedPointEvent(PointEventName, PointEventAmount) };

        public static IEnumerable PositiveScenarios
        {
            get
            {
                foreach (decimal validContract in ValidContracts)
                {
                    MemberModel member = MemberController.GenerateRandomMember(ValidTiers[0]);
                    member.MemberDetails.A_COUNTRY = ValidRSDNCTRYCDs[0];

                    yield return new TestCaseData(
                        member,
                        new TxnHeaderModel[] {
                            TxnHeaderController.GenerateTransaction("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                        checkOutDate:DateTime.Now.Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: ValidPrograms[0],
                                        RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, rentalCharges: BaseTxnAmount, sacCode: "Y",
                                        contractTypeCode: ContractTypeCode, contractNumber: validContract, checkoutWorldWideISO: ValidCHKWORLDWIDECTRYCDs[0])
                        },
                        ExpectedPointEvents,
                        new string[] { }
                    ).SetName($"{PointEventName}. ContractNum = {validContract}, EarningPref={ValidPrograms[0].EarningPreference}, Tier={ValidTiers[0].Code}, RSDNCTRYCODE = {ValidRSDNCTRYCDs[0]}, CHKOUTWORLDWIDERGNCTRYISO = {ValidCHKWORLDWIDECTRYCDs[0]}, SACCD= Y")
                     .SetCategory(BonusTestCategory.Regression)
                     .SetCategory(BonusTestCategory.Positive)
                     .SetCategory(BonusTestCategory.Smoke);
                }
                foreach (string rsdnCtryCode in ValidRSDNCTRYCDs)
                {
                    MemberModel member = MemberController.GenerateRandomMember(ValidTiers[0]);
                    member.MemberDetails.A_COUNTRY = rsdnCtryCode;

                    yield return new TestCaseData(
                       member,
                        new TxnHeaderModel[] {
                            TxnHeaderController.GenerateTransaction("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                        checkOutDate:DateTime.Now.Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: ValidPrograms[0],
                                        RSDNCTRYCD: rsdnCtryCode, HODIndicator: 0, rentalCharges: BaseTxnAmount, sacCode: "Y",
                                        contractTypeCode: ContractTypeCode, contractNumber: ValidContracts[0], checkoutWorldWideISO: ValidCHKWORLDWIDECTRYCDs[0])
                        },
                        ExpectedPointEvents,
                        new string[] { }
                    ).SetName($"{PointEventName}. EarningPref={ValidPrograms[0].EarningPreference}, Tier={ValidTiers[0].Code}, ContractNum = {ValidContracts[0]}, RSDNCTRYCODE = {rsdnCtryCode}, WWISOCTRYCODE = {ValidCHKWORLDWIDECTRYCDs[0]}, SACCD= Y")
                     .SetCategory(BonusTestCategory.Regression)
                     .SetCategory(BonusTestCategory.Positive);
                }
                foreach (string wwCheckout in ValidCHKWORLDWIDECTRYCDs)
                {
                    MemberModel member = MemberController.GenerateRandomMember(ValidTiers[0]);
                    member.MemberDetails.A_COUNTRY = ValidRSDNCTRYCDs[0];
                    yield return new TestCaseData(
                        member,
                        new TxnHeaderModel[] {
                            TxnHeaderController.GenerateTransaction("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                        checkOutDate:DateTime.Now.Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: ValidPrograms[0],
                                        RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, rentalCharges: BaseTxnAmount, sacCode: "Y",
                                        contractTypeCode: ContractTypeCode, contractNumber: ValidContracts[0], checkoutWorldWideISO: wwCheckout)
                        },
                        ExpectedPointEvents,
                        new string[] { }
                    ).SetName($"{PointEventName}. CHKOUTWORLDWIDERGNCTRYISO = {wwCheckout}, EarningPref={ValidPrograms[0].EarningPreference}, Tier={ValidTiers[0].Code}, ContractNum = {ValidContracts[0]}, RSDNCTRYCODE = {ValidRSDNCTRYCDs[0]}, SACCD= Y")
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
                                    TxnHeaderController.GenerateTransaction("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                    checkOutDate:DateTime.Now.Comparable(),
                                    bookDate:DateTime.Now.Comparable(),
                                    program: validProgram,
                                    RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, rentalCharges: BaseTxnAmount, sacCode: "Y",
                                    contractTypeCode: ContractTypeCode, contractNumber:ValidContracts[0], checkoutWorldWideISO: ValidCHKWORLDWIDECTRYCDs[0])
                            },
                            ExpectedPointEvents,
                            new string[] { }
                        ).SetName($"{PointEventName}. EarningPref={validProgram.EarningPreference}, Tier={validTier.Code}, ContractNum = {ValidContracts[0]}, RSDNCTRYCODE = {ValidRSDNCTRYCDs[0]}, CHKOUTWORLDWIDERGNCTRYISO = {ValidCHKWORLDWIDECTRYCDs[0]}, SACCD= Y")
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
