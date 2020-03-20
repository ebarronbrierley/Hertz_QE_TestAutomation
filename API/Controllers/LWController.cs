using Brierley.LoyaltyWare.ClientLib;
using Brierley.TestAutomation.Core.Database;
using Brierley.TestAutomation.Core.Reporting;
using Brierley.TestAutomation.Core.Utilities;
using Hertz.API.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
