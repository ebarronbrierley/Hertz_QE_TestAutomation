using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Brierley.TestAutomation.Core.Utilities;

namespace Hertz
{
    public class RemotePath
    {
        public static string LWInboundFeedDecrypted { get { return NetworkPath.Combine(EnvironmentManager.Get.LWFileProcessingPath, "/in/auto"); } }
        public static string LWInboundFeedEncrypted { get { return NetworkPath.Combine(EnvironmentManager.Get.LWFileProcessingPath, "/in/"); } }
        public static string LWInboundCompleted { get { return NetworkPath.Combine(EnvironmentManager.Get.LWFileProcessingPath, "/in/completed/"); } }
        public static string LWOutbound { get { return NetworkPath.Combine(EnvironmentManager.Get.LWFileProcessingPath, "/out/exp/"); } }

        public static string CDWInboundFeedDecrypted {  get { return NetworkPath.Combine(EnvironmentManager.Get.CDWFileProcessingPath, "/auto/"); } }
    }

    public class NetworkPath
    {
        public static string Combine(params string[] paths)
        {
            StringBuilder output = new StringBuilder();
            if (paths != null && paths.Length > 0)
            {
                for (int i = 0; i < paths.Length; i++)
                {
                    if (i == 0) output.Append(paths[i].TrimEnd('/'));
                    else
                    {
                        output.Append($"/{paths[i].Trim('/')}/");
                    }
                }
            }
            return output.ToString();
        }
    }
}
