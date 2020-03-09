using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Brierley.TestAutomation.Core.Utilities;
using Brierley.TestAutomation.Core.SFTP;

namespace Hertz.API.DataModels
{
    public class CsAgent
    {
        public const string TableName = "BP_HTZ.LW_CSAGENT";

		//???[Model("RoleId")]
		//???[Model("GroupId")]
		public decimal ID {get; set;}
		[Model("RoleId")]
		public decimal ROLEID {get; set;}
		[Model("GroupId")]
		public decimal? GROUPID {get; set;}
		[Model("AgentNumber")]
		public decimal? AGENTNUMBER {get; set;}
		[Model("FirstName")]
		public string FIRSTNAME {get; set;}
		[Model("LastName")]
		public string LASTNAME {get; set;}
		[Model("EmailAddress")]
		public string EMAILADDRESS {get; set;}
		[Model("PhoneNumber")]
		public string PHONENUMBER {get; set;}
		[Model("Extension")]
		public string EXTENSION {get; set;}
		//???[Model("AgentUserName")]
		public string USERNAME {get; set;}
		public string PASSWORD {get; set;}
		public string SALT {get; set;}
		public long? FAILEDPASSWORDATTEMPTCOUNT {get; set;}
		public short? PASSWORDCHANGEREQUIRED {get; set;}
		public DateTime? PASSWORDEXPIREDATE {get; set;}
		[Model("Status")]
		public long STATUS {get; set;}
		[Model("CreatedBy")]
		public decimal? CREATEDBY {get; set;}
		public string RESETCODE {get; set;}
		public DateTime? RESETCODEDATE {get; set;}
		public DateTime CREATEDATE {get; set;}
		[Model("UpdateDate")]
		public DateTime? UPDATEDATE {get; set;}
		
    }
}
