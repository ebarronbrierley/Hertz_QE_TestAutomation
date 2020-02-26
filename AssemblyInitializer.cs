using NUnit.Framework;
using Brierley.TestAutomation.Core.Reporting;
using Brierley.TestAutomation.Core.Utilities;

[SetUpFixture]
public class AssemblyIntializer
{
    TestManager manager = TestManager.Instance;
    [OneTimeSetUp]
    public void AssemblyInitialize()
    {
      //  manager.EnableRestReporting("http://CY2CAUT01:5000");
        manager.IntializeExecution(EnvironmentManager.Get.ExecutionName,
                                    EnvironmentManager.Get.EnvironmentName,
                                    EnvironmentManager.Get.ApplicationVersion,
                                    EnvironmentManager.Get.FrameworkVersion,
                                    3);
        manager.SetReportErrorLogPath(EnvironmentManager.Get.ErrorLogPath);
        manager.SetReportDetails(EnvironmentManager.Get.ReportPath, () => EnvironmentManager.Get.ReportName);
    }
    [OneTimeTearDown]
    public void AssemblyTearDown()
    {
        manager.EndExecution();
        manager.GenerateHTMLReport();
    }
}

