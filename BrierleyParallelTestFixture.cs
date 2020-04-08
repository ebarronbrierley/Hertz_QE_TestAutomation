using System;
using System.Linq;
using System.Threading;
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
    public class BrierleyParallelTestFixture
    {
        public IStepManager TestStep { get { return GetContext().TestStep; } }
        public IDatabase Database { get { return GetContext().Database; } }
        public ISftp sftpClient { get { return GetContext().sftpClient; } }
        public WebUIDriver Driver { get { return GetContext().Driver; } }

        private TestSuite testSuite;
        private readonly Dictionary<string, ParallelTestContext> PTestContext = new Dictionary<string, ParallelTestContext>();

        [OneTimeSetUp]
        public void TestSuiteSetUp()
        {
            testSuite = TestManager.Instance.CreateTestSuite(TestContext.CurrentContext.Test.ClassName);
        }
        [SetUp]
        public void BeforeTest()
        {
            var Test = TestContext.CurrentContext.Test;
            var categories = (IList)TestContext.CurrentContext.Test.Properties["Category"];
            ParallelTestContext context = new ParallelTestContext();
            var contextId = TestContext.CurrentContext.Test.ID;

            context._TestCase = testSuite.CreateTestCase(TestContext.CurrentContext.Test.Name, categories: categories);
            context.TestStep = context._TestCase;
            context.Database = new OracleDB(EnvironmentManager.Get.OracleConnection);
            context.Driver = new WebUIDriver();
            context.Driver.SetScreenShotPath(EnvironmentManager.Get.TempFilesPath);

            SFTPConfiguration sftpConfig = new SFTPConfiguration()
            {
                Host = EnvironmentManager.Get.SFTPHost,
                Port = Convert.ToInt32(EnvironmentManager.Get.SFTPPort),
                User = EnvironmentManager.Get.SFTPUser,
                Password = EnvironmentManager.Get.SFTPPassword
            };
            context.sftpClient = new RenciSFTP(sftpConfig);

            
            if (IsTestType<WebUITest>(Test.ClassName, Test.MethodName))
            {
                context.Driver.Start();
                context.Driver.Manage().Timeouts().PageLoad = new TimeSpan(0, 5, 0);
                context.Driver.Manage().Timeouts().AsynchronousJavaScript = new TimeSpan(0, 5, 0);
            }
            if (IsTestType<SFTPTest>(Test.ClassName, Test.MethodName))
            {
                context.sftpClient.Connect();
            }

            PTestContext.Add(contextId, context);
        }

        [TearDown]
        public void AfterTest()
        {
            var Test = TestContext.CurrentContext.Test;
            var context = PTestContext[TestContext.CurrentContext.Test.ID];

            context._TestCase.End();

            if (IsTestType<WebUITest>(Test.ClassName, Test.MethodName))
            {
                context.Driver.Stop();
            }
            if (IsTestType<SFTPTest>(Test.ClassName, Test.MethodName))
            {
                context.sftpClient.Dispose();
            }
            context.Database.Dispose();
        }

        [OneTimeTearDown]
        public void TestSuiteCleanUp()
        {
            testSuite.End();
        }
        private bool IsTestType<TestAttr>(string className, string methodName) where TestAttr : Attribute
        {
            Type classType = Type.GetType(className);
            var testType = classType.GetMethod(methodName);
            TestAttr attr = testType.GetCustomAttribute<TestAttr>();
            if (attr == null) return false;
            else return true;
        }

        public ParallelTestContext GetContext()
        {
            var contextId = TestContext.CurrentContext.Test.ID;
            if(PTestContext.ContainsKey(contextId))
            {
                return PTestContext[contextId];
            }
            else
            {
                throw new ThreadStateException($"Test Id [{contextId}] not found in {nameof(PTestContext)}");
            }
        }
    }

    public class ParallelTestContext
    {
        public TestCase _TestCase { get; set; }
        public IStepManager TestStep { get; set; }
        public WebUIDriver Driver { get; set; }
        public ISftp sftpClient { get; set; }
        public IDatabase Database { get; set; }
    }
}
