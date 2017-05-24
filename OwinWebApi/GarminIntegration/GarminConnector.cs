using System;
using System.Collections.Generic;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth;
using GarminIntegration.Models;
using Newtonsoft.Json;

namespace GarminIntegration
{
    public class GarminConnector
    {

        private readonly DesktopConsumer _garminConsumer;

        public GarminConnector(InMemoryTokenManager tokenManager)
        {
            _garminConsumer = new DesktopConsumer(GarminFactories.GetHmacSha1ServiceDescription(), tokenManager);
        }
        

        public IList<DailySummary> GetDailySummaries(string accessToken, DateTime? dateTime = null, DateTime? to = null)
            => GetSummaries<DailySummary>(GarminFactories.DailySummaries, accessToken, dateTime, to);

        public IList<ActivitySummary> GetActivitySummaries(string accessToken, DateTime? dateTime = null, DateTime? to = null)
            => GetSummaries<ActivitySummary>(GarminFactories.ActivitySummaries, accessToken, dateTime, to);

        private IList<T> GetSummaries<T>(
            MessageReceivingEndpoint endpoint,
            string accessToken,
            DateTime? from = null,
            DateTime? to = null)
        {
            if (to == null) // one day data
            {
                if (from == null)
                    from = DateTime.Now;

                DateTime fromToday = new DateTime(from.Value.Year, from.Value.Month, from.Value.Day);
                DateTime toTomorrow = fromToday.AddDays(1);
                var extraData = TimeHelpers.GetExtraDataFromLocal(fromToday, toTomorrow);
                return GetSummaries<T>(endpoint, accessToken, extraData);
            }

            if (to != null && from == null)
                throw new ArgumentException("from time can not be null if to time is not null");

            if (to < from)
                throw new ArgumentException($"from time {from} should be ahead of to time {to}");

            var summaries = new List<T>();
            var days = (int)Math.Ceiling((to - from).Value.TotalDays);
            for (int i = 0; i < days; i++)
            {
                var offsetDay = from.Value.AddDays(i);
                DateTime fromTheDay = new DateTime(offsetDay.Year, offsetDay.Month, offsetDay.Day);
                DateTime nextDay = fromTheDay.AddDays(1);
                var extraData = TimeHelpers.GetExtraDataFromLocal(fromTheDay, nextDay);
                summaries.AddRange(GetSummaries<T>(endpoint, accessToken, extraData));
            }
            return summaries;
        }

        private List<T> GetSummaries<T>(MessageReceivingEndpoint endpoint, string accessToken, IDictionary<string, string> extraData)
        {

            var request = _garminConsumer.PrepareAuthorizedRequest(endpoint, accessToken, extraData);
            request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Chrome/10.0.648.151 Safari/534.16";
            request.AllowAutoRedirect = true;

            var response = _garminConsumer.Channel.WebRequestHandler.GetResponse(request);
            var body = response.GetResponseReader().ReadToEnd();
            return JsonConvert.DeserializeObject<List<T>>(body);
        }
    }
}