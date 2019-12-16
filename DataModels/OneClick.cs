using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brierley.TestAutomation.Core.Utilities;
using Brierley.LoyaltyWare.ClientLib;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using Brierley.TestAutomation.Core.Reporting;
using Brierley.TestAutomation.Core.Database;
using System.Reflection;
using System.Collections;
using System.Linq.Expressions;
using Brierley.TestAutomation.Core.API;
using Newtonsoft.Json.Linq;

namespace HertzNetFramework.DataModels
{
    class OneClick
    {
        string loyaltynumber { get; set; }
        string promocode { get; set; }
        string clicktimestamp { get; set; }
        string clickcategory { get; set; }
        string userId { get; set; }
        string recordTimestamp { get; set; }
        string recordType { get; set; }
        string userAltEmail { get; set; }
        public string Filename { get; private set; }

        public static OneClick GenerateOneClick(string loyaltynumberIN, string promocodeIN)
        {
            OneClick newOneClick = new OneClick();
            newOneClick.loyaltynumber = loyaltynumberIN;
            newOneClick.promocode = promocodeIN;
            newOneClick.clicktimestamp = "2019-12-12T00:00:01Z";
            newOneClick.clickcategory = "OneClick_Registration";
            newOneClick.userId = "40031888129";
            newOneClick.recordTimestamp = "2016-11-19T14:27:51Z";
            newOneClick.recordType = "Click";
            newOneClick.userAltEmail = "LIVEDATATEST@BRIERLEY.COM";
            newOneClick.Filename = $"HTZ_ONECLICKREGISTRATION_{DateTime.Now.ToString("yyyyMMdd_hhmmss")}.csv.dec";
            return newOneClick;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Loyalty_Number|Promo_Code|Click_Timestamp|Click_Category_Name|user.Id|record.Timestamp|record.Type|user.AlternateEmail");
            sb.AppendLine();
            sb.Append($"{loyaltynumber}|{promocode}|{clicktimestamp}|{clickcategory}|{userId}|{recordTimestamp}|{recordType}|{userAltEmail}");
            return sb.ToString();
            
        }

        public static void RunDAP()
        {
            string jobName = "HertzBLine - DAP Process One Click Registration";

            RestConfiguration restConfig = new RestConfiguration()
            {
                BaseURL = "http://CY2QAAPP05:8001/VisualCron/json",
                EndPoint = $"/Job/GetByName?password=7d7Yu^m&username=qa_htz_srv_acct&name={jobName}"
            };

            RestResponse restResponse = Rest.Get(restConfig).Execute();

            JObject responseBody = JObject.Parse(restResponse.MessageBody);
            string jobID = (string)responseBody["Id"];
            restConfig = new RestConfiguration()
            {
                BaseURL = "http://CY2QAAPP05:8001/VisualCron/json",
                EndPoint = $"/Job/Run?password=7d7Yu^m&username=qa_htz_srv_acct&id={jobID}"
            };

            restResponse = Rest.Get(restConfig).Execute();

        }
    }
}
