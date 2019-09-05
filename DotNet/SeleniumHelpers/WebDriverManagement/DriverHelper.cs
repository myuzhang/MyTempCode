using System;
using System.Drawing;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.Events;

namespace WebDriverManagement
{
    public static class DriverHelper
    {
        public static IWebDriver GetDriver(string browserType = null)
        {
            IWebDriver webDriver;
            switch (browserType)
            {
                case "firefox":
                    var firefoxOptions = new FirefoxOptions();
                    webDriver.Manage().Window.Maximize();
                    break;
                default: // chrome
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArgument("--start-maximized");
                    webDriver = new ChromeDriver();
                    break;
            }
            return new EventFiringWebDriver(webDriver);
        }
    }
}
