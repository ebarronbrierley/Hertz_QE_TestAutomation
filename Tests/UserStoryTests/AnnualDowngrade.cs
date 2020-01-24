using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;
using Brierley.TestAutomation.Core.Reporting;
using Brierley.TestAutomation.Core.Utilities;
using HertzNetFramework.DataModels;
using System.Collections;
using Brierley.TestAutomation.Core.Database;
using Brierley.TestAutomation.Core.API;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;



namespace HertzNetFramework.Tests.UserStoryTests
{
    [TestFixture]
    class AnnualDowngrade : BrierleyTestFixture
    {
        [Test]
        public void AnnualDowngradeTest()
        {
            try
            {
                BPTest.Start<TestStep>($"Step 1: Create Member and Execute Transaction");
                string tierRG = "RG";
                string tierFG = "FG";
                string tierPC = "PC";
                string tierPL = "PL";
                string mktCode1 = "ECPFG";
                string mktCode2 = "ECPPC";
                Member testMember1 = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE").Set(tierRG, "MemberDetails.A_TIERCODE")
                    .Set(mktCode2, "MemberDetails.A_MKTGPROGRAMID").Set("US", "MemberDetails.A_COUNTRY").Set(new DateTime(2019, 12, 31), "MemberDetails.A_TIERENDDATE");
                Member testMember2 = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE").Set(tierFG, "MemberDetails.A_TIERCODE")
                    .Set(mktCode2, "MemberDetails.A_MKTGPROGRAMID").Set("US", "MemberDetails.A_COUNTRY").Set(new DateTime(2019, 12, 31), "MemberDetails.A_TIERENDDATE");
                Member testMember3 = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE").Set(tierPC, "MemberDetails.A_TIERCODE")
                    .Set(mktCode2, "MemberDetails.A_MKTGPROGRAMID").Set("US", "MemberDetails.A_COUNTRY").Set(new DateTime(2019, 12, 31), "MemberDetails.A_TIERENDDATE");
                Member.AddMember(testMember1);
                Member.AddMember(testMember2);
                Member.AddMember(testMember3);
                string loyaltyid1 = testMember1.GetLoyaltyID();
                string alternateid1 = testMember1.ALTERNATEID;
                string loyaltyid2 = testMember2.GetLoyaltyID();
                string alternateid2 = testMember2.ALTERNATEID;
                string loyaltyid3 = testMember3.GetLoyaltyID();
                string alternateid3 = testMember3.ALTERNATEID;
                decimal? cdp = null;
                decimal pointsTierRG = 150;
                decimal pointsTierFG = 2401;
                decimal pointsTierPC = 4000;
                DateTime checkInDt = new DateTime(2019, 11, 2);
                DateTime checkOutDt = new DateTime(2019, 11, 1);
                DateTime origBkDt = new DateTime(2019, 11, 1);
                BPTest.Pass<TestStep>($"Step 1 Passed");

                BPTest.Start<TestStep>($"Step 2: Update Members with Transaction");

                TxnHeader txnHeader1 = TxnHeader.Generate(loyaltyid1, checkInDt, checkOutDt, origBkDt, cdp, HertzProgram.GoldPointsRewards, 0, "US", pointsTierRG, null, null, "N", "US", null);
                testMember1.AddTransaction(txnHeader1);
                Member.UpdateMember(testMember1);
                TxnHeader txnHeader2 = TxnHeader.Generate(loyaltyid2, checkInDt, checkOutDt, origBkDt, cdp, HertzProgram.GoldPointsRewards, 0, "US", pointsTierFG, null, null, "N", "US", null);
                testMember2.AddTransaction(txnHeader2);
                Member.UpdateMember(testMember2);
                TxnHeader txnHeader3 = TxnHeader.Generate(loyaltyid3, checkInDt, checkOutDt, origBkDt, cdp, HertzProgram.GoldPointsRewards, 0, "US", pointsTierPC, null, null, "N", "US", null);
                testMember3.AddTransaction(txnHeader3);
                Member.UpdateMember(testMember3);

                BPTest.Pass<TestStep>($"Step 2 Passed");
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

        }

        [Test]
        public void AnnualDowngradeTest2()
        {
            try
            {
                BPTest.Start<TestStep>($"Step 1: Create Member and Execute Transaction");
                string tierRG = "RG";
                string tierFG = "FG";
                string tierPC = "PC";
                string[] tiers = {"RG", "FG", "PC", "PL" };
                string[] mktCode = { "EWB19", "AD19F", "AD19P", "BTEMF", "BTEMP", "BU19F", "BU19P", "BT19F", "BT19P" };
                string[] mktCode2 = { "ECPFG", "ECPPC", "AXPCA", "AXPCE", "TFDFG", "TFDPC", "SIEMN" };
                string[] mktCode3 = { "AXPCA", "AXPCE", "TFDFG", "TFDPC" };
                decimal? cdp = null;
                decimal pointsTierRG = 150;
                decimal pointsTierFG = 2401;
                decimal pointsTierPC = 4000;
                DateTime checkInDt = new DateTime(2019, 11, 2);
                DateTime checkOutDt = new DateTime(2019, 11, 1);
                DateTime origBkDt = new DateTime(2019, 11, 1);

                foreach (string code in mktCode)
                {
                    Member testMember1 = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE").Set(tierRG, "MemberDetails.A_TIERCODE").Set(code, "MemberDetails.A_MKTGPROGRAMID").Set("US", "MemberDetails.A_COUNTRY").Set(new DateTime(2019, 12, 31), "MemberDetails.A_TIERENDDATE");
                    Member testMember2 = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE").Set(tierFG, "MemberDetails.A_TIERCODE").Set(code, "MemberDetails.A_MKTGPROGRAMID").Set("US", "MemberDetails.A_COUNTRY").Set(new DateTime(2019, 12, 31), "MemberDetails.A_TIERENDDATE");
                    Member testMember3 = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE").Set(tierPC, "MemberDetails.A_TIERCODE").Set(code, "MemberDetails.A_MKTGPROGRAMID").Set("US", "MemberDetails.A_COUNTRY").Set(new DateTime(2019, 12, 31), "MemberDetails.A_TIERENDDATE");
                    Member.AddMember(testMember1);
                    Member.AddMember(testMember2);
                    Member.AddMember(testMember3);
                    string loyaltyid1 = testMember1.GetLoyaltyID();
                    string alternateid1 = testMember1.ALTERNATEID;
                    string loyaltyid2 = testMember2.GetLoyaltyID();
                    string alternateid2 = testMember2.ALTERNATEID;
                    string loyaltyid3 = testMember3.GetLoyaltyID();
                    string alternateid3 = testMember3.ALTERNATEID;
                }

                //foreach (string tier in tiers)
                //{
                //    Member testMember1 = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE").Set(tier, "MemberDetails.A_TIERCODE").Set("AXCUS", "MemberDetails.A_MKTGPROGRAMID").Set("US", "MemberDetails.A_COUNTRY").Set(new DateTime(2019, 12, 31), "MemberDetails.A_TIERENDDATE"); 
                //    Member testMember2 = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE").Set(tier, "MemberDetails.A_TIERCODE").Set("AXCUS", "MemberDetails.A_MKTGPROGRAMID").Set("US", "MemberDetails.A_COUNTRY").Set(new DateTime(2019, 12, 31), "MemberDetails.A_TIERENDDATE"); 
                //    Member testMember3 = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE").Set(tier, "MemberDetails.A_TIERCODE").Set("AXCUS", "MemberDetails.A_MKTGPROGRAMID").Set("US", "MemberDetails.A_COUNTRY").Set(new DateTime(2019, 12, 31), "MemberDetails.A_TIERENDDATE"); 
                //    Member.AddMember(testMember1);
                //    Member.AddMember(testMember2);
                //    Member.AddMember(testMember3);
                //    string loyaltyid1 = testMember1.GetLoyaltyID();
                //    string alternateid1 = testMember1.ALTERNATEID;
                //    string loyaltyid2 = testMember2.GetLoyaltyID();
                //    string alternateid2 = testMember2.ALTERNATEID;
                //    string loyaltyid3 = testMember3.GetLoyaltyID();
                //    string alternateid3 = testMember3.ALTERNATEID;

                //    TxnHeader txnHeader1 = TxnHeader.Generate(loyaltyid1, checkInDt, checkOutDt, origBkDt, cdp, HertzProgram.GoldPointsRewards, 0, "US", pointsTierRG, null, null, "N", "US", null);
                //    testMember1.AddTransaction(txnHeader1);
                //    Member.UpdateMember(testMember1);
                //    TxnHeader txnHeader2 = TxnHeader.Generate(loyaltyid2, checkInDt, checkOutDt, origBkDt, cdp, HertzProgram.GoldPointsRewards, 0, "US", pointsTierFG, null, null, "N", "US", null);
                //    testMember2.AddTransaction(txnHeader2);
                //    Member.UpdateMember(testMember2);
                //    TxnHeader txnHeader3 = TxnHeader.Generate(loyaltyid3, checkInDt, checkOutDt, origBkDt, cdp, HertzProgram.GoldPointsRewards, 0, "US", pointsTierPC, null, null, "N", "US", null);
                //    testMember3.AddTransaction(txnHeader3);
                //    Member.UpdateMember(testMember3);

                //}

                BPTest.Pass<TestStep>($"Step 1 Passed");



            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

        }

        [Test]
        public void ClawbackTest()
        {
            try
            {
                BPTest.Start<TestStep>($"Step 1: Create Member and Execute Transaction");
                string tier = "PC";
                Member testMember1 = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE").Set(tier, "MemberDetails.A_TIERCODE").Set("GB", "MemberDetails.A_COUNTRY").Set(new DateTime(2019, 12, 31), "MemberDetails.A_TIERENDDATE");
                Member.AddMember(testMember1);
                string loyaltyid1 = testMember1.GetLoyaltyID();
                string alternateid1 = testMember1.ALTERNATEID;
                decimal? cdp = null;
                decimal pointsTierRG = 150;
                decimal pointsTierFG = 2401;
                decimal pointsTierPC = 4000;
                DateTime checkInDt = new DateTime(2019, 11, 2);
                DateTime checkOutDt = new DateTime(2019, 11, 1);
                DateTime origBkDt = new DateTime(2019, 11, 1);
                BPTest.Pass<TestStep>($"Step 1 Passed");

                BPTest.Start<TestStep>($"Step 2: Update Members with Transaction");
                for (int x = 0; x < 1; x++)
                {
                    TxnHeader txnHeader1 = TxnHeader.Generate(loyaltyid1, checkInDt, checkOutDt, origBkDt, cdp, HertzProgram.GoldPointsRewards, 0, "US", pointsTierPC, "AAAA", 246095, "N", "US", null);
                    testMember1.AddTransaction(txnHeader1);
                    Member.UpdateMember(testMember1);
                    testMember1.RemoveTransaction();
                }
                BPTest.Pass<TestStep>($"Step 2 Passed");
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

        }

     
        public void vcjobTest()
        {
            BPTest.Start<TestStep>($"Step 1: Run VC Job");

            string jobName = "HertzBeta - Flash Sale Zero Point Event";

            RestConfiguration restConfig = new RestConfiguration()
            {
                BaseURL = "http://CY2QAAPP05:8001/VisualCron/json",
                EndPoint = $"/Job/GetByName?password=7d7Yu^m&username=qa_htz_srv_acct&name={jobName}"
            };

            RestResponse restResponse = Rest.Get(restConfig).Execute();

            JObject responseBody = JObject.Parse(restResponse.MessageBody);
            string jobID = (string)responseBody["Id"];
               
            

            restConfig = new RestConfiguration()
            {
                BaseURL = "http://CY2QAAPP05:8001/VisualCron/json",
                EndPoint = $"/Job/Run?password=7d7Yu^m&username=qa_htz_srv_acct&id={jobID}"
            };
            BPTest.Pass<TestStep>($"Step 1 Passed");
            BPTest.Start<TestStep>($"Step 1: Run VC Job");

            restResponse = Rest.Get(restConfig).Execute();

            BPTest.Pass<TestStep>($"Step 1 Passed");

        }
    }
}
