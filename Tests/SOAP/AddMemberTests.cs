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
                Member corpwinbackmember = Member.GenerateRandom(MemberStyle.PreProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE").Set("FG", "MemberDetails.A_TIERCODE").Set("GB", "MemberDetails.A_COUNTRY");
                Member memberOut = Member.AddMember(corpwinbackmember);
                Assert.IsNotNull(memberOut, "Expected populated member object, but member object returned was null");
                string loyaltyid = corpwinbackmember.GetLoyaltyID();
                BPTest.Pass<TestStep>("Step 1 Passed", memberOut.ReportDetail());
                BPTest.Start<TestStep>("Step 2: Add Member Promotion");
                IEnumerable<Promotion> promos = Promotion.GetFromDB(Database, code: "EUWinback2019R2FG1");
                string promocode1 = "GPRCorpWinback2_2019CurrentFSDowngradedfromPCBonus";
                string promocode2 = promos.First().CODE;
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
                        Thread.Sleep(1000);
                        continue;
                    }

                    if (ht["STATUS"].ToString().Equals("COMPLETE", StringComparison.OrdinalIgnoreCase))
                    {
                        complete = true;
                    }
                    else
                    {
                        count++;
                        Thread.Sleep(1000);
                    }
                }

                BPTest.Pass<TestStep>("Step 2 Passed");
                BPTest.Start<TestStep>("Step 3: Update Member with Transaction 6 Times");
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
                "ALTERNATEID = null",
                 Member.GenerateRandom(MemberStyle.PreProjectOne).Set(null,"ALTERNATEID"),
                 9994,
                 "Empty Member/AlternateId in input Xml for Member lookup"
            },//ALTERNATEID = null
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
                $"VirutalCard.MemberDetails = null",
                Member.GenerateRandom(MemberStyle.PreProjectOne).Set(null,"VirtualCards.MemberDetails"),
                9993,
                "Unable to find node MemberDetails in <VirtualCard VcKey",
            },//VirutalCard.MemberDetails = null
            new object[]
            {
                $"MemberDetails.TierCode = 'abcd'",
                Member.GenerateRandom(MemberStyle.PreProjectOne).Set("acbc","VirtualCards.MemberDetails.A_TIERCODE"),
                2,
                "Invalid tier code",
            },//MemberDetails.TierCode = 'abcd'
            new object[]
            {
                $"VirtualCards.MemberDetails.A_MEMBERSTATUSCODE = 67",
                Member.GenerateRandom(MemberStyle.PreProjectOne).Set(67,"VirtualCards.MemberDetails.A_MEMBERSTATUSCODE"),
                20,
                "Invalid status change",
            },//VirtualCards.MemberDetails.A_MEMBERSTATUSCODE = 67
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
