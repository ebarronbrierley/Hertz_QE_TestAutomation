using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hertz.FileProcessing.Utilities
{
    public class DataFeedTypeException : Exception
    {
        public DataFeedTypeException(string message) : base(message)
        {

        }
    }
}
