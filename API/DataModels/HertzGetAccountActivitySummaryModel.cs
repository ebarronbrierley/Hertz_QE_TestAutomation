using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Brierley.TestAutomation.Core.Utilities;
using Brierley.TestAutomation.Core.SFTP;

namespace Hertz.API.DataModels
{
    public class HertzGetAccountActivitySummaryModel
    {
        public const string TABLENAME = null;
        [DateTimeCompare(TimeCompare.Day | TimeCompare.Month | TimeCompare.Year)]
        [ModelAttribute("ActivityDate", ReportOption.Print)]
        public DateTime? ACTIVITYDATE { get; set; }
        public string TYPE { get; set; }
        public string DESCRIPTION { get; set; }
        public string RANUMBER { get; set; }
        public int POINTS { get; set; }
        public int DISPOSITIONCODE { get; set; }
        public string DISPOSITIONCODEDESCRIPTION { get; set; }
        public List<PointsDetailModel> pointDetails { get; set; }


    }
}
