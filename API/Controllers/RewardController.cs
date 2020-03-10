﻿using System;
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
    public class RewardController
    {
        private readonly IDatabase dbContext;
        private readonly IStepManager stepContext;
        private readonly LWIntegrationSvcClientManager lwSvc;

        public RewardController(IDatabase dbContext, IStepManager testStep = null)
        {
            this.dbContext = dbContext;
            this.stepContext = testStep;

            lwSvc = new LWIntegrationSvcClientManager(EnvironmentManager.Get.SOAPServiceURL, "CDIS", true, String.Empty);
            lwSvc.MaxReceivedMessageSize = 2147483647;
            lwSvc.MaxStringContentLength = 2147483647;
        }

        #region LoyaltyWare Methods
        //public double CancelMemberReward(string memberRewardId, string programCode, string resvId, DateTime? chkoutDt,
        //                                    string chkoutAreanum, string chkoutLocNum, string chkoutLocId, string externalId)
        //{
        //    using (ConsoleCapture capture = new ConsoleCapture())
        //    {
        //        try
        //        {
        //            return lwSvc.CancelMemberReward(Convert.ToInt64(memberRewardId), resvId, chkoutDt, chkoutAreanum, chkoutLocNum, chkoutLocId, programCode, externalId, out double time);
        //        }
        //        catch (LWClientException ex)
        //        {
        //            throw new LWServiceException(ex.Message, ex.ErrorCode);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new LWServiceException(ex.Message, -1);
        //        }
        //        finally
        //        {
        //            stepContext.AddAttachment(new Attachment("CancelMemberReward", capture.Output, Attachment.Type.Text));
        //        }
        //    }
        //}
        #endregion

        #region Database Methods
        public RewardDefModel GetRandomRewardDef(IHertzProgram program)
        {
            string query = $"select * from {RewardDefModel.TableName} sample(50) rd where rd.smallimagefile = '{program.EarningPreference}' and rd.active = '1'";
            RewardDefModel rewardDef = dbContext.QuerySingleRow<RewardDefModel>(query);
            return rewardDef;
        }
        public string GetMemberRewardIdFormDB(string memberRewardStatus)
        {
            string query = $@"Select mr.id From bp_htz.lw_memberrewards mr
                                Where mr.orderstatus = '{memberRewardStatus}' And mr.redemptiondate Is Not Null 
                                Order By mr.expiration Desc";

            var retVal = dbContext.QuerySingleRow(query);
            return retVal["ID"].ToString();
        }

        #endregion
    }
}
