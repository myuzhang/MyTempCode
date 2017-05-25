using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oauth;
using WebApi.Models;

namespace UnitTest
{
    [TestClass]
    public class GarminServiceTests
    {
        [ClassInitialize]
        public static void Initial(TestContext context)
        {
            TestingContext.Setup();
        }

        [ClassCleanup]
        public static void Clean()
        {
            TestingContext.Teardown();
        }

        [TestMethod]
        public void TestDailySummary()
        {
            var dailySummaryPing = new DailiesPing
            {
                Dailies = new List<Notification>
                {
                    new Notification
                    {
                        UserAccessToken = "7e173603-ab46-4568-b10e-81b267854843",
                        UploadStartTimeInSeconds = TimeHelpers.GetUnixTimestampFromLocal(DateTime.Now),
                        UploadEndTimeInSeconds = TimeHelpers.GetUnixTimestampFromLocal(DateTime.Now)
                    }
                }
            };

            var response = HttpClient.Instance.Post("/garminnotification/dailies", dailySummaryPing);
        }

        [TestMethod]
        public void TestWebApiWorking()
        {
            var response = HttpClient.Instance.Get("/patient/5");
        }
    }
}
