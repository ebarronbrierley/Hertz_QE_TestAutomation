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
                decimal pointsEarn = 10000;

                BPTest.Start<TestStep>($"Step 1: Create an RG Member");
                Member testMember = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE").Set("RG", "MemberDetails.A_TIERCODE");
                Member memberOut = Member.AddMember(testMember);
                string loyaltyid = testMember.GetLoyaltyID();
                BPTest.Pass<TestStep>($"Step 1 Passed: RG Member Created");

                BPTest.Start<TestStep>($"Step 2: Update With Transaction");
                string query = $@"select vckey from bp_htz.lw_virtualcard where loyaltyidnumber = '{loyaltyid}'";
                Hashtable ht = Database.QuerySingleRow(query);
                decimal vckey = Convert.ToDecimal(ht["VCKEY"]);

                DateTime checkInDt = new DateTime(2019, 10, 2);
                DateTime checkOutDt = new DateTime(2019, 10, 1);
                DateTime origBkDt = new DateTime(2019, 10, 1);
                decimal? cdp = null;
                decimal pointEventId = 10;

                TxnHeader txnHeader = TxnHeader.Generate(testMember.GetLoyaltyID(), checkInDt, checkOutDt, origBkDt, cdp, HertzProgram.GoldPointsRewards, null, "US", pointsEarn, "AAAA", 246095, "N", "US", null);
                testMember.AddTransaction(txnHeader);
                Member.UpdateMember(testMember);
                BPTest.Pass<TestStep>($"Step 2 Passed: Updated Member with Transaction");

                BPTest.Start<TestStep>($"Step 3: Validate Point Event");
                IEnumerable<PointTransaction> ptTransactions = PointTransaction.GetFromDB(Database, vckey);
                Assert.Equals(ptTransactions.First().POINTEVENTID, pointEventId);
                BPTest.Pass<TestStep>($"Step 3: Point Event Validate");


            }
            catch (Exception ex)
            {
                BPTest.Fail<TestStep>("Step Failed.");
            }
        }
    }
}
