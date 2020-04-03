using Brierley.TestAutomation.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hertz.API.DataModels
{
    public class AddAttributeSetResponseModel
    {
        public AuctionHeaderModel AttributeSet { get; set; }
        public List<EarnedPointResponseModel> EarnedPoints { get; set; }
    }

    public class EarnedPointResponseModel
    {
        [ModelAttribute("PointType", ReportOption.Print)]
        public string PointType { get; set; }
        [ModelAttribute("PointValue", ReportOption.Print)]
        public decimal PointValue { get; set; }
    }        
}
