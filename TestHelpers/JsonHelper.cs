using System;
using System.Collections.Generic;
using System.Linq;
using AutomationFramework.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium.Support.UI;

namespace TestHelpers
{
    public static class JsonHelper
    {
        public static T ToJsonObject<T>(this string value) =>
            JsonConvert.DeserializeObject<T>(value);

        public static string ToJsonString(this object value) =>
            JsonConvert.SerializeObject(
                value,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Converters = new List<JsonConverter> { new DecimalJsonConverter() }
                });

        public static string ToReadableJsonString(this object value) =>
            JsonConvert.SerializeObject(
                value,
                Formatting.Indented,
                new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});

        public static string ToReadableJsonString(this string jsonString)
        {
            try
            {
                return JsonConvert.SerializeObject(JToken.Parse(jsonString), Formatting.Indented);
            }
            catch (JsonReaderException)
            {
                var message = $"The string:\n\"{jsonString}\"\n is not Json String\n";
                Console.WriteLine(message);
                return jsonString;
            }
        }        
    }
}
