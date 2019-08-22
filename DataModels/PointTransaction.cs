using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brierley.TestAutomation.Core.Utilities;
using Brierley.LoyaltyWare.ClientLib;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using Brierley.TestAutomation.Core.Reporting;
using Brierley.TestAutomation.Core.Database;
using System.Reflection;
using System.Collections;
using System.Linq.Expressions;

namespace HertzNetFramework.DataModels
{
    public class PointTransaction
    {
        public static readonly string TableName = "lw_pointtransaction";
        private static readonly string dbUser = "bp_htz";

        public decimal POINTTRANSACTIONID { get; set; }
        public decimal VCKEY { get; set; }
        public decimal POINTTYPEID { get; set; }
        public decimal POINTEVENTID { get; set; }
        public long TRANSACTIONTYPE { get; set; }
        public DateTime TRANSACTIONDATE { get; set; }
        public DateTime POINTAWARDDATE { get; set; }
        public long? EXPIRATIONREASON { get; set; }
        public DateTime? EXPIRATIONDATE { get; set; }
        public string NOTES { get; set; }
        public string PROMOCODE { get; set; }
        public long OWNERTYPE { get; set; }
        public decimal? OWNERID { get; set; }
        public decimal? ROWKEY { get; set; }
        public decimal? PARENTTRANSACTIONID { get; set; }
        public string PTLOCATIONID { get; set; }
        public string PTCHANGEDBY { get; set; }
        public DateTime CREATEDATE { get; set; }
        public DateTime? UPDATEDATE { get; set; }
        public decimal? LAST_DML_ID { get; set; }
        public decimal POINTS { get; set; }
        public decimal POINTSCONSUMED { get; set; }
        public decimal POINTSONHOLD { get; set; }

        public static IEnumerable<PointTransaction> GetFromDB(IDatabase db, decimal vckey)
        {
            StringBuilder query = new StringBuilder();
            query.Append($"select * from {dbUser}.{TableName} where vckey = {vckey}");
            
            return db.Query<PointTransaction>(query.ToString());
        }
    }
}
