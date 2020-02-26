using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hertz.API.DataModels
{
   public class HertzAwardLoyaltyCurrencyResponseModel
    {
        public virtual long CurrencyBalance { get; set; }
        public virtual string MemberStatus { get; set; }
        public virtual DateTime MemberAddDate { get; set; }
        public virtual string CurrentTierName { get; set; }
        public virtual DateTime? CurrentTierExpirationDate { get; set; }
        public virtual long? CurrencyToNextTier { get; set; }
        public virtual long? CurrencyToNextReward { get; set; }
        public virtual long RentalsToNextTier { get; set; }
        public virtual decimal RevenueToNextTier { get; set; }
        public virtual long TotalRentalsYTD { get; set; }
        public virtual decimal TotalRevenueYTD { get; set; }
        public virtual DateTime? TierEndDate { get; set; }
        public virtual string MarketingCode { get; set; }
        public virtual DateTime? LastActivityDate { get; set; }
        public virtual string ErrorResponse { get; set; }

    }
}
