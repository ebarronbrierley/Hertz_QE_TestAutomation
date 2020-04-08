using System;

namespace Hertz.API.DataModels
{
    public class RewardCatalogSummaryResponseModel
    {
        public long RewardID { get; set; }
        public string RewardName { get; set; }
        public string TypeCode { get; set; }
        public long CategoryId { get; set; }
        public string ShortDescription { get; set; }
        public DateTime CatalogStartDate { get; set; }
        public DateTime CatalogEndDate { get; set; }
        public long CurrencyToEarn { get; set; }
        public string SmallImageFile { get; set; }
     
    }

    public class ContentSearchAttribute
    {
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
    }

}
