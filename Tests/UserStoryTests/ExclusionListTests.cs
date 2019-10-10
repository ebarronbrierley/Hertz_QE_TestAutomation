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
    class ExclusionListTests : BrierleyTestFixture
    {
        [Category("Api_Smoke")]
        [Category("Api_Positive")]
        [Category("ExclusionList")]
        [Category("ExclusionList_Positive")]
        [Test]
        public void ExclusionList_Positive()
        {
            try
            {

                BPTest.Start<TestStep>($"Step 0: Create an RG Member");
                decimal[] RemovedCDPs = new decimal[] { 552641M , 552642M , 603849M , 610205M , 663884M , 663888M , 779693M ,
                                                        792763M , 793627M , 881706M , 965871M , 538987M , 525191M  , 517402M };

                decimal[] HTZ18536RemovedCDPs = new decimal[] { 67447M, 66442M,64747M, 358685M };

                decimal[] HTZ18536_NonEarningCDPs = new decimal[] {69042M,
                    67198M,
                    67045M,
                    67046M,
                    67047M,
                    66824M,
                    68779M,
                    68826M,
                    68827M,
                    68834M,
                    68836M,
                    68895M,
                    68902M,
                    68952M,
                    68985M,
                    69022M,
                    25036M,
                    68977M,
                    68981M,
                    67049M,
                    67048M,
                    67051M,
                    66825M,
                    180334M,
                    371153M,
                    480700M,
                    431493M,
                    433369M,
                    331990M,
                    310543M,
                    265480M,
                    67406M,
                    67405M,
                    67404M,
                    67403M,
                    67355M,
                    68832M,
                    68982M,
                    69038M,
                    69107M,
                    69121M,};

                Member testMember = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE").Set("RG", "MemberDetails.A_TIERCODE");
                Member memberOut = Member.AddMember(testMember);
                string loyaltyid = testMember.GetLoyaltyID();

                string query = $@"select vckey from bp_htz.lw_virtualcard where loyaltyidnumber = '{loyaltyid}'";
                Hashtable ht = Database.QuerySingleRow(query);
                decimal vckey = Convert.ToDecimal(ht["VCKEY"]);

                DateTime checkInDt = new DateTime(2019, 10, 2);
                DateTime checkOutDt = new DateTime(2019, 10, 1);
                DateTime origBkDt = new DateTime(2019, 10, 1);
                int count = 1;
                BPTest.Pass<TestStep>($"RG Member Created");
                
                foreach (decimal cdp in HTZ18536RemovedCDPs)
                {
                    BPTest.Start<TestStep>($"Step {count}: Test CDP {cdp}");
                    TxnHeader txnHeader = TxnHeader.Generate(testMember.GetLoyaltyID(), checkInDt, checkOutDt, origBkDt, cdp, HertzProgram.GoldPointsRewards, null, "US", 100, "AAAA", 246095, "N", "US", null);
                    testMember.AddTransaction(txnHeader);
                    Member.UpdateMember(testMember);
                    testMember.RemoveTransaction();
                    BPTest.Pass<TestStep>($"Step {count++} Passed");
                }
                BPTest.Start<TestStep>($"Step {count}: Validate DB");
                string query2 = $@"select * from bp_htz.lw_pointtransaction where vckey = '{vckey}'";
                QueryResult qr = Database.Query(query2);
                int dbCount = qr.Rows.Count;
                if ( dbCount == count-1)
                {
                    BPTest.Pass<TestStep>($"Step {count} Passed");
                }
                else
                {
                    BPTest.Fail<TestStep>($"Step {count} Failed");
                }

            }
            catch(Exception ex)
            {
               string x = ex.Message;
            }
        }

        [Category("Api_Smoke")]
        [Category("Api_Positive")]
        [Category("ExclusionList")]
        [Category("ExclusionList_Positive")]
        [Test]
        public void ExclusionList_Negative()
        {
            try
            {
                BPTest.Start<TestStep>($"Step 0: Create an RG Member");
                decimal[] RemovedCDPs = new decimal[] { 552641M , 552642M , 603849M , 610205M , 663884M , 663888M , 779693M ,
                                                        792763M , 793627M , 881706M , 965871M , 538987M , 525191M  , 517402M };
                decimal[] HTZ18536_NonEarningCDPs = new decimal[] {69042M,
                    67198M,
                    67045M,
                    67046M,
                    67047M,
                    66824M,
                    68779M,
                    68826M,
                    68827M,
                    68834M,
                    68836M,
                    68895M,
                    68902M,
                    68952M,
                    68985M,
                    69022M,
                    25036M,
                    68977M,
                    68981M,
                    67049M,
                    67048M,
                    67051M,
                    66825M,
                    180334M,
                    371153M,
                    480700M,
                    431493M,
                    433369M,
                    331990M,
                    310543M,
                    265480M,
                    67406M,
                    67405M,
                    67404M,
                    67403M,
                    67355M,
                    68832M,
                    68982M,
                    69038M,
                    69107M,
                    69121M,};
                Member testMember = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE").Set("RG", "MemberDetails.A_TIERCODE");
                Member memberOut = Member.AddMember(testMember);
                string loyaltyid = testMember.GetLoyaltyID();

                string query = $@"select vckey from bp_htz.lw_virtualcard where loyaltyidnumber = '{loyaltyid}'";
                Hashtable ht = Database.QuerySingleRow(query);
                decimal vckey = Convert.ToDecimal(ht["VCKEY"]);

                DateTime checkInDt = new DateTime(2019, 10, 2);
                DateTime checkOutDt = new DateTime(2019, 10, 1);
                DateTime origBkDt = new DateTime(2019, 10, 1);
                int count = 1;
                BPTest.Pass<TestStep>($"Step 0 Passed: RG Member Created");
                string query2 = $@"select * from bp_htz.lw_pointtransaction where vckey = '{vckey}'";

                foreach (decimal cdp in HTZ18536_NonEarningCDPs)
                {
                    BPTest.Start<TestStep>($"Step {count}: Test CDP {cdp}");
                    TxnHeader txnHeader = TxnHeader.Generate(testMember.GetLoyaltyID(), checkInDt, checkOutDt, origBkDt, cdp, HertzProgram.GoldPointsRewards, null, "US", 100, "AAAA", 246095, "N", "US", null);
                    testMember.AddTransaction(txnHeader);
                    Member.UpdateMember(testMember);
                    testMember.RemoveTransaction();
                    QueryResult qr = Database.Query(query2);
                    if(qr.Rows.Count != 0)
                    {
                        BPTest.Fail<TestStep>($"Step {count++} Failed");
                    }
                    BPTest.Pass<TestStep>($"Step {count++} Passed");
                }

            }
            catch (Exception ex)
            {
                string x = ex.Message;
            }
        }

        [Test]
        public void TestOneMember()
        {
            BPTest.Start<TestStep>($"Step 1: Create Member and Execute Transaction");
            Member testMember = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE").Set("RG", "MemberDetails.A_TIERCODE");
            Member.AddMember(testMember);
            decimal cdp = 16300;
            DateTime checkInDt = new DateTime(2019, 10, 2);
            DateTime checkOutDt = new DateTime(2019, 10, 1);
            DateTime origBkDt = new DateTime(2019, 10, 1);
            TxnHeader txnHeader = TxnHeader.Generate(testMember.GetLoyaltyID(), checkInDt, checkOutDt, origBkDt, cdp, HertzProgram.GoldPointsRewards, null, "US", 1000, "AAAA", 246095, "N", "US", null);
            testMember.AddTransaction(txnHeader);
            Member.UpdateMember(testMember);
            BPTest.Pass<TestStep>($"Step 1 Passed");
        }

    }
}
