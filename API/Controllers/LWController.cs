using Brierley.LoyaltyWare.ClientLib;
using Brierley.TestAutomation.Core.Database;
using Brierley.TestAutomation.Core.Reporting;
using Brierley.TestAutomation.Core.Utilities;
using Hertz.API.DataModels;
using Hertz.API.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hertz.API.Controllers
{
    public class LWController
    {
        private IDatabase dbContext;
        private IStepManager stepContext;
        private readonly LWIntegrationSvcClientManager lwSvc;

        public LWController(IDatabase dbContext, IStepManager stepContext)
        {
            this.dbContext = dbContext;
            this.stepContext = stepContext;

            lwSvc = new LWIntegrationSvcClientManager(EnvironmentManager.Get.SOAPServiceURL, "CDIS", true, String.Empty);
            lwSvc.MaxReceivedMessageSize = 2147483647;
            lwSvc.MaxStringContentLength = 2147483647;
        }

        public string[] GetLoyaltyEventNames(string externalId)
        {
            using (ConsoleCapture capture = new ConsoleCapture())
            {
                try
                {
                    var lwMemberPromoCount = lwSvc.GetLoyaltyEventNames(externalId, out double time);
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
                    stepContext.AddAttachment(new Attachment("GetLoyaltyEventNames", capture.Output, Attachment.Type.Text));
                }
            }
        }

        public HertzGetCSAgentResponseModel HertzGetCSAgent(string agent)
        {
            HertzGetCSAgentResponseModel hertzGetCSAgent = default;
            using (ConsoleCapture capture = new ConsoleCapture())
            {
                try
                {
                    var lwTransferPoints = lwSvc.HertzGetCSAgent(agent, String.Empty, out double time);
                    hertzGetCSAgent = LODConvert.FromLW<HertzGetCSAgentResponseModel>(lwTransferPoints);
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
                    stepContext.AddAttachment(new Attachment("HertzGetCSAgent", capture.Output, Attachment.Type.Text));
                }
            }
            return hertzGetCSAgent;
        }

      
        public List<RewardCatalogSummaryResponseModel> GetRewardCatalog(bool ?activeOnly, string tier,
            string language,long ? categoryId, bool ? returnRewardCategory,
            List<ContentSearchAttribute> contentSearchAttributes, int ? startIndex,
            int ? batchSize, long ? currencyToEarnLow, long ? currencyToEarnHigh )
        {
            List<RewardCatalogSummaryResponseModel> rewardCatalogSummary = new List<RewardCatalogSummaryResponseModel>();
            using (ConsoleCapture capture = new ConsoleCapture())
            {
                try
                {
                    var lwGetRewardsCatalog = lwSvc.GetRewardCatalog(activeOnly,tier,language, categoryId,
                        returnRewardCategory,
                        contentSearchAttributes!=null?
                        contentSearchAttributes.Select(x=> 
                        new Brierley.LoyaltyWare.ClientLib.DomainModel.Client.ContentSearchAttributesStruct
                        {
                             AttributeName= x.AttributeName,
                              AttributeValue= x.AttributeValue
                        }).ToArray():null,
                        startIndex, batchSize, currencyToEarnLow, currencyToEarnHigh,
                         String.Empty, out double time);
                    foreach (var lwGetRewardCatalog in lwGetRewardsCatalog)
                    {
                        rewardCatalogSummary.Add(LODConvert.FromLW<RewardCatalogSummaryResponseModel>(lwGetRewardCatalog));
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
                    stepContext.AddAttachment(new Attachment("GetRewardCatalog", capture.Output, Attachment.Type.Text));
                }
            }
            return rewardCatalogSummary;
        }

    }
}
