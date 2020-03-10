using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Brierley.TestAutomation.Core.Utilities;
using Brierley.TestAutomation.Core.SFTP;

namespace Hertz.API.DataModels
{
    public class RewardDefModel
    {
        public const string TableName = "BP_HTZ.LW_REWARDSDEF";

		public decimal ID {get; set;}
		public string CERTIFICATETYPECODE {get; set;}
		public string NAME {get; set;}
		public string POINTTYPE {get; set;}
		public string POINTEVENT {get; set;}
		public decimal PRODUCTID {get; set;}
		public decimal? TIERID {get; set;}
		public decimal THRESHHOLD {get; set;}
		public string SMALLIMAGEFILE {get; set;}
		public string LARGEIMAGEFILE {get; set;}
		public DateTime? CATALOGSTARTDATE {get; set;}
		public DateTime? CATALOGENDDATE {get; set;}
		public short ACTIVE {get; set;}
		public DateTime CREATEDATE {get; set;}
		public DateTime? UPDATEDATE {get; set;}
		public string MEDIUMIMAGEFILE {get; set;}
		public decimal? FULFILLMENTPROVIDERID {get; set;}
		public decimal HOWMANYPOINTSTOEARN {get; set;}
		public long? REDEEMTIMELIMIT {get; set;}
		public long? PUSHNOTIFICATIONID {get; set;}
		public long REWARDTYPE {get; set;}
		public decimal? CONVERSIONRATE {get; set;}
		
    }
}
