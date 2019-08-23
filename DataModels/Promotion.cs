using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brierley.TestAutomation.Core.Utilities;
using Brierley.TestAutomation.Core.Reporting;
using Brierley.TestAutomation.Core.Database;
using System.Reflection;
using System.Collections;
using System.Linq.Expressions;

namespace HertzNetFramework.DataModels
{
    public class Promotion
    {
        public static readonly string TableName = "lw_promotion";
        private static readonly string dbUser = "bp_htz";

        [ModelAttribute("Id", ReportOption.Print)]
        public decimal ID { get; set; }
        [ModelAttribute("Code", ReportOption.Print)]
        public string CODE { get; set; }
        [ModelAttribute("Name", ReportOption.Print)]
        public string NAME { get; set; }
        [ModelAttribute("Targeted", ReportOption.Print)]
        public short TARGETED { get; set; }
        [ModelAttribute("StartDate", ReportOption.Print)]
        public DateTime STARTDATE { get; set; }
        [ModelAttribute("EndDate", ReportOption.Print)]
        public DateTime? ENDDATE { get; set; }
        [ModelAttribute("DisplayOrder")]
        public long? DISPLAYORDER { get; set; }
        public DateTime CREATEDATE { get; set; }
        public DateTime? UPDATEDATE { get; set; }
        public decimal? FOLDERID { get; set; }
        [ModelAttribute("Enrollment")]
        public long ENROLLMENTSUPPORTTYPE { get; set; }
        [ModelAttribute("Description")]
        public string PROMOTIONDESCRIPTION { get; set; }

        public static IEnumerable<Promotion> GetFromDB(IDatabase db, decimal? id = null, string code = null, string name = null, DateTime? startDate = null, DateTime? endDate = null, int? targeted = null )
        {
            StringBuilder query = new StringBuilder();
            query.Append($"select * from {dbUser}.{TableName}");

            if (id == null && code == null && name == null) return new List<Promotion>();

            query.Append($" where ");

            List<string> queryParams = new List<string>();
            if (id != null) queryParams.Add($" id = {id} ");
            if (code != null) queryParams.Add($" code = '{code}' ");
            if (name != null) queryParams.Add($" name = '{name}' ");
            if (startDate != null) queryParams.Add($" startdate = to_timestamp({startDate.Value.ToString("dd-MMM-yy hh.mm.ss.ffffff000 TT")}) ");
            if (endDate != null) queryParams.Add($" enddate = to_timestamp({endDate.Value.ToString("dd-MMM-yy hh.mm.ss.ffffff000 TT")}) ");
            if(targeted != null) queryParams.Add($" targeted = {targeted.Value} ");

            query.Append(String.Join(" and ", queryParams));
            return db.Query<Promotion>(query.ToString());
        }
    }
}
