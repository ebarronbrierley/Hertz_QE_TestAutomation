using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Brierley.TestAutomation.Core.Utilities;
using Brierley.TestAutomation.Core.SFTP;
using Hertz.FileProcessing.Utilities;

namespace Hertz.FileProcessing.DataModels
{
    public class CSUsernameFileModel : DataFeedSerializer
    {
        [Randomizer(DataType = RandomDataType.Username, Max = 100)]
        [Model("USERNAME", ReportOption.Print)]
        public string New_Username { get; set; }

        [Randomizer(new object[]{ 11,272,277,484 }, DataType = RandomDataType.Custom)]
        [Model("ROLEID", ReportOption.Print)]
        public decimal? Role_Id { get; set; }

        [Randomizer(Min = 1, Max = 100, DataType = RandomDataType.WholeDecimal)]
        [Model("GROUPID", ReportOption.Print)]
        public decimal? Group_Id { get; set; }

        [Randomizer(Max = 255, DataType = RandomDataType.FirstName)]
        [Model("FIRSTNAME")]
        public string First_Name { get; set; }

        [Randomizer(Max = 255, DataType = RandomDataType.LastName)]
        [Model("LASTNAME")]
        public string Last_Name { get; set; }

        [Randomizer(Max = 255, DataType = RandomDataType.Email)]
        [Model("EMAILADDRESS", ReportOption.Print)]
        public string Email_Address { get; set; }

        [Randomizer(Max = 20, DataType = RandomDataType.PhoneNumber)]
        [Model("PHONENUMBER")]
        public string Phone_Number { get; set; }

        [Randomizer(Max = 10, DataType = RandomDataType.Numeric)]
        [Model("EXTENSION")]
        public string Extension { get; set; }

        [Randomizer(new object[] { 1, 1, 1, 1 }, DataType = RandomDataType.Custom)]
        [Model("STATUS")]
        public long Status { get; set; }
    }
}
