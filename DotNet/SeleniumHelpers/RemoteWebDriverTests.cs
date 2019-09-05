using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.Extensions;

[assembly: Parallelize(Workers = 0, Scope = ExecutionScope.MethodLevel)]

namespace TestWeb
{
    [TestClass]
    public class UnitTest1
    {
        private Uri _hubUrl = new Uri("http://localhost:4444/wd/hub");

        [TestMethod]
        public void TestMethod1()
        {
            var driver = GetRemoteWebDriver(GetChromeOptions());
            driver.Navigate().GoToUrl("https://www.google.com/");
            Thread.Sleep(TimeSpan.FromSeconds(10));
            driver.TakeScreenshot().SaveAsFile("c:\\Temp\\test1.png");

            driver.Quit();
        }

        [TestMethod]
        public void TestMethod2()
        {
            var driver = GetRemoteWebDriver(GetChromeOptions());
            driver.Navigate().GoToUrl("https://www.abc.net.au");
            Thread.Sleep(TimeSpan.FromSeconds(10));
            driver.TakeScreenshot().SaveAsFile("c:\\Temp\\test2.png");

            driver.Quit();
        }

        [TestMethod]
        public void TestMethod3()
        {
            var driver = GetRemoteWebDriver(GetChromeOptions());
            driver.Navigate().GoToUrl("https://www.sbs.com.au/news/");
            Thread.Sleep(TimeSpan.FromSeconds(10));
            driver.TakeScreenshot().SaveAsFile("c:\\Temp\\test3.png");

            driver.Quit();
        }

        private ChromeOptions GetChromeOptions()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--ignore-certificate-errors");
            chromeOptions.AddArgument("--allow-running-insecure-content");
            chromeOptions.AddArgument("--disable-extensions");
            chromeOptions.AddArgument("--start-maximized");

            return chromeOptions;
        }

        private RemoteWebDriver GetRemoteWebDriver(ChromeOptions chromeOptions) =>
            new RemoteWebDriver(_hubUrl, chromeOptions);
    }
}
