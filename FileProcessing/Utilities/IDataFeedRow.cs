using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hertz.FileProcessing.Utilities
{
    public interface IDataFeedRow
    {
        string Description { get; set; }
        string ToString();
        int RowNumber { get; }
        object Data { get; }
        string Delimiter { get; }
        void SetRowData(object data);
        void SetRowNumber(int rowNum);
        object GetVerifications();
    }

}
