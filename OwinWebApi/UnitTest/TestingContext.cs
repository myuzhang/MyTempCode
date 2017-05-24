using System;
using System.Net.Http.Headers;
using Microsoft.Owin.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Service;

namespace UnitTest
{
    public class TestingContext
    {
        //private static TestingContext _instance;

        //private TestingContext()
        //{
        //}

        //public static TestingContext Instance => _instance ?? (_instance = new TestingContext());

        public static TestServer Server
        {
            get;
            private set;
        }

        public static void Setup()
        {
            InitializeTestServer();
            InitializeTestClient();
        }

        public static void Teardown()
        {
            if (Server != null)
            {
                Server.Dispose();
                Server = null;
            }
        }

        private static void InitializeTestServer()
        {
            Server = TestServer.Create<Startup>();

            if (Server == null)
                throw new InvalidOperationException("Failed to host OWIN TestStartup");
        }

        private static void InitializeTestClient()
        {
            if (Server == null)
                throw new InvalidOperationException("Server is missing");

            if (Server.HttpClient == null)
                throw new InvalidOperationException("Server.HttpClient is missing");

            Server.HttpClient.DefaultRequestHeaders.Accept.Clear();
            Server.HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }
    }
}
