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
    public class HertzGetAccountActivitySummaryTests : BrierleyTestFixture
    {
        [TestCaseSource(typeof(HertzGetAccountActivitySummaryTestData), "PositiveScenarios")]
        public void HertzGetAccountActivitySummaryTests_Positive(MemberModel createMember, IHertzProgram program, int transactionCount, bool displayPoints)
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
                TestStep.Start("Get Account Activity Summary from DB", "Account Activity Summary retrieved from DB");
                List<HertzGetAccountActivitySummaryModel> memberAccountActSummaryOutDb = memController.HertzGetMemberAccountActivitySummaryFromDB(vckey, displayPoints);
                Assert.IsNotNull(memberAccountActSummaryOutDb, "Account Activity Summary could not be retrieved from DB");
                Assert.AreEqual(memberAccountActSummaryOutDb.GroupBy(x => x.RANUMBER).Count(), transactionCount, "Account Activity Summary has not the same elements from DB");
                TestStep.Pass("Existing member was found", memberOut.ReportDetail());

                var loyaltyId = memVirtualCard.LOYALTYIDNUMBER;
                var programCode = program != null ? program.EarningPreference : null;
                TestStep.Start("GetAccountActivitySummary API call", "Account Activity Summary retruned from API");
                List<HertzGetAccountActivitySummaryModel> memberAccountActivitySummaryOut = memController.HertzGetAccountActivitySummary(loyaltyId, programCode, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1), displayPoints, false, 0, transactionCount, 0,0, null);
                Assert.IsNotNull(memberAccountActivitySummaryOut, "Account Activity Summary not returned from API");
                TestStep.Pass("Account Activity Summary was returned from API", memberOut.ReportDetail());
                
                TestStep.Start("Compare Account Activity Summary between DB and API", "Account Activity Summary matches");
                AssertModels.AreEqualWithAttribute(memberAccountActSummaryOutDb, memberAccountActivitySummaryOut);
                TestStep.Pass("Account Activity Summary matches between DB and API", memberOut.ReportDetail());
                
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
        
        [TestCaseSource(typeof(HertzGetAccountActivitySummaryTestData), "NegativeScenarios")]
        public void HertzGetAccountActivitySummaryTests_Negative(string loyaltyId, int errorCode, string errorMessage)
        {
            MemberController memController = new MemberController(Database, TestStep);
            try
            {
                if (loyaltyId == null)
                {
                     TestStep.Start("Make GetAccountActivitySummary Call", $"GetAccountActivitySummary call should throw exception with error code = {errorCode}");
                    LWServiceException exception = Assert.Throws<LWServiceException>(() => memController.HertzGetAccountActivitySummary(loyaltyId, "N1", DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1), false, false, 0, 5, 0, 0, null)
                    ,"Expected LWServiceException, exception was not thrown.");
                    Assert.AreEqual(errorCode, exception.ErrorCode, $"Expected Error Code: {errorCode}");
                    Assert.IsTrue(exception.Message.Contains(errorMessage), $"Expected Error Message to contain: {errorMessage}");
                    TestStep.Pass("GetAccountActivitySummary call threw expected exception", exception.ReportDetail());
                }
                else
                {
                    TestStep.Start("Make GetAccountActivitySummary Call", $"GetAccountActivitySummary call should throw exception with error code = {errorCode}");
                    LWServiceException exception = Assert.Throws<LWServiceException>(() => memController.HertzGetAccountActivitySummary(loyaltyId, "N1", DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1), false, false, 0, 5, 0, 0, null)
                                                                                            , "Expected LWServiceException, exception was not thrown.");
                    Assert.AreEqual(errorCode, exception.ErrorCode, $"Expected Error Code: {errorCode}");
                    Assert.IsTrue(exception.Message.Contains(errorMessage), $"Expected Error Message to contain: {errorMessage}");
                    TestStep.Pass("GetAccountActivitySummary call threw expected exception", exception.ReportDetail());
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
