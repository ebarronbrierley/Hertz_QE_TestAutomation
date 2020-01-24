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
using Brierley.TestAutomation.Core.SFTP;
using System.Threading;

namespace HertzNetFramework.Tests.UserStoryTests
{
    class BuyTier : BrierleyTestFixture
    {
        [Category("Api_Smoke")]
        [Category("Api_Positive")]
        [Category("ExclusionList")]
        [Category("ExclusionList_Positive")]
        [Test]
        public void TestBuyTier()
        {
            try
            {
                decimal points = 6500;
                string code = "BT_2019_MaintainFG4000_TierPurchaseReg";
                string tier = "RG";
                HertzTier htztier = GPR.Tier.RegularGold;
                SFTPConfiguration sftp_config = new SFTPConfiguration();
                sftp_config.Host = EnvironmentManager.Get.SFTPHost;
                sftp_config.Port = Convert.ToUInt16(EnvironmentManager.Get.SFTPPort);
                sftp_config.User = EnvironmentManager.Get.SFTPUser;
                sftp_config.Password = EnvironmentManager.Get.SFTPPassword;
                BPTest.Start<TestStep>("Step 1: Generate and Add Member");
                //Member buyTierMember = Member.GenerateRandom(MemberStyle.PreProjectOne, null, htztier).Set("N1", "MemberDetails.A_EARNINGPREFERENCE").Set(tier, "MemberDetails.A_TIERCODE").Set("BE", "MemberDetails.A_COUNTRY");
                Member buyTierMember = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE").Set(tier, "MemberDetails.A_TIERCODE").Set("US", "MemberDetails.A_COUNTRY");
                Member memberOut = Member.AddMember(buyTierMember);
                Assert.IsNotNull(memberOut, "Expected populated member object, but member object returned was null");
                string loyaltyid = buyTierMember.GetLoyaltyID();

                DateTime checkInDt = new DateTime(2019, 12, 2);
                DateTime checkOutDt = new DateTime(2019, 12, 1);
                DateTime origBkDt = new DateTime(2019, 12, 1);

                TxnHeader txnHeader = TxnHeader.Generate(loyaltyid, checkInDt, checkOutDt, origBkDt, null, HertzProgram.GoldPointsRewards, null, "GB", points, "", null, "N", "US", null);
                buyTierMember.AddTransaction(txnHeader);
                Member updatedMember = Member.UpdateMember(buyTierMember);
                buyTierMember.RemoveTransaction();
                BPTest.Pass<TestStep>("Step 1 Passed", memberOut.ReportDetail());

                BPTest.Start<TestStep>("Step 2: Add Member Promotion");
                IEnumerable<Promotion> promos1 = Promotion.GetFromDB(Database, code: code);
                Assert.IsNotNull(promos1, "The Promo could not be sucessfully found in the Database");
                string promocode1 = promos1.First().CODE;
                //MemberPromotion mempromo = corpwinbackmember.AddPromotion(loyaltyid, promocode1, null, null, false, null, null, false);
                OneClick oneclickObj = OneClick.GenerateOneClick(loyaltyid, promocode1);
                string oneclickString = oneclickObj.ToString();
                using (SFTP sftp = new SFTP(sftp_config))
                {
                    sftp.Connect();
                    System.IO.File.WriteAllText($@"C:\Users\oagwuegbo\Documents\HertzProjectOne\SFTPFiles\HTZ19270BuyTierOneClickFiles\{oneclickObj.Filename}", oneclickString);
                    sftp.UploadFile($@"C:\Users\oagwuegbo\Documents\HertzProjectOne\SFTPFiles\HTZ19270BuyTierOneClickFiles", oneclickObj.Filename, @"/opt/app/oracle/flatfiles/htz/lw/qa_b/in/auto");
                }
                Thread.Sleep(1000);
                Database.ExecuteNonQuery($@"BEGIN
                   sla.qa_utils.gen_trigger_and_load_file (
                         p_filename  => '{oneclickObj.Filename}'
                       , p_client_cd => 'HTZLW'
                   );
                END;");
                bool complete = false;
                int count = 0;
                while (!complete && count < 60)
                {
                    string query1 = $@"select * from bp_htz.filelog where filename like '{oneclickObj.Filename}' order by updatedate desc";
                    Hashtable ht1 = Database.QuerySingleRow(query1);
                    if (ht1["STATUS"] == null)
                    {
                        Thread.Sleep(2000);
                        OneClick.RunDAP();
                        continue;
                    }

                    if (ht1["STATUS"].ToString().Equals("COMPLETE", StringComparison.OrdinalIgnoreCase))
                    {
                        complete = true;
                    }
                    else
                    {
                        count++;
                        Thread.Sleep(8000);
                    }
                }
                BPTest.Pass<TestStep>("Step 2 Passed");
                BPTest.Start<TestStep>("Step 3: Validate that Promo was Added Sucessfully");
                string query2 = $@"select mp.code from bp_htz.lw_memberpromotion mp inner join bp_htz.lw_virtualcard vc on vc.ipcode = mp.memberid where loyaltyidnumber = '{loyaltyid}'";
                Hashtable ht2 = Database.QuerySingleRow(query2);
                if (ht2["CODE"] == null)
                {
                    BPTest.Fail<TestStep>("Step 3 Failed: Member does not have promocode");
                }else if (!ht2["CODE"].ToString().Equals(promocode1))
                {
                    BPTest.Fail<TestStep>("Step 3 Failed: Member has incorrect promocode");
                }

                BPTest.Pass<TestStep>("Step 3 Passed");

            }
            catch (AssertModelEqualityException ex)
            {
                BPTest.Fail<TestStep>(ex.Message);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                BPTest.Fail<TestStep>(ex.Message);
                Assert.Fail();
            }

        }

        [Category("Api_Smoke")]
        [Category("Api_Positive")]
        [Category("ExclusionList")]
        [Category("ExclusionList_Positive")]
        [Test]
        public void TestBuyTierNegative()
        {
            try
            {
                decimal points = 5500;
                string code = "19BuyTierMaintainPC5500Reg";
                SFTPConfiguration sftp_config = new SFTPConfiguration();
                sftp_config.Host = EnvironmentManager.Get.SFTPHost;
                sftp_config.Port = Convert.ToUInt16(EnvironmentManager.Get.SFTPPort);
                sftp_config.User = EnvironmentManager.Get.SFTPUser;
                sftp_config.Password = EnvironmentManager.Get.SFTPPassword;

                BPTest.Start<TestStep>("Step 1: Generate and Add Member");
                Member buyTierMember = Member.GenerateRandom(MemberStyle.PreProjectOne, null, new HertzTier("Presidents Circle", "FG", 0.5M)).Set("N1", "MemberDetails.A_EARNINGPREFERENCE").Set("PC", "MemberDetails.A_TIERCODE").Set("BE", "MemberDetails.A_COUNTRY");

                Member memberOut = Member.AddMember(buyTierMember);
                Assert.IsNotNull(memberOut, "Expected populated member object, but member object returned was null");
                string loyaltyid = buyTierMember.GetLoyaltyID();

                DateTime checkInDt = new DateTime(2019, 11, 2);
                DateTime checkOutDt = new DateTime(2019, 11, 1);
                DateTime origBkDt = new DateTime(2019, 11, 1);

                TxnHeader txnHeader = TxnHeader.Generate(loyaltyid, checkInDt, checkOutDt, origBkDt, null, HertzProgram.GoldPointsRewards, null, "GB", points, "", null, "N", "US", null);
                buyTierMember.AddTransaction(txnHeader);
                Member updatedMember = Member.UpdateMember(buyTierMember);
                buyTierMember.RemoveTransaction();
                BPTest.Pass<TestStep>("Step 1 Passed", memberOut.ReportDetail());

                BPTest.Start<TestStep>("Step 2: Add Member Promotion");
                IEnumerable<Promotion> promos1 = Promotion.GetFromDB(Database, code: code);
                Assert.IsNotNull(promos1, "The Promo could not be sucessfully found in the Database");
                string promocode1 = promos1.First().CODE;
                //MemberPromotion mempromo = corpwinbackmember.AddPromotion(loyaltyid, promocode1, null, null, false, null, null, false);
                OneClick oneclickObj = OneClick.GenerateOneClick(loyaltyid, promocode1);
                string oneclickString = oneclickObj.ToString();
                using (SFTP sftp = new SFTP(sftp_config))
                {
                    sftp.Connect();
                    System.IO.File.WriteAllText($@"C:\Users\oagwuegbo\Documents\HertzProjectOne\SFTPFiles\HTZ19270BuyTierOneClickFiles\{oneclickObj.Filename}", oneclickString);
                    sftp.UploadFile($@"C:\Users\oagwuegbo\Documents\HertzProjectOne\SFTPFiles\HTZ19270BuyTierOneClickFiles", oneclickObj.Filename, @"/opt/app/oracle/flatfiles/in/htz/qa/auto");
                }
                Thread.Sleep(1000);
                Database.ExecuteNonQuery($@"BEGIN
                   sla.qa_utils.gen_trigger_and_load_file (
                         p_filename  => '{oneclickObj.Filename}'
                       , p_client_cd => 'HTZ'
                   );
                END;");
                bool complete = false;
                int count = 0;
                while (!complete && count < 60)
                {
                    string query1 = $@"select * from bp_htz.filelog where filename like '{oneclickObj.Filename}' order by updatedate desc";
                    Hashtable ht1 = Database.QuerySingleRow(query1);
                    if (ht1["STATUS"] == null)
                    {
                        Thread.Sleep(8000);
                        continue;
                    }

                    if (ht1["STATUS"].ToString().Equals("COMPLETE", StringComparison.OrdinalIgnoreCase))
                    {
                        complete = true;
                    }
                    else
                    {
                        count++;
                        Thread.Sleep(8000);
                    }
                }
                BPTest.Pass<TestStep>("Step 2 Passed");
                BPTest.Start<TestStep>("Step 3: Validate that Promo was NOT Added Sucessfully");
                string query2 = $@"select d.a_mktgprogramid from bp_htz.ats_memberdetails d
                inner join bp_htz.lw_virtualcard vc on vc.vckey = d.a_vckey where vc.loyaltyidnumber = '{loyaltyid}'";
                Hashtable ht2 = Database.QuerySingleRow(query2);
                if (ht2["A_MKTGPROGRAMID"] == System.DBNull.Value)
                {
                    BPTest.Pass<TestStep>("Step 3 Passed: Member does not have promocode");
                }
                else
                {
                    BPTest.Fail<TestStep>("Step 3 Failed: Member has promocode");
                }
                

            }
            catch (AssertModelEqualityException ex)
            {
                BPTest.Fail<TestStep>(ex.Message);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                BPTest.Fail<TestStep>(ex.Message);
                Assert.Fail();
            }

        }
    }
}
