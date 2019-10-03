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
    class ExclusionListTests : BrierleyTestFixture
    {
        [Category("Api_Smoke")]
        [Category("Api_Positive")]
        [Category("ExclusionList")]
        [Category("ExclusionList_Positive")]
        [TestCaseSource("PositiveScenarios")]
        public void ExclusionList_Positive()
        {
            try
            {
                decimal[] RemovedCDPs = new decimal[] { 552641 , 552642 , 603849 , 610205 , 663884 , 663888 , 779693 ,
                                                        792763 , 793627 , 881706 , 965871 , 538987 , 525191  , 517402 };
                Member testMember = Member.GenerateRandom(MemberStyle.PreProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE").Set("RG", "MemberDetails.A_TIERCODE");
                Member memberOut = Member.AddMember(testMember);
                DateTime checkInDt = new DateTime(2019, 10, 2);
                DateTime checkOutDt = new DateTime(2019, 10, 1);
                DateTime origBkDt = new DateTime(2019, 10, 1);
                int count = 1;
                foreach (decimal cdp in RemovedCDPs)
                {
                    BPTest.Start<TestStep>($"Step {count}: Test CDP {cdp}");
                    TxnHeader txnHeader = TxnHeader.Generate(testMember.GetLoyaltyID(), checkInDt, checkOutDt, origBkDt, cdp, HertzProgram.GoldPointsRewards, null, "US", 1000, "AAAA", 246095, "N", "US", null);
                    testMember.AddTransaction(txnHeader);
                    Member.UpdateMember(testMember);
                    testMember.RemoveTransaction();
                    BPTest.Pass<TestStep>($"Step {count} Passed");
                }
            }
            catch(Exception ex)
            {

            }
        }

    }
}
