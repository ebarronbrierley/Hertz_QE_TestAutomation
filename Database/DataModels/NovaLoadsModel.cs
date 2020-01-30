using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brierley.TestAutomation.Core.Utilities;

namespace Hertz.Database.DataModels
{
    public class NovaLoadsModel
    {
        public const string TableName = "BP_HTZ.NOVA_LOADS";

        [Model(reportOption: ReportOption.Print)]
        public decimal LOAD_ID { get; set; }
        [Model(reportOption: ReportOption.Print)]
        public DateTime LOAD_DATETIME { get; set; }
        public DateTime? SOURCE_FILE_DATETIME { get; set; }
        [Model(reportOption: ReportOption.Print)]
        public string FILE_NAME { get; set; }
        [Model(reportOption: ReportOption.Print)]
        public string FEED_NAME { get; set; }
        [Model(reportOption: ReportOption.Print)]
        public decimal? DF_COUNT { get; set; }
        [Model(reportOption: ReportOption.Print)]
        public decimal? ERROR_COUNT { get; set; }
        [Model(reportOption: ReportOption.Print)]
        public string PROCESSING_STATE { get; set; }
        [Model(reportOption: ReportOption.Print)]
        public DateTime PROCESSING_STATE_DATE { get; set; }
        public string MD5SUM { get; set; }
        public decimal? SEQUENCE_NBR { get; set; }
        public decimal? SLA_FILE_ID { get; set; }
        public decimal? BEG_LAST_DML_ID { get; set; }
        public decimal? END_LAST_DML_ID { get; set; }
        public DateTime? BEG_LAST_DML_DATE { get; set; }
        public DateTime? END_LAST_DML_DATE { get; set; }
        public string MD5SUM_ENFORCED_UNIQUE { get; set; }
        public string NOTES { get; set; }
    }
}
