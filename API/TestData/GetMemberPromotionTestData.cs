using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brierley.TestAutomation.Core.Utilities;
using Hertz.API.Controllers;
using Hertz.API.DataModels;
using NUnit.Framework;


namespace Hertz.API.TestData
{
    public class GetMemberPromotionTestData
    {
        public static IEnumerable PositiveScenarios
        {
            get
            {
                string promoCode = "EMEA60DayBirthdayEM";
                yield return new TestCaseData(promoCode)
                    .SetName($"Get Member Promotion Positive - Promo Code:[{promoCode}]");
            }
        }

        public static IEnumerable NegativeScenarios
        {
            get
            {
                string loyaltyId = null;
                int errorCode = 2003;
                string errorMessage = "MemberIdentity of GetMemberPromotionsIn is a required property.  Please provide a valid value";
                yield return new TestCaseData(loyaltyId, errorCode, errorMessage)
                    .SetName($"Get Member Promotion Count Negative - Error Code:[{errorCode}]");               

                loyaltyId = "123456677890";
                errorCode = 3302;
                errorMessage = "Unable to find member with identity";
                yield return new TestCaseData(loyaltyId, errorCode, errorMessage)
                    .SetName($"Get Member Promotion Count Negative - Error Code:[{errorCode}]");

                loyaltyId = string.Empty;
                errorCode = 3362;
                errorMessage = "No member promotions found";
                yield return new TestCaseData(loyaltyId, errorCode, errorMessage)
                    .SetName($"Get Member Promotion Count Negative - Error Code:[{errorCode}]");
            }
        }
    }
}
