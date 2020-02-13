using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Brierley.TestAutomation.Core.Reporting;
using Brierley.TestAutomation.Core.Database;
using Brierley.TestAutomation.Core.Utilities;
using Brierley.TestAutomation.Core.SFTP;
using Brierley.TestAutomation.Core.WebUI;

namespace Hertz
{
    [TestFixture]
    public class BrierleyTestFixture
    {
        public IStepManager TestStep;
        public IDatabase Database = new OracleDB(EnvironmentManager.Get.OracleConnection);
        public ISftp sftpClient;
        public WebUIDriver Driver;
        private TestSuite testSuite;
        private TestCase testCase;

        [OneTimeSetUp]
        public void TestSuiteSetUp()
        {

            SFTPConfiguration sftpConfig = new SFTPConfiguration()
            {
                Host = EnvironmentManager.Get.SFTPHost,
                Port = Convert.ToInt32(EnvironmentManager.Get.SFTPPort),
                User = EnvironmentManager.Get.SFTPUser,
                Password = EnvironmentManager.Get.SFTPPassword
            };
            sftpClient = new RenciSFTP(sftpConfig);

            testSuite = TestManager.Instance.CreateTestSuite(TestContext.CurrentContext.Test.ClassName);
            Driver = new WebUIDriver();
            Driver.SetScreenShotPath(EnvironmentManager.Get.TempFilesPath);
            testSuite.AddJiraKey(GetJiraKeys(TestContext.CurrentContext.Test.ClassName));
        }
        [SetUp]
        public void BeforeTest()
        {
            var Test = TestContext.CurrentContext.Test;
            var categories = (IList)TestContext.CurrentContext.Test.Properties["Category"];
            testCase = testSuite.CreateTestCase(TestContext.CurrentContext.Test.Name, categories: categories);
            TestStep = testCase;

            if (IsTestType<WebUITest>(Test.ClassName, Test.MethodName))
            {
                Driver.Start();
                Driver.Manage().Timeouts().PageLoad = new TimeSpan(0, 5, 0);
                Driver.Manage().Timeouts().AsynchronousJavaScript = new TimeSpan(0, 5, 0);
            }
            if (IsTestType<SFTPTest>(Test.ClassName, Test.MethodName))
            {
                sftpClient.Connect();
            }

            testCase.AddJiraKey(GetJiraKeys(Test.ClassName, Test.MethodName));
        }
        [TearDown]
        public void AfterTest()
        {
            var Test = TestContext.CurrentContext.Test;
            testCase.End();

            if (IsTestType<WebUITest>(Test.ClassName, Test.MethodName))
            {
                Driver.Stop();
            }
            if (IsTestType<SFTPTest>(Test.ClassName, Test.MethodName))
            {
                sftpClient.Dispose();
            }
        }
        [OneTimeTearDown]
        public void TestSuiteCleanUp()
        {
            testSuite.End();
            Database.Dispose();
        }
        private bool IsTestType<TestAttr>(string className, string methodName) where TestAttr : Attribute
        {
            Type classType = Type.GetType(className);
            var testType = classType.GetMethod(methodName);
            TestAttr attr = testType.GetCustomAttribute<TestAttr>();
            if (attr == null) return false;
            else return true;
        }
        private IEnumerable<string> GetJiraKeys(string className, string methodName = null)
        {
            Type classType = Type.GetType(className);
            IEnumerable<JIRAKey> attrs;

            if (String.IsNullOrEmpty(methodName))
            {
                attrs = classType.GetCustomAttributes<JIRAKey>();
            }
            else
            {
                var testType = classType.GetMethod(methodName);
                attrs = testType.GetCustomAttributes<JIRAKey>();
            }
            return attrs.SelectMany(x => x.Keys);
        }
    }
}
