using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hertz.API.DataModels
{
    public class GetMemberRewardsResponseModel
    {
        public string AddressLineFour { get; set; }
        public string AddressLineOne { get; set; }
        public string AddressLineThree { get; set; }
        public string AddressLineTwo { get; set; }
        public string ChangedBy { get; set; }
        public string Channel { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string County { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<MemberRewardsModel> MemberRewardsInfo { get; set; }
        public string OrderNumber { get; set; }
        public string Source { get; set; }
        public string StateOrProvince { get; set; }
        public string ZipOrPostalCode { get; set; }


    }
}
