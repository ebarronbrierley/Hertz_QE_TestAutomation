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
                TestStep.Start("Adding Member unique LoyaltyIds for each virtual card", "Unique LoyaltyIds should be assigned");
                member = memController.AssignUniqueLIDs(member);
                TestStep.Pass("Unique LoyaltyIds assigned", member.VirtualCards.ReportDetail());

                string loyaltyID = member.VirtualCards[0].LOYALTYIDNUMBER;
                TestStep.Start($"Make AddMember Call", $"Member with LID {loyaltyID} should be added successfully and member object should be returned");
                MemberModel memberOut = memController.AddMember(member);
                AssertModels.AreEqualOnly(member, memberOut, MemberModel.BaseVerify);
                TestStep.Pass("Member was added successfully and member object was returned", memberOut.ReportDetail());

                TestStep.Start($"Find promotion in database", "Promotion should be found");
                IEnumerable<PromotionModel> promos = promoController.GetFromDB(code: promotionCode);
                Assert.NotNull(promos, "Expected Promotion.GetFromDB to return IEnumerable<Promotion> object");
                Assert.IsTrue(promos.Any(x => x.CODE.Equals(promotionCode)), "Expected promotion code was not found in database");
                TestStep.Pass("Promotion was found", promos.ReportDetail());

                TestStep.Start($"Make AddMemberPromotion Call", "AddMemberPromotion call should return MemberPromotion object");
                MemberPromotionModel memberPromoOut = memController.AddMemberPromotion(loyaltyID, promotionCode, null, null, false, null, null, false);
                Assert.IsNotNull(memberPromoOut, "Expected populated MemberPromotion object, but MemberPromotion object returned was null");
                TestStep.Pass("MemberPromotion object was returned", memberPromoOut.ReportDetail());

                TestStep.Start($"Verify member promotion exists in {MemberPromotionModel.TableName}", $"Member promotion should be in {MemberPromotionModel.TableName}");
                IEnumerable<MemberPromotionModel> dbMemberPromo = memController.GetMemberPromotionsFromDB(memberPromoOut.ID, promotionCode, memberOut.IPCODE);
                Assert.IsNotNull(dbMemberPromo, "Expected populated MemberPromotion object from database query, but MemberPromotion object returned was null");
                Assert.Greater(dbMemberPromo.Count(), 0, "Expected at least one MemberPromotion to be returned from query");
                AssertModels.AreEqualOnly(memberPromoOut, dbMemberPromo.First(), MemberPromotionModel.BaseVerify);
                TestStep.Pass("MemberPromotion object exists in table", dbMemberPromo.ReportDetail());
            }
            catch (AssertionException ex)
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
        [TestCaseSource(typeof(AddMemberPromotionTestData), "NegativeScenarios")]
        public void AddMemberPromotion_Negative(string loyaltyId, string promoCode, int errorCode, string errorMessage)
        {
            MemberController memController = new MemberController(Database, TestStep);
            PromotionController promoController = new PromotionController(Database, TestStep);
            try
            {
                if (!string.IsNullOrEmpty(promoCode) && promoCode.Equals("0000"))
                {
                    TestStep.Start("Adding Member unique LoyaltyIds for each virtual card", "Unique LoyaltyIds should be assigned");
                    MemberModel createMember = MemberController.GenerateRandomMember(HertzLoyalty.GoldPointsRewards.RegularGold);
                    createMember = memController.AssignUniqueLIDs(createMember);
                    TestStep.Pass("Unique LoyaltyIds assigned", createMember.VirtualCards.ReportDetail());

                    loyaltyId = createMember.VirtualCards[0].LOYALTYIDNUMBER;
                    TestStep.Start($"Make AddMember Call", $"Member with LID {loyaltyId} should be added successfully and member object should be returned");
                    MemberModel memberOut = memController.AddMember(createMember);
                    AssertModels.AreEqualOnly(createMember, memberOut, MemberModel.BaseVerify);
                    TestStep.Pass("Member was added successfully and member object was returned", memberOut.ReportDetail());

                    loyaltyId = memberOut.VirtualCards.First().LOYALTYIDNUMBER;
                }
                else if (promoCode == string.Empty)
                {
                    TestStep.Start("Get Expired promotion from DB","Expired promo found in DB");
                    IEnumerable<PromotionModel> promoOutDb = promoController.GetRandomExpiredPromotionFromDB();
                    Assert.Greater(promoOutDb.Count(), 0, "Expected one or more expired promotions from DB");
                    TestStep.Pass("Expired promotion found in DB", promoOutDb.ReportDetail());

                    promoCode = promoOutDb.First().CODE;
                    errorMessage = $"Promotion {promoCode} is not valid anymore";

                    TestStep.Start("Adding Member unique LoyaltyIds for each virtual card", "Unique LoyaltyIds should be assigned");
                    MemberModel createMember = MemberController.GenerateRandomMember(HertzLoyalty.GoldPointsRewards.RegularGold);
                    createMember = memController.AssignUniqueLIDs(createMember);
                    TestStep.Pass("Unique LoyaltyIds assigned", createMember.VirtualCards.ReportDetail());

                    loyaltyId = createMember.VirtualCards[0].LOYALTYIDNUMBER;
                    TestStep.Start($"Make AddMember Call", $"Member with LID {loyaltyId} should be added successfully and member object should be returned");
                    MemberModel memberOut = memController.AddMember(createMember);
                    AssertModels.AreEqualOnly(createMember, memberOut, MemberModel.BaseVerify);
                    TestStep.Pass("Member was added successfully and member object was returned", memberOut.ReportDetail());
                }

                TestStep.Start("Make AddMemberPromotion Call", $"AddMemberPromotion call should throw exception with error code = {errorCode}");
                LWServiceException exception = Assert.Throws<LWServiceException>(() => memController.AddMemberPromotion(loyaltyId, promoCode, null, null, null, null, null, null),
                                                                                    "Expected LWServiceException, exception was not thrown.");
                Assert.AreEqual(errorCode, exception.ErrorCode, $"Expected Error Code: {errorCode}");
                Assert.IsTrue(exception.Message.Contains(errorMessage), $"Expected Error Message to contain: {errorMessage}");
                TestStep.Pass("AddMemberPromotion call threw expected exception", exception.ReportDetail());
            }
            catch (AssertionException ex)
            {
                TestStep.Fail(ex.Message);
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

   
