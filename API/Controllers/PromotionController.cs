using Brierley.TestAutomation.Core.Database;
using Brierley.TestAutomation.Core.Reporting;
using Hertz.API.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hertz.API.Controllers
{
    public class PromotionController
    {
        private IDatabase dbContext;
        private IStepManager stepContext;

        public PromotionController(IDatabase dbContext, IStepManager stepContext)
        {
            this.dbContext = dbContext;
            this.stepContext = stepContext;
        }

        public IEnumerable<PromotionModel> GetFromDB(decimal? id = null, string code = null, string name = null, DateTime? startDate = null, DateTime? endDate = null, int? targeted = null)
        {
            StringBuilder query = new StringBuilder();
            query.Append($"select * from {PromotionModel.TableName}");

            if (id == null && code == null && name == null) return new List<PromotionModel>();

            query.Append($" where ");

            List<string> queryParams = new List<string>();
            if (id != null) queryParams.Add($" id = {id} ");
            if (code != null) queryParams.Add($" code = '{code}' ");
            if (name != null) queryParams.Add($" name = '{name}' ");
            if (startDate != null) queryParams.Add($" startdate = to_timestamp({startDate.Value.ToString("dd-MMM-yy hh.mm.ss.ffffff000 TT")}) ");
            if (endDate != null) queryParams.Add($" enddate = to_timestamp({endDate.Value.ToString("dd-MMM-yy hh.mm.ss.ffffff000 TT")}) ");
            if (targeted != null) queryParams.Add($" targeted = {targeted.Value} ");

            query.Append(String.Join(" and ", queryParams));

            return dbContext.Query<PromotionModel>(query.ToString());
        }

        public IEnumerable<PromotionModel> GetRandomExpiredPromotionFromDB()
        {
            string query = $"select * from {PromotionModel.TableName} SAMPLE(1) pm where pm.enddate < sysdate";
            return dbContext.Query<PromotionModel>(query.ToString());
        }
    }
}
