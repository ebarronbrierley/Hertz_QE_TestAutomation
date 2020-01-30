using Brierley.TestAutomation.Core.Database;
using Brierley.TestAutomation.Core.SFTP;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hertz.FileProcessing.Utilities
{
    public interface IDataFeed : ISftpFile
    {
        string FileType { get; }
        string FeedName { get; }
        int RowCount { get; }
        IEnumerable<string> RejectTables { get; }
        string StatusQuery { get; }
        string TriggerProcedure { get; }
        void AddRow(object row);
        object GetFileRow(int rowNum);
        object NovaLoad { get; }
        void VerifyRow(object row);
        void VerifyRows();
        void SetDatabase(IDatabase databse);
        IEnumerable<IDataFeedRow> Rows { get; }
        bool GenerateHeaderRow { get; set; }
    }

   
}
