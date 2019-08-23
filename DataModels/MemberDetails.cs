using System;
using System.Collections.Generic;
using System.Text;
using Brierley.TestAutomation.Core.Utilities;

namespace HertzNetFramework.DataModels
{
    public class MemberDetails
    {
        public static readonly string[] BaseVerify = new string[] { "A_CITY", "A_STATEORPROVINCE", "A_ZIPORPOSTALCODE", "A_COUNTRY", "A_TIERCODE", "A_MEMBERSTATUSCODE", "A_LANGUAGEPREFERENCE", "A_CONTACTNAME", "", "" };
        public static readonly string TableName = "ATS_MEMBERDETAILS";

        #region Properties
        public decimal A_ROWKEY { get; set; }
        public decimal? A_PARENTROWKEY { get; set; }
        [ModelAttribute("AddressLineThree")]
        public string A_ADDRESSLINETHREE { get; set; }
        [ModelAttribute("AddressLineFour")]
        public string A_ADDRESSLINEFOUR { get; set; }
        [ModelAttribute("City")]
        public string A_CITY { get; set; }
        [ModelAttribute("StateOrProvince")]
        public string A_STATEORPROVINCE { get; set; }
        [ModelAttribute("ZipOrPostalCode")]
        public string A_ZIPORPOSTALCODE { get; set; }
        [ModelAttribute("Country")]
        public string A_COUNTRY { get; set; }
        [ModelAttribute("TierCode", ReportOption.Print)]
        public string A_TIERCODE { get; set; }
        [ModelAttribute("MobilePhone")]
        public string A_MOBILEPHONE { get; set; }
        [ModelAttribute("MemberStatusCode", ReportOption.Print)]
        public long? A_MEMBERSTATUSCODE { get; set; }
        [ModelAttribute("EnrollDate")]
        public DateTime? A_ENROLLDATE { get; set; }
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
        [ModelAttribute("WelcomeEmailSentFlag", check: EqualityCheck.Skip)]
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
        [ModelAttribute("AddressLineOne")]
        public string A_ADDRESSLINEONE { get; set; }
        [ModelAttribute("AddressLineTwo")]
        public string A_ADDRESSLINETWO { get; set; }
        [ModelAttribute("ContractSegmentType")]
        public string A_CONTRACTSEGMENTTYPE { get; set; }
        public decimal? STATUSCODE { get; set; }
        public DateTime CREATEDATE { get; set; }
        public DateTime? UPDATEDATE { get; set; }
        public decimal? LASTDMLID { get; set; }
        public decimal? LAST_DML_ID { get; set; }
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
        [ModelAttribute("Birthdate")]
        public DateTime? A_BIRTHDATE { get; set; }
        [ModelAttribute("PrimaryPhoneNumber")]
        public string A_PRIMARYPHONENUMBER { get; set; }
        [ModelAttribute("LastActivityDate", check: EqualityCheck.Skip)]
        public DateTime? A_LASTACTIVITYDATE { get; set; }
        public decimal? A_VCKEY { get; set; }
        [ModelAttribute("MarriottRwdsNum")]
        public string A_MARRIOTTRWDSNUM { get; set; }
        [ModelAttribute("MarriottStatusMatchDate")]
        public DateTime? A_MARRIOTTSTATUSMATCHDATE { get; set; }
        [ModelAttribute("SPGRwdsNum")]
        public string A_SPGRWDSNUM { get; set; }
        [ModelAttribute("SPGStatusMatchDate")]
        public DateTime? A_SPGSTATUSMATCHDATE { get; set; }
        [ModelAttribute("SmsConsentChangeDate")]
        public DateTime? A_SMSCONSENTCHANGEDATE { get; set; }
        [ModelAttribute("SmsDblOptInComplete")]
        public short? A_SMSDBLOPTINCOMPLETE { get; set; }
        [ModelAttribute("MobilePhoneCountryCode")]
        public long? A_MOBILEPHONECOUNTRYCODE { get; set; }
        [ModelAttribute("PreviousMktgProgramID")]
        public string A_PREVIOUSMKTGPROGRAMID { get; set; }
        [ModelAttribute("PreviousTierCode")]
        public string A_PREVIOUSTIERCODE { get; set; }
        [ModelAttribute("PreviousTierEndDate")]
        public DateTime? A_PREVIOUSTIERENDDATE { get; set; }
        [ModelAttribute("EmailUpdateDate", check: EqualityCheck.Skip)]
        public DateTime? A_EMAILUPDATEDATE { get; set; }
        [ModelAttribute("AddressUpdateDate", check: EqualityCheck.Skip)]
        public DateTime? A_ADDRESSUPDATEDATE { get; set; }
        [ModelAttribute("TokenValidationDate", check: EqualityCheck.Skip)]
        public DateTime? A_TOKENVALIDATIONDATE { get; set; }
        [ModelAttribute("LastRedemptionDate", check: EqualityCheck.Skip)]
        public DateTime? A_LASTREDEMPTIONDATE { get; set; }
        public DateTime LAST_DML_DATE { get; set; }
        public decimal A_IPCODE { get; set; }
        [ModelAttribute("HertzCustomerID", ReportOption.Print, check: EqualityCheck.Skip)]
        public string A_HERTZCUSTOMERID { get; set; }
        #endregion

        public static MemberDetails GenerateMemberDetails(Member member, IHertzProgram program = null)
        {
            if (program == null) program = HertzProgram.GoldPointsRewards;

            MemberDetails details = new MemberDetails();
            details.A_FIRSTNAME = member.FIRSTNAME;
            details.A_LASTNAME = member.LASTNAME;
            details.A_NAMEPREFIX = member.NAMEPREFIX;
            details.A_NAMESUFFIX = member.NAMESUFFIX;
            details.A_PRIMARYPHONENUMBER = member.PRIMARYPHONENUMBER;
            details.A_SECONDARYEMAILADDRESS = null;
            details.A_TIERCODE = program.SpecificTier;
            details.A_ADDRESSLINEONE = "Hertz Dr";
            details.A_ADDRESSLINETWO = "APT " + StrongRandom.NumericString(4);
            details.A_ADDRESSLINETHREE = null;
            details.A_ADDRESSLINEFOUR = null;
            details.A_CITY = StrongRandom.City();
            details.A_STATEORPROVINCE = StrongRandom.FullUSState();
            details.A_COUNTRY = "US";
            details.A_ZIPORPOSTALCODE = member.PRIMARYPOSTALCODE;
            details.A_GENDER = "Male";
            details.A_MOBILEPHONECOUNTRYCODE = 1;
            details.A_MOBILEPHONE = member.PRIMARYPHONENUMBER;
            details.A_HBRRECORDTYPE = null;
            details.A_HBRACCOUNT = 0;
            details.A_RECORDTYPE = null;
            details.A_MEMBERSTATUSCODE = 1;
            details.A_MEMBERACTIONINDICATOR = null;
            details.A_ENROLLDATE = DateTime.Now.Comparable();
            details.A_LANGUAGEPREFERENCE = member.PREFERREDLANGUAGE;
            details.A_TITLENAME = null;
            details.A_MKTGPROGRAMID = null;
            details.A_NUMBERONESTATUS = null;
            details.A_CONTACTNAME = null;
            details.A_PENDINGEMAILSENTFLAG = null;
            details.A_PURGEDCONTRACT = null;
            details.A_HODINDICATOR = null;
            details.A_EARNINGPREFERENCE = program.EarningPreference;
            details.A_OTHERADDRTHREE = null;
            details.A_ADDRESSTYPE = null;
            details.A_OTHERADDRESSTYPE = null;
            details.A_PAYMENTCARDTYPECODE = null;
            details.A_OTHEREMAIL = null;
            details.A_ENROLLEDEMAILSENTFLAG = null;
            details.A_CDPNUMBER = null;
            details.A_CDPNAME = null;
            details.A_OTHERCOUNTRYCODE = null;
            details.A_COMPANYNM = null;
            details.A_OTHERADDRONE = null;
            details.A_OTHERADDRTWO = null;
            details.A_OTHERCITY = null;
            details.A_OTHERSTATEORPROVINCE = null;
            details.A_OTHERZIPORPOSTALCODE = null;
            details.A_TIERENDDATE = null;
            return details;
        }

    }
}
