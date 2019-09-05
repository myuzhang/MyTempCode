using Microsoft.Win32;
using System;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;

namespace SharedService
{
    public static class SettingDataUtils
    {
        //todo: being replaced by new oo utility lib
        public static string GetFileNameFromRegistry(string file)
        {
            string settingFile = ConfigurationManager.AppSettings.Get(file);
            if (settingFile != null)
                return settingFile;

            System.Xml.XmlDocument xml = new System.Xml.XmlDocument();
            xml.Load((string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\OptimizedOrtho\OOUtilityLibrary",
                "configPath", null));

            var selectSingleNode = xml.SelectSingleNode($"/config/OOUtilityLibrary/{file}");
            if (selectSingleNode != null)
                return selectSingleNode.InnerText;
            throw new ArgumentException($"No config found for {file}");
        }

        //todo: being replaced by new oo utility lib
        public static T GetSettingData<T>(string file)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            var reader = new StreamReader(GetFileNameFromRegistry(file));
            var settingData = (T)serializer.Deserialize(reader);
            return settingData;
        }
    }
}
