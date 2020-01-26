using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brierley.TestAutomation.Core.Database;
using Brierley.TestAutomation.Core.Utilities;
using Brierley.TestAutomation.Core.Reporting;
using Brierley.LoyaltyWare.ClientLib;
using Hertz.API.DataModels;
using Hertz.API.Utilities;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;

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

        public MemberModel AddMember(MemberModel member)
        {
            var lwMemberIn = LODConvert.ToLW<Member>(member);
            MemberModel memberOut = default;

            using(ConsoleCapture capture = new ConsoleCapture())
            {
                try
                {
                    var lwMemOut = lwSvc.AddMember(lwMemberIn, String.Empty, out double elapsed);
                    memberOut = LODConvert.FromLW<MemberModel>(lwMemOut);
                    stepContext.AddAttachment(new Attachment("AddMember", capture.Output, Attachment.Type.Text));
                }
                catch(LWClientException ex)
                {
                    stepContext.AddAttachment(new Attachment("AddMember", capture.Output, Attachment.Type.Text));
                    throw new LWServiceException(ex.Message, ex.ErrorCode);
                }
            }
            return memberOut;
        }

        public MemberModel GetFromDB(decimal ipCode)
        {
            string query = $"select * from {MemberModel.TableName} where IPCODE = {ipCode}";
            MemberModel member =  dbContext.QuerySingleRow<MemberModel>(query);
            member.MemberDetails = dbContext.QuerySingleRow<MemberDetailsModel>($"select * from {MemberDetailsModel.TableName} where A_IPCODE = {member.IPCODE}");
            member.MemberPreferences = dbContext.QuerySingleRow<MemberPreferencesModel>($"select * from {MemberPreferencesModel.TableName} where A_IPCODE = {member.IPCODE}");
            member.VirtualCards = dbContext.Query<VirtualCardModel>($"select * from {VirtualCardModel.TableName} where IPCODE = {member.IPCODE}").ToList();
            return member;
        }

        public static MemberModel GenerateRandomMember(IHertzTier tier = null)
        {
            if (tier == null) tier = HertzLoyalty.GoldPointsRewards.RegularGold;

            MemberModel member = StrongRandom.GenerateRandom<MemberModel>();
            member.MEMBERSTATUS = MemberModel.Status.Active;
            member.ISEMPLOYEE = 0;

            member.MemberDetails = GenerateMemberDetails(member, tier);
            member.MemberPreferences = GenerateMemberPreferences(member);

            VirtualCardModel vc = GenerateVirtualCard(member);
            member.VirtualCards = new List<VirtualCardModel>() { vc };
            member.ALTERNATEID = vc.LOYALTYIDNUMBER;
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
        public static VirtualCardModel GenerateVirtualCard(MemberModel member)
        {
            VirtualCardModel vc = StrongRandom.GenerateRandom<VirtualCardModel>();
            vc.IPCODE = member.IPCODE;
            vc.ISPRIMARY = 1;
            vc.STATUS = VirtualCardModel.Status.Active;

            return vc;
        }
    }
}
