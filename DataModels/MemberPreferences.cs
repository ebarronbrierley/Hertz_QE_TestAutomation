using System;
using System.Collections.Generic;
using System.Text;
using Brierley.TestAutomation.Core.Utilities;

namespace HertzNetFramework.DataModels
{
    public class MemberPreferences
    {
        public static readonly string[] BaseVerify = new string[] { };
        public static readonly string TableName = "ATS_MEMBERPREFERENCES";

        public decimal A_ROWKEY { get; set; }
        public decimal? A_PARENTROWKEY { get; set; }
        [ModelAttribute("AUEsignCode")]
        public string A_AUESIGNCODE { get; set; }
        [ModelAttribute("NZEsignCode")]
        public string A_NZESIGNCODE { get; set; }
        [ModelAttribute("DirectMailOptIn")]
        public short? A_DIRECTMAILOPTIN { get; set; }
        [ModelAttribute("EmailOptIn")]
        public short? A_EMAILOPTIN { get; set; }
        [ModelAttribute("SMSOptIn")]
        public short? A_SMSOPTIN { get; set; }
        [ModelAttribute("HCCStatus")]
        public string A_HCCSTATUS { get; set; }
        [ModelAttribute("EMEAEsignCode")]
        public string A_EMEAESIGNCODE { get; set; }
        [ModelAttribute("TPEU")]
        public string A_TPEU { get; set; }
        [ModelAttribute("FPOOptIn")]
        public string A_FPOOPTIN { get; set; }
        [ModelAttribute("eReturnOptIn")]
        public string A_ERETURNOPTIN { get; set; }
        [ModelAttribute("ShareDataIndicator")]
        public string A_SHAREDATAINDICATOR { get; set; }
        [ModelAttribute("AERAUNZ")]
        public string A_AERAUNZ { get; set; }
        [ModelAttribute("MaxCoverAU")]
        public string A_MAXCOVERAU { get; set; }
        [ModelAttribute("SiriusOptIn")]
        public string A_SIRIUSOPTIN { get; set; }
        [ModelAttribute("NeverLostOptIn")]
        public string A_NEVERLOSTOPTIN { get; set; }
        [ModelAttribute("PMA")]
        public string A_PMA { get; set; }
        [ModelAttribute("USCarClass")]
        public string A_USCARCLASS { get; set; }
        [ModelAttribute("LDWIndicator")]
        public string A_LDWINDICATOR { get; set; }
        [ModelAttribute("LISIndicator")]
        public string A_LISINDICATOR { get; set; }
        [ModelAttribute("PAIIndicator")]
        public string A_PAIINDICATOR { get; set; }
        [ModelAttribute("PECIndicator")]
        public string A_PECINDICATOR { get; set; }
        [ModelAttribute("EULDWIndicator")]
        public string A_EULDWINDICATOR { get; set; }
        [ModelAttribute("DNRCode")]
        public string A_DNRCODE { get; set; }
        [ModelAttribute("EUTDWIndicator")]
        public string A_EUTDWINDICATOR { get; set; }
        [ModelAttribute("EUPAIIndicator")]
        public string A_EUPAIINDICATOR { get; set; }
        [ModelAttribute("EUCarClass")]
        public string A_EUCARCLASS { get; set; }
        [ModelAttribute("AUCarClass")]
        public string A_AUCARCLASS { get; set; }
        [ModelAttribute("AUPAIIndicator")]
        public string A_AUPAIINDICATOR { get; set; }
        [ModelAttribute("CACarClass")]
        public string A_CACARCLASS { get; set; }
        [ModelAttribute("CALDWIndicator")]
        public string A_CALDWINDICATOR { get; set; }
        [ModelAttribute("CAPAIIndicator")]
        public string A_CAPAIINDICATOR { get; set; }
        [ModelAttribute("NZCarClass")]
        public string A_NZCARCLASS { get; set; }
        [ModelAttribute("NZDEWIndicator")]
        public string A_NZDEWINDICATOR { get; set; }
        [ModelAttribute("AUCDWIndicator")]
        public string A_AUCDWINDICATOR { get; set; }
        [ModelAttribute("AUPECIndicator")]
        public string A_AUPECINDICATOR { get; set; }
        [ModelAttribute("NYGoldIndicator")]
        public string A_NYGOLDINDICATOR { get; set; }
        [ModelAttribute("NYSpecialGoldIndicator")]
        public string A_NYSPECIALGOLDINDICATOR { get; set; }
        [ModelAttribute("HIGoldIndicator")]
        public string A_HIGOLDINDICATOR { get; set; }
        [ModelAttribute("NZPECIndicator")]
        public string A_NZPECINDICATOR { get; set; }
        [ModelAttribute("NZPAIIndicator")]
        public string A_NZPAIINDICATOR { get; set; }
        [ModelAttribute("HiSpecialGoldIndicator")]
        public string A_HISPECIALGOLDINDICATOR { get; set; }
        [ModelAttribute("CAEsignCode")]
        public string A_CAESIGNCODE { get; set; }
        [ModelAttribute("USEsignCode")]
        public string A_USESIGNCODE { get; set; }
        [ModelAttribute("SCDW")]
        public string A_SCDW { get; set; }
        [ModelAttribute("ProfileUpdateFlag")]
        public string A_PROFILEUPDATEFLAG { get; set; }
        public decimal? STATUSCODE { get; set; }
        public DateTime CREATEDATE { get; set; }
        public DateTime? UPDATEDATE { get; set; }
        public decimal? LASTDMLID { get; set; }
        public decimal? LAST_DML_ID { get; set; }
        public decimal? A_VCKEY { get; set; }
        public DateTime LAST_DML_DATE { get; set; }
        public decimal A_IPCODE { get; set; }

        public static MemberPreferences Generate()
        {
            MemberPreferences memberPreferences = new MemberPreferences()
            {
                A_DIRECTMAILOPTIN = 1,
                A_EMAILOPTIN = null,
                A_SMSOPTIN = null,
                A_HCCSTATUS = null,
                A_EMEAESIGNCODE = null,
                A_AUESIGNCODE = null,
                A_CAESIGNCODE = null,
                A_TPEU = null,
                A_FPOOPTIN = null,
                A_ERETURNOPTIN = null,
                A_SHAREDATAINDICATOR = null,
                A_AERAUNZ = null,
                A_MAXCOVERAU = null,
                A_SIRIUSOPTIN = null,
                A_NEVERLOSTOPTIN = null,
                A_DNRCODE = null,
                A_PROFILEUPDATEFLAG = null,
                A_HISPECIALGOLDINDICATOR = null,
                A_PMA = null,
                A_USCARCLASS = null,
                A_LDWINDICATOR = null,
                A_LISINDICATOR = null,
                A_PAIINDICATOR = null,
                A_PECINDICATOR = null,
                A_EULDWINDICATOR = null,
                A_EUTDWINDICATOR = null,
                A_EUPAIINDICATOR = null,
                A_EUCARCLASS = null,
                A_AUCARCLASS = null,
                A_AUPAIINDICATOR = null,
                A_SCDW = null,
                A_CACARCLASS = null,
                A_CALDWINDICATOR = null,
                A_CAPAIINDICATOR = null,
                A_NZCARCLASS = null,
                A_NZDEWINDICATOR = null,
                A_AUCDWINDICATOR = null,
                A_AUPECINDICATOR = null,
                A_NYGOLDINDICATOR = null,
                A_NYSPECIALGOLDINDICATOR = null,
                A_HIGOLDINDICATOR = null,
                A_NZPECINDICATOR = null,
                A_NZPAIINDICATOR = null
            };

            return memberPreferences;
        }

    }
}
