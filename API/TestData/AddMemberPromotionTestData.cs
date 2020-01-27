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
                IHertzTier tier = HertzLoyalty.GoldPointsRewards.RegularGold;
                MemberModel member = MemberController.GenerateRandomMember(tier);
                string promoCode = "EMEA60DayBirthdayEM";
                yield return new TestCaseData(member, promoCode)
                    .SetName($"Add Member Promotion Positive - Program:[{tier.ParentProgram.Name}] Tier:[{tier.Name}] Promo Code:[{promoCode}]");
            }
        }
    }
}
