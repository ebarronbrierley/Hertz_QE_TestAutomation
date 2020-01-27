using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brierley.TestAutomation.Core.Utilities;
using Hertz.API.Utilities;

namespace Hertz.API.DataModels
{
    public class PointEventModel
    {
        public const string TableName = "BP_HTZ.LW_POINTEVENT";


        [ModelAttribute("UPDATEDATE", ReportOption.Print)]
        public DateTime? UPDATEDATE { get; set; }
        [ModelAttribute("POINTEVENTID", ReportOption.Print)]
        public decimal POINTEVENTID { get; set; }
        [ModelAttribute("NAME", ReportOption.Print)]
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public decimal? DEFAULTPOINTS { get; set; }
        public DateTime CREATEDATE { get; set; }
    }
}
