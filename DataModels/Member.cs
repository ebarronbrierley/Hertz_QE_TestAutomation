using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brierley.TestAutomation.Core.Utilities;
using Brierley.LoyaltyWare.ClientLib;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using Brierley.TestAutomation.Core.Reporting;
using Brierley.TestAutomation.Core.Database;
using System.Reflection;
using System.Collections;
using System.Linq.Expressions;

namespace HertzNetFramework.DataModels
{
    public enum MemberStyle { PreProjectOne, ProjectOne }
    public class Member
    {
        public static readonly string TableName = "LW_LOYALTYMEMBER";
        private static readonly string dbUser = "bp_htz";

        public class MemberStatus
        {
            public static readonly long Active = 1, Disabled = 2, Terminated = 3, Locked = 4, NonMember = 5, Merged = 6, PreEnrolled = 7;
        }
        public static readonly string[] BaseVerify = new string[] { "MEMBERCLOSEDATE", "MEMBERSTATUS", "BIRTHDATE", "FIRSTNAME", "LASTNAME", "MIDDLENAME", "NAMEPREFIX", "NAMESUFFIX", "PRIMARYPHONENUMBER", "PRIMARYPOSTALCODE", "ISEMPLOYEE", "CHANGEDBY" };
        

        #region Public Properties
        [ModelAttribute("IpCode", ReportOption.Print)]
        public decimal IPCODE { get; set; }
        [ModelAttribute("MemberCreateDate", ReportOption.Print)]
        public DateTime MEMBERCREATEDATE { get; set; }
        [ModelAttribute("MemberCloseDate")]
        public DateTime? MEMBERCLOSEDATE { get; set; }
        [ModelAttribute("MemberStatus", ReportOption.Print)]
        public long MEMBERSTATUS { get; set; }
        [ModelAttribute("BirthDate")]
        public DateTime? BIRTHDATE { get; set; }
        [ModelAttribute("FirstName")]
        public string FIRSTNAME { get; set; }
        [ModelAttribute("LastName")]
        public string LASTNAME { get; set; }
        [ModelAttribute("MiddleName")]
        public string MIDDLENAME { get; set; }
        [ModelAttribute("NamePrefix")]
        public string NAMEPREFIX { get; set; }
        [ModelAttribute("NameSuffix")]
        public string NAMESUFFIX { get; set; }
        [ModelAttribute("AlternateId", ReportOption.Print)]
        public string ALTERNATEID { get; set; }
        [ModelAttribute("Username")]
        public string USERNAME { get; set; }
        [ModelAttribute("Password")]
        public string PASSWORD { get; set; }
        public string SALT { get; set; }
        [ModelAttribute("PrimaryEmailAddress")]
        public string PRIMARYEMAILADDRESS { get; set; }
        [ModelAttribute("PrimaryPhoneNumber")]
        public string PRIMARYPHONENUMBER { get; set; }
        [ModelAttribute("PrimaryPostalCode")]
        public string PRIMARYPOSTALCODE { get; set; }
        [ModelAttribute("LastActivityDate")]
        public DateTime? LASTACTIVITYDATE { get; set; }
        [ModelAttribute("IsEmployee")]
        public short? ISEMPLOYEE { get; set; }
        [ModelAttribute("ChangedBy")]
        public string CHANGEDBY { get; set; }
        public long? NEWSTATUS { get; set; }
        public DateTime? NEWSTATUSEFFECTIVEDATE { get; set; }
        public string STATUSCHANGEREASON { get; set; }
        public string RESETCODE { get; set; }
        public DateTime? RESETCODEDATE { get; set; }
        public DateTime CREATEDATE { get; set; }
        public DateTime? UPDATEDATE { get; set; }
        public long FAILEDPASSWORDATTEMPTCOUNT { get; set; }
        public short PASSWORDCHANGEREQUIRED { get; set; }
        public DateTime? PASSWORDEXPIREDATE { get; set; }
        public DateTime? STATUSCHANGEDATE { get; set; }
        public decimal? MERGEDTOMEMBER { get; set; }
        [ModelAttribute("PreferredLanguage")]
        public string PREFERREDLANGUAGE { get; set; }
        #endregion
        public MemberStyle Style { get { return style; } }

        public List<MemberPreferences> MemberPreferences = new List<MemberPreferences>();
        public List<MemberDetails> MemberDetails = new List<MemberDetails>();
        public List<VirtualCard> VirtualCards = new List<VirtualCard>();
        private MemberStyle style;

        public List<MemberDetails> GetMemberDetails(MemberStyle memberStyle)
        {
            if (memberStyle == MemberStyle.PreProjectOne)
            {
                var details = VirtualCards.Select(x => x.MemberDetails);
                List<DataModels.MemberDetails> output = new List<DataModels.MemberDetails>();
                foreach (var detail in details) output.AddRange(detail);
                return output;
            }
            else
            {
                return MemberDetails;
            }
        }
        public List<MemberPreferences> GetMemberPreferences(MemberStyle memberStyle)
        {
            if (memberStyle == MemberStyle.PreProjectOne)
            {
                var preferences = VirtualCards.Select(x => x.MemberPreferences);
                List<DataModels.MemberPreferences> output = new List<DataModels.MemberPreferences>();
                foreach (var preference in preferences) output.AddRange(preference);
                return output;
            }
            else
            {
                return MemberPreferences;
            }
        }
        public Member MakeVirtualCardLIDsUnique(IDatabase database)
        {
            foreach(VirtualCard vc in this.VirtualCards)
            {
                vc.LOYALTYIDNUMBER = GetUnusedLoyaltyId(database);
            }
            return this;
        }
        public static Member GenerateRandom(MemberStyle memberStyle = MemberStyle.PreProjectOne, IHertzProgram program = null, IHertzTier tier = null)
        {
            if (program == null) program = HertzProgram.GoldPointsRewards;

            Member member = new Member()
            {
                IPCODE = Convert.ToInt64(StrongRandom.NumericString(6)),
                MEMBERCREATEDATE = DateTime.Now.AddDays(-1).Comparable(),
                MEMBERCLOSEDATE = DateTime.Now.AddYears(20).Comparable(),
                BIRTHDATE = DateTime.Now.AddYears(-20).Comparable(),
                FIRSTNAME = StrongRandom.FirstName(),
                LASTNAME = StrongRandom.LastName(),
                MIDDLENAME = StrongRandom.FirstName(),
                NAMEPREFIX = "AUTO",
                NAMESUFFIX = "MATION",
                USERNAME = StrongRandom.Username(),
                PRIMARYEMAILADDRESS = StrongRandom.Email(),
                PRIMARYPHONENUMBER = StrongRandom.PhoneNumber(),
                PRIMARYPOSTALCODE = StrongRandom.USZipcode(),
                PREFERREDLANGUAGE = "en",
                ISEMPLOYEE = 0,
                CHANGEDBY = "Hertz_Automation",
                MEMBERSTATUS = MemberStatus.Active,
                style = memberStyle
            };
            if(memberStyle == MemberStyle.PreProjectOne)
            {
                VirtualCard virtualCard = VirtualCard.Generate(member);
                virtualCard.MemberDetails.Add(DataModels.MemberDetails.GenerateMemberDetails(member, program));
                virtualCard.MemberPreferences.Add(DataModels.MemberPreferences.Generate());
                member.VirtualCards.Add(virtualCard);
                member.ALTERNATEID = virtualCard.LOYALTYIDNUMBER;
            }
            else if(memberStyle == MemberStyle.ProjectOne)
            {
                member.MemberDetails.Add(DataModels.MemberDetails.GenerateMemberDetails(member, program));
                member.MemberPreferences.Add(DataModels.MemberPreferences.Generate());
                VirtualCard vc = VirtualCard.Generate(member);
                member.VirtualCards.Add(vc);
                member.ALTERNATEID = vc.LOYALTYIDNUMBER;
            }
            return member;
        }

        #region Retrieve Members from DB
        public static string GetUnusedLoyaltyId(IDatabase db)
        {
            string query = "select * from bp_htz.lw_virtualcard  WHERE REGEXP_LIKE(loyaltyidnumber, '^[[:digit:]]+$') and Length(loyaltyidnumber) = 13 order by to_number(loyaltyidnumber) desc";
            Hashtable result = db.QuerySingleRow(query);
            try
            {
                ulong lastLID = Convert.ToUInt64(result["LOYALTYIDNUMBER"]);
                lastLID += 1;
                return lastLID.ToString();
            }
            catch(Exception ex)
            {
                return StrongRandom.NumericString(13);
            }
        }
        public static Member GetFromDB(IDatabase db, decimal ipcode, MemberStyle memberStyle = MemberStyle.PreProjectOne)
        {
            string query = $"select * from {dbUser}.{TableName} where IPCODE = {ipcode}";
            return popluateMemberFromQuery(db, memberStyle, query);
        }
        public static Member GetFromDB(IDatabase db, long memberStatus, MemberStyle memberStyle = MemberStyle.PreProjectOne)
        {
            string query = $"select * from {dbUser}.{TableName} where MEMBERSTATUS = {memberStatus}";
            return popluateMemberFromQuery(db, memberStyle, query);
        }
        public static Member GetFromDB(IDatabase db, MemberStyle memberStyle = MemberStyle.PreProjectOne, long memberStatus = 1, string earningPref = null, string tierCode = null, bool? hasTranscations = null)
        {
            StringBuilder query = new StringBuilder();
            query.Append($"select mem.* from {dbUser}.{TableName} mem, {dbUser}.{DataModels.MemberDetails.TableName} md where");
            query.Append($" mem.MEMBERSTATUS = {memberStatus}");
            if (!String.IsNullOrEmpty(earningPref)) query.Append($" and md.a_ipcode = mem.ipcode and md.a_earningpreference = '{earningPref}'");
            if (!String.IsNullOrEmpty(tierCode)) query.Append($" and md.a_ipcode = mem.ipcode and md.a_tiercode = '{tierCode}'");
            //if(hasTranscations.HasValue)
            //{
            //    if (!hasTranscations.Value)
            //        query.Append($" and (select count(*) from {dbUser}.{DataModels.TxnHeader.TableName} where a_vckey = md.a_vckey) = 0");
            //    else query.Append($" and (select count(*) from {dbUser}.{DataModels.TxnHeader.TableName} where a_vckey = md.a_vckey) > 1");
            //}
            return popluateMemberFromQuery(db, memberStyle, query.ToString());
        }
        private static Member popluateMemberFromQuery(IDatabase db, MemberStyle memberStyle, string query)
        {
            Member memberOut = db.QuerySingleRow<Member>(query);
            if (memberStyle == MemberStyle.PreProjectOne)
            {
                memberOut.VirtualCards = db.Query<VirtualCard>($"select * from {dbUser}.{VirtualCard.TableName} where IPCODE = {memberOut.IPCODE}").ToList();
                foreach (VirtualCard dbVC in memberOut.VirtualCards)
                {
                    dbVC.MemberDetails = db.Query<MemberDetails>($"select * from {dbUser}.{DataModels.MemberDetails.TableName} where A_VCKEY = {dbVC.VCKEY}").ToList();
                    dbVC.MemberPreferences = db.Query<MemberPreferences>($"select * from {dbUser}.{DataModels.MemberPreferences.TableName} where A_VCKEY = {dbVC.VCKEY}").ToList();
                }
            }
            else if (memberStyle == MemberStyle.ProjectOne)
            {
                memberOut.MemberDetails = db.Query<MemberDetails>($"select * from {dbUser}.{DataModels.MemberDetails.TableName} where A_IPCODE = {memberOut.IPCODE}").ToList();
                memberOut.MemberPreferences = db.Query<MemberPreferences>($"select * from {dbUser}.{DataModels.MemberPreferences.TableName} where A_IPCODE = {memberOut.IPCODE}").ToList();
                memberOut.VirtualCards = db.Query<VirtualCard>($"select * from {dbUser}.{VirtualCard.TableName} where IPCODE = {memberOut.IPCODE}").ToList();
            }
            memberOut.style = memberStyle;
            return memberOut;
        }
        #endregion

        #region LoyaltyWare Methods
        public static Member AddMember(Member member)
        {
            Member memberOut = new Member();
            using (ConsoleCapture capture = new ConsoleCapture())
            {
                try
                {
                    using (LWIntegrationSvcClientManager client = new LWIntegrationSvcClientManager(EnvironmentManager.Get.SOAPServiceURL, "CDIS", true, string.Empty))
                    {
                        var lwMemberIn = ConvertToLWModel(member);
                        var lwMemberOut = client.AddMember(lwMemberIn, string.Empty, out double time);
                        TestManager.Instance.AddAttachment<TestStep>(MethodBase.GetCurrentMethod().Name, capture.Output, Attachment.Type.Text);
                        memberOut = ConvertFromLWModel(lwMemberOut, member.style);
                        memberOut.style = member.style;
                        return memberOut;
                    }
                }
                catch (LWClientException ex)
                {
                    TestManager.Instance.AddAttachment<TestStep>(MethodBase.GetCurrentMethod().Name, capture.Output, Attachment.Type.Text);
                    throw new LWServiceException(ex.Message, ex.ErrorCode);
                }
                catch (Exception ex)
                {
                    TestManager.Instance.AddAttachment<TestStep>(MethodBase.GetCurrentMethod().Name, capture.Output, Attachment.Type.Text);
                    throw new MemberException($"Unhandled exception throw in {MethodBase.GetCurrentMethod().Name}", ex.Message);
                }
            }
        }
        public static IEnumerable<Member> GetMembers(MemberStyle style ,string[] searchType, string[] searchValue, int? startIndex, int? batchSize, string id)
        {
            List<Member> membersOut = new List<Member>();
            using (ConsoleCapture capture = new ConsoleCapture())
            {
                try
                {
                    using (LWIntegrationSvcClientManager client = new LWIntegrationSvcClientManager(EnvironmentManager.Get.SOAPServiceURL, "CDIS", true, string.Empty))
                    {
                        var lwMembersOut = client.GetMembers(searchType,searchValue,startIndex,batchSize,id, out double time);
                        TestManager.Instance.AddAttachment<TestStep>(MethodBase.GetCurrentMethod().Name, capture.Output, Attachment.Type.Text);
                        foreach (var lwMemberOut in lwMembersOut)
                        {
                            Member memberOut = ConvertFromLWModel(lwMemberOut, style);
                            memberOut.style = style;
                            membersOut.Add(memberOut);
                        }
                        return membersOut;
                    }
                }
                catch (LWClientException ex)
                {
                    TestManager.Instance.AddAttachment<TestStep>(MethodBase.GetCurrentMethod().Name, capture.Output, Attachment.Type.Text);
                    throw new LWServiceException(ex.Message, ex.ErrorCode);
                }
                catch (Exception ex)
                {
                    TestManager.Instance.AddAttachment<TestStep>(MethodBase.GetCurrentMethod().Name, capture.Output, Attachment.Type.Text);
                    throw new MemberException($"Unhandled exception throw in{MethodBase.GetCurrentMethod().Name}.", ex.Message);
                }
            }
        }
        public static Member UpdateMember(Member member)
        {
            Member memberOut = new Member();
            using (ConsoleCapture capture = new ConsoleCapture())
            {
                try
                {
                    using (LWIntegrationSvcClientManager client = new LWIntegrationSvcClientManager(EnvironmentManager.Get.SOAPServiceURL, "CDIS", true, string.Empty))
                    {
                        var lwMemberIn = ConvertToLWModel(member);
                        var lwMemberOut = client.UpdateMember(lwMemberIn, string.Empty, out double time);
                        TestManager.Instance.AddAttachment<TestStep>(MethodBase.GetCurrentMethod().Name, capture.Output, Attachment.Type.Text);
                        memberOut = ConvertFromLWModel(lwMemberOut, member.style);
                        memberOut.style = member.style;
                        return memberOut;
                    }
                }
                catch (LWClientException ex)
                {
                    TestManager.Instance.AddAttachment<TestStep>(MethodBase.GetCurrentMethod().Name, capture.Output, Attachment.Type.Text);
                    throw new LWServiceException(ex.Message, ex.ErrorCode);
                }
                catch (Exception ex)
                {
                    TestManager.Instance.AddAttachment<TestStep>(MethodBase.GetCurrentMethod().Name, capture.Output, Attachment.Type.Text);
                    throw new MemberException($"Unhandled exception throw in {MethodBase.GetCurrentMethod().Name}", ex.Message);
                }
            }
        }
        public Member AddRandomTransaction(IHertzProgram Hpgm, decimal? vckey = null )
        {
            VirtualCard memberCard;
            if (vckey != null)
                memberCard = this.VirtualCards.Find(x => x.VCKEY == vckey.Value);
            else memberCard = this.VirtualCards.FirstOrDefault();

            memberCard.TxnHeaders.Add(TxnHeader.Generate(memberCard.LOYALTYIDNUMBER,program: Hpgm, qualifyingAmount:100M,
                                                         checkInDate: DateTime.Now.Comparable(),
                                                         checkOutDate: DateTime.Now.AddDays(-1).Comparable()));
            return this;
        }
        public Member AddTransaction(TxnHeader txn, decimal? vckey = null)
        {
            VirtualCard memberCard;
            if (vckey != null)
                memberCard = this.VirtualCards.Find(x => x.VCKEY == vckey.Value);
            else memberCard = this.VirtualCards.FirstOrDefault();

            memberCard.TxnHeaders.Add(txn);
            return this;
        }
        public MemberPromotion AddPromotion(string memberIdentity, string promotionCode, string programCode, string certificateNumber,
                                    bool? returnDefinition, string language, string channel, bool? returnAttributes)
        {
            MemberPromotion memberPromotionOut = new MemberPromotion();


            using (ConsoleCapture capture = new ConsoleCapture())
            {
                try
                {
                    using (LWIntegrationSvcClientManager client = new LWIntegrationSvcClientManager(EnvironmentManager.Get.SOAPServiceURL, "CDIS", true, string.Empty))
                    {
                        var lwMemberPromotion = client.AddMemberPromotion(memberIdentity, promotionCode, programCode, certificateNumber, returnDefinition, language, channel, returnAttributes, String.Empty, out double time);
                        TestManager.Instance.AddAttachment<TestStep>(MethodBase.GetCurrentMethod().Name, capture.Output, Attachment.Type.Text);
                        if (lwMemberPromotion != null)
                        {
                            memberPromotionOut = DataConverter.ConvertTo<MemberPromotion>(lwMemberPromotion);
                            if (lwMemberPromotion.PromotionDefinition != null)
                                memberPromotionOut.PromotionDefinition = DataConverter.ConvertTo<Promotion>(lwMemberPromotion.PromotionDefinition);
                        }
                        return memberPromotionOut;
                    }
                }
                catch (LWClientException ex)
                {
                    TestManager.Instance.AddAttachment<TestStep>(MethodBase.GetCurrentMethod().Name, capture.Output, Attachment.Type.Text);
                    throw new LWServiceException(ex.Message, ex.ErrorCode);
                }
                catch (Exception ex)
                {
                    TestManager.Instance.AddAttachment<TestStep>(MethodBase.GetCurrentMethod().Name, capture.Output, Attachment.Type.Text);
                    throw new MemberException($"Unhandled exception throw in {MethodBase.GetCurrentMethod().Name}", ex.Message);
                }
            }
        }

        #endregion

        #region LoyaltyWare DLL Data Conversion
        private static Brierley.LoyaltyWare.ClientLib.DomainModel.Framework.Member ConvertToLWModel(Member member)
        {
            if (member.style == MemberStyle.PreProjectOne) return ConvertToPreProjectOneLWModel(member);
            else if (member.style == MemberStyle.ProjectOne) return ConvertToProjectOneLWModel(member);
            else throw new Exception($"Unknown MemberStyle in CovertToLWModel: {member.style.ToString()}");
        }
        private static Member ConvertFromLWModel(Brierley.LoyaltyWare.ClientLib.DomainModel.Framework.Member lwMember, MemberStyle expectedStyle)
        {
            if (expectedStyle == MemberStyle.PreProjectOne) return ConvertFromPreProjectOneLWModel(lwMember);
            else if (expectedStyle == MemberStyle.ProjectOne) return ConvertFromProjectOneLWModel(lwMember);
            else throw new Exception($"Unknown MemberStyle in CovertFromLWModel: {expectedStyle.ToString()}");
        }
        private static Member ConvertFromPreProjectOneLWModel(Brierley.LoyaltyWare.ClientLib.DomainModel.Framework.Member lwMember)
        {
            Member member = DataConverter.ConvertTo<Member>(lwMember);

            IList<Brierley.LoyaltyWare.ClientLib.DomainModel.LWAttributeSetContainer> lwVirtualCards = lwMember.GetAttributeSets("VirtualCard");
            foreach (var lwVirtualCard in lwVirtualCards)
            {
                VirtualCard virtualCard = (DataConverter.ConvertTo<VirtualCard>(lwVirtualCard));

                IList<Brierley.LoyaltyWare.ClientLib.DomainModel.LWAttributeSetContainer> lwMemberPreferences = lwVirtualCard.GetAttributeSets("MemberPreferences");
                IList<Brierley.LoyaltyWare.ClientLib.DomainModel.LWAttributeSetContainer> lwMemberDetails = lwVirtualCard.GetAttributeSets("MemberDetails");
                IList<Brierley.LoyaltyWare.ClientLib.DomainModel.LWAttributeSetContainer> lwTxnHeaders = lwVirtualCard.GetAttributeSets("TxnHeader");

                foreach (var lwMemberPreference in lwMemberPreferences)
                    virtualCard.MemberPreferences.Add(DataConverter.ConvertTo<MemberPreferences>(lwMemberPreference));
                foreach (var lwMemberDetail in lwMemberDetails)
                    virtualCard.MemberDetails.Add(DataConverter.ConvertTo<MemberDetails>(lwMemberDetail));
                foreach (var lwTxnHeader in lwTxnHeaders)
                    virtualCard.TxnHeaders.Add(DataConverter.ConvertTo<TxnHeader>(lwTxnHeader));

                member.VirtualCards.Add(virtualCard);
            }

            return member;
        }
        private static Member ConvertFromProjectOneLWModel(Brierley.LoyaltyWare.ClientLib.DomainModel.Framework.Member lwMember)
        {
            Member member = DataConverter.ConvertTo<Member>(lwMember);

            IList<Brierley.LoyaltyWare.ClientLib.DomainModel.LWAttributeSetContainer> lwVirtualCards = lwMember.GetAttributeSets("VirtualCard");
            foreach (var lwVirtualCard in lwVirtualCards)
            {
                VirtualCard vc = DataConverter.ConvertTo<VirtualCard>(lwVirtualCard);
                var lwTxnHeaders = lwVirtualCard.GetAttributeSets("TxnHeader");
                //  var lwTxnHeaders = lwMember.GetAttributeSets("TxnHeader");
                foreach (var lwTxnHeader in lwTxnHeaders)
                    vc.TxnHeaders.Add(DataConverter.ConvertTo<TxnHeader>(lwTxnHeader));

                member.VirtualCards.Add(vc);
            }

            IList<Brierley.LoyaltyWare.ClientLib.DomainModel.LWAttributeSetContainer> lwMemberPreferences = lwMember.GetAttributeSets("MemberPreferences");
            foreach (var lwMemberPreference in lwMemberPreferences)
                member.MemberPreferences.Add(DataConverter.ConvertTo<MemberPreferences>(lwMemberPreference));

            IList<Brierley.LoyaltyWare.ClientLib.DomainModel.LWAttributeSetContainer> lwMemberDetails = lwMember.GetAttributeSets("MemberDetails");
            foreach (var lwMemberDetail in lwMemberDetails)
                member.MemberDetails.Add(DataConverter.ConvertTo<MemberDetails>(lwMemberDetail));

            return member;
        }
        private static Brierley.LoyaltyWare.ClientLib.DomainModel.Framework.Member ConvertToPreProjectOneLWModel(Member member)
        {
            Brierley.LoyaltyWare.ClientLib.DomainModel.Framework.Member lwMember = DataConverter.ConvertTo<Brierley.LoyaltyWare.ClientLib.DomainModel.Framework.Member>(member);

            foreach (VirtualCard virtualCard in member.VirtualCards)
            {
                var vc = DataConverter.ConvertTo<Brierley.LoyaltyWare.ClientLib.DomainModel.Framework.VirtualCard>(virtualCard);

                if (virtualCard.MemberDetails != null)
                    foreach (MemberDetails memberDetail in virtualCard.MemberDetails)
                        vc.Add(DataConverter.ConvertTo<Brierley.LoyaltyWare.ClientLib.DomainModel.Client.MemberDetails>(memberDetail));

                if(virtualCard.MemberPreferences != null)
                    foreach (MemberPreferences memberPreference in virtualCard.MemberPreferences)
                        vc.Add(DataConverter.ConvertTo<Brierley.LoyaltyWare.ClientLib.DomainModel.Client.MemberPreferences>(memberPreference));

                if (virtualCard.TxnHeaders != null)
                    foreach (TxnHeader txn in virtualCard.TxnHeaders)
                        vc.Add(DataConverter.ConvertTo<Brierley.LoyaltyWare.ClientLib.DomainModel.Client.TxnHeader>(txn));

                lwMember.Add(vc);
            }

            return lwMember;
        }
        private static Brierley.LoyaltyWare.ClientLib.DomainModel.Framework.Member ConvertToProjectOneLWModel(Member member)
        {

            Brierley.LoyaltyWare.ClientLib.DomainModel.Framework.Member lwMember = DataConverter.ConvertTo<Brierley.LoyaltyWare.ClientLib.DomainModel.Framework.Member>(member);
            if(member.MemberPreferences != null)
                foreach (MemberPreferences memberPreference in member.MemberPreferences)
                    lwMember.Add(DataConverter.ConvertTo<Brierley.LoyaltyWare.ClientLib.DomainModel.Client.MemberPreferences>(memberPreference));

            if (member.MemberDetails != null)
                foreach (MemberDetails memberDetail in member.MemberDetails)
                    lwMember.Add(DataConverter.ConvertTo<Brierley.LoyaltyWare.ClientLib.DomainModel.Client.MemberDetails>(memberDetail));

            if (member.VirtualCards != null)
                foreach (VirtualCard virtualCard in member.VirtualCards)
                {
                    var vc = DataConverter.ConvertTo<Brierley.LoyaltyWare.ClientLib.DomainModel.Framework.VirtualCard>(virtualCard);
                    if (virtualCard.TxnHeaders != null)
                    {
                        foreach (TxnHeader txn in virtualCard.TxnHeaders)
                            vc.Add(DataConverter.ConvertTo<Brierley.LoyaltyWare.ClientLib.DomainModel.Client.TxnHeader>(txn));
                    }
                    lwMember.Add(vc);
                }
            

            return lwMember;
        }
        #endregion
    }
}
