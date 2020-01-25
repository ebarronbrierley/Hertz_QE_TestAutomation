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
using Brierley.TestAutomation.Core.SFTP;
using System.Threading;

namespace HertzNetFramework.Tests.UserStoryTests
{
    class CorpWinback : BrierleyTestFixture
    {
        [Test]
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
                Member corpwinbackmember = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE").Set("PC", "MemberDetails.A_TIERCODE").Set("GB", "MemberDetails.A_COUNTRY");
                Member memberOut = Member.AddMember(corpwinbackmember);
                Assert.IsNotNull(memberOut, "Expected populated member object, but member object returned was null");
                string loyaltyid = corpwinbackmember.GetLoyaltyID();
                BPTest.Pass<TestStep>("Step 1 Passed", memberOut.ReportDetail());

                BPTest.Start<TestStep>("Step 2: Add Member Promotion");
                IEnumerable<Promotion> promos1 = Promotion.GetFromDB(Database, code: "GPRCorpWinback2_2019CurrentPCLapsingLapsedBonus");
                string promocode1 = promos1.First().CODE;
                OneClick oneclickObj = OneClick.GenerateOneClick(loyaltyid, promocode1);
                OneClick.UploadOneClick(oneclickObj, Database);
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

    }
}
