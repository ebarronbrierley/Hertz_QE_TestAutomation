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
        public string MEMBERSTATUS { get; set; }
        [DateTimeCompare(TimeCompare.Day | TimeCompare.Month | TimeCompare.Year)]
        [ModelAttribute("MemberAddDate")]
        public DateTime? CREATEDATE { get; set; }
        public string CURRENTTIERNAME { get; set; }
        public DateTime? CURRENTTIEREXPIRATIONDATE { get; set; }
        public decimal CURRENCYTONEXTTIER { get; set; }
        public decimal CURRENCYTONEXTREWARD { get; set; }
        public decimal RENTALSTONEXTTIER { get; set; }
        public decimal REVENUETONEXTTIER { get; set; }
        public decimal TOTALRENTALSYTD { get; set; }
        public decimal TOTALREVENUEYTD { get; set; }
        [DateTimeCompare(TimeCompare.Day | TimeCompare.Month | TimeCompare.Year)]
        [ModelAttribute("TierEndDate")]
        public DateTime? A_TIERENDDATE { get; set; }
        [ModelAttribute("MarketingCode")]
        public string A_MKTGPROGRAMID { get; set; }
        [DateTimeCompare(TimeCompare.Day | TimeCompare.Month | TimeCompare.Year)]
        [ModelAttribute("LastActivityDate")]
        public DateTime? A_LASTACTIVITYDATE { get; set; }
    }

}
