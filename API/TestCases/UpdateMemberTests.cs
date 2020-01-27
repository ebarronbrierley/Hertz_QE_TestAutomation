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
    public class UpdateMemberTests : BrierleyTestFixture
    {
        [TestCaseSource(typeof(UpdateMemberTestData), "PositiveScenarios")]
        public void UpdateMember_Positive(long memberStatus, IHertzTier hertzTier, int transactionCount)
        {

            MemberController memController = new MemberController(Database, TestStep);
            TxnHeaderController txnController = new TxnHeaderController(Database, TestStep);

            try
            {
                TestStep.Start("Get Existing Member from the database", "Existing member should be found");
                MemberModel member = memController.GetRandomFromDB(MemberModel.Status.Active, hertzTier);
                member.CHANGEDBY = "UpdateMemberAutomation";
                Assert.IsNotNull(member, "Member could not be retrieved from DB");
                Assert.IsNotNull(member.VirtualCards, "Expected member to have virtual cards");
                TestStep.Pass("Existing member was found", member.ReportDetail());

                var memVirtualCard = member.VirtualCards.First();

                TestStep.Start($"Add {transactionCount} random transaction(s) to members virtual card with VCKEY = {memVirtualCard.VCKEY}", "Transaction(s) should be added to members virtual card");
                memVirtualCard.Transactions = TxnHeaderController.GenerateRandomTransactions(memVirtualCard, hertzTier.ParentProgram, transactionCount, 500M);
                Assert.AreEqual(transactionCount, memVirtualCard.Transactions.Count, $"Expected {transactionCount} TxnHeader(s) to be present in members vitual card");
                TestStep.Pass("Transaction(s) is added to members virtual card", memVirtualCard.Transactions.ReportDetail());

                foreach (var transaction in memVirtualCard.Transactions)
                {
                    transaction.A_TXNQUALPURCHASEAMT = TxnHeaderController.CalculateQualifyingPurchaseAmount(transaction);
                    transaction.A_QUALTOTAMT = TxnHeaderController.CalculateQualifyingPurchaseAmount(transaction);
                }

                TestStep.Start("Update Existing Member with added transaction", "Member object should be returned from UpdateMember call");
                MemberModel updatedMember = memController.UpdateMember(member);
                Assert.IsNotNull(updatedMember, "Expected non null Member object to be returned");
                TestStep.Pass("Member object returned from UpdateMember API call", updatedMember.ReportDetail());

                TestStep.Start($"Verify API response contains expected Member", "API response should contain passed member.");
                AssertModels.AreEqualOnly(member, updatedMember, MemberModel.BaseVerify);
                TestStep.Pass("API response member matches expected member.");

                TestStep.Start($"Verify API response contains expected MemberDetails for passed member", "API response should contain passed member details.");
                AssertModels.AreEqualWithAttribute(member.MemberDetails, updatedMember.MemberDetails);
                TestStep.Pass("API response contains passed member details.");

                TestStep.Start($"Verify API response contains expected MemberPreferences for passed member", "API response should contain passed member preferences.");
                AssertModels.AreEqualWithAttribute(member.MemberPreferences, updatedMember.MemberPreferences);
                TestStep.Pass("API response contains passed member preferences.");

                TestStep.Start($"Verify API response contains expected VirtualCard for passed member", "API response should contain passed virtual card details.");
                AssertModels.AreEqualWithAttribute(memVirtualCard, updatedMember.VirtualCards.First());
                TestStep.Pass("API response contains passed virtual card.", updatedMember.VirtualCards.ReportDetail());

                TestStep.Start($"Verify Updated Member contains {transactionCount} transaction(s)", $"Members virtual card should contain {transactionCount} transaction(s)");
                Assert.AreEqual(transactionCount, updatedMember.VirtualCards.First().Transactions.Count);
                TestStep.Pass("Members virtual card contained the expected number of transactions");

                foreach (var transaction in memVirtualCard.Transactions)
                {
                    TestStep.Start($"Verify Update Member API response contains transaction RANUM [{transaction.A_RANUM}]", "Update Member API response should contain expected transaction");
                    var updateMemTransaction = updatedMember.VirtualCards.First().Transactions.Find(x => x.A_RANUM == transaction.A_RANUM);
                    Assert.NotNull(updateMemTransaction, $"Transaction with RANUM [{transaction.A_RANUM}] not found in members virtual card");
                    AssertModels.AreEqualWithAttribute(transaction, updateMemTransaction);
                    TestStep.Pass("Members virtual card contains expected transaction", updateMemTransaction.ReportDetail());

                    TestStep.Start($"Verify transaction details are in {TxnHeaderModel.TableName} for A_VCKEY = {memVirtualCard.VCKEY}, A_RANUM = {transaction.A_RANUM}", "Transaction header should be found");
                    IEnumerable<TxnHeaderModel> dbTransactions = txnController.GetFromDB(memVirtualCard.VCKEY, transaction.A_RANUM);
                    Assert.IsNotNull(dbTransactions, "No transactions returned from the database");
                    Assert.AreEqual(1, dbTransactions.Count());
                    AssertModels.AreEqualWithAttribute(transaction, dbTransactions.First());
                    TestStep.Pass("Transaction header was found in database", dbTransactions.First().ReportDetail());
                }
            }
            catch(AssertionException ex)
            {
                TestStep.Fail(ex.Message);
                Assert.Fail();
            }
            catch (LWServiceException ex)
            {
                TestStep.Fail(ex.Message, new string[] { $"Error Code: {ex.ErrorCode}", $"Error Message: {ex.Message}" });
                Assert.Fail();
            }
            catch (AssertModelEqualityException ex)
            {
                TestStep.Fail(ex.Message, ex.ComparisonFailures);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                TestStep.Abort(ex.Message, ex.StackTrace);
                Assert.Fail();
            }
        }
    }
}
