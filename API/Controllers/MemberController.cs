using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brierley.TestAutomation.Core.Database;
using Brierley.TestAutomation.Core.Utilities;
using Brierley.TestAutomation.Core.Reporting;
using Brierley.LoyaltyWare.ClientLib;
using Hertz.API.DataModels;

namespace Hertz.API.Controllers
{
    public class MemberController
    {
        private readonly IDatabase dbContext;
        private readonly IStepManager stepContext;
        private readonly LWIntegrationSvcClientManager lwSvc;
        public MemberController(IDatabase dbContext, IStepManager testStep = null)
        {
            this.dbContext = dbContext;
            this.stepContext = testStep;

            lwSvc = new LWIntegrationSvcClientManager(EnvironmentManager.Get.SOAPServiceURL, "CDIS", true, String.Empty);
            lwSvc.MaxReceivedMessageSize = 2147483647;
            lwSvc.MaxStringContentLength = 2147483647;
        }

        public static MemberModel GenerateRandomMember(IHertzTier tier = null)
        {
            if (tier == null) tier = HertzLoyalty.GoldPointsRewards.RegularGold;

            MemberModel member = StrongRandom.GenerateRandom<MemberModel>();
            member.MEMBERSTATUS = MemberStatus.Active;

            member.MemberDetails = GenerateMemberDetails(member, tier);
            member.MemberPreferences = GenerateMemberPreferences(member);
            return member;
        }
        public static MemberDetailsModel GenerateMemberDetails(MemberModel member, IHertzTier tier = null)
        {
            MemberDetailsModel details = StrongRandom.GenerateRandom<MemberDetailsModel>();

            details.A_FIRSTNAME = member.FIRSTNAME;
            details.A_LASTNAME = member.LASTNAME;
            details.A_NAMEPREFIX = member.NAMEPREFIX;
            details.A_NAMESUFFIX = member.NAMESUFFIX;
            details.A_PRIMARYPHONENUMBER = member.PRIMARYPHONENUMBER;
            details.A_ZIPORPOSTALCODE = member.PRIMARYPOSTALCODE;
            details.A_LANGUAGEPREFERENCE = member.PREFERREDLANGUAGE;

            details.A_TIERCODE = tier.Code;
            details.A_EARNINGPREFERENCE = tier.ParentProgram.EarningPreference;
            details.A_HBRACCOUNT = 0;
            details.A_MEMBERSTATUSCODE = 1;

            return details;
        }
        public static MemberPreferencesModel GenerateMemberPreferences(MemberModel member)
        {
            MemberPreferencesModel preferences = new MemberPreferencesModel()
            {
                A_DIRECTMAILOPTIN = 1
            };
            return preferences;
        }
    }
}
