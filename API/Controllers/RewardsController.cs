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

namespace Hertz.API.Controllers
{
    public class RewardsController
    {
        private readonly IDatabase dbContext;
        private readonly IStepManager stepContext;
        private readonly LWIntegrationSvcClientManager lwSvc;

        public RewardsController(IDatabase dbContext, IStepManager testStep = null)
        {
            this.dbContext = dbContext;
            this.stepContext = testStep;

            lwSvc = new LWIntegrationSvcClientManager(EnvironmentManager.Get.SOAPServiceURL, "CDIS", true, String.Empty);
            lwSvc.MaxReceivedMessageSize = 2147483647;
            lwSvc.MaxStringContentLength = 2147483647;
        }

        #region LoyaltyWare Methods
        public double CancelMemberReward(decimal? memberRewardId, string programCode, string resvId, DateTime? chkoutDt, 
                                            string chkoutAreanum, string chkoutLocNum, string chkoutLocId, string externalId)
        {
            using (ConsoleCapture capture = new ConsoleCapture())
            {
                try
                {
                    return lwSvc.CancelMemberReward((long)memberRewardId, resvId, chkoutDt, chkoutAreanum, chkoutLocNum, chkoutLocId, programCode,externalId, out double time);
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
        #endregion

        #region Database Methods
        public decimal GetMemberRewardIdFormDB(string memberRewardStatus)
        {
            string query = $@"Select mr.id From bp_htz.lw_memberrewards mr
                                Where mr.orderstatus = {memberRewardStatus} And mr.redemptiondate Is Not Null 
                                Order By mr.expiration Desc";

            var retVal = dbContext.QuerySingleRow(query);
            return (decimal)retVal["ID"];
        }

        #endregion
    }
}
