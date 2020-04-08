using Brierley.LoyaltyWare.ClientLib.DomainModel;
using Brierley.TestAutomation.Core.Utilities;

namespace Hertz.API.DataModels
{
    public class AuctionHeaderModel
    {
        [ModelAttribute("AuctionTxnType", ReportOption.Print)]
        public string AuctionTxnType { get; set; }
        [ModelAttribute("CDRewardID", ReportOption.Print)]
        public string CDRewardID { get; set; }
        [ModelAttribute("AuctionEventName", ReportOption.Print)]
        public string AuctionEventName { get; set; }
        [ModelAttribute("HeaderGPRpts", ReportOption.Print)]
        public decimal HeaderGPRpts { get;set; }
        [ModelAttribute("AuctionPointType", ReportOption.Print)]
        public string AuctionPointType { get; set; }

    }

   

}
