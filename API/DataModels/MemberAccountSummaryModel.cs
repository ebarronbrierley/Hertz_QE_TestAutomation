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
        public const string TableName = "";

        //API Fields
        public long CURRENCYBALANCE { get; set; }
        public string MEMBERSTATUS { get; set; }
        public DateTime MEMBERADDDATE { get; set; }
        public string CURRENTTIERNAME { get; set; }
        public DateTime? CURRENTTIEREXPIRATIONDATE { get; set; }
        public long? CURRENCYTONEXTTIER { get; set; }
        public long? CURRENCYTONEXTREWARD { get; set; }
        public long RENTALSTONEXTTIER { get; set; }
        public decimal REVENUETONEXTTIER { get; set; }
        public long TOTALRENTALSYTD { get; set; }
        public decimal TOTALREVENUEYTD { get; set; }
        [ModelAttribute("TIERENDDATE")]
        public DateTime? A_TIERENDDATE { get; set; }
        [ModelAttribute("MARKETINGCODE")]
        public string A_MKTGPROGRAMID { get; set; }
        [ModelAttribute("LASTACTIVITYDATE")]
        public DateTime? A_LASTACTIVITYDATE { get; set; }
    }

}
