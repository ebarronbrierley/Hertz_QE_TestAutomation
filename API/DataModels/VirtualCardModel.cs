using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brierley.TestAutomation.Core.Utilities;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;
using Hertz.API.Utilities;

namespace Hertz.API.DataModels
{
    public class VirtualCardModel
    {
        public class Status
        {
            public static readonly long Active = 1, InActive = 2, Hold = 3, Cancelled = 4, Replaced = 5, Expired = 6;
        }

        public const string TableName = "BP_HTZ.LW_VIRTUALCARD";

        [Randomizer(Min = 10000000, Max = 99999999)]
        [ModelAttribute("VcKey", ReportOption.Print)]
        public decimal VCKEY { get; set; }

        [Randomizer(Length: 7, DataType = RandomDataType.Numeric)]
        [ModelAttribute("LoyaltyIdNumber", ReportOption.Print)]
        public string LOYALTYIDNUMBER { get; set; }

        [ModelAttribute("IpCode", ReportOption.Print)]
        public decimal IPCODE { get; set; }

        public decimal? LINKKEY { get; set; }

        [DateTimeCompare(TimeCompare.Day|TimeCompare.Month|TimeCompare.Year|TimeCompare.Hour|TimeCompare.Millisecond)]
        [Randomizer(DataType = RandomDataType.CurrentDate)]
        [ModelAttribute("DateIssued", equalityCheck: EqualityCheck.Skip)]
        public DateTime DATEISSUED { get; set; }

        [DateTimeCompare(TimeCompare.Day | TimeCompare.Month | TimeCompare.Year | TimeCompare.Hour | TimeCompare.Millisecond)]
        [Randomizer(DataType = RandomDataType.CurrentDate)]
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


        [LWAttributeSet(_Type = typeof(TxnHeader))]
        public List<TxnHeaderModel> Transactions { get; set; }
    }
}
