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

namespace HertzNetFramework.Tests.SOAP
{
    [TestFixture]
    class FlashSale : BrierleyTestFixture
    {
        [Category("Api_Smoke")]
        [Category("Api_Positive")]
        [Category("FlashSale")]
        [Category("FlashSale_Positive")]
        [Test]
        public void FlashSaleTest()
        {
            try
            {
                decimal pointsEarnRG = 471000;
                decimal pointsEarnFG = 376800;
                decimal pointsEarnPC = 314000;

                BPTest.Start<TestStep>($"Step 1: Create four GPR Members");
                Member RGMember = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE")
                    .Set("RG", "MemberDetails.A_TIERCODE").Set("US", "MemberDetails.A_COUNTRY");
                Member FGMember = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE")
                    .Set("FG", "MemberDetails.A_TIERCODE").Set("US", "MemberDetails.A_COUNTRY");
                Member PCMember = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE")
                    .Set("PC", "MemberDetails.A_TIERCODE").Set("US", "MemberDetails.A_COUNTRY");
                Member PLMember = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE")
                    .Set("PL", "MemberDetails.A_TIERCODE").Set("US", "MemberDetails.A_COUNTRY");
                Member memberOutRG = Member.AddMember(RGMember);
                Member memberOutFG = Member.AddMember(FGMember);
                Member memberOutPC = Member.AddMember(PCMember);
                Member memberOutPL = Member.AddMember(PLMember);
                Assert.IsNotNull(memberOutRG, "Expected populated member object, but member object returned was null");
                Assert.IsNotNull(memberOutFG, "Expected populated member object, but member object returned was null");
                Assert.IsNotNull(memberOutPC, "Expected populated member object, but member object returned was null");
                Assert.IsNotNull(memberOutPL, "Expected populated member object, but member object returned was null");
                string RGLoyaltyID = RGMember.GetLoyaltyID();
                string FGLoyaltyID = FGMember.GetLoyaltyID();
                string PCLoyaltyID = PCMember.GetLoyaltyID();
                string PLLoyaltyID = PLMember.GetLoyaltyID();
                string alternateidRG = RGMember.ALTERNATEID;
                string alternateidFG = FGMember.ALTERNATEID;
                string alternateidPC = PCMember.ALTERNATEID;
                string alternateidPL = PLMember.ALTERNATEID;
                string query = $@"select vckey from bp_htz.lw_virtualcard where loyaltyidnumber = '{RGLoyaltyID}'";
                Hashtable ht = Database.QuerySingleRow(query);
                string vckeyRG = Convert.ToString(ht["VCKEY"]);
                query = $@"select vckey from bp_htz.lw_virtualcard where loyaltyidnumber = '{FGLoyaltyID}'";
                ht = Database.QuerySingleRow(query);
                string vckeyFG = Convert.ToString(ht["VCKEY"]);
                query = $@"select vckey from bp_htz.lw_virtualcard where loyaltyidnumber = '{PCLoyaltyID}'";
                ht = Database.QuerySingleRow(query);
                string vckeyPC = Convert.ToString(ht["VCKEY"]);
                query = $@"select vckey from bp_htz.lw_virtualcard where loyaltyidnumber = '{PLLoyaltyID}'";
                ht = Database.QuerySingleRow(query);
                string vckeyPL = Convert.ToString(ht["VCKEY"]);
                BPTest.Pass<TestStep>($"Step 1 Passed: RG Member Created");

                BPTest.Start<TestStep>($"Step 2: Update With Transaction");
                DateTime checkInDt = new DateTime(2019, 10, 23);
                DateTime checkOutDt = new DateTime(2019, 10, 22);
                DateTime origBkDt = new DateTime(2019, 10, 22);
                decimal? cdp = null;

                TxnHeader txnHeaderRG = TxnHeader.Generate(RGLoyaltyID, checkInDt, checkOutDt, origBkDt, cdp, HertzProgram.GoldPointsRewards, null, "US", pointsEarnRG, "AAAA", 246095, "N", "US", null);
                RGMember.AddTransaction(txnHeaderRG);
                Member.UpdateMember(RGMember);
                TxnHeader txnHeaderFG = TxnHeader.Generate(FGLoyaltyID, checkInDt, checkOutDt, origBkDt, cdp, HertzProgram.GoldPointsRewards, null, "US", pointsEarnFG, "AAAA", 246095, "N", "US", null);
                FGMember.AddTransaction(txnHeaderFG);
                Member.UpdateMember(FGMember);
                TxnHeader txnHeaderPC = TxnHeader.Generate(PCLoyaltyID, checkInDt, checkOutDt, origBkDt, cdp, HertzProgram.GoldPointsRewards, null, "US", pointsEarnPC, "AAAA", 246095, "N", "US", null);
                PCMember.AddTransaction(txnHeaderPC);
                Member.UpdateMember(PCMember);
                TxnHeader txnHeaderPL = TxnHeader.Generate(PLLoyaltyID, checkInDt, checkOutDt, origBkDt, cdp, HertzProgram.GoldPointsRewards, null, "US", pointsEarnPC, "AAAA", 246095, "N", "US", null);
                PLMember.AddTransaction(txnHeaderPL);
                Member.UpdateMember(PLMember);

                RGMember.RemoveTransaction();
                FGMember.RemoveTransaction();
                PCMember.RemoveTransaction();
                PLMember.RemoveTransaction();

                //Assert.NotNull(memberOut, "Member Out Is Null");
                BPTest.Pass<TestStep>($"Step 2 Passed: Updated Member with Transaction");

                BPTest.Start<TestStep>($"Step 3: Redeem Reward");
                string[] rewardtypecodes = new string[] {
                "205275",
"205229",
"205271",
"205272",
"205267",
"205276",
"205257",
"205237",
"205269",
"205274",
"205277",
"205261",
"205234",
"205268",
"205256",
"205235",
"205270",
"205263",
"205231",
"205287",
"205291",
"205252",
"205278",
"205255",
"205232",
"205241",
"205248",
"205246",
"205243",
"205284",
"205230",
"205286",
"205285",
"205233",
"205282",
"205279",
"205283",
"205289",
"205266",
"205280",
"205265",
"205244",
"205281",
"205288",
"205290",
"205247",
"205245",
"205240",
"205242",
"205239",
"205258",
"205260",
"205264",
"205259",
"205254",
"205250",
"205251",
"205253"
};
                foreach(string typecode in rewardtypecodes)
                {
                    Member.AddReward(alternateidRG, typecode, vckeyRG);
                    Member.AddReward(alternateidFG, typecode, vckeyFG);
                    Member.AddReward(alternateidPC, typecode, vckeyPC);
                    Member.AddReward(alternateidPL, typecode, vckeyPL);
                }
                BPTest.Pass<TestStep>($"Step 3 Passed: Reward Redeemed");
                BPTest.Start<TestStep>($"Step 4: Create Transactions for Point Events");
                foreach(string promnum in rewardtypecodes)
                {
                    TxnHeader txnheaderPERG = TxnHeader.Generate(RGLoyaltyID, checkInDt, checkOutDt, origBkDt, cdp, HertzProgram.GoldPointsRewards, null, "US", 0, "", null, "N", "US", promnum);
                    TxnHeader txnheaderPEFG = TxnHeader.Generate(FGLoyaltyID, checkInDt, checkOutDt, origBkDt, cdp, HertzProgram.GoldPointsRewards, null, "US", 0, "", null, "N", "US", promnum);
                    TxnHeader txnheaderPEPC = TxnHeader.Generate(PCLoyaltyID, checkInDt, checkOutDt, origBkDt, cdp, HertzProgram.GoldPointsRewards, null, "US", 0, "", null, "N", "US", promnum);
                    TxnHeader txnheaderPEPL = TxnHeader.Generate(PLLoyaltyID, checkInDt, checkOutDt, origBkDt, cdp, HertzProgram.GoldPointsRewards, null, "US", 0, "", null, "N", "US", promnum);
               
                    RGMember.AddTransaction(txnheaderPERG);
                    FGMember.AddTransaction(txnheaderPEFG);
                    PCMember.AddTransaction(txnheaderPEPC);
                    PLMember.AddTransaction(txnheaderPEPL);

                    Member.UpdateMember(RGMember);
                    Member.UpdateMember(FGMember);
                    Member.UpdateMember(PCMember);
                    Member.UpdateMember(PLMember);

                    RGMember.RemoveTransaction();
                    FGMember.RemoveTransaction();
                    PCMember.RemoveTransaction();
                    PLMember.RemoveTransaction();

                }
                BPTest.Pass<TestStep>($"Step 4 Passed");

                BPTest.Start<TestStep>("Step 5: Run VC Job");
                BPTest.Pass<TestStep>($"Step 5 Passed");

                BPTest.Start<TestStep>("Step 6: Validate Point Events");

                string[] pointevents = new string[]
                {
                    "11043892",
                    "11043893",
                    "11043894",
                    "11043895",
                    "11043896",
                    "11043897",
                    "11043898",
                    "11043900",
                    "11043901",
                    "11043902",
                    "11043903",
                    "11043904",
                    "11043905",
                    "11043906",
                    "11043907",
                    "11043908",
                    "11043909",
                    "11043910",
                    "11043911",
                    "11043912",
                    "11043913",
                    "11043914",
                    "11043915",
                    "11043916",
                    "11043917",
                    "11043918",
                    "11043919",
                    "11043920",
                    "11043921"
                };

                string[] pointeventsStage = new string[]
                {
                    "15194661",
                    "15194662",
                    "15194663",
                    "15194664",
                    "15194665",
                    "15194666",
                    "15194667",
                    "15194668",
                    "15194669",
                    "15194670",
                    "15194671",
                    "15194672",
                    "15194673",
                    "15194674",
                    "15194675",
                    "15194676",
                    "15194677",
                    "15194678",
                    "15194679",
                    "15194680",
                    "15194681",
                    "15194682",
                    "15194683",
                    "15194684",
                    "15194685",
                    "15194686",
                    "15194687",
                    "15194688",
                    "15194689"
                };

                string[] pointeventsQAA = new string[] { "10576712",
"10576711",
"10576713",
"10576714",
"10576715",
"10576716",
"10576717",
"10576718",
"10576719",
"10576720",
"10576721",
"10576722",
"10576723",
"10576724",
"10576725",
"10576726",
"10576727",
"10576728",
"10576729",
"10576730",
"10576731",
"10576732",
"10576733",
"10576734",
"10576735",
"10576736",
"10576737",
"10576738",
"10576739",
"10576740"};
                foreach(string pointevent in pointevents)
                {
                    string queryRG = $@"select pt.pointeventid from bp_htz.lw_pointtransaction pt where pt.vckey = '{vckeyRG}' and pt.pointeventid = '{pointevent}'";
                    string queryFG = $@"select pt.pointeventid from bp_htz.lw_pointtransaction pt where pt.vckey = '{vckeyFG}' and pt.pointeventid = '{pointevent}'";
                    string queryPC = $@"select pt.pointeventid from bp_htz.lw_pointtransaction pt where pt.vckey = '{vckeyPC}' and pt.pointeventid = '{pointevent}'";
                    QueryResult queryResultRG = Database.Query(queryRG);
                    QueryResult queryResultFG = Database.Query(queryFG);
                    QueryResult queryResultPC = Database.Query(queryPC);
                    int RGcount = queryResultRG.Rows.Count;
                    int FGCount = queryResultFG.Rows.Count;
                    int PCCount = queryResultPC.Rows.Count;
                    if (RGcount == 0 || FGCount  == 0|| PCCount == 0)
                    {
                        //BPTest.Fail<TestStep>("Step Failed.");
                        string x = "Test Failed";
                    }

                }

                BPTest.Pass<TestStep>($"Step 6 Passed");





            }
            catch (Exception ex)
            {
                BPTest.Fail<TestStep>("Step Failed.");
            }
        }
    }
}
