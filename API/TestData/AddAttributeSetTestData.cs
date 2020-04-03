using Hertz.API.Controllers;
using Hertz.API.DataModels;
using NUnit.Framework;
using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace Hertz.API.TestData
{
    public class AddAttributeSetTestData
    {
        public static decimal points = 100;

        public static IEnumerable PositiveScenarios
        {
            get
            {
                bool useRanum = true;
                IHertzProgram program = HertzLoyalty.Programs.Where(x => x.CARDTYPE == 0)
                                            .FirstOrDefault();

                IHertzTier tier = program.Tiers.FirstOrDefault();
                MemberModel member = MemberController.GenerateRandomMember(tier);
                AuctionHeaderModel auctionHeader = new AuctionHeaderModel()
                {
                    AuctionEventName = "HTZAuctionReward",
                    AuctionPointType = "C",
                    AuctionTxnType = "C",
                    CDRewardID = "TEST-" + DateTime.Now.ToString("yyyyMMdd-HHmmss"),
                    HeaderGPRpts = 10
                };
                yield return new TestCaseData(member, auctionHeader, tier, useRanum)
                    .SetName($"AddAttributeSet Positive - AuctionTxnType [{auctionHeader.AuctionTxnType}] AuctionPointType:[{auctionHeader.AuctionPointType}] AuctionEventName: [{auctionHeader.AuctionEventName}]");
                             
                member = MemberController.GenerateRandomMember(tier);
                auctionHeader = new AuctionHeaderModel()
                {
                    AuctionEventName = "HTZAuctionReward",
                    AuctionPointType = "C",
                    AuctionTxnType = "E",
                    CDRewardID = "TEST-" + DateTime.Now.ToString("yyyyMMdd-HHmmss"),
                    HeaderGPRpts = 10
                };
                yield return new TestCaseData(member, auctionHeader, tier, useRanum)
                    .SetName($"AddAttributeSet Positive - AuctionTxnType [{auctionHeader.AuctionTxnType}] AuctionPointType:[{auctionHeader.AuctionPointType}] AuctionEventName: [{auctionHeader.AuctionEventName}]");

                member = MemberController.GenerateRandomMember(tier);
                auctionHeader = new AuctionHeaderModel()
                {
                    AuctionEventName = "HTZAuctionReward",
                    AuctionPointType = "C",
                    AuctionTxnType = "D",
                    CDRewardID = "TEST-" + DateTime.Now.ToString("yyyyMMdd-HHmmss"),
                    HeaderGPRpts = 10
                };
                yield return new TestCaseData(member, auctionHeader, tier, useRanum)
                    .SetName($"AddAttributeSet Positive - AuctionTxnType [{auctionHeader.AuctionTxnType}] AuctionPointType:[{auctionHeader.AuctionPointType}] AuctionEventName: [{auctionHeader.AuctionEventName}]");

                member = MemberController.GenerateRandomMember(tier);
                auctionHeader = new AuctionHeaderModel()
                {
                    AuctionEventName = "HTZAuctionReward",
                    AuctionPointType = "C",
                    AuctionTxnType = "R",
                    CDRewardID = "TEST-" + DateTime.Now.ToString("yyyyMMdd-HHmmss"),
                    HeaderGPRpts = 10
                };
                yield return new TestCaseData(member, auctionHeader, tier, useRanum)
                    .SetName($"AddAttributeSet Positive - AuctionTxnType [{auctionHeader.AuctionTxnType}] AuctionPointType:[{auctionHeader.AuctionPointType}] AuctionEventName: [{auctionHeader.AuctionEventName}]");

                member = MemberController.GenerateRandomMember(tier);
                auctionHeader = new AuctionHeaderModel()
                {
                    AuctionEventName = "HTZAuctionReward",
                    AuctionPointType = "D",
                    AuctionTxnType = "C",
                    CDRewardID = "TEST-" + DateTime.Now.ToString("yyyyMMdd-HHmmss"),
                    HeaderGPRpts = 10
                };
                yield return new TestCaseData(member, auctionHeader, tier, useRanum)
                    .SetName($"AddAttributeSet Positive - AuctionTxnType [{auctionHeader.AuctionTxnType}] AuctionPointType:[{auctionHeader.AuctionPointType}] AuctionEventName: [{auctionHeader.AuctionEventName}]");


                member = MemberController.GenerateRandomMember(tier);
                auctionHeader = new AuctionHeaderModel()
                {
                    AuctionEventName = "HTZContestPurchase",
                    AuctionPointType = "D",
                    AuctionTxnType = "C",
                    CDRewardID = "TEST-" + DateTime.Now.ToString("yyyyMMdd-HHmmss"),
                    HeaderGPRpts = 10
                };
                yield return new TestCaseData(member, auctionHeader, tier, useRanum)
                    .SetName($"AddAttributeSet Positive - AuctionTxnType [{auctionHeader.AuctionTxnType}] AuctionPointType:[{auctionHeader.AuctionPointType}] AuctionEventName: [{auctionHeader.AuctionEventName}]");

                member = MemberController.GenerateRandomMember(tier);
                auctionHeader = new AuctionHeaderModel()
                {
                    AuctionEventName = "HTZDonationPurchase",
                    AuctionPointType = "C",
                    AuctionTxnType = "C",
                    CDRewardID = "TEST-" + DateTime.Now.ToString("yyyyMMdd-HHmmss"),
                    HeaderGPRpts = 10
                };
                yield return new TestCaseData(member, auctionHeader, tier, useRanum)
                    .SetName($"AddAttributeSet Positive - AuctionTxnType [{auctionHeader.AuctionTxnType}] AuctionPointType:[{auctionHeader.AuctionPointType}] AuctionEventName: [{auctionHeader.AuctionEventName}]");
            }

        }

        public static IEnumerable NegativeScenarios
        {
            get
            {
                StringBuilder errorMessage = new StringBuilder();
                IHertzProgram program = HertzLoyalty.Programs.Where(x => x.CARDTYPE == 0)
                                          .FirstOrDefault();
                IHertzTier tier = program.Tiers.First();
                MemberModel member = MemberController.GenerateRandomMember(tier);
                AuctionHeaderModel  auctionHeader = new AuctionHeaderModel()
                {
                    AuctionEventName = "HTZAuctionReward",
                    AuctionPointType = "C",
                    AuctionTxnType = "C",
                    CDRewardID =string.Empty,
                    HeaderGPRpts = 10
                };
                int errorCode = 2003;
                errorMessage.Clear().Append("CDRewardID of AuctionHeader is a required property");
                yield return new TestCaseData(member, auctionHeader,  errorCode, errorMessage.ToString())
                    .SetName($"AddAttributeSet Negative - Member: [{member.IPCODE}] CDRewardID of AuctionHeader is a required property");


                member = MemberController.GenerateRandomMember(tier);
                auctionHeader = new AuctionHeaderModel()
                {
                    AuctionEventName = "HTZAuctionReward",
                    AuctionPointType = "C",
                    AuctionTxnType = "L",
                    CDRewardID = "TEST-" + DateTime.Now.ToString("yyyyMMdd-HHmmss"),
                    HeaderGPRpts = 10
                };
                errorCode = 800;
                errorMessage.Clear().Append("Invalid AuctionTxnType");
                yield return new TestCaseData(member, auctionHeader, errorCode, errorMessage.ToString())
                     .SetName($"AddAttributeSet Negative - Member: [{member.IPCODE}] Invalid AuctionTxnType");


                member = MemberController.GenerateRandomMember(tier);
                auctionHeader = new AuctionHeaderModel()
                {
                    AuctionEventName = "HTZAuctionReward",
                    AuctionPointType = "C",
                    AuctionTxnType = "C",
                    CDRewardID = "TEST-" + DateTime.Now.ToString("yyyyMMdd-HHmmss"),
                    HeaderGPRpts = -10
                };
                errorCode = 801;
                errorMessage.Clear().Append("HeaderGPRpts must be greater than zero");
                yield return new TestCaseData(member, auctionHeader, errorCode, errorMessage.ToString())
                    .SetName($"AddAttributeSet Negative - Member: [{member.IPCODE}] HeaderGPRpts must be greater than zero");


                member = MemberController.GenerateRandomMember(tier);
                auctionHeader = new AuctionHeaderModel()
                {
                    AuctionEventName = "HTZAuctionReward",
                    AuctionPointType = "L",
                    AuctionTxnType = "C",
                    CDRewardID = "TEST-" + DateTime.Now.ToString("yyyyMMdd-HHmmss"),
                    HeaderGPRpts = 10
                };
                errorCode = 802;
                errorMessage.Clear().Append("Invalid AuctionPointType");
                yield return new TestCaseData(member, auctionHeader, errorCode, errorMessage.ToString())
                    .SetName($"AddAttributeSet Negative - Member: [{member.IPCODE}] Invalid AuctionPointType");


                member = MemberController.GenerateRandomMember(tier);
                auctionHeader = new AuctionHeaderModel()
                {
                    AuctionEventName = "GPRBase",
                    AuctionPointType = "C",
                    AuctionTxnType = "C",
                    CDRewardID = "TEST-" + DateTime.Now.ToString("yyyyMMdd-HHmmss"),
                    HeaderGPRpts = 10
                };
                errorCode = 803;
                errorMessage.Clear().Append("Invalid AuctionEventName");
                yield return new TestCaseData(member, auctionHeader, errorCode, errorMessage.ToString())
                    .SetName($"AddAttributeSet Negative - Member: [{member.IPCODE}] Invalid AuctionEventName");


                member = MemberController.GenerateRandomMember(tier);
                auctionHeader = new AuctionHeaderModel()
                {
                    AuctionEventName = "HTZContestPurchase",
                    AuctionPointType = "C",
                    AuctionTxnType = "C",
                    CDRewardID = "TEST-" + DateTime.Now.ToString("yyyyMMdd-HHmmss"),
                    HeaderGPRpts = 10
                };
                errorCode = 804;
                errorMessage.Clear().Append("AuctionEventName 'HTZContestPurchase' is only valid with AuctionPointType 'D'");
                yield return new TestCaseData(member, auctionHeader, errorCode, errorMessage.ToString())
                    .SetName($"AddAttributeSet Negative - Member: [{member.IPCODE}] AuctionEventName 'HTZContestPurchase' is only valid with AuctionPointType 'D'");


                member = MemberController.GenerateRandomMember(tier);
                auctionHeader = new AuctionHeaderModel()
                {
                    AuctionEventName = "HTZAuctionReward",
                    AuctionPointType = "D",
                    AuctionTxnType = "C",
                    CDRewardID = "TEST-" + DateTime.Now.ToString("yyyyMMdd-HHmmss"),
                    HeaderGPRpts = 10
                };
                errorCode = 805;
                errorMessage.Clear().Append("Point balance is insufficient");
                yield return new TestCaseData(member, auctionHeader, errorCode, errorMessage.ToString())
                    .SetName($"AddAttributeSet Negative - Member: [{member.IPCODE}] Point balance is insufficient");


                program = HertzLoyalty.Programs.Where(x => x.CARDTYPE != 0)
                                       .FirstOrDefault();
                tier = program.Tiers.First();
                member = MemberController.GenerateRandomMember(tier);
                auctionHeader = new AuctionHeaderModel()
                {
                    AuctionEventName = "HTZAuctionReward",
                    AuctionPointType = "C",
                    AuctionTxnType = "C",
                    CDRewardID = "TEST-" + DateTime.Now.ToString("yyyyMMdd-HHmmss"),
                    HeaderGPRpts = 10
                };
                errorCode = 101;
                errorMessage.Clear().Append("Invalid Loyalty Member Id");
                yield return new TestCaseData(member, auctionHeader, errorCode, errorMessage.ToString())
                    .SetName($"AddAttributeSet Negative - Member: [{member.IPCODE}] Invalid Loyalty Member Id");

            }
        }
    }
}
