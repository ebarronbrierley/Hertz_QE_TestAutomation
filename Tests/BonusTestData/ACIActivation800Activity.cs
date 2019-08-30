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
        public const decimal ValidMinimumAmount = 25M;
        public static readonly DateTime ValidStartDate = DateTime.Parse("08/01/2018");
        public static readonly DateTime EndTime = DateTime.Parse("08/01/2099");

        public static IEnumerable PositiveScenarios
        {
            get
            {
                IHertzProgram program = ValidPrograms[0];
                program.SpecificTier = ValidTiers[0].Code;
                Member tcMember = Member.GenerateRandom(MemberStyle.ProjectOne, program)
                                        .Set(ValidRSDNCTRYCDs[0], "MemberDetails.A_COUNTRY");

                TxnHeader txn = TxnHeader.Generate("", ValidStartDate.AddDays(2), ValidStartDate.AddDays(1), ValidStartDate,
                                                    CDP: ValidCDPs[0], program: program, HODIndicator: 0, RSDNCTRYCD: ValidRSDNCTRYCDs[0], qualifyingAmount: ValidMinimumAmount,
                                                    contractTypeCode: null,contractNumber:null);
                yield return new TestCaseData(
                    tcMember,
                    new TxnHeader[] { txn},
                    new ExpectedPointEvent[] { new ExpectedPointEvent(PointEventName, PointEventAmount) },
                    new string[] { }
                    ).SetName($"{PointEventName} CDP={ValidCDPs[0]} RSDNCTRYCD={ValidRSDNCTRYCDs[0]} EarningPref={ValidPrograms[0].EarningPreference} Tier={ValidTiers[0].Code}");
            }
        }




    }
}
