using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;
using Brierley.TestAutomation.Core.Utilities;
using Hertz.API.Utilities;

namespace Hertz.API.DataModels
{
    public class MemberRewardsModel
    {
        public const string TableName = "BP_HTZ.LW_MEMBERREWARDS";
        [ModelAttribute("Id", ReportOption.Print)]
        public decimal ID { get; set; }
        public decimal REWARDDEFID { get; set; }
        [ModelAttribute("CertificateNmbr", ReportOption.Print)]
        public string CERTIFICATENMBR { get; set; }
        public string OFFERCODE { get; set; }
        public decimal? AVAILABLEBALANCE { get; set; }
        public long? FULFILLMENTOPTION { get; set; }
        [ModelAttribute("MemberId", ReportOption.Print)]
        public decimal MEMBERID { get; set; }
        public decimal PRODUCTID { get; set; }
        public decimal PRODUCTVARIANTID { get; set; }
        public DateTime DATEISSUED { get; set; }
        public DateTime? EXPIRATION { get; set; }
        public DateTime? FULFILLMENTDATE { get; set; }
        public DateTime? REDEMPTIONDATE { get; set; }
        public string CHANGEDBY { get; set; }
        public string LWORDERNUMBER { get; set; }
        public string LWCANCELLATIONNUMBER { get; set; }
        public string ORDERSTATUS { get; set; }
        public DateTime CREATEDATE { get; set; }
        public DateTime? UPDATEDATE { get; set; }
        public decimal? LAST_DML_ID { get; set; }
        public decimal? FULFILLMENTPROVIDERID { get; set; }
        public string FPORDERNUMBER { get; set; }
        public string FPCANCELLATIONNUMBER { get; set; }
        public decimal? POINTSCONSUMED { get; set; }
        public string FROMCURRENCY { get; set; }
        public string TOCURRENCY { get; set; }
        public decimal? POINTCONVERSIONRATE { get; set; }
        public decimal? EXCHANGERATE { get; set; }
        public decimal? MONETARYVALUE { get; set; }
        public decimal? CARTTOTALMONETARYVALUE { get; set; }

    }
}
