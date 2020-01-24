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
                //MemberDetails md1 = createMember.MemberDetails.First();
                //MemberDetails md2 = dbMember.GetMemberDetails(memberStyle).First();
                AssertModels.AreEqualWithAttribute(createMember.GetMemberDetails(memberStyle).First(), dbMember.GetMemberDetails(memberStyle).First());
                //AssertModels.AreEqualWithAttribute(md1, md2);
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

        static object[] PositiveScenarios =
        {
            new object[]
            {
                "EarningPreference = DX (Dollar Member), TierCode = empty string",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne).Set("DX","MemberDetails.A_EARNINGPREFERENCE").Set(null, "MemberDetails.A_TIERCODE")
            },//EarningPreference = DX (Dollar Member), TierCode = empty string

            new object[]
            {
                "EarningPreference = N1 (GoldPointsReward Member), TierCode = FG",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1","MemberDetails.A_EARNINGPREFERENCE").Set("FG", "MemberDetails.A_TIERCODE")
            },//EarningPreference = N1 (GoldPointsReward Member), TierCode = FG
            new object[]
            {
                "EarningPreference = N1 (GoldPointsReward Member), TierCode = PC",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1","MemberDetails.A_EARNINGPREFERENCE").Set("PC", "MemberDetails.A_TIERCODE")
            },//EarningPreference = N1 (GoldPointsReward Member), TierCode = PC
            new object[]
            {
                "EarningPreference = BC (Thrifty Member), TierCode = empty string",
                MemberStyle.ProjectOne,
                Member.GenerateRandom(MemberStyle.ProjectOne).Set("BC","MemberDetails.A_EARNINGPREFERENCE").Set(null, "MemberDetails.A_TIERCODE")
            }//EarningPreference = BC (Thrifty Member), TierCode = empty string
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
