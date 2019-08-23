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
    public class GetMembers : BrierleyTestFixture
    {
        [Category("Api_Smoke")]
        [Category("Api_Positive")]
        [Category("GetMember")]
        [Category("GetMember_Positive")]
        [Test]
        public void GetMembers_Positive()
        {
            MemberStyle memberStyle = MemberStyle.PreProjectOne;
            string searchType = "CardID";
            string searchValue = String.Empty;
            try
            {
                BPTest.Start<TestStep>("Get Existing Member from the database", "Existing member should be found");
                Member dbMember = Member.GetFromDB(Database, Member.MemberStatus.Active, memberStyle);
                Assert.IsNotNull(dbMember, "Member could not be retrieved from DB");
                BPTest.Pass<TestStep>("Existing member was found", dbMember.ReportDetail());

                searchValue = dbMember.VirtualCards.First().LOYALTYIDNUMBER;

                BPTest.Start<TestStep>($"Make GetMembers Call by SearchType = {searchType}, SearchValue = {searchValue}", "GetMembers should return one member");
                IEnumerable<Member> getMembersOut = Member.GetMembers(MemberStyle.PreProjectOne, new[] { searchType }, new[] { searchValue }, null, null, String.Empty);
                Assert.AreEqual(1, getMembersOut.Count());
                BPTest.Pass<TestStep>("GetMembers returned one member", getMembersOut.ReportDetail());

                Member getMember = getMembersOut.First();

                BPTest.Start<TestStep>($"Verify GetMembers returned member matches DB Member", "API member should match existing member found from DB");
                AssertModels.AreEqualOnly(dbMember, getMember, Member.BaseVerify);
                BPTest.Pass<TestStep>("API member matches existing member found from DB");

                BPTest.Start<TestStep>($"Verify MemberDetails in {MemberDetails.TableName} table", "Database member details should match API member");
                AssertModels.AreEqualWithAttribute(dbMember.GetMemberDetails(memberStyle).First(), getMember.GetMemberDetails(memberStyle).First());
                BPTest.Pass<TestStep>("API Member details matches member details in database");

                BPTest.Start<TestStep>($"Verify MemberPreferences in {MemberPreferences.TableName} table", "Database member preferences should match API member");
                AssertModels.AreEqualWithAttribute(dbMember.GetMemberPreferences(memberStyle).First(), getMember.GetMemberPreferences(memberStyle).First());
                BPTest.Pass<TestStep>("API Member preferences created matches member preferences in database");
            }
            catch (LWServiceException ex)
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
        [Category("GetMembers")]
        [Category("GetMembers_Negative")]
        [TestCaseSource("NegativeScenarios")]
        public void GetMembers_Negative(string name, string[] searchTypes, string[] searchValues, int? startIdx, int? batchSize, bool validSearchValue, int errorCode, string errorMessage)
        {
            MemberStyle memberStyle = MemberStyle.PreProjectOne;

            try
            {
                BPTest.Start<TestStep>("Get Existing Member from the database", "Existing member should be found");
                Member dbMember = Member.GetFromDB(Database, Member.MemberStatus.Active, memberStyle);
                Assert.IsNotNull(dbMember, "Member could not be retrieved from DB");
                BPTest.Pass<TestStep>("Existing member was found", dbMember.ReportDetail());

                if(validSearchValue)
                    searchValues = new string[] { dbMember.VirtualCards.First().LOYALTYIDNUMBER };

                BPTest.Start<TestStep>($"Make GetMembers Call with {name}", 
                                       $"GetMembers call should throw exception with error code = {errorCode}");
                LWServiceException exception = Assert.Throws<LWServiceException>(() => Member.GetMembers(MemberStyle.PreProjectOne, searchTypes, searchValues, startIdx, batchSize, String.Empty), 
                                                                                 "Excepted LWServiceException, exception was not thrown.");
                Assert.AreEqual(errorCode, exception.ErrorCode, $"Expected Error Code: {errorCode}");
                Assert.IsTrue(exception.Message.Contains(errorMessage), $"Expected Error Message to contain: {errorMessage}");
                BPTest.Pass<TestStep>("GetMembers call threw expected exception", exception.ReportDetail());

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
        public static object[] NegativeScenarios =
        {
            new object[]
            {
                "SearchType = empty string, SearchValue = empty string",
                new []{ "" }, new []{ "" }, null, null, true,
                3317,
                "No search type provided for member search."
            },//SearchType = empty string, SearchValue = empty string
            new object[]
            {
                "SearchType = 'NOTREAL', SearchValue = valid value from DB",
                new []{ "NOTREAL" }, new []{ "" }, null, null, true,
                3375,
                "Invalid search type NOTREAL provided. Valid search values are: MemberID, CardID, EmailAddress, PhoneNumber, AlternateID, LastName, Username, PostalCode."
            },//SearchType = 'NOTREAL', SearchValue = valid value from DB
            new object[]
            {
                "SearchType = 'AlternateID', SearchValue = empty string",
                new []{ "AlternateID" }, new []{ "" }, null, null, false,
                3320,
                "No alternate id provided for member search."
            },//SearchType = 'AlternateID', SearchValue = empty string
            new object[]
            {
                "SearchType = 'MemberID', SearchValue = empty string",
                new []{ "MemberID" }, new []{ "" }, null, null, false,
                3301,
                "No member id provided for member search."
            },//SearchType = 'MemberID', SearchValue = empty string
            new object[]
            {
                "SearchType = 'CardID', SearchValue = empty string",
                new []{ "CardID" }, new []{ "" }, null, null, false,
                3304,
                "No card id provided for member search."
            },//SearchType = 'CardID', SearchValue = empty string
            new object[]
            {
                "SearchType = 'EmailAddress', SearchValue = empty string",
                new []{ "EmailAddress" }, new []{ "" }, null, null, false,
                3318,
                "No email address provided for member search."
            },//SearchType = 'EmailAddress', SearchValue = empty string
            new object[]
            {
                "SearchType = 'PhoneNumber', SearchValue = empty string",
                new []{ "PhoneNumber" }, new []{ "" }, null, null, false,
                3319,
                "No phone number provided for member search."
            },//SearchType = 'PhoneNumber', SearchValue = empty string
            new object[]
            {
                "SearchType = 'LastName', SearchValue = empty string",
                new []{ "LastName" }, new []{ "" }, null, null, false,
                3359,
                "Empty parameters provided for member search by name."
            },//SearchType = 'LastName', SearchValue = empty string
            new object[]
            {
                "SearchType = 'Username', SearchValue = empty string",
                new []{ "Username" }, new []{ "" }, null, null, false,
                3321,
                "No username provided for member search."
            },//SearchType = 'Username', SearchValue = empty string
            new object[]
            {
                "SearchType = 'PostalCode', SearchValue = empty string",
                new []{ "PostalCode" }, new []{ "" }, null, null, false,
                3322,
                "No postal code provided for member search."
            },//SearchType = 'PostalCode', SearchValue = empty string
            new object[]
            {
                "SearchType = 'AlternateID', SearchValue = 345678",
                new []{ "AlternateID" }, new []{ "345678" }, null, null, false,
                3323,
                "No members found."
            },//SearchType = 'AlternateID', SearchValue = 345678
            new object[]
            {
                "SearchType = null, SearchValue = null",
                null, null, null, null, false,
                2003,
                "MemberSearchType of GetMembersIn is a required property."
            },//SearchType = null, SearchValue = null
            new object[]
            {
                "SearchType = null, SearchValue = valid value from DB",
                null, null, null, null, true,
                2003,
                "MemberSearchType of GetMembersIn is a required property."
            },//SearchType = null, SearchValue = valid value from DB
            new object[]
            {
                "SearchType = empty string, SearchValue = null",
                new string[]{ "" }, null, null, null, false,
                2003,
                "SearchValue of GetMembersIn is a required property.  Please provide a valid value."
            }//SearchType = empty string, SearchValue = null
        };
    }
}
