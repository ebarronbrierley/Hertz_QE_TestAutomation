using Brierley.TestAutomation.Core.Utilities;
using Hertz.API.Controllers;
using Hertz.API.DataModels;
using Hertz.API.TestData;
using Hertz.API.Utilities;
using NUnit.Framework;
using System;
using System.Linq;

namespace Hertz.API.TestCases
{
    [TestFixture]
    public class HTZUpdateTierTests : BrierleyTestFixture
    {
        [TestCaseSource(typeof(HTZUpdateTierTestData), "PositiveScenarios")]
        public void HTZUpdateTier_Positive(MemberModel member,
           IHertzTier newTier, DateTime newTierEndDate, string newMarketingCode, string csAgent)
        {
            MemberController memController = new MemberController(Database, TestStep);
            try
            {
                //Generate unique LIDs for each virtual card in the member
                member = memController.AssignUniqueLIDs(member);
                TestStep.Start($"Make AddMember Call", "Source Member should be added successfully");
                MemberModel memberOut = memController.AddMember(member);
                AssertModels.AreEqualOnly(member, memberOut, MemberModel.BaseVerify);
                TestStep.Pass("Source Member was added successfully and member object was returned", memberOut.ReportDetail());
                VirtualCardModel vcSource = memberOut.VirtualCards.First();
                                
                var loyaltyId = memberOut.VirtualCards.First().LOYALTYIDNUMBER;
                var vckey = memberOut.VirtualCards.First().VCKEY.ToString();

                TestStep.Start($"Make HertzUpdateTier Call", "HertzUpdateTier call should return HertzUpdateTierResponse object");
                HertzUpdateTierResponseModel memberUpdateTier = memController.HertzUpdateTier(loyaltyId,
                    csAgent, newTier.Code, newTierEndDate.ToString("MM/dd/yyyy"), newMarketingCode);
                Assert.IsNotNull(memberUpdateTier, "Expected populated HertzUpdateTierResponseModel object, but HertzUpdateTierResponseModel object returned was null");
                TestStep.Pass("HertzUpdateTierResponseModel object was returned", memberUpdateTier.ReportDetail());

                var tierCodeResponse = string.Empty;
                switch (memberUpdateTier.CURRENTTIERNAME)
                {
                    case "Platinum Select": tierCodeResponse = "PS";  break;
                    case "Platinum VIP": tierCodeResponse = "VP"; break;
                    case "Gold": tierCodeResponse = "RG"; break;
                    case "Five Star": tierCodeResponse = "FG"; break;
                    case "Presidents Circle": tierCodeResponse = "PC"; break;
                    case "Platinum": tierCodeResponse = "PL"; break;
                }                 

                TestStep.Start($"Verify New Tier values", "New Tier values returned should be correct");
                Assert.IsNotEmpty(tierCodeResponse);
                Assert.AreEqual(newTier.Code.ToUpper(), tierCodeResponse);
                Assert.IsNotNull(memberUpdateTier.A_TIERENDDATE);
                Assert.AreEqual(newTierEndDate.ToString("yyyy-MM-dd"), ((DateTime) memberUpdateTier.A_TIERENDDATE).ToString("yyyy-MM-dd"));
                Assert.AreEqual(newMarketingCode.ToUpper(), memberUpdateTier.A_MKTGPROGRAMID.ToUpper());
                TestStep.Pass("New Tier values response is as expcted", memberUpdateTier.ReportDetail());
                          
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

        [TestCaseSource(typeof(HTZUpdateTierTestData), "NegativeScenarios")]
        public void HTZUpdateTier_Negative(MemberModel member,
           string newTier, string tierEndDate, string marketingCode, string csAgent,
           int errorCode, string errorMessage )
        {
            MemberController memController = new MemberController(Database, TestStep);
            try
            {
                MemberModel memberOut = null;
                if (member != null)
                {
                    //Generate unique LIDs for each virtual card in the member
                    member = memController.AssignUniqueLIDs(member);
                    TestStep.Start($"Make AddMember Call", "Member should be added successfully");
                    memberOut = memController.AddMember(member);
                    AssertModels.AreEqualOnly(member, memberOut, MemberModel.BaseVerify);
                    TestStep.Pass("Member was added successfully and member object was returned", memberOut.ReportDetail());
                }
                var loyaltyId = memberOut?.VirtualCards?.First().LOYALTYIDNUMBER ?? "";
                var tierCode = newTier ?? "";

                TestStep.Start($"Make HertzUpdateTier Call", $"HertzUpdateTier call should throw exception with error code = {errorCode}");
                LWServiceException exception =
                    Assert.Throws<LWServiceException>(() =>
                        memController.HertzUpdateTier(loyaltyId, csAgent, tierCode, tierEndDate, marketingCode),
                        "Expected LWServiceException, exception was not thrown.");
                Assert.AreEqual(errorCode, exception.ErrorCode);
                Assert.IsTrue(exception.Message.Contains(errorMessage));
                TestStep.Pass("HertzUpdateTier call threw expected exception", exception.ReportDetail());
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
