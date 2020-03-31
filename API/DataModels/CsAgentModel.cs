using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Brierley.TestAutomation.Core.Utilities;
using Brierley.TestAutomation.Core.SFTP;

namespace Hertz.API.DataModels
{
   public  enum AgentStatus
    {
        Active = 1
    }
    public class CsAgentModel
    {
        public const string TableName = "BP_HTZ.LW_CSAGENT";
		
		public decimal ID {get; set;}
		[Model("RoleId", ReportOption.Print)]
		public decimal ROLEID {get; set;}
        [Model("GroupId")]
        public decimal? GROUPID {get; set;}
        [Model("AgentNumber")]
        public decimal? AGENTNUMBER {get; set;}
        [Randomizer(DataType = RandomDataType.FirstName)]
        [Model("FirstName", ReportOption.Print)]
        public string FIRSTNAME {get; set;}
        [Randomizer(DataType = RandomDataType.LastName)]
        [Model("LastName", ReportOption.Print)]
		public string LASTNAME {get; set;}
        [Randomizer(DataType = RandomDataType.Email)]
        [Model("EmailAddress", ReportOption.Print)]
		public string EMAILADDRESS {get; set;}
        [Randomizer(Length: 10, DataType = RandomDataType.Numeric)]
        [Model("PhoneNumber", ReportOption.Print)]
		public string PHONENUMBER {get; set;}
		[Model("Extension")]
		public string EXTENSION {get; set;}
        [Randomizer(DataType = RandomDataType.Username)]
        [Model("AgentUserName", ReportOption.Print)]      
        public string USERNAME {get; set;}        
        public string PASSWORD {get; set;}        
        public string SALT {get; set;}
		public long? FAILEDPASSWORDATTEMPTCOUNT {get; set;}
		public short? PASSWORDCHANGEREQUIRED {get; set;}
		public DateTime? PASSWORDEXPIREDATE {get; set;}
		[Model("Status", ReportOption.Print)]
		public AgentStatus STATUS {get; set;}         
		public decimal? CREATEDBY {get; set;}
		public string RESETCODE {get; set;}
		public DateTime? RESETCODEDATE {get; set;}
        [DateTimeCompare(TimeCompare.Day | TimeCompare.Month | TimeCompare.Year | TimeCompare.Hour )]
        [Randomizer(DataType = RandomDataType.CurrentDate)]
        [Model("CreatedDate", ReportOption.Print)]        
        public DateTime CREATEDATE {get; set;}
        [DateTimeCompare(TimeCompare.Day | TimeCompare.Month | TimeCompare.Year | TimeCompare.Hour)]
        [Randomizer(DataType = RandomDataType.CurrentDate)]
        [Model("UpdateDate", ReportOption.Print)]
		public DateTime? UPDATEDATE {get; set;}
		
    }
}
