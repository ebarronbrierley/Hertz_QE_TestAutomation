using Hertz.API.Controllers;
using Hertz.API.DataModels;
using NUnit.Framework;
using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace Hertz.API.TestData
{
    public class HTZUpdateTierTestData
    {
        public static IEnumerable PositiveScenarios
        {
            get
            {
                DateTime newTierEndDate = new DateTime(DateTime.Now.Year, 12, 31);
                string newMarketingCode = "RENTS";
                //exclude VP tier since it is the last tier and it could be upgraded.
                var tiersGPR = HertzLoyalty.Programs
                                .Where(x => x.CARDTYPE == 0).First()
                                .Tiers.Where(y => y.Code != "VP")
                                .OrderBy(z => z.Rank).ToList();
                foreach (IHertzTier tier in tiersGPR)
                {
                    IHertzTier nextTier = HertzLoyalty.Programs
                                            .Where(x => x.CARDTYPE == 0).First()
                                            .Tiers.Where(y => y.Rank > tier.Rank)
                                            .OrderBy(z => z.Rank)
                                            .FirstOrDefault();

                    MemberModel member = MemberController.GenerateRandomMember(tier);
                    member.MemberDetails.A_COUNTRY = "US";

                    yield return new TestCaseData(member, nextTier, newTierEndDate, newMarketingCode, "csadmin")
                        .SetName($"HTZUpdateTier Positive - Tier:[{tier.Name}] New Tier:[{nextTier.Name}] New Tier End Date:[{newTierEndDate.ToShortDateString()}] and New Marketing Code:[{newMarketingCode}]");

                }
            }

        }

        public static IEnumerable NegativeScenarios
        {
            get
            {
                StringBuilder errorMessage = new StringBuilder();
                string newTierEndDate = DateTime.Now.ToString("12/31/yyyy");
                string newMarketingCode = "RENTS";

                //exclude VP tier since it is the last tier and it could be upgraded.
                var tiersGPR = HertzLoyalty.Programs
                                .Where(x => x.CARDTYPE == 0).First()
                                .Tiers.OrderBy(z => z.Rank).ToList();

                foreach (IHertzTier tier in tiersGPR)
                {
                    IHertzTier nextTier = HertzLoyalty.Programs
                                            .Where(x => x.CARDTYPE == 0).First()
                                            .Tiers.Where(y => y.Rank > tier.Rank)
                                            .OrderBy(z => z.Rank)
                                            .FirstOrDefault();
                    nextTier = nextTier ?? tier;

                    IHertzTier previousTier = HertzLoyalty.Programs
                                            .Where(x => x.CARDTYPE == 0).First()
                                            .Tiers.Where(y => y.Rank < tier.Rank)
                                            .OrderBy(z => z.Rank)
                                            .FirstOrDefault();

                    int errorCode = 100;
                    errorMessage.Clear().Append("Loyalty Member Id must be populated");
                    yield return new TestCaseData(null, nextTier.Code,
                        newTierEndDate, newMarketingCode, "csadmin", errorCode, errorMessage.ToString())
                        .SetName($"HTZUpdateTier  Negative - New Tier:[{nextTier.Name}] Loyalty Member Id must be populated");

                    MemberModel member = MemberController.GenerateRandomMember(tier);

                    errorCode = 200;
                    errorMessage.Clear().Append("Agent Username must be populated");
                    yield return new TestCaseData(member, nextTier.Code, newTierEndDate, newMarketingCode,
                        "", errorCode, errorMessage.ToString())
                        .SetName($"HTZUpdateTier  Negative - Tier:[{tier.Name}] New Tier:[{nextTier.Name}] Agent Username must be populated");

                    errorCode = 300;
                    errorMessage.Clear().Append("NewTier must be populated");
                    yield return new TestCaseData(member, null, newTierEndDate, newMarketingCode,
                        "csadmin", errorCode, errorMessage.ToString())
                        .SetName($"HTZUpdateTier  Negative - Tier:[{tier.Name}] NewTier must be populated");

                    errorCode = 400;
                    errorMessage.Clear().Append("NewTierEndDate must be populated");
                    yield return new TestCaseData(member, nextTier.Code, null, newMarketingCode,
                        "csadmin", errorCode, errorMessage.ToString())
                        .SetName($"HTZUpdateTier  Negative - Tier:[{tier.Name}] New Tier:[{nextTier.Name}] NewTierEndDate must be populated");

                    errorCode = 500;
                    errorMessage.Clear().Append("MarketingProgramId must be populated");
                    yield return new TestCaseData(member, nextTier.Code, newTierEndDate, "",
                        "csadmin", errorCode, errorMessage.ToString())
                        .SetName($"HTZUpdateTier  Negative - Tier:[{tier.Name}] New Tier:[{nextTier.Name}] MarketingProgramId must be populated");

                    errorCode = 201;
                    errorMessage.Clear().Append("Invalid Agent Username");
                    yield return new TestCaseData(member, nextTier.Code, newTierEndDate, newMarketingCode,
                        "testautm1", errorCode, errorMessage.ToString())
                        .SetName($"HTZUpdateTier  Negative - Tier:[{tier.Name}] New Tier:[{nextTier.Name}] and New Marketing Code:[{newMarketingCode}] Invalid Agent Username");

                    errorCode = 301;
                    errorMessage.Clear().Append("Invalid NewTier");
                    yield return new TestCaseData(member, "FR", newTierEndDate, newMarketingCode,
                        "csadmin", errorCode, errorMessage.ToString())
                        .SetName($"HTZUpdateTier  Negative - Tier:[{tier.Name}] and New Marketing Code:[{newMarketingCode}] Invalid NewTier");

                    errorCode = 302;
                    errorMessage.Clear().Append("Member already at requested tier, not updated");
                    yield return new TestCaseData(member, tier.Code, newTierEndDate, newMarketingCode,
                        "csadmin", errorCode, errorMessage.ToString())
                        .SetName($"HTZUpdateTier  Negative - Tier:[{tier.Name}] New Tier:[{tier.Name}] and New Marketing Code:[{newMarketingCode}] Member already at requested tier, not updated");

                    //to ignore RG tier without a previous tier
                    if (previousTier != null)
                    {
                        errorCode = 303;
                        errorMessage.Clear().Append("Requested Tier is lower than Current Tier");
                        yield return new TestCaseData(member, previousTier.Code, newTierEndDate, newMarketingCode,
                            "csadmin", errorCode, errorMessage.ToString())
                            .SetName($"HTZUpdateTier  Negative - Tier:[{tier.Name}] New Tier:[{previousTier.Name}] and New Marketing Code:[{newMarketingCode}] Requested Tier is lower than Current Tier");
                    }

                    //to exclude VP updating to VP
                    if (nextTier.Code != tier.Code)
                    {
                       
                        errorCode = 401;
                        errorMessage.Clear().Append("Invalid NewTierEndDate");
                        yield return new TestCaseData(member, nextTier.Code,
                            DateTime.Now.AddYears(-2).ToString("MM/dd/yyyy"),
                            newMarketingCode,
                            "csadmin", errorCode, errorMessage.ToString())
                            .SetName($"HTZUpdateTier  Negative - Tier:[{tier.Name}] New Tier:[{nextTier.Name}] and New Marketing Code:[{newMarketingCode}] Invalid NewTierEndDate");
                    }

                }

                foreach (IHertzProgram program in HertzLoyalty.Programs.Where(x=>x.CARDTYPE!=0))
                {
                    IHertzTier tier = program.Tiers.First();
                    MemberModel member = MemberController.GenerateRandomMember(tier);

                    int errorCode = 304;
                    errorMessage.Clear().Append("Invalid Brand, Only GPR tiers are eligible");
                    yield return new TestCaseData(member,tiersGPR.First().Code,
                        newTierEndDate, newMarketingCode, "csadmin", errorCode, errorMessage.ToString())
                        .SetName($"HTZUpdateTier  Negative - Program: [{program.Name}] Invalid Brand, Only GPR tiers are eligible");
                }

            }
        }

    }
}
