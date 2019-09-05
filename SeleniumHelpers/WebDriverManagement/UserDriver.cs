using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace WebDriverManagement
{
    public class UserDriver
    {
        public UserDriver(string user, IWebDriver driver)
        {
            User = user;
            Driver = driver;
            TestTrace = new List<string>();
        }

        public void AddTestName(string testName) => TestTrace.Add(testName);

        public string GetTestName() => string.Join(",", TestTrace.ToArray());

        public IWebDriver Driver { get; }
        public string User { get; }
        public bool Loaned { get; set; } = true;
        public IList<string> TestTrace { get; }
    }
}
