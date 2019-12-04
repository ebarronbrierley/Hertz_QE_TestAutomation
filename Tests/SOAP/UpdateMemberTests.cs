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
    public class UpdateMember : BrierleyTestFixture
    {
        [Category("Api_Smoke")]
        [Category("Api_Positive")]
        [Category("AddMember")]
        [Category("AddMember_Positive")]
        [TestCaseSource("PositiveScenarios")]
        public void UpdateMember_Positive(string name, MemberStyle memberStyle, long memberStatus, IHertzProgram hertzProgram, string tierCode,bool hasTransactions)
        {
            try
            {
                BPTest.Start<TestStep>("Get Existing Member from the database", "Existing member should be found");
                Member dbMember = Member.GetFromDB(Database, memberStyle, Member.MemberStatus.Active, hertzProgram.EarningPreference, tierCode,true);             
                Assert.IsNotNull(dbMember, "Member could not be retrieved from DB");
                Assert.IsTrue(dbMember.VirtualCards.Count > 0);
                BPTest.Pass<TestStep>("Existing member was found", dbMember.ReportDetail());

                var memberVCKEY = dbMember.VirtualCards.First().VCKEY;

                BPTest.Start<TestStep>($"Add random transaction to members virtual card with VCKEY = {memberVCKEY}", "Transaction should be added to members virtual card");
                dbMember.AddRandomTransaction(hertzProgram, memberVCKEY,500);
                Assert.IsTrue(dbMember.VirtualCards.First().TxnHeaders.Count > 0, "Expected 1 TxnHeader to be present in members vitual card");
                BPTest.Pass<TestStep>("Transaction is added to members virtual card", dbMember.VirtualCards.First().TxnHeaders.First().ReportDetail());

                TxnHeader expectedTransaction = dbMember.VirtualCards.First().TxnHeaders.First();
                expectedTransaction.A_TXNQUALPURCHASEAMT = (expectedTransaction.A_SBTOTAMT + expectedTransaction.A_LDWCDWCHRGAMT + expectedTransaction.A_ADDLSRVCCHRGAMT + expectedTransaction.A_AGEDIFFCHRGAMT + expectedTransaction.A_ADDLAUTHDRVRCHRGAMT + expectedTransaction.A_CHILDSEATTOTAMT + expectedTransaction.A_MISCGRPAMT + expectedTransaction.A_GARSPECLEQMNTAMT + expectedTransaction.A_TOTCHRGAMT + expectedTransaction.A_NVGTNSYSTOTAMT + expectedTransaction.A_SATLTRADIOTOTAMT + expectedTransaction.A_REFUELINGCHRGAMT) * expectedTransaction.A_RNTINGCTRYCRNCYUSDEXCHRT;
                expectedTransaction.A_QUALTOTAMT = (expectedTransaction.A_SBTOTAMT + expectedTransaction.A_LDWCDWCHRGAMT + expectedTransaction.A_ADDLSRVCCHRGAMT + expectedTransaction.A_AGEDIFFCHRGAMT + expectedTransaction.A_ADDLAUTHDRVRCHRGAMT + expectedTransaction.A_CHILDSEATTOTAMT + expectedTransaction.A_MISCGRPAMT + expectedTransaction.A_GARSPECLEQMNTAMT + expectedTransaction.A_TOTCHRGAMT + expectedTransaction.A_NVGTNSYSTOTAMT + expectedTransaction.A_SATLTRADIOTOTAMT + expectedTransaction.A_REFUELINGCHRGAMT) * expectedTransaction.A_RNTINGCTRYCRNCYUSDEXCHRT;

                BPTest.Start<TestStep>("Update Existing Member with added transaction", "Member object should be returned from UpdateMember call");
                Member updatedMember = Member.UpdateMember(dbMember);
                Assert.IsNotNull(updatedMember, "Expected non null Member object to be returned");
                BPTest.Pass<TestStep>("Member object returned from UpdateMember API call", updatedMember.ReportDetail());

                BPTest.Start<TestStep>($"Verify API response contains expected Member", "API response should contain passed member.");
                AssertModels.AreEqualOnly(dbMember, updatedMember, Member.BaseVerify);
                BPTest.Pass<TestStep>("API response member matches expected member.");

                BPTest.Start<TestStep>($"Verify API response contains expected MemberDetails for passed member", "API response should contain passed member details.");
                AssertModels.AreEqualWithAttribute(dbMember.GetMemberDetails(memberStyle).First(), updatedMember.GetMemberDetails(memberStyle).First());
                BPTest.Pass<TestStep>("API response contains passed member details.");

                BPTest.Start<TestStep>($"Verify API response contains expected MemberPreferences for passed member", "API response should contain passed member preferences.");
                AssertModels.AreEqualWithAttribute(dbMember.GetMemberPreferences(memberStyle).First(), updatedMember.GetMemberPreferences(memberStyle).First());
                BPTest.Pass<TestStep>("API response contains passed member preferences.");

                BPTest.Start<TestStep>($"Verify API response contains expected VirtualCard for passed member", "API response should contain passed virtual card details.");
                AssertModels.AreEqualWithAttribute(dbMember.VirtualCards.First(), updatedMember.VirtualCards.First());
                BPTest.Pass<TestStep>("API response contains passed virtual card.", dbMember.VirtualCards.ReportDetail());

                BPTest.Start<TestStep>($"Verify API response contains expected transaction", "API response contains expected transaction");
                Assert.IsTrue(updatedMember.VirtualCards.First().TxnHeaders.Count > 0, "Expected API response to contain one TxnHeader");
                AssertModels.AreEqualWithAttribute(expectedTransaction, updatedMember.VirtualCards.First().TxnHeaders.First());
                BPTest.Pass<TestStep>("UpdateMember API response TxnHeader matches expected TxnHeader");

                BPTest.Start<TestStep>($"Verify transaction details are in {TxnHeader.TableName} for A_VCKEY = {memberVCKEY}, A_RANUM = {expectedTransaction.A_RANUM}", "Transaction header should be found");
                IEnumerable<TxnHeader> dbTransactions = TxnHeader.GetFromDB(Database, memberVCKEY, expectedTransaction.A_RANUM);
                Assert.IsNotNull(dbTransactions, "No transactions returned from the database");
                Assert.IsTrue(dbTransactions.Count() == 1);
                AssertModels.AreEqualWithAttribute(expectedTransaction, dbTransactions.First());
                BPTest.Pass<TestStep>("Transaction header was found in database", dbTransactions.First().ReportDetail());

            }
            catch(LWServiceException ex)
            {
                BPTest.Fail<TestStep>(ex.Message, new string[] { $"Error Code: {ex.ErrorCode}", $"Error Message: {ex.Message}" });
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

        static object[] PositiveScenarios =
        {
            new object[]
            {
                "Active GPR Member with RG Tier and existing transactions",
                MemberStyle.ProjectOne,
                Member.MemberStatus.Active,
                HertzProgram.GoldPointsRewards,
                GPR.Tier.RegularGold.Code,
                true

            },//"Active GPR Member with RG Tier and existing transactions"

            new object[]
            {
                "Active GPR Member with FG Tier and existing transactions",
                MemberStyle.ProjectOne,
                Member.MemberStatus.Active,
                HertzProgram.GoldPointsRewards,
                GPR.Tier.FiveStar.Code,
                true

            },//"Active GPR Member with FG Tier and existing transactions"

           new object[]
            {
                "Active GPR Member with PC Tier and existing transactions",
                MemberStyle.ProjectOne,
                Member.MemberStatus.Active,
                HertzProgram.GoldPointsRewards,
                GPR.Tier.PresidentsCircle.Code,
                true

            },//"Active GPR Member with PC Tier and existing transactions"

            new object[]
            {
                "Active GPR Member with PL Tier and existing transactions",
                MemberStyle.ProjectOne,
                Member.MemberStatus.Active,
                HertzProgram.GoldPointsRewards,
                GPR.Tier.Platinum.Code,
                false

            },//"Active GPR Member with PL Tier and existing transactions"

             new object[]
              {
                "Active Thrifty Member with new transactions",
                MemberStyle.ProjectOne,
                Member.MemberStatus.Active,
                HertzProgram.ThriftyBlueChip,
                "",
                false
            },//"Active Thrifty Member with new transactions"

            new object[]
            {
                "Active Dollar Member with new transactions",
                MemberStyle.ProjectOne,
                Member.MemberStatus.Active,
                HertzProgram.DollarExpressRenters,
                "",
                false
            }//"Active Dollar Member with new transactions"
      
        };
    }
}
