﻿using System;
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
    public class GetMemberPromotionTests : BrierleyTestFixture
    {
        [TestCaseSource(typeof(GetMemberPromotionTestData), "PositiveScenarios")]
        public void GetMemberPromotion_Positive(MemberModel createMember, string promotionCode)
        {
            MemberController memController = new MemberController(Database, TestStep);
            PromotionController promoController = new PromotionController(Database, TestStep);
            try
            {
                TestStep.Start("Assing Member unique LoyaltyIds for each virtual card", "Unique LoyaltyIds should be assigned");
                createMember = memController.AssignUniqueLIDs(createMember);
                TestStep.Pass("Unique LoyaltyIds assigned", createMember.VirtualCards.ReportDetail());

                TestStep.Start($"Make AddMember Call", "Member should be added successfully and member object should be returned");
                MemberModel memberOut = memController.AddMember(createMember);
                AssertModels.AreEqualOnly(createMember, memberOut, MemberModel.BaseVerify);
                TestStep.Pass("Member was added successfully and member object was returned", memberOut.ReportDetail());

                TestStep.Start("Find promotion in database", "Promotion should be found");
                IEnumerable<PromotionModel> promos = promoController.GetFromDB(code: promotionCode);
                Assert.NotNull(promos, "Expected Promotion.GetFromDB to return IEnumerable<Promotion> object");
                Assert.IsTrue(promos.Any(x => x.CODE.Equals(promotionCode)), "Expected promotion code was not found in database");
                TestStep.Pass("Promotion was found", promos.ReportDetail());
                
                var loyaltyId = memberOut.VirtualCards.First().LOYALTYIDNUMBER;
                TestStep.Start("Make AddMemberPromotion Call", "AddMemberPromotion call should return MemberPromotion object");
                MemberPromotionModel memberPromoOut = memController.AddMemberPromotion(loyaltyId, promotionCode, null, null, false, null, null, false);
                Assert.IsNotNull(memberPromoOut, "Expected populated MemberPromotion object, but MemberPromotion object returned was null");
                TestStep.Pass("MemberPromotion object was returned", memberPromoOut.ReportDetail());

                TestStep.Start($"Verify added member promotion exists in {MemberPromotionModel.TableName}", $"Member promotion should be in {MemberPromotionModel.TableName}");
                IEnumerable<MemberPromotionModel>  dbMemberPromo = memController.GetMemberPromotionsFromDB(null, null, memberOut.IPCODE);
                Assert.IsNotNull(dbMemberPromo, "Expected populated MemberPromotion object from database query, but MemberPromotion object returned was null");
                Assert.Greater(dbMemberPromo.Count(), 0, "Expected at least one MemberPromotion to be returned from query");
                AssertModels.AreEqualOnly(memberPromoOut, dbMemberPromo.OrderByDescending(x => x.CREATEDATE).First(), MemberPromotionModel.BaseVerify);
                TestStep.Pass("MemberPromotion object exists in table", dbMemberPromo.ReportDetail());

                TestStep.Start("Make GetMemberPromotion call", "Member promotion should be returned");
                IEnumerable<MemberPromotionModel> memberPromotionModelOut = memController.GetMemberPromotion(loyaltyId, null, null, false, string.Empty, string.Empty, false, null);
                Assert.Greater(memberPromotionModelOut.Count(), 0, "Expected at least one MemberPromotion to be returned from API");
                TestStep.Pass("MemberPromotion object was returned from API", memberPromotionModelOut.ReportDetail());

                TestStep.Start("Compare Member Promotion Count", "Member promotion count from API should match count from DB");
                Assert.AreEqual(memberPromotionModelOut.Count(), dbMemberPromo.Count(), "Expected Member Promotion count to match");
                TestStep.Pass("MemberPromotion count match completed");

                TestStep.Start($"Verify Member Promotion  in {MemberPromotionModel.TableName} table", "Member Promo from API GetMemberPromotion");
                AssertModels.AreEqualWithAttribute(memberPromotionModelOut, dbMemberPromo);
                TestStep.Pass("API Member promotion created matches member promotions in database", memberOut.MemberPreferences.ReportDetail());

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

        [TestCaseSource(typeof(GetMemberPromotionTestData), "NegativeScenarios")]
        public void GetMemberPromotion_Negative(string loyaltyId, int errorCode, string errorMessage)
        {
            MemberController memController = new MemberController(Database, TestStep);
            try
            {
                if (loyaltyId == null)
                {
                    TestStep.Start("Make GetMemberPromotions Call", $"GetMemberPromotions call should throw exception with error code = {errorCode}");
                    LWServiceException exception = Assert.Throws<LWServiceException>(() => memController.GetMemberPromotion(loyaltyId, null, null, null, null, null, null, null),
                                                                                     "Expected LWServiceException, exception was not thrown.");
                    Assert.AreEqual(errorCode, exception.ErrorCode, $"Expected Error Code: {errorCode}");
                    Assert.IsTrue(exception.Message.Contains(errorMessage), $"Expected Error Message to contain: {errorMessage}");
                    TestStep.Pass("GetMemberPromotions call threw expected exception", exception.ReportDetail());
                }
                else if (string.Equals(loyaltyId, string.Empty))
                {
                    TestStep.Start("Get Existing Member without promotion from the database", "Existing member should be found");
                    MemberModel dbMember = memController.GetRandomMemberDBForMemberPromotion(MemberModel.Status.Active);
                    IEnumerable<MemberPromotionModel> dbMemPromo = memController.GetMemberPromotionsFromDB(memberId: dbMember.IPCODE);
                    Assert.IsNotNull(dbMember, "Member could not be retrieved from DB");
                    Assert.IsNotNull(dbMemPromo, "MemberPromotion could not be retrieved from DB");
                    TestStep.Pass("Existing member without promotion was found", dbMember.ReportDetail());

                    loyaltyId = dbMember.VirtualCards.First().LOYALTYIDNUMBER;
                    TestStep.Start("Make GetMemberPromotions Call", $"GetMemberPromotions call should throw exception with error code = {errorCode}");
                    LWServiceException exception = Assert.Throws<LWServiceException>(() => memController.GetMemberPromotion(loyaltyId, null, null, null, null, null, null, null),
                                                                 "Expected LWServiceException, exception was not thrown.");
                    Assert.AreEqual(errorCode, exception.ErrorCode, $"Expected Error Code: {errorCode}");
                    Assert.IsTrue(exception.Message.Contains(errorMessage), $"Expected Error Message to contain: {errorMessage}");
                    TestStep.Pass("GetMemberPromotions call threw expected exception", exception.ReportDetail());
                }
                else
                {
                    TestStep.Start("Make GetMemberPromotions Call", $"GetMemberPromotions call should throw exception with error code = {errorCode}");
                    LWServiceException exception = Assert.Throws<LWServiceException>(() => memController.GetMemberPromotion(loyaltyId, null, null, null, null, null, null, null),
                                                                 "Expected LWServiceException, exception was not thrown.");
                    Assert.AreEqual(errorCode, exception.ErrorCode, $"Expected Error Code: {errorCode}");
                    Assert.IsTrue(exception.Message.Contains(errorMessage), $"Expected Error Message to contain: {errorMessage}");
                    TestStep.Pass("GetMemberPromotions call threw expected exception", exception.ReportDetail());
                }
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
