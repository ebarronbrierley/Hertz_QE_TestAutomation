using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hertz.FileProcessing.Utilities
{
    public class FileFeedVerificationException : Exception
    {
        public string[] FailureReasons { get; set; }
        public FileFeedVerificationException(string message, params string[] failureReasons):base(message)
        {
            FailureReasons = failureReasons;
        }
    }
}
