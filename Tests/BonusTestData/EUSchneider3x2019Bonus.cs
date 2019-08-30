﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brierley.TestAutomation.Core.Utilities;
using HertzNetFramework.DataModels;
using NUnit.Framework;

namespace HertzNetFramework.Tests.BonusTestData
{
    public class EUSchneider3x2019Bonus
    {
        public const string PointEventName = "EUSchneider3x2019BonusActivity";
        public const string ContractTypeCode = "COMM";
        public const decimal PointEventAmount = 550M;
        public const decimal BaseTxnAmount = 25M;
        public static readonly DateTime StartDate = DateTime.Parse("04/01/2019 12:00:01 AM");
        public static readonly DateTime EndDate = DateTime.Parse("12/31/2019 12:00:00 AM");

        public static readonly string[] ValidRSDNCTRYCDs = new string[] { "BE", "FR", "DE", "IT", "LU", "NL", "ES", "CH", "GB", "IE", "SE", "NO", "DK", "FI" };
        public static readonly decimal[] ValidCDPs = new decimal[] { 830063M, 777967M, 771845M, 771844M, 797934M, 772513M, 867985M, 867984M, 825961M, 820736M, 818718M,
                                                                     818717M, 677115M, 525278M, 967352M, 963004M, 852405M, 852374M, 851608M, 851595M, 824332M, 794724M,
                                                                     629921M, 629910M, 629881M, 629878M, 629875M, 629872M, 629869M, 629829M, 629815M, 629812M, 629811M,
                                                                     627624M, 627254M, 625477M, 614208M, 601094M, 601093M, 562179M, 559734M, 540054M, 509251M, 804801M,
                                                                     804802M, 973980M, 806782M, 799627M, 795644M, 787155M, 787154M, 787153M, 776526M, 776116M, 774802M,
                                                                     769821M, 769820M, 769819M, 769818M, 769817M, 769816M, 769815M, 769814M, 769813M, 769812M, 769811M,
                                                                     769810M, 769809M, 769807M, 769806M, 769805M, 769804M, 769803M, 769802M, 769801M, 769800M, 769799M,
                                                                     769798M, 769797M, 769795M, 769794M, 769793M, 769792M, 769791M, 768049M, 758173M, 633124M, 609221M,
                                                                     607942M, 572271M, 869337M, 834039M, 817865M, 811523M, 799470M, 799469M, 797748M, 796362M, 769912M,
                                                                     769910M, 769909M, 769908M, 724973M, 652069M, 637986M, 511452M, 769914M, 634178M, 862011M, 861479M,
                                                                     807840M, 791754M, 791753M, 859989M, 803752M, 796736M, 796242M, 768561M, 672790M, 668335M, 666472M,
                                                                     660745M, 814292M, 810422M, 810421M, 810419M, 810418M, 810417M, 810315M, 810313M, 810310M, 810308M,
                                                                     810169M, 803738M, 797933M, 795853M, 791870M, 790288M, 780690M, 780689M, 780688M, 780687M, 768870M,
                                                                     960007M, 831570M, 808643M, 864417M, 797244M, 773721M, 770474M, 770470M, 738956M, 736930M, 872050M,
                                                                     872048M, 824871M, 824868M, 774444M, 774432M, 774422M, 774410M, 774405M, 774391M, 774338M, 824641M,
                                                                     824640M, 778413M, 778411M, 778410M, 769751M, 769750M, 769749M, 769748M, 769747M, 769746M, 769745M,
                                                                     769688M, 767174M, 754743M, 754616M, 753055M, 552207M, 552206M, 797888M, 719350M, 719345M, 710953M,
                                                                     529849M, 524304M };
        public static readonly IHertzProgram[] ValidPrograms = new IHertzProgram[] { HertzProgram.GoldPointsRewards };
        public static readonly IHertzTier[] ValidTiers = new IHertzTier[] { GPR.Tier.RegularGold, GPR.Tier.FiveStar, GPR.Tier.PresidentsCircle };
        public static TimeSpan ValidRentalLength = TimeSpan.FromDays(1);
        public static TimeSpan ValidBookingDate = TimeSpan.FromDays(-8);
        public static ExpectedPointEvent[] ExpectedPointEvents = new ExpectedPointEvent[] { new ExpectedPointEvent("EUSchneider3x2019BonusActivity", BaseTxnAmount*2) };


        public static IEnumerable PositiveScenarios
        {
            get
            {
                foreach (string rsdnCtry in ValidRSDNCTRYCDs)
                {
                    yield return new TestCaseData(
                        Member.GenerateRandom(MemberStyle.ProjectOne, ValidPrograms[0].Set(ValidTiers[0].Code, "SpecificTier"))
                                                                                      .Set(rsdnCtry, "MemberDetails.A_COUNTRY")
                                                                                      .Set(ValidCDPs[0], "MemberDetails.A_CDPNUMBER")
                        ,
                        new TxnHeader[] {
                            TxnHeader.Generate("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                        checkOutDate:DateTime.Now.Comparable(),
                                        bookDate:DateTime.Now.AddTicks(ValidBookingDate.Ticks).Comparable(),
                                        program: ValidPrograms[0].Set(ValidTiers[0].Code,"SpecificTier"), CDP: ValidCDPs[0],
                                        RSDNCTRYCD: rsdnCtry, HODIndicator: 0, qualifyingAmount: BaseTxnAmount)
                        },
                        ExpectedPointEvents,
                        new string[] { }
                    ).SetName($"{PointEventName}. RSDNCTRYCODE = {rsdnCtry}, EarningPref={ValidPrograms[0].EarningPreference}, Tier={ValidTiers[0].Code}, CDP = {ValidCDPs[0]}")
                     .SetCategory("Bonus_Regression,Regression");
                }
                foreach (decimal validCDP in ValidCDPs)
                {
                    yield return new TestCaseData(
                        Member.GenerateRandom(MemberStyle.ProjectOne, ValidPrograms[0].Set(ValidTiers[0].Code, "SpecificTier"))
                                                                                      .Set(ValidRSDNCTRYCDs[0], "MemberDetails.A_COUNTRY")
                                                                                      .Set(validCDP, "MemberDetails.A_CDPNUMBER")
                        ,
                        new TxnHeader[] {
                            TxnHeader.Generate("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                        checkOutDate:DateTime.Now.Comparable(),
                                        bookDate:DateTime.Now.AddTicks(ValidBookingDate.Ticks).Comparable(),
                                        program: ValidPrograms[0].Set(ValidTiers[0].Code,"SpecificTier"), CDP: validCDP,
                                        RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, qualifyingAmount: BaseTxnAmount)
                        },
                        ExpectedPointEvents,
                        new string[] { }
                    ).SetName($"{PointEventName}. CDP = {validCDP},  RSDNCTRYCODE = {ValidRSDNCTRYCDs[0]}, EarningPref={ValidPrograms[0].EarningPreference}, Tier={ValidTiers[0].Code}")
                     .SetCategory("Bonus_Regression,Regression");
                }
                foreach (IHertzProgram validProgram in ValidPrograms)
                {
                    foreach (IHertzTier validTier in validProgram.Tiers)
                    {
                        if (!ValidTiers.ToList().Any(x => x.Name.Equals(validTier.Name))) continue;

                        yield return new TestCaseData(
                            Member.GenerateRandom(MemberStyle.ProjectOne, validProgram.Set(validTier.Code, "SpecificTier"))
                                                                                          .Set(ValidRSDNCTRYCDs[0], "MemberDetails.A_COUNTRY")
                                                                                          .Set(ValidCDPs[0], "MemberDetails.A_CDPNUMBER")
                            ,
                            new TxnHeader[] {
                                TxnHeader.Generate("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                checkOutDate:DateTime.Now.Comparable(),
                                bookDate:DateTime.Now.AddTicks(ValidBookingDate.Ticks).Comparable(),
                                program: validProgram.Set(validTier.Code,"SpecificTier"), CDP: ValidCDPs[0],
                                RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, qualifyingAmount: BaseTxnAmount)
                            },
                            ExpectedPointEvents,
                            new string[] { }
                        ).SetName($"{PointEventName}. EarningPref={validProgram.EarningPreference}, Tier={validTier.Code}, CDP = {ValidCDPs[0]},  RSDNCTRYCODE = {ValidRSDNCTRYCDs[0]}")
                         .SetCategory("Bonus_Regression,Regression,Bonus_Smoke");
                    }
                }
            }
        }
    }
}
