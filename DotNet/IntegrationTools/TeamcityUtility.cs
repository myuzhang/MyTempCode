using System;
using System.Net;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace IntegrationTools
{
    public class TeamcityUtility
    {
        public string TeamcityUrl { get; }

        private readonly RestClient _teamcityClient;

        // teamcityAuth is a basic auth, e.g. Basic xxxxxxxxxxxxxxxxx
        public TeamcityUtility(string teamcityUrl, string teamcityAuth)
        {
            TeamcityUrl = teamcityUrl;
            _teamcityClient = new RestClient(teamcityUrl);
            _teamcityClient.AddDefaultHeader("Authorization", teamcityAuth);
            _teamcityClient.AddDefaultHeader("Accept", "application/json");
        }

        public TestResult GetTestResult(string buildId)
        {
            try
            {
                var details = GetBuildDetails(buildId);
                var testOccurrences = details?["testOccurrences"];
                if (testOccurrences != null)
                {
                    return new TestResult
                    {
                        Count = GetValue(testOccurrences["count"]),
                        Failed = GetValue(testOccurrences["failed"]),
                        Ignored = GetValue(testOccurrences["ignored"]),
                        NewFailed = GetValue(testOccurrences["newFailed"]),
                        Passed = GetValue(testOccurrences["passed"]),
                        WebUrl = TeamcityUrl
                    };
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// Get latest build: app/rest/builds?locator=buildType:CsrConnectWeb_UnitTests_AutomationE2eSeleniumTests,count:1
        /// Get running build: app/rest/builds?locator=buildType:CsrConnectWeb_UnitTests_AutomationE2eSeleniumTests,state:running
        private JContainer GetBuildDetails(string buildId)
        {
            if (string.IsNullOrWhiteSpace(buildId)) return null;

            var request = new RestRequest("app/rest/builds/{buildId}", Method.GET);
            request.AddUrlSegment("buildId", buildId);
            var response = _teamcityClient.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            var data = (JContainer) JToken.Parse(response.Content);
            return data;
        }

        private int GetValue(JToken jValue) => jValue != null ? (int) jValue : 0;
    }

    public class TestResult
    {
        public int Passed { get; set; }
        public int Failed { get; set; }
        public int NewFailed { get; set; }
        public int Ignored { get; set; }
        public int Count { get; set; }
        public string WebUrl { get; set; }
    }
}
