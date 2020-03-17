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
    public class AddMemberPromotionTestData
    {
        public static IEnumerable PositiveScenarios
        {
            get
            {
                string promoCode = "EMEA60DayBirthdayEM";
                foreach (IHertzProgram program in HertzLoyalty.Programs)
                {
                    foreach (IHertzTier tier in program.Tiers)
                    {
                        MemberModel createMember = MemberController.GenerateRandomMember(tier);
                        yield return new TestCaseData(createMember, promoCode)
                                        .SetName($"Add Member Promotion Positive - Program:[{tier.ParentProgram.Name}] Tier:[{tier.Name}] Promo Code:[{promoCode}]");

                    }
                }
            }
        }

        public static IEnumerable NegativeScenarios
        {
            get
            {
                string promocode = null;
                string loyaltyId = "123456677890";
                int errorCode = 2003;
                string errorMessage = "PromotionCode of AddMemberPromotionIn is a required property.  Please provide a valid value";
                yield return new TestCaseData(loyaltyId, promocode, errorCode, errorMessage)
                    .SetName($"AddMemberPromotion Negative Null PromoCode - Error Code:[{errorCode}]");

                promocode = "EMEA60DayBirthdayEM";
                loyaltyId = null;
                errorCode = 2003;
                errorMessage = "MemberIdentity of AddMemberPromotionIn is a required property.  Please provide a valid value";
                yield return new TestCaseData(loyaltyId, promocode, errorCode, errorMessage)
                    .SetName($"AddMemberPromotion Negative Null LoyaltyId - Error Code:[{errorCode}]");

                promocode = "EMEA60DayBirthdayEM";
                loyaltyId = "123456677890";
                errorCode = 3302;
                errorMessage = "Unable to find member with identity";
                yield return new TestCaseData(loyaltyId, promocode, errorCode, errorMessage)
                    .SetName($"AddMemberPromotion Negative - Error Code:[{errorCode}]");

                promocode = "0000";
                loyaltyId = "123456677890";
                errorCode = 3362;
                errorMessage = "No content available that matches the specified criteria";
                yield return new TestCaseData(loyaltyId, promocode, errorCode, errorMessage)
                    .SetName($"AddMemberPromotion Negative - Error Code:[{errorCode}]");

                promocode = string.Empty;
                loyaltyId = string.Empty;
                errorCode = 3343;
                errorMessage = null;
                yield return new TestCaseData(loyaltyId, promocode, errorCode, errorMessage)
                    .SetName($"AddMemberPromotion Negative - Error Code:[{errorCode}]");
            }
        }
    }
}
