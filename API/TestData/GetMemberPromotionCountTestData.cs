﻿using System;
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

namespace Hertz.API.TestData
{
    public class GetMemberPromotionCountTestData
    {
        public static IEnumerable PositiveScenarios
        {
            get
            {
                yield return new TestCaseData();
            }
        }

        public static IEnumerable NegativeScenarios
        {
            get
            {
                int errorCode = 9998;
                string errorMessage = "Unauthorized access to operation GetMemberPromotionsCount";
                yield return new TestCaseData(errorCode, errorMessage)
                    .SetName($"Get Member Promotion Count Negative - Error Code:[{errorCode}]");
            }
        }
    }
}
