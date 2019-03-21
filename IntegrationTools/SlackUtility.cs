using System;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace IntegrationTools
{
    public class SlackUtility
    {
        private readonly RestClient _client;

        private readonly string _botToken;

        // the botToken starts with xoxb-
        public SlackUtility(string botToken)
        {
            _client = new RestClient("https://slack.com");
            _botToken = botToken;
        }
        
        public bool Post(string message, string to, string imageFile = null)
        {
            RestRequest request;
            if (string.IsNullOrWhiteSpace(message) || string.IsNullOrWhiteSpace(to))
                throw new ArgumentNullException($"message and to should have values.");

            if (!string.IsNullOrWhiteSpace(imageFile) && File.Exists(imageFile))
            {
                request = new RestRequest("api/files.upload", Method.POST);
                request.AddParameter("token", _botToken);
                request.AddParameter("channels", to);
                if (!string.IsNullOrWhiteSpace(message)) request.AddParameter("initial_comment", message);
                request.AddParameter("title", Path.GetFileName(imageFile));

                var fileInfo = new FileInfo(imageFile);
                request.AddFile("file", File.ReadAllBytes(imageFile), fileInfo.Name, "multipart/form-data");
            }
            else
            {
                request = new RestRequest("api/chat.postMessage", Method.POST);
                request.AddParameter("token", _botToken);
                request.AddParameter("channel", to);
                request.AddParameter("text", message);
                // if slack sends messages to channel instead of IM, as_user can't be true
                if (!to.StartsWith("#"))
                    request.AddParameter("as_user", "true");
            }

            var response = _client.Execute(request);

            return response.StatusCode.Equals(HttpStatusCode.OK);
        }

        public JObject GetMessage(string channel, int messageNumber = 1)
        {
            var request = new RestRequest("api/channels.history", Method.GET);
            request.AddHeader("Accept", "application/x-www-form-urlencoded");
            request.AddQueryParameter("token", _botToken);
            request.AddQueryParameter("channel", channel);
            request.AddQueryParameter("count", messageNumber.ToString());

            var response = _client.Execute(request);
            return JObject.Parse(response.Content);
        }

        public JContainer GetSlackUsers()
        {
            var request = new RestRequest("api/users.list", Method.GET);
            request.AddHeader("Accept", "application/x-www-form-urlencoded");
            request.AddQueryParameter("token", _botToken);

            var response = _client.Execute(request);
            if (response.StatusCode.Equals(HttpStatusCode.OK))
            {
                return (JContainer)JToken.Parse(response.Content);
            }
            return null;
        }
    }
}
