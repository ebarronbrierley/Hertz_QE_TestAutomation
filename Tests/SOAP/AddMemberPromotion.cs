using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;
using Brierley.TestAutomation.Core.Reporting;
using Brierley.TestAutomation.Core.Utilities;
using HertzNetFramework.DataModels;

namespace HertzNetFramework.Tests.SOAP
{
    [TestFixture]
    public class AddMemberPromotion : BrierleyTestFixture
    {
        [Category("Api_Smoke")]
        [Category("Api_Positive")]
        [Category("AddMemberPromotion")]
        [Category("AddMemberPromotion_Positive")]
        [TestCaseSource("PositiveScenarios")]
        public void AddMemberPromotion_Positive(string name, MemberStyle memberStyle, Member member)
        {
            try
            {
                string expectedCode = "EMEA60DayBirthdayEM";

                Member createMember = member;
                BPTest.Start<TestStep>($"Make AddMember Call for {memberStyle} Member with {name}", "Member should be added successfully and member object should be returned");
                Member memberOut = Member.AddMember(createMember);
                Assert.IsNotNull(memberOut, "Expected populated member object, but member object returned was null");
                BPTest.Pass<TestStep>("Member was added successfully and member object was returned", memberOut.ReportDetail());

                BPTest.Start<TestStep>($"Find promotion in database", "Promotion should be found");
                IEnumerable<Promotion> promos = Promotion.GetFromDB(Database, code: expectedCode);
                Assert.NotNull(promos, "Expected Promotion.GetFromDB to return IEnumerable<Promotion> object");
                Assert.IsTrue(promos.Count() > 0, "Expected at least 1 promotion to be returned from DB");
                Assert.IsTrue(promos.First().CODE.Equals(expectedCode), "Promotion.CODE does not match expected");
                BPTest.Pass<TestStep>("Promotion was found", promos.ReportDetail());

                var loyaltyId = memberOut.VirtualCards.First().LOYALTYIDNUMBER;
                BPTest.Start<TestStep>($"Make AddMemberPromotion Call for LID {loyaltyId} with code {promos.First().CODE}", "AddMemberPromotion call should return MemberPromotion object");
                MemberPromotion memberPromoOut = memberOut.AddPromotion(loyaltyId, promos.First().CODE, null, null, false, null, null, false);
                Assert.IsNotNull(memberPromoOut, "Expected populated MemberPromotion object, but MemberPromotion object returned was null");
                BPTest.Pass<TestStep>("MemberPromotion object was returned", memberPromoOut.ReportDetail());

                BPTest.Start<TestStep>($"Verify member promotion exists in {MemberPromotion.TableName}", $"Member promotion should be in {MemberPromotion.TableName}");
                IEnumerable<MemberPromotion> dbMemberPromo = MemberPromotion.GetFromDB(Database, code: expectedCode, id: memberPromoOut.ID, memberId: memberOut.IPCODE);
                Assert.IsNotNull(dbMemberPromo, "Expected populated MemberPromotion object from database query, but MemberPromotion object returned was null");
                Assert.IsTrue(dbMemberPromo.Count() > 0, "Expected at least one MemberPromotion to be returned from query");
                AssertModels.AreEqualOnly(memberPromoOut, dbMemberPromo.First(), MemberPreferences.BaseVerify);
                BPTest.Pass<TestStep>("MemberPromotion object exists in table", dbMemberPromo.ReportDetail());
            }
            catch (LWServiceException ex)
            {
                BPTest.Fail<TestStep>(ex.Message, new[] { $"Error Code: {ex.ErrorCode}",$"Error Message: {ex.ErrorMessage}" });
                Assert.Fail();
            }
            catch (AssertModelEqualityException ex)
            {
                BPTest.Fail<TestStep>(ex.Message, ex.ComparisonFailures);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                BPTest.Fail<TestStep>(ex.Message);
                Assert.Fail();
            }

        }
        [TestCase]
        public void AddMemberPromotion_Negative()
        {

        }

        static object[] PositiveScenarios =
        {
            new object[]
            {
                $"EarningPreference = GPR, TierCode = '{GPR.Tier.RegularGold.Code}'",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne, HertzProgram.GoldPointsRewards.Set(GPR.Tier.RegularGold.Code,"SpecificTier"))
            },//EarningPreference = GPR, TierCode = '{GPR.Tier.RegularGold.Code}',
       };
    }
}
