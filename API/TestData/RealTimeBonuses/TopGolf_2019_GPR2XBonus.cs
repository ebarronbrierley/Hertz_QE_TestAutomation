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
    public class TopGolf_2019_GPR2XBonus
    {
        public const string PointEventName = "TopGolf_2019_GPR2XBonus";
        public const decimal BaseTxnAmount = 25M;
        public const decimal PointEventAmount = BaseTxnAmount;
        public static readonly DateTime StartDate = DateTime.Parse("06/15/2019 12:00:01 AM");
        public static readonly DateTime EndDate = DateTime.Parse("09/15/2019 12:00:00 AM");

        public static readonly string[] ValidCHKOUTWORLDWIDERGNCTRYISO = new string[] { "US", "CA", "PR", "VI" };
        public static readonly decimal[] ValidCDPs = new decimal[] { 2152921M };
        public static readonly IHertzProgram[] ValidPrograms = new IHertzProgram[] { HertzProgram.GoldPointsRewards };
        public static readonly IHertzTier[] ValidTiers = new IHertzTier[] { GPR.Tier.RegularGold, GPR.Tier.FiveStar, GPR.Tier.PresidentsCircle, GPR.Tier.Platinum, GPR.Tier.PlatinumSelect, GPR.Tier.PlatinumVIP };
        public static TimeSpan ValidRentalLength = TimeSpan.FromDays(1);
        public static ExpectedPointEvent[] ExpectedPointEvents = new ExpectedPointEvent[] { new ExpectedPointEvent(PointEventName, BaseTxnAmount) };


        public static IEnumerable PositiveScenarios
        {
            get
            {
                foreach (string chkoutWW in ValidCHKOUTWORLDWIDERGNCTRYISO)
                {
                    yield return new TestCaseData(
                        Member.GenerateRandom(MemberStyle.ProjectOne, ValidPrograms[0].Set(ValidTiers[0].Code, "SpecificTier"))
                                                                                      .Set(chkoutWW, "MemberDetails.A_COUNTRY")
                                                                                      .Set(ValidCDPs[0], "MemberDetails.A_CDPNUMBER")
                        ,
                        new TxnHeader[] {
                            TxnHeader.Generate("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                        checkOutDate:DateTime.Now.Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: ValidPrograms[0].Set(ValidTiers[0].Code,"SpecificTier"), CDP: ValidCDPs[0],
                                        RSDNCTRYCD: chkoutWW, HODIndicator: 0, rentalCharges: BaseTxnAmount)
                        },
                        ExpectedPointEvents,
                        new string[] { }
                    ).SetName($"{PointEventName}. CHKOUTWORLDWIDERGNCTRYISO = {chkoutWW}, EarningPref={ValidPrograms[0].EarningPreference}, Tier={ValidTiers[0].Code}, CDP = {ValidCDPs[0]}")
                     .SetCategory(BonusTestCategory.Regression)
                     .SetCategory(BonusTestCategory.Positive);
                }
                foreach (decimal validCDP in ValidCDPs)
                {
                    yield return new TestCaseData(
                        Member.GenerateRandom(MemberStyle.ProjectOne, ValidPrograms[0].Set(ValidTiers[0].Code, "SpecificTier"))
                                                                                      .Set(ValidCHKOUTWORLDWIDERGNCTRYISO[0], "MemberDetails.A_COUNTRY")
                                                                                      .Set(validCDP, "MemberDetails.A_CDPNUMBER")
                        ,
                        new TxnHeader[] {
                            TxnHeader.Generate("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                        checkOutDate:DateTime.Now.Comparable(),
                                        bookDate:DateTime.Now.Comparable(),
                                        program: ValidPrograms[0].Set(ValidTiers[0].Code,"SpecificTier"), CDP: validCDP,
                                        RSDNCTRYCD: ValidCHKOUTWORLDWIDERGNCTRYISO[0], HODIndicator: 0, rentalCharges: BaseTxnAmount)
                        },
                        ExpectedPointEvents,
                        new string[] { }
                    ).SetName($"{PointEventName}. CDP = {validCDP},  CHKOUTWORLDWIDERGNCTRYISO = {ValidCHKOUTWORLDWIDERGNCTRYISO[0]}, EarningPref={ValidPrograms[0].EarningPreference}, Tier={ValidTiers[0].Code}")
                     .SetCategory(BonusTestCategory.Regression)
                     .SetCategory(BonusTestCategory.Positive);
                }
                foreach (IHertzProgram validProgram in ValidPrograms)
                {
                    foreach (IHertzTier validTier in validProgram.Tiers)
                    {
                        if (!ValidTiers.ToList().Any(x => x.Name.Equals(validTier.Name))) continue;

                        yield return new TestCaseData(
                            Member.GenerateRandom(MemberStyle.ProjectOne, validProgram.Set(validTier.Code, "SpecificTier"))
                                                                                          .Set(ValidCHKOUTWORLDWIDERGNCTRYISO[0], "MemberDetails.A_COUNTRY")
                                                                                          .Set(ValidCDPs[0], "MemberDetails.A_CDPNUMBER")
                            ,
                            new TxnHeader[] {
                                TxnHeader.Generate("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                checkOutDate:DateTime.Now.Comparable(),
                                bookDate:DateTime.Now.Comparable(),
                                program: validProgram.Set(validTier.Code,"SpecificTier"), CDP: ValidCDPs[0],
                                RSDNCTRYCD: ValidCHKOUTWORLDWIDERGNCTRYISO[0], HODIndicator: 0, rentalCharges: BaseTxnAmount)
                            },
                            ExpectedPointEvents,
                            new string[] { }
                        ).SetName($"{PointEventName}. EarningPref={validProgram.EarningPreference}, Tier={validTier.Code}, CDP = {ValidCDPs[0]},  RSDNCTRYCODE = {ValidCHKOUTWORLDWIDERGNCTRYISO[0]}")
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
