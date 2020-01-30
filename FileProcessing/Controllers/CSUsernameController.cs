using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hertz.FileProcessing.DataModels;
using Brierley.TestAutomation.Core.Utilities;
using System.Reflection;
using Brierley.TestAutomation.Core.Database;
using Hertz.FileProcessing.Utilities;
using Hertz.Database.DataModels;
using NUnit.Framework;

namespace Hertz.FileProcessing.Controllers
{
    public class CSUsernameController : IDataFeed
    {
        public string FeedName { get { return "HTZ_CUSTOMER_SERVICE_USERNAME"; } }
        public string FileType { get { return this.GetType().Name.Replace("Controller", String.Empty); } }
        public string Filename { get; set; }
        public int RowCount { get { return fileRows.Count; } }
        public IEnumerable<string> RejectTables { get { return new List<string>() { "EXPSTG.NOVA_ERRORS", "BP_EXP.ERR_DF_EXP_PRODUCT" }; } }
        public string StatusQuery { get { return $"SELECT processing_state from {NovaLoadsModel.TableName} where FILE_NAME = '{Filename}'"; } }
        public string TriggerProcedure
        {
            get
            {
                return $@"BEGIN 
                            sla.stg_utils.gen_trigger_file(p_filename => '{Filename}', 
                                p_client_cd => 'HTZLW',
                                p_directory_name => 'HTZ_LW_IN_AUTO');
                        END;";
            }
        }
        public object NovaLoad { get { return novaLoad; } }
        public IEnumerable<IDataFeedRow> Rows { get { return fileRows; } }

        public bool GenerateHeaderRow { get; set; }

        private const string fileNamePrefix = "HTZ_CUSTOMER_SERVICE_USERNAME_";
        private IDatabase database;
        private NovaLoadsModel novaLoad => this.database.QuerySingleRow<NovaLoadsModel>($"SELECT * from {NovaLoadsModel.TableName} where FILE_NAME = '{Filename}'");
        private readonly List<CSUsernameRow> fileRows;

        public CSUsernameController(IDatabase database = null, int createRandomRows = 0, bool generateHeaderRow = true)
        {
            GenerateHeaderRow = generateHeaderRow;
            fileRows = new List<CSUsernameRow>();
            Filename = $"{fileNamePrefix}{DateTime.Now.ToString("yyyyMMdd_hhmmss")}.TXT.dec";
            for (int i = 0; i < createRandomRows; i++)
            {
                CSUsernameRow row = GenerateRandomRow();
                row.Verifications.Add((str, data, rowNum) => str.VerifyInDatabase(data));
                row.SetRowNumber(fileRows.Count + 1); //Rows are not 0 indexed.
                fileRows.Add(row);
            }
            this.database = database;
        }
        public void SetDatabase(IDatabase database)
        {
            this.database = database;
        }
        public void AddRow(object row)
        {
            if (row is CSUsernameRow convRow)
            {
                convRow.SetRowNumber(fileRows.Count + 1);   //File rows are not 0 indexed;
                fileRows.Add(convRow);
            }
            else
            {
                throw new NotImplementedException($"ProdcutController does not implement [{row.GetType().Name}] row types");
            }
        }
        public string GetContent()
        {
            StringBuilder output = new StringBuilder();

            bool firstRow = true;
            foreach (CSUsernameRow csUserRow in fileRows)
            {
                if (GenerateHeaderRow && firstRow)
                {
                    output.Append(csUserRow.RowData.GenerateHeader() + "\r\n");
                    firstRow = false;
                }
                output.Append(csUserRow.RowData.ToString() + "\r\n");
            }
            return output.ToString();
        }
        public object GetFileRow(int rowNum)
        {
            return null;
        }
        public static CSUsernameRow GenerateRandomRow()
        {
            CSUsernameFileModel data = StrongRandom.GenerateRandom<CSUsernameFileModel>();
            CSUsernameRow output = new CSUsernameRow()
            {
                RowData = data
            };
            return output;
        }
        public void VerifyRow(object row)
        {
            if (row is CSUsernameRow csUserRow)
            {
                foreach (var verification in csUserRow.Verifications)
                {
                    verification.Invoke(this, csUserRow.RowData, csUserRow.RowNumber);
                }
            }
            else
            {
                throw new DataFeedTypeException("Attempted to verify non StoreMasterRow row type in StoreMasterController.VerifyRow");
            }
        }
        public void VerifyRows()
        {
            foreach (var row in fileRows)
            {
                VerifyRow(row);
            }
        }
        public bool VerifyInDatabase(CSUsernameFileModel row)
        {
            CSUsernameFileModel csUser = database.QuerySingleRow<CSUsernameFileModel, ModelAttribute>($"select * from {CSAgentModel.TableName} where USERNAME = '{row.New_Username}'", "AlternateName");
            AssertModels.AreEqual(row, csUser);
            return true;
        }
        public bool VerifyError(int rowNum, string errorMessage = null, decimal? errorCode = null)
        {
            //DataFeedError<ProductModel> error = database.QuerySingleRow<DataFeedError<ProductModel>, ModelAttribute>($"select * from {DataFeedErrorTable.Product} where LOAD_ID = {this.novaLoad.LOAD_ID} AND REC_NUM = {rowNum}", "AlternateName");
            //if (!String.IsNullOrEmpty(errorMessage))
            //{
            //    if (errorMessage.Equals(error.ORA_ERR_MESG, StringComparison.OrdinalIgnoreCase))
            //        return true;
            //    else
            //        throw new AssertionException($"Error message {error.ORA_ERR_MESG} does not match expected: {errorMessage}");
            //}
            //if (errorCode.HasValue)
            //{
            //    if (errorCode.Value == error.ORA_ERR_NUMBER)
            //        return true;
            //    else
            //        throw new AssertionException($"Error code {error.ORA_ERR_NUMBER} does not match expected {errorCode}");
            //}
            return false;
        }
    }
    public class CSUsernameRow : IDataFeedRow
    {
        public string Description { get; set; }
        public CSUsernameFileModel RowData;
        public List<Func<CSUsernameController, CSUsernameFileModel, int, bool>> Verifications { get; set; }

        public int RowNumber { get; private set; }

        public object Data
        {
            get { return RowData; }
        }

        public string Delimiter
        {
            get { return RowData.Delimiter; }
            set { RowData.Delimiter = value; }
        }

        public CSUsernameRow(int rowNum)
        {
            Verifications = new List<Func<CSUsernameController, CSUsernameFileModel, int, bool>>();
            this.RowNumber = rowNum;
        }
        public CSUsernameRow()
        {
            Verifications = new List<Func<CSUsernameController, CSUsernameFileModel, int, bool>>();
        }
        public void SetRowData(object data)
        {
            if ((data is CSUsernameFileModel row))
            {
                RowData = row;
            }
            else
            {
                throw new DataFeedTypeException($"Unknown data type was added to {this.GetType().Name}");
            }
        }
        public void SetRowNumber(int rowNum)
        {
            this.RowNumber = rowNum;
        }
        public object GetVerifications()
        {
            return Verifications;
        }
    }
}
