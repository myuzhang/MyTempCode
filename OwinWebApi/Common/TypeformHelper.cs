using System;
using System.Configuration;

namespace Common
{
    public class TypeformHelper
    {
        private string _url;

        public string Url => _url;

        public TypeformHelper(string key, string uid)
        {
            _url = ConfigurationManager.AppSettings["questionApi"];
            _url = _url.Replace("{uid}", uid).Replace("{key}", key);
        }

        public TypeformHelper HasCompleted(bool isCompleted = true)
        {
            if (isCompleted) _url += "&completed=true";
            else _url += "&completed=false";

            return this;
        }

        public TypeformHelper Since(int daysAgo)
        {
            var when = DateTime.Today.AddDays(0 - daysAgo);
            Int32 unixTimestamp = (Int32)(when.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            _url += $"&since={unixTimestamp}";

            return this;
        }
    }
}
