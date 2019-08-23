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
    public class MemberPromotion
    {
        public static readonly string TableName = "lw_memberpromotion";
        private static readonly string dbUser = "bp_htz";

        public static readonly string[] BaseVerify = new string[] { "ID", "CERTIFICATENMBR", "ENROLLED" };

        [ModelAttribute("Id", ReportOption.Print)]
        public decimal ID { get; set; }
        public string CODE { get; set; }
        [ModelAttribute(" ", ReportOption.Print)]
        public decimal MEMBERID { get; set; }
        public decimal? MTOUCHID { get; set; }
        public DateTime CREATEDATE { get; set; }
        public DateTime? UPDATEDATE { get; set; }
        [ModelAttribute("CertificateNmbr", ReportOption.Print)]
        public string CERTIFICATENMBR { get; set; }
        [ModelAttribute("Enrolled", ReportOption.Print)]
        public short ENROLLED { get; set; }
        public DateTime LAST_DML_DATE { get; set; }

        public Promotion PromotionDefinition { get; set; }

        public static IEnumerable<MemberPromotion> GetFromDB(IDatabase db, decimal? id = null, string code = null, decimal? memberId = null)
        {
            StringBuilder query = new StringBuilder();
            query.Append($"select * from {dbUser}.{TableName}");

            if (id == null && code == null && memberId == null) return new List<MemberPromotion>();

            query.Append($" where ");

            List<string> queryParams = new List<string>();
            if (id != null) queryParams.Add($" id = {id} ");
            if (code != null) queryParams.Add($" code = '{code}' ");
            if (memberId != null) queryParams.Add($" memberId = '{memberId.Value}' ");
            
            query.Append(String.Join(" and ", queryParams));
            return db.Query<MemberPromotion>(query.ToString());
        }
    }
}
