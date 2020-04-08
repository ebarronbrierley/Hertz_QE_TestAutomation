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
using System.Diagnostics;
using System.Collections;
using Brierley.LoyaltyWare.ClientLib.DomainModel;
using Hertz.Database.DataModels;

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

        #region LoyaltyWare Methods
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
        public IEnumerable<MemberModel> GetMembers(string[] searchType, string[] searchValue, int? startIndex, int? batchSize, string id)
        {
            List<MemberModel> membersOut = new List<MemberModel>();
            using(ConsoleCapture capture = new ConsoleCapture())
            {
                try
                {
                    var lwMembersOut = lwSvc.GetMembers(searchType, searchValue, startIndex, batchSize, id, out double time);
                    foreach(var lwMem in lwMembersOut)
                    {
                        membersOut.Add(LODConvert.FromLW<MemberModel>(lwMem));
                    }
                }
                catch(LWClientException ex)
                {
                    throw new LWServiceException(ex.Message, ex.ErrorCode);
                }
                catch(Exception ex)
                {
                    throw new LWServiceException(ex.Message, -1);
                }
                finally
                {
                    stepContext.AddAttachment(new Attachment("GetMembers", capture.Output, Attachment.Type.Text));
                }
            }
            return membersOut;
        }
        public MemberModel UpdateMember(MemberModel member)
        {
            MemberModel memOut = default;
            using (ConsoleCapture capture = new ConsoleCapture())
            {
                try
                {
                    var lwMemIn = LODConvert.ToLW<Member>(member);
                    var lwMemOut = lwSvc.UpdateMember(lwMemIn, String.Empty, out double time);
                    memOut = LODConvert.FromLW<MemberModel>(lwMemOut);
                }
                catch (LWClientException ex)
                {
                    throw new LWServiceException(ex.Message, ex.ErrorCode);
                }
                catch (Exception ex)
                {
                    throw new LWServiceException(ex.Message, -1);
                }
                finally
                {
                    stepContext.AddAttachment(new Attachment("UpdateMember", capture.Output, Attachment.Type.Text));
                }
            }
            return memOut;
        }
        public MemberPromotionModel AddMemberPromotion(string loyaltyId, string promotionCode,string programCode, string certificateNumber,
                                    bool? returnDefinition, string language, string channel, bool? returnAttributes)
        {
            MemberPromotionModel memberPromoOut = default;
            using(ConsoleCapture capture = new ConsoleCapture())
            {
                try
                {
                    var lwMemberPromo = lwSvc.AddMemberPromotion(loyaltyId, promotionCode, promotionCode, certificateNumber, returnDefinition, language, channel, returnDefinition, String.Empty, out double time);
                    memberPromoOut = LODConvert.FromLW<MemberPromotionModel>(lwMemberPromo);
                }
                catch(LWClientException ex)
                {
                    throw new LWServiceException(ex.Message, ex.ErrorCode);
                }
                catch(Exception ex)
                {
                    throw new LWServiceException(ex.Message, -1);
                }
                finally
                {
                    stepContext.AddAttachment(new Attachment("AddMemberPromotion", capture.Output, Attachment.Type.Text));
                }
            }
            return memberPromoOut;
        }

        public AddMemberRewardsResponseModel AddMemberReward(string alternateID, string rewardTypeCode, string programCode)
        {
            using (ConsoleCapture capture = new ConsoleCapture())
            {
                AddMemberRewardsResponseModel memberRewardsOut = default;
                try
                {
                    double time = 0;
                    RewardOrderInfoStruct[] rewardInfoStruct = new RewardOrderInfoStruct[1];
                    RewardOrderInfoStruct rewardInfo = new RewardOrderInfoStruct();
                    rewardInfo.TypeCode = rewardTypeCode;
                    rewardInfoStruct[0] = rewardInfo;
                    string changedBy = "oagwuegbo";
                    var lwMemberReward = lwSvc.AddMemberRewards(alternateID, null, programCode, null, null, null, null, null, null, null, null, null, null, null, null, null, changedBy, rewardInfoStruct, string.Empty, out time);
                    memberRewardsOut = LODConvert.FromLW<AddMemberRewardsResponseModel>(lwMemberReward);
                }
                catch (LWClientException ex)
                {
                    throw new LWServiceException(ex.Message, ex.ErrorCode);
                }
                finally
                {
                    stepContext.AddAttachment(new Attachment("AddMemberRewards", capture.Output, Attachment.Type.Text));
                }

                return memberRewardsOut;
            }
            
        }
        public long CancelMemberReward(string memberRewardId, string programCode, string resvId, DateTime? chkoutDt,
                                        string chkoutAreanum, string chkoutLocNum, string chkoutLocId, string externalId)
        {
            using (ConsoleCapture capture = new ConsoleCapture())
            {
                try
                {
                    return lwSvc.CancelMemberReward(Convert.ToInt64(memberRewardId), resvId, chkoutDt, chkoutAreanum, chkoutLocNum, chkoutLocId, programCode, externalId, out double time);
                }
                catch (LWClientException ex)
                {
                    throw new LWServiceException(ex.Message, ex.ErrorCode);
                }
                catch (Exception ex)
                {
                    throw new LWServiceException(ex.Message, -1);
                }
                finally
                {
                    stepContext.AddAttachment(new Attachment("CancelMemberReward", capture.Output, Attachment.Type.Text));
                }
            }
        }
        public MemberRewardSummaryModel GetMemberRewardSummaryById(long memberRewardId, string language, string programCode, string externalId)
        {
            using (ConsoleCapture capture = new ConsoleCapture())
            {
                MemberRewardSummaryModel memberRewardsModelOut = default;
                try
                {
                    var lwOut = lwSvc.GetMemberRewardSummaryById(memberRewardId, language, programCode, externalId, out double time);
                    memberRewardsModelOut = LODConvert.FromLW<MemberRewardSummaryModel>(lwOut.MemberRewardSummary);
                    return memberRewardsModelOut;
                }
                catch (LWClientException ex)
                {
                    throw new LWServiceException(ex.Message, ex.ErrorCode);
                }
                catch (Exception ex)
                {
                    throw new LWServiceException(ex.Message, -1);
                }
                finally
                {
                    stepContext.AddAttachment(new Attachment("GetMemberRewardSummaryById", capture.Output, Attachment.Type.Text));
                }
            }
        }
        public AwardLoyaltyCurrencyResponseModel AwardLoyaltyCurrency(string loyaltyID, decimal points)
        {
            using (ConsoleCapture capture = new ConsoleCapture())
            {
                AwardLoyaltyCurrencyResponseModel awardLoyaltyCurrencyOut = default;
                try
                {
                    double time = 0;
                    string changedBy = "oagwuegbo";
                    long pointeventID = 6263265;
                    DateTime pointExpirationDate = DateTime.Now.AddMonths(18);
                    var lwAwardLoyalty = lwSvc.HertzAwardLoyaltyCurrency(loyaltyID, changedBy, points, pointeventID, "Automated Appeasement", null, null, out time);
                    awardLoyaltyCurrencyOut = LODConvert.FromLW<AwardLoyaltyCurrencyResponseModel>(lwAwardLoyalty);
                }
                catch (LWClientException ex)
                {
                    throw new LWServiceException(ex.Message, ex.ErrorCode);
                }
                catch (Exception ex)
                {
                    throw new LWServiceException(ex.Message, -1);
                }
                finally
                {
                    stepContext.AddAttachment(new Attachment("AwardLoyaltyCurrency", capture.Output, Attachment.Type.Text));
                }
                return awardLoyaltyCurrencyOut;
            }
        }
        public MemberAccountSummaryModel GetAccountSummary(string loyaltyId, string programCode, string externalId)
        {
            MemberAccountSummaryModel memberAccountSummaryOut = default;
            using (ConsoleCapture capture = new ConsoleCapture())
            {
                try
                {
                    var lwMemberPromo = lwSvc.GetAccountSummary(loyaltyId, programCode, externalId, out double time);
                    memberAccountSummaryOut = LODConvert.FromLW<MemberAccountSummaryModel>(lwMemberPromo);
                }
                catch (LWClientException ex)
                {
                    throw new LWServiceException(ex.Message, ex.ErrorCode);
                }
                catch (Exception ex)
                {
                    throw new LWServiceException(ex.Message, -1);
                }
                finally
                {
                    stepContext.AddAttachment(new Attachment("GetAccountSummary", capture.Output, Attachment.Type.Text));
                }
            }
            return memberAccountSummaryOut;
        }
        public List<MemberPromotionModel> GetMemberPromotion(string loyaltyId, int? startIndex, int? batchSize, bool? returnDefinition,
                                            string language, string channel, bool? returnAttributes, string externalId)
        {
            List<MemberPromotionModel> memberPromoOut = new List<MemberPromotionModel>();
            using (ConsoleCapture capture = new ConsoleCapture())
            {
                try
                {
                    var lwMemberPromotions = lwSvc.GetMemberPromotions(loyaltyId, startIndex, batchSize, returnDefinition, language, channel, returnAttributes, externalId, out double time);

                    foreach (var lwMp in lwMemberPromotions)
                    {
                        memberPromoOut.Add(LODConvert.FromLW<MemberPromotionModel>(lwMp));
                    }
                }
                catch (LWClientException ex)
                {
                    throw new LWServiceException(ex.Message, ex.ErrorCode);
                }
                catch (Exception ex)
                {
                    throw new LWServiceException(ex.Message, -1);
                }
                finally
                {
                    stepContext.AddAttachment(new Attachment("GetMemberPromotion", capture.Output, Attachment.Type.Text));
                }
            }
            return memberPromoOut;
        }
        public int GetMemberPromotionCount(string ipCode)
        {
            using (ConsoleCapture capture = new ConsoleCapture())
            {
                try
                {
                    var lwMemberPromoCount = lwSvc.GetMemberPromotionsCount(ipCode, null, out double time);
                    return lwMemberPromoCount;
                }
                catch (LWClientException ex)
                {
                    throw new LWServiceException(ex.Message, ex.ErrorCode);
                }
                catch (Exception ex)
                {
                    throw new LWServiceException(ex.Message, -1);
                }
                finally
                {
                    stepContext.AddAttachment(new Attachment("GetMemberPromotionCount", capture.Output, Attachment.Type.Text));
                }
            }
        }
        public List<GetMemberRewardsResponseModel> GetMemberRewards(string loyaltyID)
        {
            using (ConsoleCapture capture = new ConsoleCapture())
            {
                List<GetMemberRewardsResponseModel> getMemberRewardsOut = new List<GetMemberRewardsResponseModel>();
                MemberRewardsModel memberRewardsOut = default;
                try
                {
                    var lwGetMemberRewards = lwSvc.GetMemberRewards(loyaltyID, null, null, null, null, null, null, out double time);
                    foreach (var lwGMR in lwGetMemberRewards)
                    {
                        GetMemberRewardsResponseModel temp = LODConvert.FromLW<GetMemberRewardsResponseModel>(lwGMR);
                        memberRewardsOut = LODConvert.FromLW<MemberRewardsModel>(lwGMR.MemberRewardInfo[0]);
                        temp.MemberRewardsInfo = new List<MemberRewardsModel>();
                        temp.MemberRewardsInfo.Add(memberRewardsOut);
                        getMemberRewardsOut.Add(temp);
                        memberRewardsOut = default;
                    }
                }
                catch (LWClientException ex)
                {
                    throw new LWServiceException(ex.Message, ex.ErrorCode);
                }
                finally
                {
                    stepContext.AddAttachment(new Attachment("GetMemberRewards", capture.Output, Attachment.Type.Text));
                }
                return getMemberRewardsOut;

            }
        }
        #endregion

        #region Database Methods
        public MemberModel GetFromDB(decimal? ipCode=null, string ipCodeQuery = null)
        {
            string query = String.Empty;

            if (ipCode == null && !String.IsNullOrEmpty(ipCodeQuery))
            {
                query = ipCodeQuery;
            }
            else
            {
                query = $"select * from {MemberModel.TableName} where IPCODE = {ipCode.Value}";
            }

            MemberModel member =  dbContext.QuerySingleRow<MemberModel>(query);
            member.MemberDetails = dbContext.QuerySingleRow<MemberDetailsModel>($"select * from {MemberDetailsModel.TableName} where A_IPCODE = {member.IPCODE}");
            member.MemberPreferences = dbContext.QuerySingleRow<MemberPreferencesModel>($"select * from {MemberPreferencesModel.TableName} where A_IPCODE = {member.IPCODE}");
            member.VirtualCards = dbContext.Query<VirtualCardModel>($"select * from {VirtualCardModel.TableName} where IPCODE = {member.IPCODE}").ToList();
            return member;
        }
        public  MemberModel GetRandomFromDB(long memberStatus, IHertzTier tier = null)
        {
            StringBuilder sb = new StringBuilder();
            //sample is percentage
            string sql = @"
select lm.*
  from BP_HTZ.LW_LOYALTYMEMBER SAMPLE(1) lm 
  join bp_htz.ats_memberdetails md on lm.ipcode = md.a_ipcode

 where lm.MEMBERSTATUS = 1
";
            sb.Append(sql);
            if (tier != null)
            {
                sb.Append(String.Format(" and md.a_earningpreference = 'N1' and md.a_tiercode = '{0}'  ", tier.Code));
            }
            return GetFromDB(ipCodeQuery: sb.ToString());
        } 
        public MemberModel GetRandomMemberDBForMemberPromotion(long memberStatus)
        {
            string query = string.Format($@"select lm.ipcode from BP_HTZ.lw_loyaltymember lm
                            where not exists (select NULL from BP_HTZ.Lw_Memberpromotion mp where mp.memberid = lm.ipcode)
                            and lm.memberstatus = {memberStatus}
                            and rownum <=1");

            var retVal = dbContext.QuerySingleRow(query);

            return GetFromDB((decimal)retVal["IPCODE"]);
        }
        public IEnumerable<MemberPromotionModel> GetMemberPromotionsFromDB(decimal? id = null, string code = null, decimal? memberId = null)
        {
            StringBuilder query = new StringBuilder();
            query.Append($"select * from {MemberPromotionModel.TableName}");

            if (id == null && code == null && memberId == null) return new List<MemberPromotionModel>();

            query.Append($" where ");

            List<string> queryParams = new List<string>();
            if (id != null) queryParams.Add($" id = {id} ");
            if (code != null) queryParams.Add($" code = '{code}' ");
            if (memberId != null) queryParams.Add($" memberId = '{memberId.Value}' ");

            query.Append(String.Join(" and ", queryParams));
            return dbContext.Query<MemberPromotionModel>(query.ToString());
        }

        public IEnumerable<MemberRewardsModel> GetMemberRewardsFromDB(decimal? ipcode, long? memberrewardid)
        {
            string ipcodestring = ipcode.ToString();
            string memberrewardidstring = memberrewardid.ToString();
            StringBuilder query = new StringBuilder();
            query.Append($"select * from {MemberRewardsModel.TableName}");

            if (ipcode == null && memberrewardid == null) return new List<MemberRewardsModel>();

            query.Append($" where ");
            List<string> queryParams = new List<string>();
            if (ipcode != null) queryParams.Add($" memberid = '{ipcode}' ");
            if (memberrewardid != null) queryParams.Add($" id = '{memberrewardid}' ");

            query.Append(String.Join(" and ", queryParams));
            return dbContext.Query<MemberRewardsModel>(query.ToString());
        }

        public IEnumerable<MemberRewardSummaryModel> GetMemberRewardsummaryFromDB(decimal? ipcode, long? memberrewardid)
        {
            string query = $@"select rd.howmanypointstoearn as POINTSCONSUMED, rd.name as REWARDNAME, mr.dateissued, mr.expiration from BP_HTZ.Lw_Memberrewards mr 
                                inner join BP_HTZ.Lw_Rewardsdef rd on rd.id = mr.rewarddefid
                                where mr.ID = {memberrewardid} and mr.memberid = {ipcode}";

            return dbContext.Query<MemberRewardSummaryModel>(query.ToString());
        }

        public decimal GetPointSumFromDB(string loyaltyid)
        {
            StringBuilder query = new StringBuilder();
            query.Append($"SELECT SUM(PTT.POINTS) FROM {PointTransactionModel.TableName} PTT INNER JOIN BP_HTZ.LW_VIRTUALCARD VC ON VC.VCKEY = PTT.VCKEY WHERE 1=1 AND VC.LOYALTYIDNUMBER = '{loyaltyid}'");
            Hashtable ht = dbContext.QuerySingleRow(query.ToString());
            decimal pointsOut = (decimal)ht["SUM(PTT.POINTS)"];
            return pointsOut;
        }

        public IEnumerable<MemberModel> GetMembersFromDB(decimal? ipCode = null, string ipCodeQuery = null)
        {
            string query = String.Empty;

            if (ipCode == null && !String.IsNullOrEmpty(ipCodeQuery))
            {
                query = ipCodeQuery;
            }
            else
            {
                query = $"select * from {MemberModel.TableName} where IPCODE = {ipCode.Value}";
            }

            return dbContext.Query<MemberModel>(query);
        }
        public MemberAccountSummaryModel GetMemberAccountSummaryFromDB(string vckey)
        {
            StringBuilder query = new StringBuilder();

            query.Append("select SUM(p.points) as currencybalance, lm.memberstatus, vc.createdate, md.a_tierenddate, md.a_lastactivitydate, md.a_mktgprogramid");
            query.Append(" ,case when md.a_tiercode = 'RG' THEN 'Gold'");
            query.Append(" when md.a_tiercode = 'FG' THEN 'Five Star'");
            query.Append(" when md.a_tiercode = 'PC' THEN 'Presidents Circle'");
            query.Append(" when md.a_tiercode = 'PL' THEN 'Platinum'");
            query.Append(" when md.a_tiercode = 'PS' THEN 'Platinum Select'");
            query.Append(" when md.a_tiercode = 'VP' THEN 'Platinum VIP'");         
            query.Append(" END as CURRENTTIERNAME");
            query.Append(",case when lm.memberstatus = 1 THEN 'Active'");
            query.Append(" when lm.memberstatus = 2 THEN 'Disabled'");
            query.Append(" END as MEMBERSTATUS");
            query.Append(" from bp_htz.ats_memberdetails md");
            query.Append(" inner join bp_htz.lw_virtualcard vc on vc.ipcode = md.a_ipcode");
            query.Append(" inner join bp_htz.lw_loyaltymember lm on lm.ipcode = vc.ipcode ");
            query.Append(" left join (select pt.vckey, pt.points, pt.expirationdate from bp_htz.lw_pointtransaction pt");
            query.Append(" where pt.expirationdate > CURRENT_TIMESTAMP)p on p.vckey = vc.vckey");
            query.Append($" where vc.vckey = {vckey}");
            query.Append(" group by lm.memberstatus, vc.createdate, md.a_tierenddate, md.a_lastactivitydate, md.a_mktgprogramid");
            query.Append(" ,case when md.a_tiercode = 'RG' THEN 'Gold'");
            query.Append(" when md.a_tiercode = 'FG' THEN 'Five Star'");
            query.Append(" when md.a_tiercode = 'PC' THEN 'Presidents Circle'");
            query.Append(" when md.a_tiercode = 'PL' THEN 'Platinum'");
            query.Append(" when md.a_tiercode = 'PS' THEN 'Platinum Select'");
            query.Append(" when md.a_tiercode = 'VP' THEN 'Platinum VIP' END");
            query.Append(" ,case when lm.memberstatus = 1 THEN 'Active'");
            query.Append(" when lm.memberstatus = 2 THEN 'Disabled'");
            query.Append(" END");         

            MemberAccountSummaryModel memberAccountSummary = dbContext.QuerySingleRow<MemberAccountSummaryModel>(query.ToString());
            return memberAccountSummary;
        }

        public MemberModel AssignUniqueLIDs(MemberModel member)
        {
            foreach(var virtualCard in member.VirtualCards)
            {
                virtualCard.LOYALTYIDNUMBER = GetUniqueLID();
            }
            return member;
        }
        public string GetUniqueLID()
        {
            Stopwatch timer = new Stopwatch();
            TimeSpan timeout = new TimeSpan(0, 0, 10);
            timer.Start();
            bool timedOut = false;
            int attempts = 0;
            int lidSize = 7;

            while(!timedOut)
            {
                string potentialLID = StrongRandom.NumericString(lidSize);

                if (String.IsNullOrEmpty(dbContext.QuerySingleColumn<string>($"select LOYALTYIDNUMBER from {VirtualCardModel.TableName} where LOYALTYIDNUMBER = '{potentialLID}';")))
                    return potentialLID;

                if (timer.ElapsedTicks > timeout.Ticks)
                {
                    timedOut = true;
                }
                ++attempts;
            }

            //If we couldn't generate anything above, try increasing the size once
            string biggerLIDAttempt = StrongRandom.NumericString(++lidSize);
            if (String.IsNullOrEmpty(dbContext.QuerySingleColumn<string>($"select LOYALTYIDNUMBER from {VirtualCardModel.TableName} where LOYALTYIDNUMBER = '{biggerLIDAttempt}';")))
                return biggerLIDAttempt;

            //If we get here, we timed out
            throw new Exception($"Timed out attempting to find unique Loyalty Id. Attempted generating {attempts} times.");
        }
        #endregion

        #region Static Generation Methods
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

            if (String.IsNullOrEmpty(tier.Code)) details.A_TIERENDDATE = null;

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
        #endregion

        public HertzAwardLoyaltyCurrencyResponseModel HertzAwardLoyaltyCurrency(string loyaltyId, string agent, decimal points, long pointeventid, string reasoncode, string rentalagreementnumber)
        {
            HertzAwardLoyaltyCurrencyResponseModel memberAwardCurrency = default;
            using (ConsoleCapture capture = new ConsoleCapture())
            {
                try
                {
                    var lwAwardCurrency = lwSvc.HertzAwardLoyaltyCurrency(loyaltyId, agent, points, pointeventid, reasoncode, rentalagreementnumber, String.Empty, out double time);

                    memberAwardCurrency = LODConvert.FromLW<HertzAwardLoyaltyCurrencyResponseModel>(lwAwardCurrency);
                }
                catch (LWClientException ex)
                {
                    throw new LWServiceException(ex.Message, ex.ErrorCode);
                }
                catch (Exception ex)
                {
                    throw new LWServiceException(ex.Message, -1);
                }
                finally
                {
                    stepContext.AddAttachment(new Attachment("HertzAwardLoyaltyCurrency", capture.Output, Attachment.Type.Text));
                }
            }
            return memberAwardCurrency;
        }


        public HertzTransferPointsResponseModel HertzTransferPoints(string loyaltyIdSource, string agent, string points, string loyaltyIdDestination, string reasoncode)
        {
            HertzTransferPointsResponseModel hertzTransferPoints = default;
            using (ConsoleCapture capture = new ConsoleCapture())
            {
                try
                {
                    var lwTransferPoints = lwSvc.HertzTransferPoints(loyaltyIdSource, agent, points, loyaltyIdDestination, reasoncode, String.Empty, out double time);

                    hertzTransferPoints = LODConvert.FromLW<HertzTransferPointsResponseModel>(lwTransferPoints);
                }
                catch (LWClientException ex)
                {
                    throw new LWServiceException(ex.Message, ex.ErrorCode);
                }
                catch (Exception ex)
                {
                    throw new LWServiceException(ex.Message, -1);
                }
                finally
                {
                    stepContext.AddAttachment(new Attachment("HertzTransferPoints", capture.Output, Attachment.Type.Text));
                }
            }
            return hertzTransferPoints;
        }

        public HertzUpdateTierResponseModel HertzUpdateTier(string loyaltyMemberId, string agent, string newTier, string newTierEndDate, string marketingCode)
        {
            HertzUpdateTierResponseModel htzUpdateTier = default;
            using (ConsoleCapture capture = new ConsoleCapture())
            {
                try
                {
                    var lwHtzUpdateTier = lwSvc.HertzUpdateTier(loyaltyMemberId, agent, newTier, 
                                            newTierEndDate, marketingCode, String.Empty, out double time);

                    htzUpdateTier = LODConvert.FromLW<HertzUpdateTierResponseModel>(lwHtzUpdateTier);
                }
                catch (LWClientException ex)
                {
                    throw new LWServiceException(ex.Message, ex.ErrorCode);
                }
                catch (Exception ex)
                {
                    throw new LWServiceException(ex.Message, -1);
                }
                finally
                {
                    stepContext.AddAttachment(new Attachment("HertzUpdateTier", capture.Output, Attachment.Type.Text));
                }
            }
            return htzUpdateTier;
        }


        public AddAttributeSetResponseModel AddAttributeSet( VirtualCardModel virtualCard,
            AuctionHeaderModel auctionHeader)
        {
            AddAttributeSetResponseModel addAttributeSetResponse = default;
            using (ConsoleCapture capture = new ConsoleCapture())
            {
                try
                {
                    string loyaltyId = virtualCard?.LOYALTYIDNUMBER ?? string.Empty;
                    AuctionHeader attributeSet = null;
                    if (auctionHeader != null)
                    {
                        attributeSet = new AuctionHeader()
                        {
                            AuctionEventName = auctionHeader.AuctionEventName,
                            AuctionPointType = auctionHeader.AuctionPointType,
                            AuctionTxnType = auctionHeader.AuctionTxnType,
                            CDRewardID = auctionHeader.CDRewardID,
                            HeaderGPRpts = auctionHeader.HeaderGPRpts
                        };
                        
                        if (virtualCard != null)
                        {
                            attributeSet.AddTransientProperty("VcKey", virtualCard.VCKEY);
                        }
                    }
                    
                    var lwAddAttributeSet = lwSvc.AddAttributeSet(loyaltyId,
                        attributeSet, String.Empty, out double time);
                    if(lwAddAttributeSet!=null)
                    {  
                        addAttributeSetResponse = new AddAttributeSetResponseModel()
                        {
                            EarnedPoints = lwAddAttributeSet
                                            .EarnedPoints
                                            .Select(x => new EarnedPointResponseModel()
                                            {
                                                 PointType=x.PointType,
                                                  PointValue=(decimal)(x.PointValue??0)
                                            })
                                            .ToList()
                        };
                    }
                }
                catch (LWClientException ex)
                {
                    throw new LWServiceException(ex.Message, ex.ErrorCode);
                }
                catch (Exception ex)
                {
                    throw new LWServiceException(ex.Message, -1);
                }
                finally
                {
                    stepContext.AddAttachment(new Attachment("AddAttributeSet", capture.Output, Attachment.Type.Text));
                }
            }
            return addAttributeSetResponse;
        }
    }
}
