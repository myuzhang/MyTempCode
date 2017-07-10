using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Utility
{
    public class DictionaryToJsonCreator
    {
        private readonly StringBuilder _builder;

        private readonly Dictionary<string, string> _dictionary;

        public DictionaryToJsonCreator()
        {
            _builder = new StringBuilder();
            _builder.Append("{}");
            _dictionary = new Dictionary<string, string>();
        }

        public static DictionaryToJsonCreator GetBuilder() => new DictionaryToJsonCreator();

        public string this[string key]
        {
            get
            {
                if (_dictionary.ContainsKey(key.Trim()))
                    return _dictionary[key.Trim()];

                return null;
            }
        }

        public DictionaryToJsonCreator Add(Dictionary<string, string> dictionary)
        {
            foreach (var keyValue in dictionary)
            {
                _dictionary[keyValue.Key.Trim()] = keyValue.Value.Trim();
            }
            return this;
        }

        public DictionaryToJsonCreator Add(string key, string value)
        {
            _dictionary[key.Trim()] = value.Trim();
            return this;
        }

        public string GetString()
        {
            foreach (var keyValue in _dictionary)
            {
                _builder.Insert(_builder.Length - 1, $",\"{keyValue.Key}\":\"{keyValue.Value}\"");
            }
            return RemoveFirst(_builder.ToString(), ",");
        }

        public T GetObject<T>() where T : class => JsonConvert.DeserializeObject<T>(GetString());

        public string GetStringFromJsonFile(string jsonFile)
        {
            string jsonString;
            using (StreamReader r = new StreamReader(jsonFile))
            {
                jsonString = r.ReadToEnd();
            }
            return jsonString;
        }

        public T GetObjectFromJsonFile<T>(string jsonFile) where T : class => JsonConvert.DeserializeObject<T>(GetStringFromJsonFile(jsonFile));

        private string RemoveFirst(string source, string remove)
        {
            int index = source.IndexOf(remove, StringComparison.Ordinal);
            return (index < 0)
                ? source
                : source.Remove(index, remove.Length);
        }
    }
}
