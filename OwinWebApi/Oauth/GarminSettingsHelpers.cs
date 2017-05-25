using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Oauth.SettingsModels;
using Environment = Oauth.SettingsModels.Environment;

namespace Oauth
{
    public class GarminSettingsHelpers
    {
        private static GarminSettingsHelpers _instance;

        private readonly Environment _environment;

        private GarminSettingsHelpers()
        {
            string jsonString = System.IO.File.ReadAllText("GarminSettings.json");
            var settings = JsonConvert.DeserializeObject<GarminSettings>(jsonString);
            _environment = settings.Environments.First(e => e.Platform.Equals(settings.Running));

        }

        public static GarminSettingsHelpers Instance =>
            _instance ?? (_instance = new GarminSettingsHelpers());

        public Credential GetCredential() => _environment.Credential;

        public string GetProviderEndpoint(string endpointName) =>
            GetEndpoint(endpointName, _environment.ServiceProvider);

        public string GetMessageReceivingEndpoint(string messageName) =>
            GetEndpoint(messageName, _environment.MessageReceivingEndpoint);

        private string GetEndpoint<T>(string endpointName, T endpoint)
        {
            foreach (MemberInfo memberInfo in typeof(T).GetMembers())
            {
                if (memberInfo.Name.Equals(endpointName, StringComparison.CurrentCultureIgnoreCase))
                {
                    var propertyInfo = memberInfo as PropertyInfo;
                    if (propertyInfo != null)
                    {
                        return propertyInfo.GetValue(endpoint).ToString();
                    }
                }
            }
            return null;
        }
    }
}
