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
    public class PointController
    {
        private IDatabase dbContext;
        private IStepManager stepContext;

        public PointController(IDatabase dbContext, IStepManager stepContext)
        {
            this.dbContext = dbContext;
            this.stepContext = stepContext;
        }

        public IEnumerable<PointEventModel> GetPointEventsFromDb(params decimal[] pointEventIds)
        {
            StringBuilder query = new StringBuilder();
            query.Append($"select * from {PointEventModel.TableName}");

            if (pointEventIds == null) return dbContext.Query<PointEventModel>(query.ToString());
            if (pointEventIds.Length == 0) return dbContext.Query<PointEventModel>(query.ToString());

            query.Append($" where pointeventid in ({String.Join(",", pointEventIds)})");
            return dbContext.Query<PointEventModel>(query.ToString());
        }
        public IEnumerable<PointTransactionModel> GetPointTransactionsFromDb(decimal vcKey)
        {
            StringBuilder query = new StringBuilder();
            query.Append($"select * from {PointTransactionModel.TableName} where VCKEY = {vcKey}");

            return dbContext.Query<PointTransactionModel>(query.ToString());
        }
    }
}
