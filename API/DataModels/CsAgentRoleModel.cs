using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Brierley.TestAutomation.Core.Utilities;
using Brierley.TestAutomation.Core.SFTP;

namespace Hertz.API.DataModels
{
    public class CsAgentRoleModel
    {
        public const string TableName = "BP_HTZ.LW_CSROLE";

		public decimal ID {get; set;}
		public string NAME {get; set;}
		public long? POINTAWARDLIMIT {get; set;}
		public string DESCRIPTION {get; set;}
		public DateTime CREATEDATE {get; set;}
		public DateTime? UPDATEDATE {get; set;}
		public DateTime LAST_DML_DATE {get; set;}
		
    }
}
