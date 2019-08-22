using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brierley.TestAutomation.Core.Database;
using Brierley.TestAutomation.Core.Utilities;

namespace HertzNetFramework.DataModels
{
    public class PointEvent
    {
        public static readonly string TableName = "LW_POINTEVENT";
        private static readonly string dbUser = "bp_htz";

        public DateTime? UPDATEDATE { get; set; }
        public decimal POINTEVENTID { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public decimal? DEFAULTPOINTS { get; set; }
        public DateTime CREATEDATE { get; set; }

        public static IEnumerable<PointEvent> GetFromDB(IDatabase db, params decimal[] pointEventIds)
        {
            StringBuilder query = new StringBuilder();
            query.Append($"select * from {dbUser}.{TableName}");

            if (pointEventIds == null) return db.Query<PointEvent>(query.ToString());
            if(pointEventIds.Length == 0) return db.Query<PointEvent>(query.ToString());

            query.Append($" where pointeventid in ({String.Join(",",pointEventIds)})");
            return db.Query<PointEvent>(query.ToString());
        }
    }
}
