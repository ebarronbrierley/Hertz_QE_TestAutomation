using Brierley.TestAutomation.Core.Utilities;
using Hertz.API.Controllers;
using Hertz.API.DataModels;
using Hertz.API.TestData;
using Hertz.API.Utilities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hertz.API.TestCases
{
    [TestFixture]
    public class HertzTransferPointsTests : BrierleyTestFixture
    {
        [TestCaseSource(typeof(HertzTransferPointsTestData), "PositiveScenarios")]
        public void HertzTransferPoints_Positive(MemberModel memberSource, MemberModel memberDestionation, decimal points, IHertzTier tier, bool useRanum)
        {
            MemberController memController = new MemberController(Database, TestStep);
            try
            {
                //Generate unique LIDs for each virtual card in the member
                memberSource = memController.AssignUniqueLIDs(memberSource);
                string ranum;

                TestStep.Start($"Make AddMember Call", "Source Member should be added successfully");
                MemberModel memberOutSource = memController.AddMember(memberSource);
                AssertModels.AreEqualOnly(memberSource, memberOutSource, MemberModel.BaseVerify);
                TestStep.Pass("Source Member was added successfully and member object was returned", memberOutSource.ReportDetail());
                VirtualCardModel vcSource = memberOutSource.VirtualCards.First();

                memberDestionation = memController.AssignUniqueLIDs(memberDestionation);
                TestStep.Start($"Make AddMember Call", "Destination Member should be added successfully");
                MemberModel memberOutDestination = memController.AddMember(memberDestionation);
                AssertModels.AreEqualOnly(memberDestionation, memberOutDestination, MemberModel.BaseVerify);
                TestStep.Pass("Destination Member was added successfully and member object was returned", memberOutDestination.ReportDetail());
                VirtualCardModel vcDestination = memberOutDestination.VirtualCards.First();


                //Transactions are added to test the API with ranum and also to test the negative points scenario
                TestStep.Start($"Add Transaction to the source member", "Transactions added successfully");
                vcSource.Transactions = TxnHeaderController.GenerateRandomTransactions(vcSource, tier.ParentProgram, 1, 500);
                if (useRanum)
                {
                    ranum = vcSource.Transactions.Select(x => x.A_RANUM).First();
                }
                else
                {
                    ranum = null;
                }
                Assert.IsNotNull(vcSource.Transactions, "Expected populated transaction object, but transaction object returned was null");
                TestStep.Pass("Transaction added to the source member", vcSource.Transactions.ReportDetail());

                TestStep.Start("Update Existing Source Member with added transaction", "Source Member object should be returned from UpdateMember call");
                MemberModel updatedMember = memController.UpdateMember(memberOutSource);
                Assert.IsNotNull(updatedMember, "Expected non null Member object to be returned");
                TestStep.Pass("Member object returned from UpdateMember API call", updatedMember.ReportDetail());


                var loyaltyIdSource = memberOutSource.VirtualCards.First().LOYALTYIDNUMBER;
                var loyaltyIdDestination = memberOutDestination.VirtualCards.First().LOYALTYIDNUMBER;
                var vckeySource = memberOutSource.VirtualCards.First().VCKEY.ToString();
                var vckeyDestination = memberOutDestination.VirtualCards.First().VCKEY.ToString();

                TestStep.Start("Get Account Summary from DB for Source Member", "Account Summary retrieved from DB");
                MemberAccountSummaryModel memberAccountSummaryOutDbSourceInitial = memController.GetMemberAccountSummaryFromDB(vckeySource);
                Assert.IsNotNull(memberAccountSummaryOutDbSourceInitial, "Account Summary could not be retrieved from DB");
                TestStep.Pass("Existing member was found", memberAccountSummaryOutDbSourceInitial.ReportDetail());

                TestStep.Start("Get Account Summary from DB for Destination Member", "Account Summary retrieved from DB");
                MemberAccountSummaryModel memberAccountSummaryOutDbDestinationInitial = memController.GetMemberAccountSummaryFromDB(vckeyDestination);
                Assert.IsNotNull(memberAccountSummaryOutDbDestinationInitial, "Account Summary could not be retrieved from DB");
                TestStep.Pass("Existing member was found", memberAccountSummaryOutDbDestinationInitial.ReportDetail());


                TestStep.Start($"Make HertzTransferPoints Call", "HertzTransferPoints call should return HertzTransferPoints object");
                HertzTransferPointsResponseModel memberTransferPoints = memController.HertzTransferPoints(loyaltyIdSource, "csadmin", points.ToString(),
                   loyaltyIdDestination, "automation");
                Assert.IsNotNull(memberTransferPoints, "Expected populated HertzTransferPoints object, but HertzTransferPoints object returned was null");
                TestStep.Pass("HertzTransferPoints object was returned", memberTransferPoints.ReportDetail());

                TestStep.Start($"Verify CurrencyBalance of Source Member was decreased", "CurrencyBalance returned should be correct");
                MemberAccountSummaryModel memberAccountSummaryOutDbSourceFinal = memController.GetMemberAccountSummaryFromDB(vckeySource);
                Assert.IsNotNull(memberAccountSummaryOutDbSourceFinal, "Account Summary could not be retrieved from DB");
                Assert.AreEqual(memberAccountSummaryOutDbSourceInitial.CURRENCYBALANCE - points,
                    memberAccountSummaryOutDbSourceFinal.CURRENCYBALANCE,
                    $"CurrencyBalance wasn't decrease due TransferPoints");
                TestStep.Pass("CurrencyBalance response is as expcted", memberAccountSummaryOutDbSourceFinal.ReportDetail());

                TestStep.Start($"Verify CurrencyBalance of Destination Member was increased", "CurrencyBalance returned should be correct");
                MemberAccountSummaryModel memberAccountSummaryOutDbDestinationFinal = memController.GetMemberAccountSummaryFromDB(vckeyDestination);
                Assert.IsNotNull(memberAccountSummaryOutDbDestinationFinal, "Account Summary could not be retrieved from DB");
                Assert.AreEqual(memberAccountSummaryOutDbDestinationInitial.CURRENCYBALANCE + points,
                    memberAccountSummaryOutDbDestinationFinal.CURRENCYBALANCE,
                    $"CurrencyBalance wasn't decrease due TransferPoints");
                TestStep.Pass("CurrencyBalance response is as expcted", memberAccountSummaryOutDbDestinationFinal.ReportDetail());

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

        [TestCaseSource(typeof(HertzTransferPointsTestData), "NegativeScenarios")]
        public void HertzTransferPoints_Negative(MemberModel memberSource, MemberModel memberDestination,
            int errorCode, string errorMessage, string points, string csagent, string reasonCode)
        {
            MemberController memController = new MemberController(Database, TestStep);
            try
            {
                MemberModel memberOutSource = null;
                if (memberSource != null)
                {
                    //Generate unique LIDs for each virtual card in the member
                    memberSource = memController.AssignUniqueLIDs(memberSource);
                    TestStep.Start($"Make AddMember Call", "Member should be added successfully");
                    memberOutSource = memController.AddMember(memberSource);
                    AssertModels.AreEqualOnly(memberSource, memberOutSource, MemberModel.BaseVerify);
                    TestStep.Pass("Member was added successfully and member object was returned", memberOutSource.ReportDetail());
                }
                MemberModel memberOutDest = null;
                if (memberDestination != null)
                {
                    memberDestination = memController.AssignUniqueLIDs(memberDestination);
                    TestStep.Start($"Make AddMember Call", "Member should be added successfully");
                    memberOutDest = memController.AddMember(memberDestination);
                    AssertModels.AreEqualOnly(memberDestination, memberOutDest, MemberModel.BaseVerify);
                    TestStep.Pass("Member was added successfully and member object was returned", memberOutDest.ReportDetail());
                }
                var loyaltyIdSource = memberOutSource?.VirtualCards?.First().LOYALTYIDNUMBER??"";
                var loyaltyIdDestination = memberOutDest?.VirtualCards?.First().LOYALTYIDNUMBER??"";

                TestStep.Start($"Make HertzTransferPoints Call", $"HertzTransferPoints call should throw exception with error code = {errorCode}");
                LWServiceException exception = 
                    Assert.Throws<LWServiceException>(() => 
                        memController.HertzTransferPoints(loyaltyIdSource, csagent, points,
                            loyaltyIdDestination,reasonCode), 
                        "Expected LWServiceException, exception was not thrown.");
                Assert.AreEqual(errorCode, exception.ErrorCode);
                Assert.IsTrue(exception.Message.Contains(errorMessage));
                TestStep.Pass("HertzTransferPoints call threw expected exception", exception.ReportDetail());
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
