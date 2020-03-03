using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brierley.TestAutomation.Core.Utilities;

namespace Hertz.API.DataModels
{
    public class MemberDetailsModel
    {
        public static readonly string[] BaseVerify = new string[] { "A_CITY", "A_STATEORPROVINCE", "A_ZIPORPOSTALCODE", "A_COUNTRY", "A_TIERCODE", "A_MEMBERSTATUSCODE", "A_ENROLLDATE", "A_LANGUAGEPREFERENCE", "A_CONTACTNAME", "A_HODINDICATOR", "A_CDPNUMBER" };

        public const string TableName = "BP_HTZ.ATS_MEMBERDETAILS";


        public decimal A_ROWKEY { get; set; }
        public decimal? A_PARENTROWKEY { get; set; }
        [ModelAttribute("AddressLineThree")]
        public string A_ADDRESSLINETHREE { get; set; }

        [ModelAttribute("AddressLineFour")]
        public string A_ADDRESSLINEFOUR { get; set; }

        [Randomizer(DataType = RandomDataType.City)]
        [ModelAttribute("City")]
        public string A_CITY { get; set; }

        [Randomizer(DataType = RandomDataType.State)]
        [ModelAttribute("StateOrProvince")]
        public string A_STATEORPROVINCE { get; set; }

        [ModelAttribute("ZipOrPostalCode")]
        public string A_ZIPORPOSTALCODE { get; set; }
               
        [Randomizer(DataType = RandomDataType.CountryAbbreviation)]
        [ModelAttribute("Country")]
        public string A_COUNTRY { get; set; }

        [ModelAttribute("TierCode", ReportOption.Print)]
        public string A_TIERCODE { get; set; }

        [ModelAttribute("MobilePhone")]
        public string A_MOBILEPHONE { get; set; }

        [ModelAttribute("MemberStatusCode", ReportOption.Print)]
        public long? A_MEMBERSTATUSCODE { get; set; }

        [DateTimeCompare(TimeCompare.Day|TimeCompare.Month|TimeCompare.Year|TimeCompare.Hour|TimeCompare.Minute)]
        [Randomizer(DataType = RandomDataType.CurrentDate)]
        [ModelAttribute("EnrollDate")]
        public DateTime? A_ENROLLDATE { get; set; }

        [Randomizer(DataType = RandomDataType.Gender)]
        [ModelAttribute("Gender")]
        public string A_GENDER { get; set; }

        [ModelAttribute("LanguagePreference")]
        public string A_LANGUAGEPREFERENCE { get; set; }

        [ModelAttribute("TitleName")]
        public string A_TITLENAME { get; set; }

        [ModelAttribute("MktgProgramID")]
        public string A_MKTGPROGRAMID { get; set; }

        [ModelAttribute("EnrollmentChannel")]
        public string A_ENROLLMENTCHANNEL { get; set; }

        [ModelAttribute("NumberOneStatus")]
        public string A_NUMBERONESTATUS { get; set; }

        [ModelAttribute("ContactName")]
        public string A_CONTACTNAME { get; set; }

        [ModelAttribute("WelcomeEmailSentFlag", equalityCheck: EqualityCheck.Skip)]
        public short? A_WELCOMEEMAILSENTFLAG { get; set; }

        [ModelAttribute("MemberActionIndicator")]
        public string A_MEMBERACTIONINDICATOR { get; set; }

        [ModelAttribute("HODIndicator", ReportOption.Print)]
        public string A_HODINDICATOR { get; set; }

        [ModelAttribute("MembershipPurchseDate")]
        public DateTime? A_MEMBERSHIPPURCHSEDATE { get; set; }

        [ModelAttribute("EarningPreference", ReportOption.Print)]
        public string A_EARNINGPREFERENCE { get; set; }

        [ModelAttribute("OtherAddrThree")]
        public string A_OTHERADDRTHREE { get; set; }

        [ModelAttribute("AddressType")]
        public string A_ADDRESSTYPE { get; set; }

        [ModelAttribute("OtherAddressType")]
        public string A_OTHERADDRESSTYPE { get; set; }

        [ModelAttribute("PurgedContract")]
        public decimal? A_PURGEDCONTRACT { get; set; }

        [ModelAttribute("PaymentCardTypeCode")]
        public string A_PAYMENTCARDTYPECODE { get; set; }

        [ModelAttribute("OtherEmail")]
        public string A_OTHEREMAIL { get; set; }

        [ModelAttribute("EnrolledEmailSentFlag")]
        public short? A_ENROLLEDEMAILSENTFLAG { get; set; }

        [ModelAttribute("CDPNumber", ReportOption.Print)]
        public string A_CDPNUMBER { get; set; }

        [ModelAttribute("CDPName")]
        public string A_CDPNAME { get; set; }

        [ModelAttribute("OtherCountryCode")]
        public string A_OTHERCOUNTRYCODE { get; set; }

        [ModelAttribute("CompanyNm")]
        public string A_COMPANYNM { get; set; }

        [ModelAttribute("OtherAddrOne")]
        public string A_OTHERADDRONE { get; set; }

        [ModelAttribute("OtherAddrTwo")]
        public string A_OTHERADDRTWO { get; set; }

        [ModelAttribute("OtherCity")]
        public string A_OTHERCITY { get; set; }

        [ModelAttribute("OtherStateOrProvince")]
        public string A_OTHERSTATEORPROVINCE { get; set; }

        [ModelAttribute("OtherZiporPostalCode")]
        public string A_OTHERZIPORPOSTALCODE { get; set; }

        [ModelAttribute("AcquisitionMethodTypeCode")]
        public string A_ACQUISITIONMETHODTYPECODE { get; set; }

        [ModelAttribute("RecordType")]
        public string A_RECORDTYPE { get; set; }

        [ModelAttribute("HBRAccount")]
        public short? A_HBRACCOUNT { get; set; }

        [ModelAttribute("PendingEmailSentFlag")]
        public short? A_PENDINGEMAILSENTFLAG { get; set; }

        [ModelAttribute("SecondaryEmailAddress")]
        public string A_SECONDARYEMAILADDRESS { get; set; }

        [ModelAttribute("HBRRecordType")]
        public string A_HBRRECORDTYPE { get; set; }

        [Randomizer(DataType = RandomDataType.Address)]
        [ModelAttribute("AddressLineOne")]
        public string A_ADDRESSLINEONE { get; set; }

        [Randomizer(DataType = RandomDataType.Address)]
        [ModelAttribute("AddressLineTwo")]
        public string A_ADDRESSLINETWO { get; set; }

        [ModelAttribute("ContractSegmentType")]
        public string A_CONTRACTSEGMENTTYPE { get; set; }

        public decimal? STATUSCODE { get; set; }
        public DateTime CREATEDATE { get; set; }
        public DateTime? UPDATEDATE { get; set; }
        public decimal? LASTDMLID { get; set; }
        public decimal? LAST_DML_ID { get; set; }

        [DateTimeCompare(TimeCompare.Day | TimeCompare.Month | TimeCompare.Year | TimeCompare.Hour | TimeCompare.Minute)]
        [Randomizer(DataType = RandomDataType.FutureDateMonths, Max =11)]
        [ModelAttribute("TierEndDate")]
        public DateTime? A_TIERENDDATE { get; set; }

        [ModelAttribute("FirstName")]
        public string A_FIRSTNAME { get; set; }

        [ModelAttribute("LastName")]
        public string A_LASTNAME { get; set; }

        [ModelAttribute("MiddleName")]
        public string A_MIDDLENAME { get; set; }

        [ModelAttribute("NamePrefix")]
        public string A_NAMEPREFIX { get; set; }

        [ModelAttribute("NameSuffix")]
        public string A_NAMESUFFIX { get; set; }

        [DateTimeCompare(TimeCompare.Day | TimeCompare.Month | TimeCompare.Year)]
        [ModelAttribute("Birthdate")]
        public DateTime? A_BIRTHDATE { get; set; }

        [ModelAttribute("PrimaryPhoneNumber")]
        public string A_PRIMARYPHONENUMBER { get; set; }

        [ModelAttribute("LastActivityDate", equalityCheck: EqualityCheck.Skip)]
        public DateTime? A_LASTACTIVITYDATE { get; set; }

        public decimal? A_VCKEY { get; set; }

        [ModelAttribute("MarriottRwdsNum")]
        public string A_MARRIOTTRWDSNUM { get; set; }

        [DateTimeCompare(TimeCompare.Day|TimeCompare.Month|TimeCompare.Year|TimeCompare.Hour|TimeCompare.Minute|TimeCompare.Second)]
        [ModelAttribute("MarriottStatusMatchDate")]
        public DateTime? A_MARRIOTTSTATUSMATCHDATE { get; set; }

        [ModelAttribute("SPGRwdsNum")]
        public string A_SPGRWDSNUM { get; set; }

        [DateTimeCompare(TimeCompare.Day | TimeCompare.Month | TimeCompare.Year | TimeCompare.Hour | TimeCompare.Minute | TimeCompare.Second)]
        [ModelAttribute("SPGStatusMatchDate")]
        public DateTime? A_SPGSTATUSMATCHDATE { get; set; }

        [DateTimeCompare(TimeCompare.Day | TimeCompare.Month | TimeCompare.Year | TimeCompare.Hour | TimeCompare.Minute | TimeCompare.Second)]
        [ModelAttribute("SmsConsentChangeDate")]
        public DateTime? A_SMSCONSENTCHANGEDATE { get; set; }

        [ModelAttribute("SmsDblOptInComplete")]
        public short? A_SMSDBLOPTINCOMPLETE { get; set; }

        [Randomizer(Min = 1, Max = 9)]
        [ModelAttribute("MobilePhoneCountryCode")]
        public long? A_MOBILEPHONECOUNTRYCODE { get; set; }

        [ModelAttribute("PreviousMktgProgramID")]
        public string A_PREVIOUSMKTGPROGRAMID { get; set; }

        [ModelAttribute("PreviousTierCode")]
        public string A_PREVIOUSTIERCODE { get; set; }

        [DateTimeCompare(TimeCompare.Day | TimeCompare.Month | TimeCompare.Year)]
        [ModelAttribute("PreviousTierEndDate")]
        public DateTime? A_PREVIOUSTIERENDDATE { get; set; }

        [ModelAttribute("EmailUpdateDate", equalityCheck: EqualityCheck.Skip)]
        public DateTime? A_EMAILUPDATEDATE { get; set; }

        [ModelAttribute("AddressUpdateDate", equalityCheck: EqualityCheck.Skip)]
        public DateTime? A_ADDRESSUPDATEDATE { get; set; }

        [ModelAttribute("TokenValidationDate", equalityCheck: EqualityCheck.Skip)]
        public DateTime? A_TOKENVALIDATIONDATE { get; set; }

        [ModelAttribute("LastRedemptionDate", equalityCheck: EqualityCheck.Skip)]
        public DateTime? A_LASTREDEMPTIONDATE { get; set; }
        public DateTime LAST_DML_DATE { get; set; }

        public decimal A_IPCODE { get; set; }


        [ModelAttribute("HertzCustomerID", ReportOption.Print, equalityCheck: EqualityCheck.Skip)]
        public string A_HERTZCUSTOMERID { get; set; }
    }
}
