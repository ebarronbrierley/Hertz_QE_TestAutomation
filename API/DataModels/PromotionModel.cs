using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brierley.TestAutomation.Core.Utilities;

namespace Hertz.API.DataModels
{
    public class PromotionModel
    {
        public const string TableName = "BP_HTZ.LW_PROMOTION";


        [ModelAttribute("Id", ReportOption.Print)]
        public decimal ID { get; set; }

        [ModelAttribute("Code", ReportOption.Print)]
        public string CODE { get; set; }

        [ModelAttribute("Name", ReportOption.Print)]
        public string NAME { get; set; }

        [ModelAttribute("Targeted", ReportOption.Print)]
        public short TARGETED { get; set; }

        [ModelAttribute("StartDate", ReportOption.Print)]
        public DateTime STARTDATE { get; set; }

        [ModelAttribute("EndDate", ReportOption.Print)]
        public DateTime? ENDDATE { get; set; }

        [ModelAttribute("DisplayOrder")]
        public long? DISPLAYORDER { get; set; }

        public DateTime CREATEDATE { get; set; }

        public DateTime? UPDATEDATE { get; set; }

        public decimal? FOLDERID { get; set; }

        [ModelAttribute("Enrollment")]
        public long ENROLLMENTSUPPORTTYPE { get; set; }

        [ModelAttribute("Description")]
        public string PROMOTIONDESCRIPTION { get; set; }


    }
}
