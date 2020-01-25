using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brierley.TestAutomation.Core.Utilities;

namespace Hertz.API.DataModels
{
    public class MemberStatus
    {
        public static readonly long Active = 1, Disabled = 2, Terminated = 3, Locked = 4, NonMember = 5, Merged = 6, PreEnrolled = 7;
    }

    public class MemberModel
    {
        public const string TableName = "BP_HTZ.LW_LOYALTYMEMBER";

        [ModelAttribute("IpCode", ReportOption.Print)]
        public decimal IPCODE { get; set; }

        [Randomizer(DataType = RandomDataType.FutureDateDays, Max = -1)]
        [ModelAttribute("MemberCreateDate", ReportOption.Print)]
        public DateTime MEMBERCREATEDATE { get; set; }

        [Randomizer(DataType = RandomDataType.FutureDateYears, Max = 20)]
        [ModelAttribute("MemberCloseDate")]
        public DateTime? MEMBERCLOSEDATE { get; set; }

        [ModelAttribute("MemberStatus", ReportOption.Print)]
        public long MEMBERSTATUS { get; set; }

        [DateTimeCompare(TimeCompare.Day|TimeCompare.Month|TimeCompare.Year)]
        [Randomizer(DataType = RandomDataType.FutureDateYears, Max = -20)]
        [ModelAttribute("BirthDate")]
        public DateTime? BIRTHDATE { get; set; }

        [Randomizer(DataType = RandomDataType.FirstName)]
        [ModelAttribute("FirstName")]
        public string FIRSTNAME { get; set; }

        [Randomizer(DataType = RandomDataType.LastName)]
        [ModelAttribute("LastName")]
        public string LASTNAME { get; set; }

        [Randomizer(DataType = RandomDataType.FirstName)]
        [ModelAttribute("MiddleName")]
        public string MIDDLENAME { get; set; }

        [Randomizer(DataType = RandomDataType.Word)]
        [ModelAttribute("NamePrefix")]
        public string NAMEPREFIX { get; set; }

        [Randomizer(DataType = RandomDataType.Word)]
        [ModelAttribute("NameSuffix")]
        public string NAMESUFFIX { get; set; }

        [ModelAttribute("AlternateId", ReportOption.Print)]
        public string ALTERNATEID { get; set; }

        [Randomizer(DataType = RandomDataType.Username)]
        [ModelAttribute("Username")]
        public string USERNAME { get; set; }

        [ModelAttribute("Password")]
        public string PASSWORD { get; set; }

        public string SALT { get; set; }

        [Randomizer(DataType = RandomDataType.Email)]
        [ModelAttribute("PrimaryEmailAddress")]
        public string PRIMARYEMAILADDRESS { get; set; }

        [Randomizer(DataType = RandomDataType.PhoneNumber)]
        [ModelAttribute("PrimaryPhoneNumber")]
        public string PRIMARYPHONENUMBER { get; set; }

        [Randomizer(DataType = RandomDataType.Zipcode)]
        [ModelAttribute("PrimaryPostalCode")]
        public string PRIMARYPOSTALCODE { get; set; }

        [ModelAttribute("LastActivityDate")]
        public DateTime? LASTACTIVITYDATE { get; set; }

        [Randomizer(Min =0, Max =1)]
        [ModelAttribute("IsEmployee")]
        public short? ISEMPLOYEE { get; set; }

        [Randomizer(DataType = RandomDataType.Word)]
        [ModelAttribute("ChangedBy")]
        public string CHANGEDBY { get; set; }
        public long? NEWSTATUS { get; set; }
        public DateTime? NEWSTATUSEFFECTIVEDATE { get; set; }
        public string STATUSCHANGEREASON { get; set; }
        public string RESETCODE { get; set; }
        public DateTime? RESETCODEDATE { get; set; }
        public DateTime CREATEDATE { get; set; }
        public DateTime? UPDATEDATE { get; set; }
        public long FAILEDPASSWORDATTEMPTCOUNT { get; set; }
        public short PASSWORDCHANGEREQUIRED { get; set; }
        public DateTime? PASSWORDEXPIREDATE { get; set; }
        public DateTime? STATUSCHANGEDATE { get; set; }
        public decimal? MERGEDTOMEMBER { get; set; }

        [Randomizer(DataType = RandomDataType.LanguageCode)]
        [ModelAttribute("PreferredLanguage")]
        public string PREFERREDLANGUAGE { get; set; }

        public List<VirtualCardModel> VirtualCards { get; set; }
        public MemberDetailsModel MemberDetails { get; set; }
        public MemberPreferencesModel MemberPreferences { get; set; }
    }
}
