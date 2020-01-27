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
        public static IHertzProgram[] ValidPrograms = new IHertzProgram[] { new GoldPointsRewards() };
        public static IHertzTier[] ValidTiers = new IHertzTier[] { HertzLoyalty.GoldPointsRewards.RegularGold, HertzLoyalty.GoldPointsRewards.FiveStar, HertzLoyalty.GoldPointsRewards.PresidentsCircle, HertzLoyalty.GoldPointsRewards.Platinum };
        public const decimal BaseTxnAmount = 25M;
        public static ExpectedPointEvent[] ExpectedPointEvents = new ExpectedPointEvent[] { new ExpectedPointEvent(PointEventName, PointEventAmount) };
        public static TimeSpan ValidRentalLength = TimeSpan.FromDays(1);
        public static readonly DateTime StartDate = DateTime.Parse("08/01/2018");
        public static readonly DateTime EndDate = DateTime.Parse("08/01/2099");
        public const string ChkOutLocId = "00004";


        public static IEnumerable PositiveScenarios
        {
            get
            {
                TxnHeaderController txnController = new TxnHeaderController();

                foreach (IHertzProgram validProgram in ValidPrograms)
                {
                    foreach (IHertzTier validTier in validProgram.Tiers)
                    {
                        if (!ValidTiers.ToList().Any(x => x.Name.Equals(validTier.Name))) continue;

                        MemberModel member = MemberController.GenerateRandomMember(validTier);
                        member.MemberDetails.A_COUNTRY = ValidRSDNCTRYCDs[0];
                        member.MemberDetails.A_CDPNUMBER = $"{ValidCDPs[0]}";
                        member.MemberDetails.A_CONTRACTSEGMENTTYPE = ValidContractSegment;

                        yield return new TestCaseData(
                           member,
                            new TxnHeaderModel[] {
                                TxnHeaderController.GenerateTransaction("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                checkOutDate:DateTime.Now.Comparable(),
                                bookDate:DateTime.Now.Comparable(),
                                program: validProgram.Set(validTier.Code,"SpecificTier"), CDP: ValidCDPs[0],
                                RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, rentalCharges: BaseTxnAmount,
                                contractTypeCode: ValidContractTypeCode,chkoutlocnum:null,chkoutareanum:null,chkoutlocid: ChkOutLocId)
                            },
                            ExpectedPointEvents,
                            new string[] { }
                        ).SetName($"{PointEventName}. EarningPref={validProgram.EarningPreference},ChkOutLocId={ChkOutLocId}, Tier={validTier.Code}, CDP = {ValidCDPs[0]},  RSDNCTRYCODE = {ValidRSDNCTRYCDs[0]}");


                        member = MemberController.GenerateRandomMember(validTier);
                        member.MemberDetails.A_COUNTRY = ValidRSDNCTRYCDs[0];
                        member.MemberDetails.A_CDPNUMBER = $"{ValidCDPs[0]}";
                        member.MemberDetails.A_CONTRACTSEGMENTTYPE = ValidContractSegment;
                        yield return new TestCaseData(
                            member,
                            new TxnHeaderModel[] {
                                TxnHeaderController.GenerateTransaction("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                checkOutDate:DateTime.Now.Comparable(),
                                bookDate:DateTime.Now.Comparable(),
                                program: validProgram.Set(validTier.Code,"SpecificTier"), CDP: ValidCDPs[0],
                                RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, rentalCharges: BaseTxnAmount,
                                contractTypeCode: ValidContractTypeCode)
                            },
                            ExpectedPointEvents,
                            new string[] { }
                        ).SetName($"{PointEventName}. EarningPref={validProgram.EarningPreference},Tier={validTier.Code}, CDP = {ValidCDPs[0]},  RSDNCTRYCODE = {ValidRSDNCTRYCDs[0]}");
                    }
                }

                foreach (decimal validCDP in ValidCDPs)
                {
                    MemberModel member = MemberController.GenerateRandomMember(ValidTiers[0]);
                    member.MemberDetails.A_COUNTRY = ValidRSDNCTRYCDs[0];
                    member.MemberDetails.A_CDPNUMBER = $"{validCDP}";
                    member.MemberDetails.A_CONTRACTSEGMENTTYPE = ValidContractSegment;

                    yield return new TestCaseData(
                            member,
                            new TxnHeaderModel[] {
                                TxnHeaderController.GenerateTransaction("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                checkOutDate:DateTime.Now.Comparable(),
                                bookDate:DateTime.Now.Comparable(),
                                program: ValidPrograms[0].Set(ValidTiers[0].Code,"SpecificTier"), CDP: validCDP,
                                RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, rentalCharges: BaseTxnAmount,
                                contractTypeCode: ValidContractTypeCode)
                            },
                            ExpectedPointEvents,
                            new string[] { }
                        ).SetName($"{PointEventName}. CDP = {validCDP}, EarningPref={ValidPrograms[0].EarningPreference}, Tier={ValidTiers[0].Code}, RSDNCTRYCODE = {ValidRSDNCTRYCDs[0]}");
                }

                foreach (string validRSDN in ValidRSDNCTRYCDs)
                {
                    MemberModel member = MemberController.GenerateRandomMember(ValidTiers[0]);
                    member.MemberDetails.A_COUNTRY = validRSDN;
                    member.MemberDetails.A_CDPNUMBER = $"{ValidCDPs[0]}";
                    member.MemberDetails.A_CONTRACTSEGMENTTYPE = ValidContractSegment;

                    yield return new TestCaseData(
                            member,
                            new TxnHeaderModel[] {
                                TxnHeaderController.GenerateTransaction("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                checkOutDate:DateTime.Now.Comparable(),
                                bookDate:DateTime.Now.Comparable(),
                                program: ValidPrograms[0].Set(ValidTiers[0].Code,"SpecificTier"), CDP: ValidCDPs[0],
                                RSDNCTRYCD: validRSDN, HODIndicator: 0, rentalCharges: BaseTxnAmount,
                                contractTypeCode: ValidContractTypeCode)
                            },
                            ExpectedPointEvents,
                            new string[] { }
                        ).SetName($"{PointEventName}. RSDNCTRYCODE = {validRSDN}, EarningPref={ValidPrograms[0].EarningPreference}, Tier={ValidTiers[0].Code}, CDP = {ValidCDPs[0]}");
                }
            }
        }




        public static IHertzProgram[] InvalidPrograms = new IHertzProgram[]{ HertzLoyalty.DollarExpressRenters, HertzLoyalty.ThriftyBlueChip };
        public static decimal[] InvalidCDPs = new decimal[] { 1234567M };
        public static decimal[] InvalidBaseTxnAmount = new decimal[] { 10M, 24M };
        public static IEnumerable NegativeScenarios
        {
            get
            {
                foreach (IHertzProgram invalidProgram in InvalidPrograms)
                {
                    MemberModel member = MemberController.GenerateRandomMember(invalidProgram.Tiers.FirstOrDefault());
                    member.MemberDetails.A_COUNTRY = ValidRSDNCTRYCDs[0];
                    member.MemberDetails.A_CDPNUMBER = $"{ValidCDPs[0]}";
                    member.MemberDetails.A_CONTRACTSEGMENTTYPE = ValidContractSegment;

                    yield return new TestCaseData(
                            member,
                            new TxnHeaderModel[] {
                            TxnHeaderController.GenerateTransaction("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                            checkOutDate:DateTime.Now.Comparable(),
                            bookDate:DateTime.Now.Comparable(),
                            program: invalidProgram, CDP: ValidCDPs[0],
                            RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, rentalCharges: BaseTxnAmount,
                            contractTypeCode: ValidContractTypeCode)
                            },
                           ExpectedPointEvents,
                            new string[] { }
                        ).SetName($"{PointEventName}. Invalid EarningPref={invalidProgram.EarningPreference}, CDP = {ValidCDPs[0]},  RSDNCTRYCODE = {ValidRSDNCTRYCDs[0]}");
                }
                foreach (decimal invalidCDP in InvalidCDPs)
                {
                    MemberModel member = MemberController.GenerateRandomMember(ValidTiers[0]);
                    member.MemberDetails.A_COUNTRY = ValidRSDNCTRYCDs[0];
                    member.MemberDetails.A_CDPNUMBER = $"{invalidCDP}";
                    member.MemberDetails.A_CONTRACTSEGMENTTYPE = ValidContractSegment;

                    yield return new TestCaseData(
                                member,
                                new TxnHeaderModel[] {
                                TxnHeaderController.GenerateTransaction("", checkInDate: DateTime.Now.AddTicks(ValidRentalLength.Ticks).Comparable(),
                                checkOutDate:DateTime.Now.Comparable(),
                                bookDate:DateTime.Now.Comparable(),
                                program: ValidPrograms[0].Set(ValidTiers[0].Code,"SpecificTier"), CDP: invalidCDP,
                                RSDNCTRYCD: ValidRSDNCTRYCDs[0], HODIndicator: 0, rentalCharges: BaseTxnAmount,
                                contractTypeCode: ValidContractTypeCode)
                                },
                              ExpectedPointEvents,
                                new string[] { }
                            ).SetName($"{PointEventName}. Invalid CDP = {invalidCDP}, EarningPref={ValidPrograms[0].EarningPreference}, Tier={ValidTiers[0].Code}, RSDNCTRYCODE = {ValidRSDNCTRYCDs[0]}");
                }
                foreach(decimal invalidTxnAmount in InvalidBaseTxnAmount)
                {

                }
            }
        }
    }
}
