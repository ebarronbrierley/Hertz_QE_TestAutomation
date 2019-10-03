using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;
using Brierley.TestAutomation.Core.Reporting;
using Brierley.TestAutomation.Core.Utilities;
using Brierley.TestAutomation.Core.SFTP;
using HertzNetFramework.DataModels;
using System.Collections;
using System.Threading;

namespace HertzNetFramework.Tests.SOAP
{
    [TestFixture]
    public class AddMember:BrierleyTestFixture
    {

        [Category("Api_Smoke")]
        [Category("Api_Positive")]
        [Category("AddMember")]
        [Category("AddMember_Positive")]
        [TestCaseSource("PositiveScenarios")]
        public void AddMember_Positive(string name, MemberStyle memberStyle, Member member)
        {
            try
            {
                Member createMember = member;
                BPTest.Start<TestStep>($"Make AddMember Call for {memberStyle} Member with {name}", "Member should be added successfully and member object should be returned");
                Member memberOut = Member.AddMember(createMember);
                Assert.IsNotNull(memberOut, "Expected populated member object, but member object returned was null");
                BPTest.Pass<TestStep>("Member was added successfully and member object was returned", memberOut.ReportDetail());

                BPTest.Start<TestStep>($"Verify Member in {Member.TableName} table", "Database member should match created member");
                Member dbMember = Member.GetFromDB(Database, memberOut.IPCODE, memberStyle);
                AssertModels.AreEqualOnly(createMember, dbMember, Member.BaseVerify);
                BPTest.Pass<TestStep>("Member created matches member in database");

                BPTest.Start<TestStep>($"Verify MemberDetails in {MemberDetails.TableName} table", "Database member details should match created member");
                AssertModels.AreEqualWithAttribute(createMember.GetMemberDetails(memberStyle).First(), dbMember.GetMemberDetails(memberStyle).First());
                BPTest.Pass<TestStep>("Member details created matches member details in database");

                BPTest.Start<TestStep>($"Verify MemberPreferences in {MemberPreferences.TableName} table", "Database member preferences should match created member");
                AssertModels.AreEqualWithAttribute(createMember.GetMemberPreferences(memberStyle).First(), dbMember.GetMemberPreferences(memberStyle).First());
                BPTest.Pass<TestStep>("Member preferences created matches member preferences in database");

                BPTest.Start<TestStep>($"Verify VirtualCard in {VirtualCard.TableName} table", "Database VirtualCard should API member VirtualCard");
                AssertModels.AreEqualWithAttribute(dbMember.VirtualCards.First(), memberOut.VirtualCards.First());
                BPTest.Pass<TestStep>("API Member VirtualCard matches database member VirtualCard");

            }
            catch(LWServiceException ex)
            {
                BPTest.Fail<TestStep>(ex.Message, new[] { $"Error Code: {ex.ErrorCode}", $"Error Message: {ex.ErrorMessage}" });
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

        [Category("Api_Smoke")]
        [Category("Api_Negative")]
        [Category("AddMember")]
        [Category("AddMember_Negative")]
        [TestCaseSource("NegativeScenarios")]
        public void AddMember_Negative(string name, Member member, int errorCode, string errorMessage)
        {
            try
            {
                BPTest.Start<TestStep>($"Make AddMember Call with {name}", $"Add member call should throw exception with error code = {errorCode}");
                LWServiceException exception = Assert.Throws<LWServiceException>(() => Member.AddMember(member), "Excepted LWServiceException, exception was not thrown.");
                Assert.AreEqual(errorCode, exception.ErrorCode);
                Assert.IsTrue(exception.Message.Contains(errorMessage));
                BPTest.Pass<TestStep>("Add Member call threw expected exception", exception.ReportDetail());

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

        [Category("Api_Smoke")]
        [Category("Api_Positive")]
        [Category("AddMember")]
        [Category("AddMember_Positive")]
        [TestCaseSource("PositiveScenarios")]
        public void CorpWinback(string name, MemberStyle memberStyle, Member member)
        {
            try
            {
                SFTPConfiguration sftp_config = new SFTPConfiguration();
                sftp_config.Host = EnvironmentManager.Get.SFTPHost;
                sftp_config.Port = Convert.ToUInt16(EnvironmentManager.Get.SFTPPort);
                sftp_config.User = EnvironmentManager.Get.SFTPUser;
                sftp_config.Password = EnvironmentManager.Get.SFTPPassword;
                BPTest.Start<TestStep>("Step 1: Generate and Add Member");
                Member corpwinbackmember = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE").Set("FG", "MemberDetails.A_TIERCODE").Set("GB", "MemberDetails.A_COUNTRY");
                Member memberOut = Member.AddMember(corpwinbackmember);
                Assert.IsNotNull(memberOut, "Expected populated member object, but member object returned was null");
                string loyaltyid = corpwinbackmember.GetLoyaltyID();
                BPTest.Pass<TestStep>("Step 1 Passed", memberOut.ReportDetail());
                BPTest.Start<TestStep>("Step 2: Add Member Promotion");
                IEnumerable<Promotion> promos1 = Promotion.GetFromDB(Database, code: "GPRCorpWinback2_2019CurrentFSNotDowngradedLapsingLapsedBonus");
                IEnumerable<Promotion> promos2 = Promotion.GetFromDB(Database, code: "EUWinback2019R2FG1");
                string promocode1 = promos1.First().CODE;
                string promocode2 = promos2.First().CODE;
                MemberPromotion mempromo = corpwinbackmember.AddPromotion(loyaltyid, promocode2, null, null, false, null, null, false);
                OneClick oneclickObj = OneClick.GenerateOneClick(loyaltyid, promocode1);
                string oneclickString = oneclickObj.ToString();
                using (SFTP sftp = new SFTP(sftp_config))
                {
                    sftp.Connect();
                    System.IO.File.WriteAllText($@"C:\Users\oagwuegbo\Documents\HertzProjectOne\SFTPFiles\{oneclickObj.Filename}", oneclickString);
                    sftp.UploadFile($@"C:\Users\oagwuegbo\Documents\HertzProjectOne\SFTPFiles", oneclickObj.Filename, @"/opt/app/oracle/flatfiles/in/htz/qa/auto");
                }
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
                    string query = $@"select * from bp_htz.filelog where filename like '{oneclickObj.Filename}' order by updatedate desc";
                    Hashtable ht = Database.QuerySingleRow(query);
                    if (ht["STATUS"] == null)
                    {
                        Thread.Sleep(15000);
                        continue;
                    }

                    if (ht["STATUS"].ToString().Equals("COMPLETE", StringComparison.OrdinalIgnoreCase))
                    {
                        complete = true;
                    }
                    else
                    {
                        count++;
                        Thread.Sleep(30000);
                    }
                }
                BPTest.Pass<TestStep>("Step 2 Passed");
                BPTest.Start<TestStep>("Step 3: Update Member with Transaction 13 Times");
                //IEnumerable<Member> getMembersOut = Member.GetMembers(MemberStyle.ProjectOne, new[] { "CardID" }, new[] { "1818626" }, null, null, string.Empty);
                //Member corpwinbackmember = getMembersOut.First<Member>();
                //string loyaltyid = "1818626";
                DateTime checkInDt = new DateTime(2019, 9, 4);
                DateTime checkOutDt = new DateTime(2019, 9, 3);
                DateTime origBkDt = new DateTime(2019, 9, 3);
                for (int x = 0; x < 13; x++)
                {
                    TxnHeader txnHeader = TxnHeader.Generate(loyaltyid, checkInDt, checkOutDt, origBkDt, null, HertzProgram.GoldPointsRewards, null, "GB", 50, "AAAA", 246095, "N", "US", null);
                    corpwinbackmember.AddTransaction(txnHeader);
                    Member updatedMember = Member.UpdateMember(corpwinbackmember);
                    corpwinbackmember.RemoveTransaction();
                }
                BPTest.Pass<TestStep>("Step 3 Passed");

            }
            catch(AssertModelEqualityException ex)
            {
                BPTest.Fail<TestStep>(ex.Message);
                Assert.Fail();
            }
            catch(Exception ex)
            {
                BPTest.Fail<TestStep>(ex.Message);
                Assert.Fail();
            }


        }

        [Category("Api_Smoke")]
        [Category("Api_Positive")]
        [Category("AddMember")]
        [Category("AddMember_Positive")]
        [TestCaseSource("PositiveScenarios")]
        public void CorpWinbackFG(string name, MemberStyle memberStyle, Member member)
        {
            try
            {
                SFTPConfiguration sftp_config = new SFTPConfiguration();
                sftp_config.Host = EnvironmentManager.Get.SFTPHost;
                sftp_config.Port = Convert.ToUInt16(EnvironmentManager.Get.SFTPPort);
                sftp_config.User = EnvironmentManager.Get.SFTPUser;
                sftp_config.Password = EnvironmentManager.Get.SFTPPassword;

                BPTest.Start<TestStep>("Step 1: Generate and Add Member");
                Member corpwinbackmember = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE").Set("PC", "MemberDetails.A_TIERCODE").Set("GB", "MemberDetails.A_COUNTRY");
                Member memberOut = Member.AddMember(corpwinbackmember);
                Assert.IsNotNull(memberOut, "Expected populated member object, but member object returned was null");
                string loyaltyid = corpwinbackmember.GetLoyaltyID();
                BPTest.Pass<TestStep>("Step 1 Passed", memberOut.ReportDetail());

                BPTest.Start<TestStep>("Step 2: Update Member with transaction 8 times");
                DateTime checkInDt = new DateTime(2019, 9, 4);
                DateTime checkOutDt = new DateTime(2019, 9, 3);
                DateTime origBkDt = new DateTime(2019, 9, 3);
                //for (int x = 0; x < 8; x++)
                //{
                //    TxnHeader txnHeader = TxnHeader.Generate(loyaltyid, checkInDt, checkOutDt, origBkDt, null, HertzProgram.GoldPointsRewards, null, "GB", 50, "AAAA", 246095, "N", "US", null);
                //    corpwinbackmember.AddTransaction(txnHeader);
                //    Member updatedMember = Member.UpdateMember(corpwinbackmember);
                //    corpwinbackmember.RemoveTransaction();
                //}
                BPTest.Pass<TestStep>("Step 2: Passed");

                BPTest.Start<TestStep>("Step 3: Add Member Promotion");
                IEnumerable<Promotion> promos1 = Promotion.GetFromDB(Database, code: "GPRCorpWinback2_2019CurrentPCLapsingLapsedBonus");
                IEnumerable<Promotion> promos2 = Promotion.GetFromDB(Database, code: "EUWinback2019R2FG1");
                string promocode1 = promos1.First().CODE;
                string promocode2 = promos2.First().CODE;
                //MemberPromotion mempromo = corpwinbackmember.AddPromotion(loyaltyid, promocode2, null, null, false, null, null, false);
                OneClick oneclickObj = OneClick.GenerateOneClick(loyaltyid, promocode1);
                string oneclickString = oneclickObj.ToString();
                using (SFTP sftp = new SFTP(sftp_config))
                {
                    sftp.Connect();
                    System.IO.File.WriteAllText($@"C:\Users\oagwuegbo\Documents\HertzProjectOne\SFTPFiles\{oneclickObj.Filename}", oneclickString);
                    sftp.UploadFile($@"C:\Users\oagwuegbo\Documents\HertzProjectOne\SFTPFiles", oneclickObj.Filename, @"/opt/app/oracle/flatfiles/in/htz/uat/auto");
                }
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
                    string query = $@"select * from bp_htz.filelog where filename like '{oneclickObj.Filename}' order by updatedate desc";
                    Hashtable ht = Database.QuerySingleRow(query);
                    if (ht["STATUS"] == null)
                    {
                        Thread.Sleep(15000);
                        continue;
                    }

                    if (ht["STATUS"].ToString().Equals("COMPLETE", StringComparison.OrdinalIgnoreCase))
                    {
                        complete = true;
                    }
                    else
                    {
                        count++;
                        Thread.Sleep(15000);
                    }
                }
                BPTest.Pass<TestStep>("Step 3 Passed");

                BPTest.Start<TestStep>("Step 4: Update Member with Transaction 5 more Times");
                //IEnumerable<Member> getMembersOut = Member.GetMembers(MemberStyle.ProjectOne, new[] { "CardID" }, new[] { "1818626" }, null, null, string.Empty);
                //Member corpwinbackmember = getMembersOut.First<Member>();
                //string loyaltyid = "1818626";
                //DateTime checkInDt = new DateTime(2019, 9, 4);
                //DateTime checkOutDt = new DateTime(2019, 9, 3);
                //DateTime origBkDt = new DateTime(2019, 9, 3);
                for (int x = 0; x < 13; x++)
                {
                    TxnHeader txnHeader = TxnHeader.Generate(loyaltyid, checkInDt, checkOutDt, origBkDt, null, HertzProgram.GoldPointsRewards, null, "GB", 50, "AAAA", 246095, "N", "US", null);
                    corpwinbackmember.AddTransaction(txnHeader);
                    Member updatedMember = Member.UpdateMember(corpwinbackmember);
                    corpwinbackmember.RemoveTransaction();
                }
                BPTest.Pass<TestStep>("Step 4 Passed");

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
        [Category("AddMember")]
        [Category("AddMember_Positive")]
        [TestCaseSource("PositiveScenarios")]
        public void CorpWinbackFG2(string name, MemberStyle memberStyle, Member member)
        {
            try
            {
                SFTPConfiguration sftp_config = new SFTPConfiguration();
                sftp_config.Host = EnvironmentManager.Get.SFTPHost;
                sftp_config.Port = Convert.ToUInt16(EnvironmentManager.Get.SFTPPort);
                sftp_config.User = EnvironmentManager.Get.SFTPUser;
                sftp_config.Password = EnvironmentManager.Get.SFTPPassword;

                BPTest.Start<TestStep>("Step 1: Generate and Add Member");
                Member corpwinbackmember = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE").Set("FG", "MemberDetails.A_TIERCODE").Set("GB", "MemberDetails.A_COUNTRY");
                Member memberOut = Member.AddMember(corpwinbackmember);
                Assert.IsNotNull(memberOut, "Expected populated member object, but member object returned was null");
                string loyaltyid = corpwinbackmember.GetLoyaltyID();
                BPTest.Pass<TestStep>("Step 1 Passed", memberOut.ReportDetail());

                BPTest.Start<TestStep>("Step 2: Update Member with transaction 8 times");
                DateTime checkInDt = new DateTime(2019, 9, 4);
                DateTime checkOutDt = new DateTime(2019, 9, 3);
                DateTime origBkDt = new DateTime(2019, 9, 3);
                for (int x = 0; x < 8; x++)
                {
                    TxnHeader txnHeader = TxnHeader.Generate(loyaltyid, checkInDt, checkOutDt, origBkDt, null, HertzProgram.GoldPointsRewards, null, "GB", 50, "AAAA", 246095, "N", "US", null);
                    corpwinbackmember.AddTransaction(txnHeader);
                    Member updatedMember = Member.UpdateMember(corpwinbackmember);
                    corpwinbackmember.RemoveTransaction();
                }
                BPTest.Pass<TestStep>("Step 2: Passed");

                BPTest.Start<TestStep>("Step 3: Add Member Promotion");
                IEnumerable<Promotion> promos1 = Promotion.GetFromDB(Database, code: "GPRCorpWinback2_2019CurrentFSNotDowngradedLapsingLapsedBonus");
                IEnumerable<Promotion> promos2 = Promotion.GetFromDB(Database, code: "GPRCorpWinback2_2019CurrentFSDowngradedfromPCBonus");
                string promocode1 = promos1.First().CODE;
                string promocode2 = promos2.First().CODE;
                MemberPromotion mempromo = corpwinbackmember.AddPromotion(loyaltyid, promocode2, null, null, false, null, null, false);
                OneClick oneclickObj = OneClick.GenerateOneClick(loyaltyid, promocode1);
                string oneclickString = oneclickObj.ToString();
                OneClick oneclickObj2 = OneClick.GenerateOneClick(loyaltyid, promocode2);
                string oneclickString2 = oneclickObj2.ToString();
                using (SFTP sftp = new SFTP(sftp_config))
                {
                    sftp.Connect();
                    System.IO.File.WriteAllText($@"C:\Users\oagwuegbo\Documents\HertzProjectOne\SFTPFiles\{oneclickObj.Filename}", oneclickString);
                    System.IO.File.WriteAllText($@"C:\Users\oagwuegbo\Documents\HertzProjectOne\SFTPFiles\{oneclickObj2.Filename}", oneclickString2);
                    sftp.UploadFile($@"C:\Users\oagwuegbo\Documents\HertzProjectOne\SFTPFiles", oneclickObj.Filename, @"/opt/app/oracle/flatfiles/in/htz/qa/auto");
                    sftp.UploadFile($@"C:\Users\oagwuegbo\Documents\HertzProjectOne\SFTPFiles", oneclickObj2.Filename, @"/opt/app/oracle/flatfiles/in/htz/qa/auto");
                }
                Database.ExecuteNonQuery($@"BEGIN
                   sla.qa_utils.gen_trigger_and_load_file (
                         p_filename  => '{oneclickObj.Filename}'
                       , p_client_cd => 'HTZ'
                   );
                END;");
                Database.ExecuteNonQuery($@"BEGIN
                   sla.qa_utils.gen_trigger_and_load_file (
                         p_filename  => '{oneclickObj2.Filename}'
                       , p_client_cd => 'HTZ'
                   );
                END;");
                bool complete = false;
                int count = 0;
                while (!complete && count < 60)
                {
                    string query = $@"select * from bp_htz.filelog where filename like '{oneclickObj.Filename}' order by updatedate desc";
                    Hashtable ht = Database.QuerySingleRow(query);
                    if (ht["STATUS"] == null)
                    {
                        Thread.Sleep(15000);
                        continue;
                    }

                    if (ht["STATUS"].ToString().Equals("COMPLETE", StringComparison.OrdinalIgnoreCase))
                    {
                        complete = true;
                    }
                    else
                    {
                        count++;
                        Thread.Sleep(15000);
                    }
                }
                complete = false;
                count = 0;
                while (!complete && count < 60)
                {
                    string query = $@"select * from bp_htz.filelog where filename like '{oneclickObj2.Filename}' order by updatedate desc";
                    Hashtable ht = Database.QuerySingleRow(query);
                    if (ht["STATUS"] == null)
                    {
                        Thread.Sleep(15000);
                        continue;
                    }

                    if (ht["STATUS"].ToString().Equals("COMPLETE", StringComparison.OrdinalIgnoreCase))
                    {
                        complete = true;
                    }
                    else
                    {
                        count++;
                        Thread.Sleep(15000);
                    }
                }
                BPTest.Pass<TestStep>("Step 3 Passed");

                BPTest.Start<TestStep>("Step 4: Update Member with Transaction 5 more Times");
                //IEnumerable<Member> getMembersOut = Member.GetMembers(MemberStyle.ProjectOne, new[] { "CardID" }, new[] { "1818626" }, null, null, string.Empty);
                //Member corpwinbackmember = getMembersOut.First<Member>();
                //string loyaltyid = "1818626";
                //DateTime checkInDt = new DateTime(2019, 9, 4);
                //DateTime checkOutDt = new DateTime(2019, 9, 3);
                //DateTime origBkDt = new DateTime(2019, 9, 3);
                for (int x = 0; x < 5; x++)
                {
                    TxnHeader txnHeader = TxnHeader.Generate(loyaltyid, checkInDt, checkOutDt, origBkDt, null, HertzProgram.GoldPointsRewards, null, "GB", 50, "AAAA", 246095, "N", "US", null);
                    corpwinbackmember.AddTransaction(txnHeader);
                    Member updatedMember = Member.UpdateMember(corpwinbackmember);
                    corpwinbackmember.RemoveTransaction();
                }
                BPTest.Pass<TestStep>("Step 4 Passed");

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
        [Category("AddMember")]
        [Category("AddMember_Positive")]
        [TestCaseSource("PositiveScenarios")]
        public void CorpWinbackPC(string name, MemberStyle memberStyle, Member member)
        {
            try
            {
                SFTPConfiguration sftp_config = new SFTPConfiguration();
                sftp_config.Host = EnvironmentManager.Get.SFTPHost;
                sftp_config.Port = Convert.ToUInt16(EnvironmentManager.Get.SFTPPort);
                sftp_config.User = EnvironmentManager.Get.SFTPUser;
                sftp_config.Password = EnvironmentManager.Get.SFTPPassword;

                BPTest.Start<TestStep>("Step 1: Generate and Add Member");
                Member corpwinbackmember = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE")
                    .Set("FG", "MemberDetails.A_TIERCODE").Set("GB", "MemberDetails.A_COUNTRY").Set("0602642", "MemberDetails.A_CDPNUMBER");
                Member memberOut = Member.AddMember(corpwinbackmember);
                Assert.IsNotNull(memberOut, "Expected populated member object, but member object returned was null");
                string loyaltyid = corpwinbackmember.GetLoyaltyID();
                BPTest.Pass<TestStep>("Step 1 Passed", memberOut.ReportDetail());

                //BPTest.Start<TestStep>("Step 2: Add Member Promotion");
                //IEnumerable<Promotion> promos1 = Promotion.GetFromDB(Database, code: "EUWinback2019R2FG1");
                //string promocode1 = promos1.First().CODE;
                //MemberPromotion mempromo = corpwinbackmember.AddPromotion(loyaltyid, promocode1, null, null, false, null, null, false);
                //BPTest.Pass<TestStep>("Step 2 Passed");

                BPTest.Start<TestStep>("Step 3: Update Member with Transaction 13 Times");
                //IEnumerable<Member> getMembersOut = Member.GetMembers(MemberStyle.ProjectOne, new[] { "CardID" }, new[] { "1818626" }, null, null, string.Empty);
                //Member corpwinbackmember = getMembersOut.First<Member>();
                //string loyaltyid = "1818626";
                DateTime checkInDt = new DateTime(2019, 9, 4);
                DateTime checkOutDt = new DateTime(2019, 9, 3);
                DateTime origBkDt = new DateTime(2019, 9, 3);
                for (int x = 0; x < 3; x++)
                {
                    TxnHeader txnHeader = TxnHeader.Generate(loyaltyid, checkInDt, checkOutDt, origBkDt, null, HertzProgram.GoldPointsRewards, null, "GB", 1000, "AAAA", 246095, "N", "US", null);
                    corpwinbackmember.AddTransaction(txnHeader);
                    Member updatedMember = Member.UpdateMember(corpwinbackmember);
                    corpwinbackmember.RemoveTransaction();
                }
                //for (int x = 0; x < 6; x++)
                //{
                //    TxnHeader txnHeader = TxnHeader.Generate(loyaltyid, checkInDt, checkOutDt, origBkDt, null, HertzProgram.GoldPointsRewards, null, "GB", 1000, "AAAA", 246095, "N", "US", null);
                //    corpwinbackmember.AddTransaction(txnHeader);
                //    Member updatedMember = Member.UpdateMember(corpwinbackmember);
                //    corpwinbackmember.RemoveTransaction();
                //}
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
        [Category("AddMember")]
        [Category("AddMember_Positive")]
        [TestCaseSource("PositiveScenarios")]
        public void MantiwokCDP(string name, MemberStyle memberStyle, Member member)
        {
            try
            {
                SFTPConfiguration sftp_config = new SFTPConfiguration();
                sftp_config.Host = EnvironmentManager.Get.SFTPHost;
                sftp_config.Port = Convert.ToUInt16(EnvironmentManager.Get.SFTPPort);
                sftp_config.User = EnvironmentManager.Get.SFTPUser;
                sftp_config.Password = EnvironmentManager.Get.SFTPPassword;

                BPTest.Start<TestStep>("Step 1: Generate and Add RG Member");
                
                //Member RGMember = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE")
                //    .Set("RG", "MemberDetails.A_TIERCODE").Set("MX", "MemberDetails.A_COUNTRY").Set("537021", "MemberDetails.A_CDPNUMBER");
                //Member FGMember = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE")
                //    .Set("FG", "MemberDetails.A_TIERCODE").Set("MX", "MemberDetails.A_COUNTRY").Set("537021", "MemberDetails.A_CDPNUMBER");
                //Member PCMember = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE")
                //    .Set("PC", "MemberDetails.A_TIERCODE").Set("MX", "MemberDetails.A_COUNTRY").Set("537021", "MemberDetails.A_CDPNUMBER");
                //Member PLMember = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE")
                //    .Set("PL", "MemberDetails.A_TIERCODE").Set("MX", "MemberDetails.A_COUNTRY").Set("537021", "MemberDetails.A_CDPNUMBER");
                Member RGMember = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE")
                    .Set("RG", "MemberDetails.A_TIERCODE").Set("BZ", "MemberDetails.A_COUNTRY");
                Member FGMember = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE")
                    .Set("FG", "MemberDetails.A_TIERCODE").Set("BZ", "MemberDetails.A_COUNTRY");
                Member PCMember = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE")
                    .Set("PC", "MemberDetails.A_TIERCODE").Set("BZ", "MemberDetails.A_COUNTRY");
                Member PLMember = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE")
                    .Set("PL", "MemberDetails.A_TIERCODE").Set("BZ", "MemberDetails.A_COUNTRY");
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
                BPTest.Pass<TestStep>($"Step 1 Passed. Created Members with LIDs: {RGLoyaltyID} {FGLoyaltyID} {PCLoyaltyID} {PLLoyaltyID}", memberOutRG.ReportDetail());

                BPTest.Start<TestStep>("Step 2: Update Member with Transaction 3 Times");
                DateTime checkInDt = new DateTime(2018, 10, 26);
                DateTime checkOutDt = new DateTime(2018, 10, 25);
                DateTime origBkDt = new DateTime(2018, 10, 25);
                decimal? hercCDP = null;
                for (int x = 0; x < 3; x++)
                {
                    TxnHeader txnHeaderRG = TxnHeader.Generate(RGLoyaltyID, checkInDt, checkOutDt, origBkDt, hercCDP, HertzProgram.GoldPointsRewards, null, "BZ", 1000, "AAAA", 246095, "N", "US", null);
                    TxnHeader txnHeaderFG = TxnHeader.Generate(FGLoyaltyID, checkInDt, checkOutDt, origBkDt, hercCDP, HertzProgram.GoldPointsRewards, null, "BZ", 1000, "AAAA", 246095, "N", "US", null);
                    TxnHeader txnHeaderPC = TxnHeader.Generate(PCLoyaltyID, checkInDt, checkOutDt, origBkDt, hercCDP, HertzProgram.GoldPointsRewards, null, "BZ", 1000, "AAAA", 246095, "N", "US", null);
                    TxnHeader txnHeaderPL = TxnHeader.Generate(PLLoyaltyID, checkInDt, checkOutDt, origBkDt, hercCDP, HertzProgram.GoldPointsRewards, null, "BZ", 1000, "AAAA", 246095, "N", "US", null);
                    RGMember.AddTransaction(txnHeaderRG);
                    FGMember.AddTransaction(txnHeaderFG);
                    PCMember.AddTransaction(txnHeaderPC);
                    PLMember.AddTransaction(txnHeaderPL);
                    Member updatedMemberRG = Member.UpdateMember(RGMember);
                    Member updatedMemberFG = Member.UpdateMember(FGMember);
                    Member updatedMemberPC = Member.UpdateMember(PCMember);
                    Member updatedMemberPL = Member.UpdateMember(PLMember);
                    RGMember.RemoveTransaction();
                    FGMember.RemoveTransaction();
                    PCMember.RemoveTransaction();
                    PLMember.RemoveTransaction();
                }
                BPTest.Pass<TestStep>("Step 2 Passed");
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
        [Category("AddMember")]
        [Category("AddMember_Positive")]
        [TestCaseSource("PositiveScenarios")]
        public void CorpWinbackPC2(string name, MemberStyle memberStyle, Member member)
        {
            try
            {
                SFTPConfiguration sftp_config = new SFTPConfiguration();
                sftp_config.Host = EnvironmentManager.Get.SFTPHost;
                sftp_config.Port = Convert.ToUInt16(EnvironmentManager.Get.SFTPPort);
                sftp_config.User = EnvironmentManager.Get.SFTPUser;
                sftp_config.Password = EnvironmentManager.Get.SFTPPassword;

                BPTest.Start<TestStep>("Step 1: Generate and Add Member");
                Member corpwinbackmember = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE").Set("PC", "MemberDetails.A_TIERCODE").Set("GB", "MemberDetails.A_COUNTRY");
                Member memberOut = Member.AddMember(corpwinbackmember);
                Assert.IsNotNull(memberOut, "Expected populated member object, but member object returned was null");
                string loyaltyid = corpwinbackmember.GetLoyaltyID();
                BPTest.Pass<TestStep>("Step 1 Passed", memberOut.ReportDetail());

                BPTest.Start<TestStep>("Step 2: Add Member Promotion");
                IEnumerable<Promotion> promos1 = Promotion.GetFromDB(Database, code: "GPRCorpWinback2_2019CurrentPCLapsingLapsedBonus");
                string promocode1 = promos1.First().CODE;
                OneClick oneclickObj = OneClick.GenerateOneClick(loyaltyid, promocode1);
                string oneclickString = oneclickObj.ToString();
                using (SFTP sftp = new SFTP(sftp_config))
                {
                    sftp.Connect();
                    System.IO.File.WriteAllText($@"C:\Users\oagwuegbo\Documents\HertzProjectOne\SFTPFiles\{oneclickObj.Filename}", oneclickString);
                    sftp.UploadFile($@"C:\Users\oagwuegbo\Documents\HertzProjectOne\SFTPFiles", oneclickObj.Filename, @"/opt/app/oracle/flatfiles/in/htz/qa/auto");
                }
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
                    string query = $@"select * from bp_htz.filelog where filename like '{oneclickObj.Filename}' order by updatedate desc";
                    Hashtable ht = Database.QuerySingleRow(query);
                    if (ht["STATUS"] == null)
                    {
                        Thread.Sleep(15000);
                        continue;
                    }

                    if (ht["STATUS"].ToString().Equals("COMPLETE", StringComparison.OrdinalIgnoreCase))
                    {
                        complete = true;
                    }
                    else
                    {
                        count++;
                        Thread.Sleep(15000);
                    }
                }
                BPTest.Pass<TestStep>("Step 2 Passed");

                BPTest.Start<TestStep>("Step 3: Update Member with Transaction 13 Times");
                //IEnumerable<Member> getMembersOut = Member.GetMembers(MemberStyle.ProjectOne, new[] { "CardID" }, new[] { "1818626" }, null, null, string.Empty);
                //Member corpwinbackmember = getMembersOut.First<Member>();
                //string loyaltyid = "1818626";
                DateTime checkInDt = new DateTime(2019, 9, 4);
                DateTime checkOutDt = new DateTime(2019, 9, 3);
                DateTime origBkDt = new DateTime(2019, 9, 3);
                for (int x = 0; x < 13; x++)
                {
                    TxnHeader txnHeader = TxnHeader.Generate(loyaltyid, checkInDt, checkOutDt, origBkDt, null, HertzProgram.GoldPointsRewards, null, "GB", 50, "AAAA", 246095, "N", "US", null);
                    corpwinbackmember.AddTransaction(txnHeader);
                    Member updatedMember = Member.UpdateMember(corpwinbackmember);
                    corpwinbackmember.RemoveTransaction();
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

        static object[] PositiveScenarios =
        {
            new object[]
            {
                "EarningPreference = DX (Dollar Member), TierCode = empty string",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne).Set("DX","MemberDetails.A_EARNINGPREFERENCE").Set(null, "MemberDetails.A_TIERCODE")
            }//EarningPreference = DX (Dollar Member), TierCode = empty string

            //new object[]
            //{
            //    "EarningPreference = N1 (GoldPointsReward Member), TierCode = FG",
            //    MemberStyle.ProjectOne,
            //    Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1","MemberDetails.A_EARNINGPREFERENCE").Set("FG", "MemberDetails.A_TIERCODE")
            //},//EarningPreference = N1 (GoldPointsReward Member), TierCode = FG
            //new object[]
            //{
            //    "EarningPreference = N1 (GoldPointsReward Member), TierCode = PC",
            //    MemberStyle.ProjectOne,
            //    Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1","MemberDetails.A_EARNINGPREFERENCE").Set("PC", "MemberDetails.A_TIERCODE")
            //},//EarningPreference = N1 (GoldPointsReward Member), TierCode = PC
            //new object[]
            //{
            //    "EarningPreference = BC (Thrifty Member), TierCode = empty string",
            //    MemberStyle.ProjectOne,
            //    Member.GenerateRandom(MemberStyle.ProjectOne).Set("BC","MemberDetails.A_EARNINGPREFERENCE").Set(null, "MemberDetails.A_TIERCODE")
            //}//EarningPreference = BC (Thrifty Member), TierCode = empty string
        };
        static object[] NegativeScenarios =
        {
           new object[]
            {
                "VirtualCard.LoyaltyIdNumber = null",
                Member.GenerateRandom(MemberStyle.PreProjectOne).Set(null,"VirtualCards.LOYALTYIDNUMBER"),
                2003,
                "LoyaltyIdNumber of VirtualCard is a required property.  Please provide a valid value",
            },//VirtualCard.LoyaltyIdNumber = null
            new object[]
            {
                $"Existing VirutalCard.LoyaltyIdNumber = 00000200",
                Member.GenerateRandom(MemberStyle.PreProjectOne).Set("00000200","VirtualCards.LOYALTYIDNUMBER"),
                9991,
                "A member already exists",
            },//Existing VirutalCard.LoyaltyIdNumber = 00000200
     
            new object[]
            {
                $"MemberDetails.TierCode = 'abcd'",
                Member.GenerateRandom(MemberStyle.PreProjectOne).Set("acbc","VirtualCards.MemberDetails.A_TIERCODE"),
                2,
                "Invalid tier code",
            },//MemberDetails.TierCode = 'abcd'
   
            new object[]
            {
                $"VirtualCards.MemberDetails.A_EARNINGPREFERENCE = 'notreal'",
                Member.GenerateRandom(MemberStyle.PreProjectOne).Set("notreal","VirtualCards.MemberDetails.A_EARNINGPREFERENCE"),
                1,
                "Object reference not set to an instance of an object",
            }//VirtualCards.MemberDetails.A_EARNINGPREFERENCE = 'notreal'
        };
    }
}
