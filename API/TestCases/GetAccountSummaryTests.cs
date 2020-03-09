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
    public class GetAccountSummaryTests : BrierleyTestFixture
    {
        [TestCaseSource(typeof(GetAccountSummaryTestData), "PositiveScenarios")]
        public void GetAccountSummaryTestsTest_Positive(MemberModel createMember, IHertzProgram program)
        {
            MemberController memController = new MemberController(Database, TestStep);
            try
            {
                TestStep.Start("Assing Member unique LoyaltyIds for each virtual card", "Unique LoyaltyIds should be assigned");
                createMember = memController.AssignUniqueLIDs(createMember);
                TestStep.Pass("Unique LoyaltyIds assigned", createMember.VirtualCards.ReportDetail());

                TestStep.Start($"Make AddMember Call", "Member should be added successfully and member object should be returned");
                MemberModel memberOut = memController.AddMember(createMember);
                AssertModels.AreEqualOnly(createMember, memberOut, MemberModel.BaseVerify);
                TestStep.Pass("Member was added successfully and member object was returned", memberOut.ReportDetail());

                int transactionCount = 1;
                var memVirtualCard = memberOut.VirtualCards.First();
                TestStep.Start($"Add random transaction(s) to members virtual card with VCKEY = {memVirtualCard.VCKEY}", "Transaction(s) should be added to members virtual card");
                memVirtualCard.Transactions = TxnHeaderController.GenerateRandomTransactions(memVirtualCard, program, transactionCount, 500M);
                Assert.AreEqual(transactionCount, memVirtualCard.Transactions.Count, $"Expected {transactionCount} TxnHeader(s) to be present in members vitual card");
                TestStep.Pass("Transaction(s) is added to members virtual card", memVirtualCard.Transactions.ReportDetail());

                foreach (var transaction in memVirtualCard.Transactions)
                {
                    transaction.A_TXNQUALPURCHASEAMT = TxnHeaderController.CalculateQualifyingPurchaseAmount(transaction);
                    transaction.A_QUALTOTAMT = TxnHeaderController.CalculateQualifyingPurchaseAmount(transaction);
                }

                TestStep.Start("Update Existing Member with added transaction", "Member object should be returned from UpdateMember call");
                MemberModel updatedMember = memController.UpdateMember(memberOut);
                Assert.IsNotNull(updatedMember, "Expected non null Member object to be returned");
                TestStep.Pass("Member object returned from UpdateMember API call", updatedMember.ReportDetail());

                var vckey = memVirtualCard.VCKEY.ToString();
                TestStep.Start("Get Account Summary from DB", "Account Summary retrieved from DB");
                MemberAccountSummaryModel memberAccountSummaryOutDb = memController.GetMemberAccountSummaryFromDB(vckey);
                Assert.IsNotNull(memberAccountSummaryOutDb, "Account Summary could not be retrieved from DB");
                TestStep.Pass("Existing member was found", memberOut.ReportDetail());

                var loyaltyId = memVirtualCard.LOYALTYIDNUMBER;
                var programCode = program != null ? program.EarningPreference : null;
                TestStep.Start("GetAccountSummary API call", "Account Summary retruned from API");
                MemberAccountSummaryModel memberAccountSummaryOut = memController.GetAccountSummary(loyaltyId, programCode, null);
                Assert.IsNotNull(memberAccountSummaryOut, "Account Summary not returned from API");
                TestStep.Pass("Account Summary was returned from API", memberOut.ReportDetail());

                TestStep.Start("Compare Account Summary between DB and API", "Account Summary matches");
                AssertModels.AreEqualWithAttribute(memberAccountSummaryOutDb, memberAccountSummaryOut);
                TestStep.Pass("Account Summary matches between DB and API", memberOut.ReportDetail());

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

        [TestCaseSource(typeof(GetAccountSummaryTestData), "NegativeScenarios")]
        public void GetAccountSummaryTestsTest_Negative(string loyaltyId, int errorCode, string errorMessage)
        {
            MemberController memController = new MemberController(Database, TestStep);
            try
            {
                if (loyaltyId == null)
                {
                    TestStep.Start("Make GetAccountSummary Call", $"GetAccountSummary call should throw exception with error code = {errorCode}");
                    LWServiceException exception = Assert.Throws<LWServiceException>(() => memController.GetAccountSummary(loyaltyId, null, null),
                                                                 "Expected LWServiceException, exception was not thrown.");
                    Assert.AreEqual(errorCode, exception.ErrorCode, $"Expected Error Code: {errorCode}");
                    Assert.IsTrue(exception.Message.Contains(errorMessage), $"Expected Error Message to contain: {errorMessage}");
                    TestStep.Pass("GetAccountSummary call threw expected exception", exception.ReportDetail());
                }
                else 
                {
                    TestStep.Start("Make GetAccountSummary Call", $"GetAccountSummary call should throw exception with error code = {errorCode}");
                    LWServiceException exception = Assert.Throws<LWServiceException>(() => memController.GetAccountSummary(loyaltyId, null, null),
                                                                 "Expected LWServiceException, exception was not thrown.");
                    Assert.AreEqual(errorCode, exception.ErrorCode, $"Expected Error Code: {errorCode}");
                    Assert.IsTrue(exception.Message.Contains(errorMessage), $"Expected Error Message to contain: {errorMessage}");
                    TestStep.Pass("GetAccountSummary call threw expected exception", exception.ReportDetail());
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
