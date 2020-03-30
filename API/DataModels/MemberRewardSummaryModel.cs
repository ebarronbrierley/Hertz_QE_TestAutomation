using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Brierley.TestAutomation.Core.Utilities;
using Brierley.TestAutomation.Core.SFTP;

namespace Hertz.API.DataModels
{
    public class MemberRewardSummaryModel
    {
        public decimal MEMBERID { get; set; }
        [ModelAttribute("Pointsconsumed", ReportOption.Print)]
        public decimal POINTSCONSUMED { get; set; }
        [ModelAttribute("CertificateNmbr", ReportOption.Print)]
        public string CERTIFICATENMBR { get; set; }
        [ModelAttribute("AvailableBalance", ReportOption.Print)]
        public decimal AVAILABLEBALANCE { get; set; }
        [DateTimeCompare(TimeCompare.Day | TimeCompare.Month | TimeCompare.Year)]
        [ModelAttribute("DateIssued", ReportOption.Print)]
        public DateTime? DATEISSUED { get; set; }
        [DateTimeCompare(TimeCompare.Day | TimeCompare.Month | TimeCompare.Year)]
        [ModelAttribute("ExpirationDate", ReportOption.Print)]
        public DateTime? EXPIRATION { get; set; }
        public DateTime? FULFILLMENTDATE { get; set; }
        public DateTime? REDEMPTIONDATE { get; set; }
        [ModelAttribute("RewardName", ReportOption.Print)]
        public string REWARDNAME { get; set; }
        public string CATEGORYNAME { get; set; }
        public string SHORTDESCRIPTION { get; set; }
        public string LONGDESCRIPTION { get; set; }
        public string LEGALTEXT { get; set; }
        [ModelAttribute("smallImageFile", ReportOption.Print)]
        public string SMALLIMAGEFILE { get; set; }
        public string MEMBERIDENTITY { get; set; }
    }
}
