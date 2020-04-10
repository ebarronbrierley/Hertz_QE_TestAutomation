using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;
using Brierley.TestAutomation.Core.Utilities;
using Hertz.API.Utilities;

namespace Hertz.Database.DataModels
{
    public class CSAgentModel
    {
        public const string TableName = "BP_HTZ.LW_CSAGENT";
        public decimal ID { get; set; }
        public decimal ROLEID { get; set; }
        public decimal? GROUPID { get; set; }
        public decimal? AGENTNUMBER { get; set; }
        public string FIRSTNAME { get; set; }
        public string LASTNAME { get; set; }
        public string EMAILADDRESS { get; set; }
        public string PHONENUMBER { get; set; }
        public string EXTENSION { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public string SALT { get; set; }
        public long? FAILEDPASSWORDATTEMPTCOUNT { get; set; }
        public short? PASSWORDCHANGEREQUIRED { get; set; }
        public DateTime? PASSWORDEXPIREDATE { get; set; }
        public long STATUS { get; set; }
        public decimal? CREATEDBY { get; set; }
        public string RESETCODE { get; set; }
        public DateTime? RESETCODEDATE { get; set; }
        public DateTime CREATEDATE { get; set; }
        public DateTime? UPDATEDATE { get; set; }

    }
}
