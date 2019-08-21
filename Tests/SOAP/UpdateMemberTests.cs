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
    public class UpdateMember : BrierleyTestFixture
    {
        [Category("Api_Smoke")]
        [Category("Api_Positive")]
        [Category("AddMember")]
        [Category("AddMember_Positive")]
        [TestCaseSource("PositiveScenarios")]
        public void UpdateMember_Positive(string name, MemberStyle memberStyle, long memberStatus, string earningPreference, string tierCode, bool hasTransactions)
        {
            BPTest.Start<TestStep>("Get Existing Member from the database", "Existing member should be found");
            Member dbMember = Member.GetFromDB(Database, memberStyle, Member.MemberStatus.Active, "N1", "RG", true);
            Assert.IsNotNull(dbMember, "Member could not be retrieved from DB");
            BPTest.Pass<TestStep>("Existing member was found", dbMember.ReportDetail());

           
        }

        static object[] PositiveScenarios =
        {
            new object[]
            {
                "Active GPR Member with RG Tier and existing transactions",
                MemberStyle.ProjectOne,
                Member.MemberStatus.Active,
                "N1",
                "RG",
                true
            }//"Active GPR Member with RG Tier and existing transactions",
        };
    }
}
