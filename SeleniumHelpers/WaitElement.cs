using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using AutomationFramework.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace AutomationFramework.Extensions
{
    /// <summary>
    ///     Extension methods for the Selenium IWebDriver
    /// </summary>
    public static class Selenium
    {
        /// <summary>
        ///     Angular Javascript for determining the page load script
        /// </summary>
        private const string AngularWaitForScript =
                "return (document.readyState == 'complete') && (window.angular !== undefined) && (angular.element(document).injector() !== undefined) && (angular.element(document).injector().get('$http').pendingRequests.length === 0)"
            ;

        /// <summary>
        ///     Wait for extension to wait for the page load for WaitDriverInSecond seconds with the above script
        /// </summary>
        /// <param name="driver">The web driver</param>
        private static void WaitFor(IWebDriver driver)
        {
            try
            {
                var waitForDocumentReady =
                    new WebDriverWait(driver, TimeSpan.FromSeconds(TestRunHelper.Settings.WaitDriverInSecond));
                waitForDocumentReady.Until(wdriver =>
                {
                    if (!(driver is IJavaScriptExecutor wjdriver))
                        throw new ApplicationException("Unable to cast driver to the required type");

                    return Boolean.Parse(wjdriver.ExecuteScript(AngularWaitForScript).ToString());
                });
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Step $$ - WARNING. Waiting page load meets an exception - {e.Message}");
                throw;
            }
        }

        /// <summary>
        /// Wait for the element exist
        /// </summary>
        /// <param name="driver">The web driver</param>
        /// <param name="locator">The web locator</param>
        /// <returns>The web element if it is existing</returns>
        public static IWebElement WaitForElement(this IWebDriver driver, By locator) =>
            driver.GetWebDriverWait().Until(ExpectedConditions.ElementExists(locator));

        public static IList<IWebElement> WaitForElements(this IWebDriver driver, By locator) => 
            driver.IsWaitingElementsMatchCount(locator) ? driver.FindElements(locator) : null;

        /// <summary>
        /// Wait for the element visible
        /// </summary>
        /// <param name="driver">The web driver</param>
        /// <param name="locator">The web locator</param>
        /// <returns>The web element if it is visible</returns>
        public static IWebElement WaitForElementVisible(this IWebDriver driver, By locator) =>
            driver.GetWebDriverWait().Until(ExpectedConditions.ElementIsVisible(locator));


        public static IWebElement WaitForElementVisible(this IWebElement element)
        {
            var wait = element.GetWebDriverWait();
            return wait.Until(e => element.Displayed ? element : null);
        }

        public static string WaitForElementText(this IWebElement element)
        {
            var wait = element.GetWebDriverWait();
            return wait.Until(e => element.Displayed && !string.IsNullOrWhiteSpace(element.Text) ? element.Text : null);
        }

        /// <summary>
        /// Wait for the element clickable
        /// </summary>
        /// <param name="driver">The web driver</param>
        /// <param name="locator">The web locator</param>
        /// <returns>The web element if it is clickable</returns>
        public static IWebElement WaitForElementClickable(this IWebDriver driver, By locator) =>
            driver.GetWebDriverWait().Until(ExpectedConditions.ElementToBeClickable(locator));

        public static IWebElement WaitForElementClickable(this IWebElement element)
        {
            var wait = element.GetWebDriverWait();
            return wait.Until(e => element.Displayed && element.Enabled ? element : null);
        }

        /// <summary>
        /// Wait for the number of elements appearing matching the given count.
        /// If the count is not given, wait for any number of elements appearing.
        /// </summary>
        /// <param name="driver">The web driver</param>
        /// <param name="locator">The web locator</param>
        /// <param name="count">The number of element.</param>
        /// <param name="timeoutInSeconds">The timeout in seconds</param>
        /// <returns>True if the number of elements appear, vice vesa</returns>
        public static bool IsWaitingElementsMatchCount(
            this IWebDriver driver,
            By locator,
            int? count = null,
            int? timeoutInSeconds = null)
        {
            var wait = driver.GetWebDriverWait();

            try
            {
                return count == null
                    ? wait.Until(d => d.FindElements(locator).Count != 0)
                    : wait.Until(d => d.FindElements(locator).Count == count);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsAnyWaitingElementsAppear(
            this IWebDriver driver,
            params By[] locators)
        {
            if (locators == null || locators.Length == 0)
                return false;

            try
            {
                return driver.GetWebDriverWait().Until(d => locators.Any(l => d.FindElements(l).Count != 0));
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool WaitUntil(this IWebDriver driver, Func<bool> condition, int? timeoutInSeconds = null)
        {
            var wait = driver.GetWebDriverWait(timeoutInSeconds);

            try
            {
                return wait.Until(d => condition());
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Wait until the page has loaded, determined by the angularWaitForScript javascript
        /// </summary>
        /// <param name="driver">Selenium Driver</param>
        public static void WaitUntilPageIsReady(this IWebDriver driver)
        {
            WaitFor(driver);
        }

        /// <summary>
        ///     Navigate to a page and wait until the page has loaded, determined by the angularWaitForScript javascript
        /// </summary>
        /// <param name="driver">Selenium Driver</param>
        /// <param name="pageUrl">Page to navigate too</param>
        public static void NavigateToPageAndWaitUntilPageIsReady(this IWebDriver driver, string pageUrl)
        {
            driver.Navigate().GoToUrl(pageUrl);

            WaitFor(driver);
        }

        public static string LogBrowserScreenshot(this IWebDriver webDriver, string fileName)
        {
            var screenshotTimestamp = DateTime.Now.ToString("hhmmss");
            var screenshotFullName = Path.Combine(LogFolder, $"{fileName}-{screenshotTimestamp}.png");
            webDriver.TakeScreenshot().SaveAsFile(screenshotFullName, ScreenshotImageFormat.Png);
            return screenshotFullName;
        }

        public static void JsClick(this IWebDriver webDriver, IWebElement webElement)
        {
            var executor = (IJavaScriptExecutor)webDriver;
            executor.ExecuteScript("arguments[0].click()", webElement);
        }

        public static void JsClear(this IWebDriver webDriver, IWebElement webElement)
        {
            var executor = (IJavaScriptExecutor)webDriver;
            executor.ExecuteScript("arguments[0].value=''", webElement);
        }

        public static IWebElement SendClearKeys(this IWebElement webElement, string text)
        {
            if (webElement == null)
                throw new ArgumentNullException(nameof(webElement));

            webElement.Clear();
            webElement.SendKeys(text);

            return webElement;
        }

        // for example, the escape key: OpenQA.Selenium.Keys.Escape
        public static void ActionOn(this IWebDriver webDriver, string key)
        {
            Actions action = new Actions(webDriver);
            action.SendKeys(key);
        }

        /// <summary>
        /// Create a log folder
        /// </summary>
        public static string LogFolder
        {
            get
            {
                // Create a log folder if not exist
                var folderName = Path.Combine(TestRunHelper.Settings.ScreenshotFolder,
                    DateTime.Now.ToString("yyyy-MM-dd"));
                if (!Directory.Exists(folderName))
                {
                    Directory.CreateDirectory(folderName);
                }

                return folderName;
            }
        }

        #region Private Methods

        private static WebDriverWait GetWebDriverWait(this IWebDriver driver, int? timeoutInSeconds = null)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds ?? TestRunHelper.Settings.WaitElementInSecond));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));

            return wait;
        }

        private static DefaultWait<IWebElement> GetWebDriverWait(this IWebElement element)
        {
            var wait = new DefaultWait<IWebElement>(element)
            {
                Timeout = TimeSpan.FromSeconds(TestRunHelper.Settings.WaitElementInSecond),
                PollingInterval = TimeSpan.FromMilliseconds(500)
            };
            wait.IgnoreExceptionTypes(typeof(NotFoundException), typeof(StaleElementReferenceException));

            return wait;
        }

        #endregion
    }
}
