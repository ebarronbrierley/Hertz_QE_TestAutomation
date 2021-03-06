﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brierley.TestAutomation.Core.Utilities;

namespace Hertz.API.Utilities
{
    public class LWServiceException : Exception
    {
        [ModelAttribute("ErrorCode", ReportOption.Print)]
        public int ErrorCode { get; private set; }
        [ModelAttribute("ErrorMessage", ReportOption.Print)]
        public string ErrorMessage { get; private set; }

        public LWServiceException(string message, int errorCode)
            : base(message)
        {
            this.ErrorCode = errorCode;
            this.ErrorMessage = message;
        }
    }
}
