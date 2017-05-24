using System;
using System.IO;
using System.Threading;
using GarminIntegration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service;

namespace UnitTest
{
    [TestClass]
    public class CommonTests
    {
        [TestMethod]
        public void TestLog()
        {
            Log.Instance.WriteLine("This is error", Log.LogLevel.Error);

            Log.Instance.WriteLine("This is warning", Log.LogLevel.Warning);

            Log.Instance.WriteLine("This is info", Log.LogLevel.Info);

            Assert.IsTrue(File.Exists(Log.Instance.LogFile));
        }

        [TestMethod]
        [DeploymentItem("GarminSettings.json")]
        [DeploymentItem("System.Data.SqlClient")]
        public void TestScheduleTask()
        {
            var timer = new ScheduleTimer(DateTime.Now.Hour, DateTime.Now.Minute + 1, () => true);
            Thread.Sleep(70000);
        }

        [TestMethod]
        public void TestEndpointSettings()
        {
            var credential = GarminSettingsHelpers.Instance.GetCredential();
            Assert.IsNotNull(credential);

            var serviceProvider = GarminSettingsHelpers.Instance.GetProviderEndpoint("requestTokenEndpoint");
            Assert.IsNotNull(serviceProvider);

            var dailySummaries = GarminSettingsHelpers.Instance.GetMessageReceivingEndpoint("dailySummaries");
            Assert.IsNotNull(dailySummaries);
        }
    }
}
