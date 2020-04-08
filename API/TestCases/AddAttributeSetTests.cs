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

    public class AddAttributeSetTests : BrierleyTestFixture
    {
        [TestCaseSource(typeof(AddAttributeSetTestData), "PositiveScenarios")]
        public void AddAttributeSet_Positive(MemberModel member, 
            AuctionHeaderModel auctionHeaderRequestModel,
            IHertzTier tier, bool useRanum)
        {
            MemberController memController = new MemberController(Database, TestStep);
            try
            {
                //Generate unique LIDs for each virtual card in the member
                member = memController.AssignUniqueLIDs(member);
                string ranum;

                TestStep.Start($"Make AddMember Call", "Source Member should be added successfully");
                MemberModel memberOut = memController.AddMember(member);
                AssertModels.AreEqualOnly(member, memberOut, MemberModel.BaseVerify);
                TestStep.Pass("Source Member was added successfully and member object was returned", memberOut.ReportDetail());
                VirtualCardModel vc = memberOut.VirtualCards.First();
                             
                //Transactions are added to test the API with ranum and also to test the negative points scenario
                TestStep.Start($"Add Transaction to the source member", "Transactions added successfully");
                vc.Transactions = TxnHeaderController.GenerateRandomTransactions(vc, tier.ParentProgram, 1, 500);
                if (useRanum)
                {
                    ranum = vc.Transactions.Select(x => x.A_RANUM).First();
                }
                else
                {
                    ranum = null;
                }
                Assert.IsNotNull(vc.Transactions, "Expected populated transaction object, but transaction object returned was null");
                TestStep.Pass("Transaction added to the source member", vc.Transactions.ReportDetail());

                TestStep.Start("Update Existing Source Member with added transaction", "Source Member object should be returned from UpdateMember call");
                MemberModel updatedMember = memController.UpdateMember(memberOut);
                Assert.IsNotNull(updatedMember, "Expected non null Member object to be returned");
                TestStep.Pass("Member object returned from UpdateMember API call", updatedMember.ReportDetail());

                TestStep.Start($"Make AddAttributeSet Call", "AddAttributeSet call should return AddAttributeSet object");
                AddAttributeSetResponseModel addAttributeSetResponseModel = memController.AddAttributeSet(vc,auctionHeaderRequestModel);
                Assert.IsNotNull(addAttributeSetResponseModel, "Expected populated AddAttributeSetResponseModel object, but AddAttributeSetResponseModel object returned was null");
                Assert.IsNotNull(addAttributeSetResponseModel.EarnedPoints, "Expected populated EarnedPoints list object, but it was null");
                TestStep.Pass("AddAttributeSetResponseModel object was returned", addAttributeSetResponseModel.EarnedPoints.ReportDetail());

                TestStep.Start($"Verify AuctionHeader ", "AuctionHeader returned should be correct");
                Assert.AreNotEqual(0, addAttributeSetResponseModel.EarnedPoints.Count);
                Assert.AreEqual(auctionHeaderRequestModel.HeaderGPRpts, Math.Abs( addAttributeSetResponseModel.EarnedPoints.FirstOrDefault().PointValue));
                TestStep.Pass("AuctionHeader response is as expcted", addAttributeSetResponseModel.EarnedPoints.ReportDetail());


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

        [TestCaseSource(typeof(AddAttributeSetTestData), "NegativeScenarios")]
        public void AddAttributeSet_Negative(MemberModel member,
          AuctionHeaderModel auctionHeaderRequestModel,
          int errorCode, string errorMessage)
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
                VirtualCardModel vc = memberOut.VirtualCards.First();

                TestStep.Start($"Make AddAttributeSet Call", "AddAttributeSet call should return AddAttributeSet object");
                LWServiceException exception =
                   Assert.Throws<LWServiceException>(() =>
                       memController.AddAttributeSet(vc, auctionHeaderRequestModel),
                       "Expected LWServiceException, exception was not thrown.");
                Assert.AreEqual(errorCode, exception.ErrorCode);
                Assert.IsTrue(exception.Message.Contains(errorMessage));
                TestStep.Pass("AddAttributeSet call threw expected exception", exception.ReportDetail());

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

