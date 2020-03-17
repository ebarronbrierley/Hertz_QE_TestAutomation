using Brierley.LoyaltyWare.ClientLib;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;
using Brierley.TestAutomation.Core.Database;
using Brierley.TestAutomation.Core.Reporting;
using Brierley.TestAutomation.Core.Utilities;
using Hertz.API.DataModels;
using Hertz.API.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hertz.API.Controllers
{
    public class CsAgentController
    {
        private readonly IDatabase dbContext;
        private readonly IStepManager stepContext;
        private readonly LWIntegrationSvcClientManager lwSvc;

        public CsAgentController(IDatabase dbContext, IStepManager testStep = null)
        {
            this.dbContext = dbContext;
            this.stepContext = testStep;

            lwSvc = new LWIntegrationSvcClientManager(EnvironmentManager.Get.SOAPServiceURL, "CDIS", true, String.Empty);
            lwSvc.MaxReceivedMessageSize = 2147483647;
            lwSvc.MaxStringContentLength = 2147483647;
        }

        public CsAgentModel AddCsAgent(CsAgentModel csAgent)
        {
            using (ConsoleCapture capture = new ConsoleCapture())
            {                
                try
                {
                   
                    var lwCsAgent = lwSvc.HertzAddUpdateCSAgent(csAgent.USERNAME,csAgent.FIRSTNAME,csAgent.LASTNAME, csAgent.ROLEID.ToString(),((int)(csAgent.STATUS)).ToString(), csAgent.GROUPID.ToString(), csAgent.AGENTNUMBER.ToString(), csAgent.EMAILADDRESS,csAgent.PHONENUMBER,csAgent.EXTENSION,null,out double elapsedTime);
                    return LODConvert.FromLW<CsAgentModel>(lwCsAgent);
                }
                catch (LWClientException ex)
                {
                    throw new LWServiceException(ex.Message, ex.ErrorCode);
                }
                finally
                {
                    stepContext.AddAttachment(new Attachment("HertzAddUpdateCSAgent", capture.Output, Attachment.Type.Text));
                }
            }
            
        }


        public static CsAgentModel GenerateRandomAgent()
        {
            CsAgentModel csAgent = StrongRandom.GenerateRandom<CsAgentModel>();     
            return csAgent;
        }

        public CsAgentModel GetFromDB(string userName, string userNameQuery = null)
        {
            string query = String.Empty;

            if (userName == null && !String.IsNullOrEmpty(userNameQuery))
            {
                query = userNameQuery;
            }
            else
            {
                query = $"select * from {CsAgentModel.TableName} where username = '{userName}'";
            }

            CsAgentModel csAgent = dbContext.QuerySingleRow<CsAgentModel>(query);          
            return csAgent;
        }
    }
}
