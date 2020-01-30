using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Brierley.TestAutomation.Core.Reporting;
using Brierley.TestAutomation.Core.Database;
using Brierley.TestAutomation.Core.Utilities;
using Brierley.TestAutomation.Core.SFTP;
using Hertz.FileProcessing.Utilities;
using Hertz.FileProcessing.Controllers;

namespace Hertz.FileProcessing.TestData
{
    public class CSUsernameFileTestData
    {
        public static IEnumerable Scenarios
        {
            get
            {
                IDataFeed csUserFile = new CSUsernameController();

                CSUsernameRow row =  CSUsernameController.GenerateRandomRow();
                row.Description = "Positive - Random row valid fields";
                row.Verifications.Add((str, data, rowNum) => str.VerifyInDatabase(data));
                csUserFile.AddRow(row);

                yield return new TestCaseData(csUserFile).SetName("Customer Service Username - Positive: All fields valid");
            }
        }
    }
}
