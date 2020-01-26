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
    public class AddMemberTestData
    {
        public static IEnumerable PositiveScenarios
        {
            get
            {
                foreach (IHertzProgram program in HertzLoyalty.Programs)
                {
                    foreach (IHertzTier tier in program.Tiers)
                    {
                        MemberModel gprMember = MemberController.GenerateRandomMember(tier);
                        yield return new TestCaseData(gprMember).SetName($"Add Member Positive - Program: [{program.Name}] Tier: [{tier.Name}]");
                    }
                }
            }
        }

        public static IEnumerable NegativeScenarios
        {
            get
            {
                StringBuilder errorMessage = new StringBuilder();

                MemberModel member = MemberController.GenerateRandomMember(HertzLoyalty.GoldPointsRewards.RegularGold);
                member.VirtualCards = null;
                int errorCode = 9993;
                errorMessage.Clear().Append("Unable to find node VirtualCard");
                yield return new TestCaseData(member, errorCode, errorMessage.ToString()).SetName($"Add Member Negative - No VirtualCard null");

                member = MemberController.GenerateRandomMember(HertzLoyalty.GoldPointsRewards.RegularGold);
                member.VirtualCards.First().LOYALTYIDNUMBER = "00000200";
                errorCode = 9991;
                errorMessage.Clear().Append("A member already exists");
                yield return new TestCaseData(member,errorCode, errorMessage.ToString()).SetName($"Add Member Negative - Existing Loyalty ID");

                member = MemberController.GenerateRandomMember(HertzLoyalty.GoldPointsRewards.RegularGold);
                member.MemberDetails.A_TIERCODE = "abcd";
                errorCode = 2;
                errorMessage.Clear().Append("Invalid tier code");
                yield return new TestCaseData(member, errorCode, errorMessage.ToString()).SetName($"Add Member Negative - Invalid Tier Code [{member.MemberDetails.A_TIERCODE}]");

                member = MemberController.GenerateRandomMember(HertzLoyalty.GoldPointsRewards.RegularGold);
                member.MemberDetails.A_EARNINGPREFERENCE = "invalid";
                errorCode = 1;
                errorMessage.Clear().Append("Object reference not set to an instance of an object");
                yield return new TestCaseData(member, errorCode, errorMessage.ToString()).SetName($"Add Member Negative - Invalid Earning Preference [{member.MemberDetails.A_EARNINGPREFERENCE}]");
            }
        }
    }
}
