using System.Net.Http;
using System.Net.Http.Formatting;
using WebApi;

namespace UnitTest
{
    public class HttpClient
    {
        private static HttpClient _instance;

        private HttpClient()
        {
        }

        public static HttpClient Instance => _instance ?? (_instance = new HttpClient());

        public HttpResponseMessage Get(string request)
        {
            return TestingContext.Server.CreateRequest($"api/{Constants.ApiVersion}" + request)
                .GetAsync()
                .Result;
        }

        public HttpResponseMessage Post<T>(string request, T payload)
        {
            return TestingContext.Server.CreateRequest($"api/{Constants.ApiVersion}" + request)
                .AddHeader("Content-Type", @"application/json")
                .And(message => message.Content = new ObjectContent(typeof(T), payload, new JsonMediaTypeFormatter()))
                .SendAsync("POST")
                .Result;
        }

        public HttpResponseMessage Put(string request)
        {
            return TestingContext.Server.CreateRequest($"api/{Constants.ApiVersion}" + request)
                .AddHeader("Content-Type", @"application/json")
                .SendAsync("PUT")
                .Result;
        }

        public HttpResponseMessage Put<T>(string request, T payload)
        {
            return TestingContext.Server.CreateRequest($"api/{Constants.ApiVersion}" + request)
                .AddHeader("Content-Type", @"application/json")
                .And(message => message.Content = new ObjectContent(typeof(T), payload, new JsonMediaTypeFormatter()))
                .SendAsync("PUT")
                .Result;
        }

        public HttpResponseMessage Delete(string request)
        {
            return TestingContext.Server.CreateRequest($"api/{Constants.ApiVersion}" + request)
                .AddHeader("Content-Type", @"application/json")
                .SendAsync("DELETE")
                .Result;
        }

        public HttpResponseMessage Delete<T>(string request, T payload)
        {
            return TestingContext.Server.CreateRequest($"api/{Constants.ApiVersion}" + request)
                .AddHeader("Content-Type", @"application/json")
                .And(message => message.Content = new ObjectContent(typeof(T), payload, new JsonMediaTypeFormatter()))
                .SendAsync("DELETE")
                .Result;
        }
    }
}
