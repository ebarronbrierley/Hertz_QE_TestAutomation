using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brierley.TestAutomation.Core.Utilities;

namespace Hertz.API.DataModels
{
    public class VirtualCardModel
    {
        public const string TableName = "BP_HTZ.LW_VIRTUALCARD";

        [ModelAttribute("VcKey", ReportOption.Print)]
        public decimal VCKEY { get; set; }
        [ModelAttribute("LoyaltyIdNumber", ReportOption.Print)]
        public string LOYALTYIDNUMBER { get; set; }
        [ModelAttribute("IpCode", ReportOption.Print)]
        public decimal IPCODE { get; set; }
        public decimal? LINKKEY { get; set; }
        [ModelAttribute("DateIssued", equalityCheck: EqualityCheck.Skip)]
        public DateTime DATEISSUED { get; set; }
        [ModelAttribute("DateRegistered", equalityCheck: EqualityCheck.Skip)]
        public DateTime DATEREGISTERED { get; set; }
        [ModelAttribute("Status", ReportOption.Print)]
        public long STATUS { get; set; }
        public long? NEWSTATUS { get; set; }
        public DateTime? NEWSTATUSEFFECTIVEDATE { get; set; }
        public string STATUSCHANGEREASON { get; set; }
        [ModelAttribute("IsPrimary", ReportOption.Skip)]
        public short ISPRIMARY { get; set; }
        [ModelAttribute("CardType", ReportOption.Skip)]
        public decimal CARDTYPE { get; set; }
        public string CHANGEDBY { get; set; }
        public DateTime CREATEDATE { get; set; }
        public DateTime? UPDATEDATE { get; set; }
        public DateTime? EXPIRATIONDATE { get; set; }
    }
}
