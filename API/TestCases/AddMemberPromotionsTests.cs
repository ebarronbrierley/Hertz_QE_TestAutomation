using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brierley.TestAutomation.Core.Reporting;
using Brierley.TestAutomation.Core.Database;
using Brierley.TestAutomation.Core.Utilities;
using Hertz.API.Controllers;
using Hertz.API.DataModels;
using Hertz.API.Utilities;
using Hertz.API.TestData;
using NUnit.Framework;

namespace Hertz.API.TestCases
{
    [TestFixture]
    public class AddMemberPromotionsTests : BrierleyTestFixture
    {
        [TestCaseSource(typeof(AddMemberPromotionTestData), "PositiveScenarios")]
        public void AddMemberPromotion_Positive(MemberModel member, string promotionCode)
        {
            MemberController memController = new MemberController(Database, TestStep);
            PromotionController promoController = new PromotionController(Database, TestStep);

            try
            {
                TestStep.Start($"Make AddMember Call", "Member should be added successfully");
                MemberModel memberOut = memController.AddMember(member);
                AssertModels.AreEqualOnly(member, memberOut, MemberModel.BaseVerify);
                TestStep.Pass("Member was added successfully and member object was returned", memberOut.ReportDetail());

                TestStep.Start($"Find promotion in database", "Promotion should be found");
                IEnumerable<PromotionModel> promos = promoController.GetFromDB(code: promotionCode);
                Assert.NotNull(promos, "Expected Promotion.GetFromDB to return IEnumerable<Promotion> object");
                Assert.IsTrue(promos.Any(x=>x.CODE.Equals(promotionCode)), "Expected promotion code was not found in database");
                TestStep.Pass("Promotion was found", promos.ReportDetail());

                var loyaltyId = memberOut.VirtualCards.First().LOYALTYIDNUMBER;
                TestStep.Start($"Make AddMemberPromotion Call", "AddMemberPromotion call should return MemberPromotion object");
                MemberPromotionModel memberPromoOut = memController.AddMemberPromotion(loyaltyId, promotionCode, null, null, false, null, null, false);
                Assert.IsNotNull(memberPromoOut, "Expected populated MemberPromotion object, but MemberPromotion object returned was null");
                TestStep.Pass("MemberPromotion object was returned", memberPromoOut.ReportDetail());

                TestStep.Start($"Verify member promotion exists in {MemberPromotionModel.TableName}", $"Member promotion should be in {MemberPromotionModel.TableName}");
                IEnumerable<MemberPromotionModel> dbMemberPromo = memController.GetMemberPromotionsFromDB(memberPromoOut.ID, promotionCode, memberOut.IPCODE);
                Assert.IsNotNull(dbMemberPromo, "Expected populated MemberPromotion object from database query, but MemberPromotion object returned was null");
                Assert.Greater(dbMemberPromo.Count(), 0,  "Expected at least one MemberPromotion to be returned from query");
                AssertModels.AreEqualOnly(memberPromoOut, dbMemberPromo.First(), MemberPromotionModel.BaseVerify);
                TestStep.Pass("MemberPromotion object exists in table", dbMemberPromo.ReportDetail());
            }
            catch(AssertionException ex)
            {
                TestStep.Fail(ex.Message);
                Assert.Fail();
            }
            catch (LWServiceException ex)
            {
                TestStep.Fail(ex.Message, new[] { $"Error Code: {ex.ErrorCode}", $"Error Message: {ex.ErrorMessage}" });
                Assert.Fail();
            }
            catch (AssertModelEqualityException ex)
            {
                TestStep.Fail(ex.Message, ex.ComparisonFailures);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                TestStep.Abort(ex.Message);
                Assert.Fail();
            }
        }
    }
}
