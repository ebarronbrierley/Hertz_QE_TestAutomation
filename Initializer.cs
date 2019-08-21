using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Brierley.TestAutomation.Core.Reporting;
using Brierley.TestAutomation.Core.Utilities;
using System.IO;
using System.Diagnostics;
using System.Linq;

public class dbInfo
{
    public static readonly string dbUser = "bp_htz";
}
[SetUpFixture]
public class AssemblyInit
{
    [OneTimeSetUp]
    public void AssemblyInitialize()
    {
        TestManager.Instance.Version = EnvironmentManager.Get.ApplicationVersion;
        TestManager.Instance.FrameworkVersion = EnvironmentManager.Get.FrameworkVersion;
        TestManager.Instance.Environment = EnvironmentManager.Get.EnvironmentName;
        TestManager.Instance.Name = EnvironmentManager.Get.ExecutionName;
        TestManager.Instance.ClientId = "3";
    }

    [OneTimeTearDown]
    public void AssemblyTearDown()
    {
        try
        {
            TestManager.Instance.End<TestSuite>();

            File.WriteAllText(EnvironmentManager.Get.ReportPath + EnvironmentManager.Get.ReportName, TestManager.Instance.GenerateReport());
            if (EnvironmentManager.Get.isDBReportingRequired)
                TestManager.Instance.PostResultsToServer();

            Directory.Delete(EnvironmentManager.Get.TempFilesPath); //Delete the temporary files directory to clean up workspace

            //Killing IE, chrome and gecko driver server process if any present during Assembly cleanup
            Process.GetProcessesByName("IEDriverServer").ToList().ForEach(p => p.Kill());
            Process.GetProcessesByName("chromedriver").ToList().ForEach(p => p.Kill());
            Process.GetProcessesByName("geckodriver").ToList().ForEach(p => p.Kill());
        }
        catch (Exception ex)
        {
            //Killing IE, chrome and gecko driver server process if any present during Assembly cleanup
            Console.WriteLine(ex.Message);

            Process.GetProcessesByName("IEDriverServer").ToList().ForEach(p => p.Kill());
            Process.GetProcessesByName("chromedriver").ToList().ForEach(p => p.Kill());
            Process.GetProcessesByName("geckodriver").ToList().ForEach(p => p.Kill());
        }
    }
}

