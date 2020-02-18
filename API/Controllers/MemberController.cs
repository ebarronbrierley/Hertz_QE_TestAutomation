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

        public AddMemberRewardsResponseModel AddMemberReward(string alternateID, string rewardTypeCode, IHertzProgram program)
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
                    string programcode = program.EarningPreference.ToString();
                    var lwMemberReward = lwSvc.AddMemberRewards(alternateID, null, programcode, null, null, null, null, null, null, null, null, null, null, null, null, null, changedBy, rewardInfoStruct, string.Empty, out time);
                    memberRewardsOut = LODConvert.FromLW<AddMemberRewardsResponseModel>(lwMemberReward);
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
                    stepContext.AddAttachment(new Attachment("AddMemberRewards", capture.Output, Attachment.Type.Text));
                }

                return memberRewardsOut;
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
            StringBuilder query = new StringBuilder($"select * from {MemberModel.TableName} SAMPLE(10) mem where mem.MEMBERSTATUS = {memberStatus}");
            if(tier != null)
            {
                query.Append(" and ");
                query.Append($"(select a_earningpreference from {MemberDetailsModel.TableName} where A_IPCODE = mem.IPCODE) = '{tier.ParentProgram.EarningPreference}'");
                query.Append(" and ");
                if(!String.IsNullOrEmpty(tier.Code))
                    query.Append($"(select a_tiercode from {MemberDetailsModel.TableName} where A_IPCODE = mem.IPCODE) = '{tier.Code}'");
                else
                    query.Append($"(select a_tiercode from {MemberDetailsModel.TableName} where A_IPCODE = mem.IPCODE) is null");
            }
            return GetFromDB(ipCodeQuery: query.ToString());
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

            if (ipcode == null || memberrewardid == null) return new List<MemberRewardsModel>();

            query.Append($" where ");
            List<string> queryParams = new List<string>();
            if (ipcode != null) queryParams.Add($" memberid = '{ipcode}' ");
            if (memberrewardid != null) queryParams.Add($" id = '{memberrewardid}' ");

            query.Append(String.Join(" and ", queryParams));
            return dbContext.Query<MemberRewardsModel>(query.ToString());
        }

        public decimal GetPointSumFromDB(string loyaltyid)
        {
            StringBuilder query = new StringBuilder();
            query.Append($"SELECT SUM(PTT.POINTS) FROM BP_HTZ.LW_POINTTRANSACTION PTT INNER JOIN BP_HTZ.LW_VIRTUALCARD VC ON VC.VCKEY = PTT.VCKEY WHERE 1=1 AND VC.LOYALTYIDNUMBER = '{loyaltyid}'");
            Hashtable ht = dbContext.QuerySingleRow(query.ToString());
            decimal pointsOut = (decimal)ht["SUM(PTT.POINTS)"];
            return pointsOut;
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
    }
}
