using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;
using Brierley.TestAutomation.Core.Utilities;
using Hertz.API.Utilities;

namespace Hertz.API.DataModels
{
    public class MemberPromotionModel
    {
        public const string TableName = "BP_HTZ.LW_MEMBERPROMOTION";
        public static readonly string[] BaseVerify = new string[] { "ID", "CERTIFICATENMBR", "ENROLLED" };

        [ModelAttribute("Id", ReportOption.Print)]
        public decimal ID { get; set; }
        public string CODE { get; set; }

        [ModelAttribute(" ", ReportOption.Print)]
        public decimal MEMBERID { get; set; }

        public decimal? MTOUCHID { get; set; }

        public DateTime CREATEDATE { get; set; }

        public DateTime? UPDATEDATE { get; set; }

        [ModelAttribute("CertificateNmbr", ReportOption.Print)]
        public string CERTIFICATENMBR { get; set; }

        [ModelAttribute("Enrolled", ReportOption.Print)]
        public short ENROLLED { get; set; }

        public DateTime LAST_DML_DATE { get; set; }


        [LWAttributeSet(_Type = typeof(PromotionDefinitionStruct))]
        public PromotionModel PromotionDefinition { get; set; }
    }
}
