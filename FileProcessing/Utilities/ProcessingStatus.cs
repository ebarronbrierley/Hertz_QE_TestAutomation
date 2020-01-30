using Brierley.TestAutomation.Core.Database;
using Hertz.FileProcessing.DataModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Reflection;

namespace Hertz.FileProcessing.Utilities
{
    public class ProcessingStatus
    {
        private const int sleepInterval = 10000;

        public static bool Verify(IDatabase db, string checkStateName, params IDataFeed[] files)
        {
            List<KeyValuePair<string,string>> currStatuses = new List<KeyValuePair<string,string>>();
            TimeSpan timeOutPeriod = new TimeSpan(0, 20, 0);
            Stopwatch watchDog = new Stopwatch();
            TimeSpan currTime;
            
            bool complete = false;
            watchDog.Start();

            while (!complete)
            {
                bool queryResult = true;
                currStatuses = new List<KeyValuePair<string, string>>();
                foreach (IDataFeed file in files)
                {
                    var result = db.QuerySingleColumn<string>(file.StatusQuery);
                    currStatuses.Add(new KeyValuePair<string, string>(file.Filename,result));
                    queryResult &= result.Equals(checkStateName, StringComparison.OrdinalIgnoreCase);
                }
                if (queryResult)
                {
                    return true;
                }
                else
                {
                    Thread.Sleep(sleepInterval);
                }
                currTime = watchDog.Elapsed;
                if(currTime.Ticks > timeOutPeriod.Ticks)
                {
                    throw new FileProcessingStatusException($"Timeout waiting for {checkStateName} status", currStatuses);
                }
            }
            
            return false;
        }
        public static bool HasProcessedToday(IDatabase Database, params string[] feedNames)
        {
            bool result = true;
            foreach (string feedName in feedNames)
            {
                var state = Database.QuerySingleColumn<string>($"select nl.processing_state_date from bp_exp.nova_loads nl where nl.feed_name = '{feedName}' and trunc(nl.processing_state_date)= trunc(sysdate)");
                if (String.IsNullOrEmpty(state))
                    result = false;
            }
            return result;
        }
    }
    public class FileProcessingStatusException : Exception
    {
        public IEnumerable<KeyValuePair<string, string>> FinalStatuses { get; private set; }
        public FileProcessingStatusException(string message) :base(message)
        {
            FinalStatuses = new List<KeyValuePair<string, string>>();
        }
        public FileProcessingStatusException(string message, List<KeyValuePair<string, string>> statuses) : base(message)
        {
            FinalStatuses = statuses;
        }
    }
}
