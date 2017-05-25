using Newtonsoft.Json.Linq;
using RestSharp;

namespace Common
{
    public class SendHttpRequest
    {
        private readonly RestClient _client;

        public SendHttpRequest(string url)
        {
            _client = new RestClient(url);    
        }

        public RestClient Client => _client;

        public JObject GetJson(RestRequest request = null)
        {
            if (request == null)
            {
                request = new RestRequest(Method.GET)
                {
                    OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; }
                };
            }
            IRestResponse response = _client.Execute(request);
            return JObject.Parse(response.Content);
        }
    }
}
