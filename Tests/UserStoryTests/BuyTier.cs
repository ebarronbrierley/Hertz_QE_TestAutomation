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

namespace HertzNetFramework.Tests.SOAP
{
    [TestFixture]
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
        public void TestBuyTierEMERGENCY()
        {
            try
            {
                decimal points = 3750;
                string tier = "RG";
                string code1 = "BT_2019_MaintainFG4000_TierPurchaseReg";
                string code2 = "BT_2019_MaintainFG5000_TierPurchaseReg";
                //string code3 = "BT_2019_UpgradeFGtoPC5500_TierPurchaseReg";
                //string code4 = "BT_2019_UpgradeFGtoPC6000_TierPurchaseReg";
                //string code5 = "";
                HertzTier htztier = GPR.Tier.RegularGold;
                SFTPConfiguration sftp_config = new SFTPConfiguration();
                sftp_config.Host = EnvironmentManager.Get.SFTPHost;
                sftp_config.Port = Convert.ToUInt16(EnvironmentManager.Get.SFTPPort);
                sftp_config.User = EnvironmentManager.Get.SFTPUser;
                sftp_config.Password = EnvironmentManager.Get.SFTPPassword;
                BPTest.Start<TestStep>("Step 1: Generate and Add Member");
                //Member buyTierMember = Member.GenerateRandom(MemberStyle.PreProjectOne, null, htztier).Set("N1", "MemberDetails.A_EARNINGPREFERENCE").Set(tier, "MemberDetails.A_TIERCODE").Set("BE", "MemberDetails.A_COUNTRY");
                Member buyTierMember1 = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE").Set(tier, "MemberDetails.A_TIERCODE").Set("US", "MemberDetails.A_COUNTRY");
                Member buyTierMember2 = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE").Set(tier, "MemberDetails.A_TIERCODE").Set("US", "MemberDetails.A_COUNTRY");

                Member memberOut = Member.AddMember(buyTierMember1);
                Member.AddMember(buyTierMember2);
                Assert.IsNotNull(memberOut, "Expected populated member object, but member object returned was null");

                string loyaltyid1 = buyTierMember1.GetLoyaltyID();
                string loyaltyid2 = buyTierMember2.GetLoyaltyID();

                DateTime checkInDt = new DateTime(2019, 12, 2);
                DateTime checkOutDt = new DateTime(2019, 12, 1);
                DateTime origBkDt = new DateTime(2019, 12, 1);

                TxnHeader txnHeader1 = TxnHeader.Generate(loyaltyid1, checkInDt, checkOutDt, origBkDt, null, HertzProgram.GoldPointsRewards, null, "US", points, "", null, "N", "US", null);
                TxnHeader txnHeader2 = TxnHeader.Generate(loyaltyid1, checkInDt, checkOutDt, origBkDt, null, HertzProgram.GoldPointsRewards, null, "US", points, "", null, "N", "US", null);

                buyTierMember1.AddTransaction(txnHeader1);
                buyTierMember2.AddTransaction(txnHeader2);
                Member.UpdateMember(buyTierMember1);
                Member.UpdateMember(buyTierMember2);

                buyTierMember1.RemoveTransaction();
                buyTierMember2.RemoveTransaction();


                BPTest.Pass<TestStep>("Step 1 Passed", memberOut.ReportDetail());

                BPTest.Start<TestStep>("Step 2: Add Member Promotion");
                //IEnumerable<Promotion> promos1 = Promotion.GetFromDB(Database, code: code);
                //Assert.IsNotNull(promos1, "The Promo could not be sucessfully found in the Database");
                //string promocode1 = promos1.First().CODE;
                //MemberPromotion mempromo = corpwinbackmember.AddPromotion(loyaltyid, promocode1, null, null, false, null, null, false);
                OneClick oneclickObj1 = OneClick.GenerateOneClick(loyaltyid1, code1);
                Thread.Sleep(1500);
                OneClick oneclickObj2 = OneClick.GenerateOneClick(loyaltyid2, code2);

                string oneclickString1 = oneclickObj1.ToString();
                string oneclickString2 = oneclickObj2.ToString();

                using (SFTP sftp = new SFTP(sftp_config))
                {
                    sftp.Connect();
                    System.IO.File.WriteAllText($@"C:\Users\oagwuegbo\Documents\HertzProjectOne\SFTPFiles\HTZ19270BuyTierOneClickFiles\{oneclickObj1.Filename}", oneclickString1);
                    //System.IO.File.WriteAllText($@"C:\Users\oagwuegbo\Documents\HertzProjectOne\SFTPFiles\HTZ19270BuyTierOneClickFiles\{oneclickObj1.Filename}", oneclickString2);

                    sftp.UploadFile($@"C:\Users\oagwuegbo\Documents\HertzProjectOne\SFTPFiles\HTZ19270BuyTierOneClickFiles", oneclickObj1.Filename, @"/opt/app/oracle/flatfiles/htz/lw/qa_b/in/auto");
                    //sftp.UploadFile($@"C:\Users\oagwuegbo\Documents\HertzProjectOne\SFTPFiles\HTZ19270BuyTierOneClickFiles", oneclickObj2.Filename, @"/opt/app/oracle/flatfiles/htz/lw/qa_b/in/auto");

                }
                Thread.Sleep(1000);
                Database.ExecuteNonQuery($@"BEGIN
                   sla.qa_utils.gen_trigger_and_load_file (
                         p_filename  => '{oneclickObj1.Filename}'
                       , p_client_cd => 'HTZLW'
                   );
                END;");
                //Thread.Sleep(2000);
                //Database.ExecuteNonQuery($@"BEGIN
                //   sla.qa_utils.gen_trigger_and_load_file (
                //         p_filename  => '{oneclickObj2.Filename}'
                //       , p_client_cd => 'HTZLW'
                //   );
                //END;");
                bool complete = false;
                int count = 0;
                while (!complete && count < 60)
                {
                    string query1 = $@"select * from bp_htz.filelog where filename like '{oneclickObj1.Filename}' order by updatedate desc";
                    Hashtable ht1 = Database.QuerySingleRow(query1);
                    if (ht1["STATUS"] == null)
                    {
                        OneClick.RunDAP();
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
                BPTest.Start<TestStep>("Step 3: Validate that Promo was Added Sucessfully");
                string query2 = $@"select mp.code from bp_htz.lw_memberpromotion mp inner join bp_htz.lw_virtualcard vc on vc.ipcode = mp.memberid where loyaltyidnumber = '{loyaltyid1}'";
                Hashtable ht2 = Database.QuerySingleRow(query2);
                if (ht2["CODE"] == null)
                {
                    BPTest.Fail<TestStep>("Step 3 Failed: Member does not have promocode");
                }
                else if (!ht2["CODE"].ToString().Equals(code1))
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
        public void TestBuyTierEMERGENCY2()
        {
            try
            {
                BPTest.Start<TestStep>("Step 1: Generate and Add Member");
                string tier = "PC";
                Member RGMember = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE")
                   .Set(tier, "MemberDetails.A_TIERCODE").Set("US", "MemberDetails.A_COUNTRY");
                Member RGMember2 = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE")
                    .Set(tier, "MemberDetails.A_TIERCODE").Set("US", "MemberDetails.A_COUNTRY");
                Member RGMember3 = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE")
                    .Set(tier, "MemberDetails.A_TIERCODE").Set("US", "MemberDetails.A_COUNTRY");
                Member RGMember4 = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE")
                    .Set(tier, "MemberDetails.A_TIERCODE").Set("US", "MemberDetails.A_COUNTRY");
                Member RGMember5 = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE")
                   .Set(tier, "MemberDetails.A_TIERCODE").Set("US", "MemberDetails.A_COUNTRY");
                Member RGMember6 = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE")
                   .Set(tier, "MemberDetails.A_TIERCODE").Set("US", "MemberDetails.A_COUNTRY");
                Member RGMember7 = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE")
                   .Set(tier, "MemberDetails.A_TIERCODE").Set("US", "MemberDetails.A_COUNTRY");
                Member RGMember8 = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE")
                   .Set(tier, "MemberDetails.A_TIERCODE").Set("US", "MemberDetails.A_COUNTRY");
                Member RGMember9 = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE")
                   .Set(tier, "MemberDetails.A_TIERCODE").Set("US", "MemberDetails.A_COUNTRY");
                Member RGMember10 = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE")
                   .Set(tier, "MemberDetails.A_TIERCODE").Set("US", "MemberDetails.A_COUNTRY");
                Member RGMember11 = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE")
                   .Set(tier, "MemberDetails.A_TIERCODE").Set("US", "MemberDetails.A_COUNTRY");
                Member.AddMember(RGMember);
                Member.AddMember(RGMember2);
                Member.AddMember(RGMember3);
                Member.AddMember(RGMember4);
                Member.AddMember(RGMember5);
                Member.AddMember(RGMember6);
                Member.AddMember(RGMember7);
                Member.AddMember(RGMember8);
                Member.AddMember(RGMember9);
                Member.AddMember(RGMember10);
                Member.AddMember(RGMember11);

                string id1 = RGMember.GetLoyaltyID();
                string id2 = RGMember2.GetLoyaltyID();
                string id3 = RGMember3.GetLoyaltyID();
                string id4 = RGMember4.GetLoyaltyID();
                string id5 = RGMember5.GetLoyaltyID();
                string id6 = RGMember6.GetLoyaltyID();
                string id7 = RGMember7.GetLoyaltyID();
                string id8 = RGMember8.GetLoyaltyID();
                string id9 = RGMember9.GetLoyaltyID();
                string id10 = RGMember10.GetLoyaltyID();
                string id11 = RGMember11.GetLoyaltyID();


                DateTime checkInDt = new DateTime(2019, 12, 2);
                DateTime checkOutDt = new DateTime(2019, 12, 1);
                DateTime origBkDt = new DateTime(2019, 12, 1);
                decimal points = 6500;
                TxnHeader txnHeader1 = TxnHeader.Generate(id1, checkInDt, checkOutDt, origBkDt, null, HertzProgram.GoldPointsRewards, null, "US", points, "", null, "N", "US", null);
                TxnHeader txnHeader2 = TxnHeader.Generate(id2, checkInDt, checkOutDt, origBkDt, null, HertzProgram.GoldPointsRewards, null, "US", points, "", null, "N", "US", null);
                TxnHeader txnHeader3 = TxnHeader.Generate(id3, checkInDt, checkOutDt, origBkDt, null, HertzProgram.GoldPointsRewards, null, "US", points, "", null, "N", "US", null);
                TxnHeader txnHeader4 = TxnHeader.Generate(id4, checkInDt, checkOutDt, origBkDt, null, HertzProgram.GoldPointsRewards, null, "US", points, "", null, "N", "US", null);
                TxnHeader txnHeader5 = TxnHeader.Generate(id5, checkInDt, checkOutDt, origBkDt, null, HertzProgram.GoldPointsRewards, null, "US", points, "", null, "N", "US", null);
                TxnHeader txnHeader6 = TxnHeader.Generate(id6, checkInDt, checkOutDt, origBkDt, null, HertzProgram.GoldPointsRewards, null, "US", points, "", null, "N", "US", null);
                TxnHeader txnHeader7 = TxnHeader.Generate(id7, checkInDt, checkOutDt, origBkDt, null, HertzProgram.GoldPointsRewards, null, "US", points, "", null, "N", "US", null);
                TxnHeader txnHeader8 = TxnHeader.Generate(id8, checkInDt, checkOutDt, origBkDt, null, HertzProgram.GoldPointsRewards, null, "US", points, "", null, "N", "US", null);
                TxnHeader txnHeader9 = TxnHeader.Generate(id9, checkInDt, checkOutDt, origBkDt, null, HertzProgram.GoldPointsRewards, null, "US", points, "", null, "N", "US", null);
                TxnHeader txnHeader10 = TxnHeader.Generate(id10, checkInDt, checkOutDt, origBkDt, null, HertzProgram.GoldPointsRewards, null, "US", points, "", null, "N", "US", null);
                TxnHeader txnHeader11 = TxnHeader.Generate(id11, checkInDt, checkOutDt, origBkDt, null, HertzProgram.GoldPointsRewards, null, "US", points, "", null, "N", "US", null);

                RGMember.AddTransaction(txnHeader1);
                RGMember2.AddTransaction(txnHeader2);
                RGMember3.AddTransaction(txnHeader3);
                RGMember4.AddTransaction(txnHeader4);
                RGMember5.AddTransaction(txnHeader5);
                RGMember6.AddTransaction(txnHeader6);
                RGMember7.AddTransaction(txnHeader7);
                RGMember8.AddTransaction(txnHeader8);
                RGMember9.AddTransaction(txnHeader9);
                RGMember10.AddTransaction(txnHeader10);
                RGMember11.AddTransaction(txnHeader11);

                Member.UpdateMember(RGMember);
                Member.UpdateMember(RGMember2);
                Member.UpdateMember(RGMember3);
                Member.UpdateMember(RGMember4);
                Member.UpdateMember(RGMember5);
                Member.UpdateMember(RGMember6);
                Member.UpdateMember(RGMember7);
                Member.UpdateMember(RGMember8);
                Member.UpdateMember(RGMember9);
                Member.UpdateMember(RGMember10);
                Member.UpdateMember(RGMember11);


                BPTest.Pass<TestStep>("Step 1 Passed");


            }
            catch (Exception ex)
            {

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
