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

        #region Database Methods
        public RewardDefModel GetRandomRewardDef(IHertzProgram program)
        {
            string query = $"select * from {RewardDefModel.TableName} sample(50) rd where rd.smallimagefile = '{program.EarningPreference}' and rd.active = '1'";
            RewardDefModel rewardDef = dbContext.QuerySingleRow<RewardDefModel>(query);
            return rewardDef;
        }

        #endregion
    }
}
