using System;
using System.Collections.Generic;

namespace GarminIntegration
{
    public class TimeHelpers
    {
        public static int GetUnixTimestampFromUtc(DateTime utcTime) =>
            (int)utcTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

        public static int GetUnixTimestampFromLocal(DateTime localTime) =>
            (int)localTime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

        public static Dictionary<string, string> GetExtraDataFromUtc(DateTime from, DateTime to) => new Dictionary<string, string>
        {
            {"summaryStartTimeInSeconds", GetUnixTimestampFromUtc(@from).ToString()},
            {"summaryEndTimeInSeconds", GetUnixTimestampFromUtc(to).ToString()}
        };

        public static Dictionary<string, string> GetExtraDataFromLocal(DateTime from, DateTime to) => new Dictionary<string, string>
        {
            {"summaryStartTimeInSeconds", GetUnixTimestampFromLocal(@from).ToString()},
            {"summaryEndTimeInSeconds", GetUnixTimestampFromLocal(to).ToString()}
        };
    }
}