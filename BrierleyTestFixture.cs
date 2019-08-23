using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Brierley.TestAutomation.Core.Reporting;
using Brierley.TestAutomation.Core.Database;
using Brierley.TestAutomation.Core.Utilities;

namespace HertzNetFramework
{
    [TestFixture]
    public class BrierleyTestFixture
    {
        public TestManager BPTest = TestManager.Instance;
        public OracleDB Database = null;

        [OneTimeSetUp]
        public void BeforeSuite()
        {
            Database = new OracleDB(EnvironmentManager.Get.OracleConnection);
            BPTest.Start<TestSuite>(TestContext.CurrentContext.Test.ClassName);
        }
        [SetUp]
        public void BeforeTest()
        {
            BPTest.Start<TestCase>(TestContext.CurrentContext.Test.Name, groupName: TestContext.CurrentContext.Test.ClassName);
        }

        [TearDown]
        public void AfterTest()
        {
            BPTest.End<TestCase>();
        }
        [OneTimeTearDown]
        public void AfterSuite()
        {
            Database.Dispose();
        }
    }
}
