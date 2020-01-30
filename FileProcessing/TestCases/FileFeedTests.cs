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
using Brierley.TestAutomation.Core.WebUI;
using Hertz.FileProcessing.Utilities;
using Hertz.FileProcessing.TestData;

namespace Hertz.FileProcessing.TestCases
{
    [TestFixture]
    public class FileFeedTests : BrierleyTestFixture
    {
        [SetUp]
        public void SetFileProcessingDirectory()
        {
            sftpClient.DefaultRemotePath = RemotePath.LWInboundFeedDecrypted;
        }

        [Test]
        [SFTPTest]
        [TestCaseSource(typeof(CSUsernameFileFeedTestData), "Scenarios")]
        public void FileFeedTest(IDataFeed fileFeed)
        {
            try
            {
                fileFeed.SetDatabase(Database);

                TestStep.Start($"Upload {fileFeed.FileType} file", $"{fileFeed.FileType} file should be uploaded to {sftpClient.DefaultRemotePath}");
                sftpClient.UploadFileToRemote(fileFeed);
                TestStep.AddAttachment(new Attachment(fileFeed.FileType, fileFeed.GetContent(), Attachment.Type.Text));
                TestStep.Pass($"{fileFeed.FileType} file uploaded to {sftpClient.DefaultRemotePath}");

                TestStep.Start($"Create trigger file for {fileFeed.FileType} file", "Trigger file should be created");
                Database.ExecuteNonQuery(fileFeed.TriggerProcedure);
                TestStep.AddAttachment(new Attachment("TriggerProcedure", fileFeed.TriggerProcedure, Attachment.Type.Text));
                TestStep.Pass("Trigger file created");

                TestStep.Start($"Verify {fileFeed.FileType} file completes processing", "File status should be COMPLETE");
                Assert.IsTrue(ProcessingStatus.Verify(Database, "COMPLETE", fileFeed));
                TestStep.Pass("Proccessing Complete");

                foreach (IDataFeedRow row in fileFeed.Rows)
                {
                    TestStep.Start($"Verify Row Number {row.RowNumber} Scenario {row.Description}", "File row verification should pass");

                    try
                    {
                        fileFeed.VerifyRow(row);
                        TestStep.Pass("Files row verified successfully", row.Data.ToString());
                    }
                    catch (FileFeedVerificationException ex)
                    {
                        TestStep.Fail(ex.Message, ex.FailureReasons);
                    }
                }
            }
            
            catch (DatabaseException ex)
            {
                TestStep.Fail(ex.Message);
                Assert.Fail();
            }
            catch (AssertionException ex)
            {
                TestStep.Fail(ex.Message);
                Assert.Fail();
            }
            catch (AssertModelEqualityException ex)
            {
                TestStep.Fail(ex.Message, ex.ComparisonFailures);
                Assert.Fail();
            }
            catch (SFTPException ex)
            {
                TestStep.Fail(ex.Message, ex.ErrorMessage);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                TestStep.Abort(ex.Message);
                Assert.Fail(ex.Message);
            }
        }
    }
}
