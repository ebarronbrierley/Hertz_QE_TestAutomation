using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Brierley.TestAutomation.Core.Utilities;
using Brierley.TestAutomation.Core.SFTP;

namespace Hertz.API.DataModels
{
    public class MemberAccountSummaryModel
    {
        public const string TableName = null;

        public decimal CURRENCYBALANCE { get; set; }
        [ModelAttribute("MemberStatus", ReportOption.Print)]
        public string MEMBERSTATUS { get; set; }
        [DateTimeCompare(TimeCompare.Day | TimeCompare.Month | TimeCompare.Year)]
        [ModelAttribute("MemberAddDate", ReportOption.Print)]
        public DateTime? CREATEDATE { get; set; }
        [ModelAttribute("CurrentTierName", ReportOption.Print)]
        public string CURRENTTIERNAME { get; set; }        
        public DateTime? CURRENTTIEREXPIRATIONDATE { get; set; }
        public decimal CURRENCYTONEXTTIER { get; set; }
        public decimal CURRENCYTONEXTREWARD { get; set; }
        public decimal RENTALSTONEXTTIER { get; set; }
        public decimal REVENUETONEXTTIER { get; set; }
        public decimal TOTALRENTALSYTD { get; set; }
        public decimal TOTALREVENUEYTD { get; set; }
        [DateTimeCompare(TimeCompare.Day | TimeCompare.Month | TimeCompare.Year)]
        [ModelAttribute("TierEndDate", ReportOption.Print)]
        public DateTime? A_TIERENDDATE { get; set; }
        [ModelAttribute("MarketingCode", ReportOption.Print)]
        public string A_MKTGPROGRAMID { get; set; }
        [DateTimeCompare(TimeCompare.Day | TimeCompare.Month | TimeCompare.Year)]
        [ModelAttribute("LastActivityDate", ReportOption.Print)]
        public DateTime? A_LASTACTIVITYDATE { get; set; }
    }

}
