using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Brierley.TestAutomation.Core.Utilities;
using Brierley.TestAutomation.Core.SFTP;

namespace Hertz.API.DataModels
{
    public class MemberRewardModel
    {
        public sealed class OrderStatus
        {
            public static readonly string Cancelled = "Cancelled";
        }

        public const string TableName = "BP_HTZ.LW_MEMBERREWARDS";

		//???[Model("MemberRewardID")]
		public decimal ID {get; set;}
		public decimal REWARDDEFID {get; set;}
		public string CERTIFICATENMBR {get; set;}
		public string OFFERCODE {get; set;}
		[Model("AvailableBalance")]
		public decimal? AVAILABLEBALANCE {get; set;}
		public long? FULFILLMENTOPTION {get; set;}
		public decimal MEMBERID {get; set;}
		public decimal PRODUCTID {get; set;}
		public decimal PRODUCTVARIANTID {get; set;}
		[Model("DateIssued")]
		public DateTime DATEISSUED {get; set;}
		//???[Model("ExpirationDate")]
		public DateTime? EXPIRATION {get; set;}
		[Model("FulfillmentDate")]
		public DateTime? FULFILLMENTDATE {get; set;}
		[Model("RedemptionDate")]
		public DateTime? REDEMPTIONDATE {get; set;}
		public string CHANGEDBY {get; set;}
		public string LWORDERNUMBER {get; set;}
		public string LWCANCELLATIONNUMBER {get; set;}
		public string ORDERSTATUS {get; set;}
		public DateTime CREATEDATE {get; set;}
		public DateTime? UPDATEDATE {get; set;}
		public decimal? LAST_DML_ID {get; set;}
		public decimal? FULFILLMENTPROVIDERID {get; set;}
		public string FPORDERNUMBER {get; set;}
		public string FPCANCELLATIONNUMBER {get; set;}
		[Model("PointsConsumed")]
		public decimal? POINTSCONSUMED {get; set;}
		public string FROMCURRENCY {get; set;}
		public string TOCURRENCY {get; set;}
		public decimal? POINTCONVERSIONRATE {get; set;}
		public decimal? EXCHANGERATE {get; set;}
		public decimal? MONETARYVALUE {get; set;}
		public decimal? CARTTOTALMONETARYVALUE {get; set;}
		
    }
}
