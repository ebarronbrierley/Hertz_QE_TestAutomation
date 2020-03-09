using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hertz.API.DataModels
{
    public class AwardLoyaltyCurrencyResponseModel
    {
        public long CurrencyBalance { get; set; }
        public long? CurrencyToNextReward { get; set; }
        public long? CurrencyToNextTier { get; set; }
        public DateTime? CurrentTierExpirationDate { get; set; }
        public string CurrentTierName { get; set; }
        public string ErrorResponse { get; set; }
        public DateTime? LastActivityDate { get; set; }
        public string MarketingCode { get; set; }
        public DateTime MemberAddDate { get; set; }
        public string MemberStatus { get; set; }
        public long RentalsToNextTier { get; set; }
        public decimal RevenueToNextTier { get; set; }
        public DateTime? TierEndDate { get; set; }
        public long TotalRentalsYTD { get; set; }
        public decimal TotalRevenueYTD { get; set; }
    }
}
