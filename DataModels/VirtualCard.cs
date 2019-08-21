using System;
using System.Collections.Generic;
using System.Text;
using Brierley.TestAutomation.Core.Utilities;

namespace HertzNetFramework.DataModels
{
    public class VirtualCard
    {
        public class VirtualCardStatus
        {
            public static readonly long Active = 1, InActive = 2, Hold = 3, Cancelled = 4, Replaced = 5, Expired = 6;
        }
        public static readonly string TableName = "LW_VIRTUALCARD";
        public static readonly string[] BaseVerify = new string[] { "" };

        [ModelAttribute("VcKey")]
        public decimal VCKEY { get; set; }
        [ModelAttribute("LoyaltyIdNumber")]
        public string LOYALTYIDNUMBER { get; set; }
        [ModelAttribute("IpCode")]
        public decimal IPCODE { get; set; }
        public decimal? LINKKEY { get; set; }
        [ModelAttribute("DateIssued", check: EqualityCheck.Skip)]
        public DateTime DATEISSUED { get; set; }
        [ModelAttribute("DateRegistered", check: EqualityCheck.Skip)]
        public DateTime DATEREGISTERED { get; set; }
        [ModelAttribute("Status")]
        public long STATUS { get; set; }
        public long? NEWSTATUS { get; set; }
        public DateTime? NEWSTATUSEFFECTIVEDATE { get; set; }
        public string STATUSCHANGEREASON { get; set; }
        [ModelAttribute("IsPrimary")]
        public short ISPRIMARY { get; set; }
        [ModelAttribute("CardType")]
        public decimal CARDTYPE { get; set; }
        public string CHANGEDBY { get; set; }
        public DateTime CREATEDATE { get; set; }
        public DateTime? UPDATEDATE { get; set; }
        public DateTime? EXPIRATIONDATE { get; set; }

        public List<MemberDetails> MemberDetails = new List<MemberDetails>();
        public List<MemberPreferences> MemberPreferences = new List<MemberPreferences>();

        public static VirtualCard Generate(Member member)
        {
            VirtualCard virtualCard = new VirtualCard();
            virtualCard.LOYALTYIDNUMBER = StrongRandom.NumericString(7);
            virtualCard.IPCODE = member.IPCODE;
            virtualCard.VCKEY = Convert.ToInt64(StrongRandom.NumericString(8));
            virtualCard.DATEISSUED = DateTime.Now.Comparable();
            virtualCard.DATEREGISTERED = DateTime.Now.Comparable();
            virtualCard.ISPRIMARY = 1;
            virtualCard.STATUS = VirtualCardStatus.Active;

            return virtualCard;
        }

    }
}
