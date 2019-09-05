using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Utility
{
    internal static class TestCommon
    {
        public static string SampleDataFolder = "Samples";

        public static string PartialName = "PartialName";

        public static string EarliestOutputFolder => OutputRomFolders.FirstOrDefault();

        public static IList<string> OutputRomFolders
        {
            get
            {
                string directory = Path.Combine(Environment.CurrentDirectory, SampleDataFolder);
                DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(directory);
                FileSystemInfo[] filesAndDirs = hdDirectoryInWhichToSearch.GetFileSystemInfos("*" + PartialName + "*");
                var fileSystemInfos = filesAndDirs.ToList().OrderBy(fileSystemInfo => fileSystemInfo.CreationTime);
                return fileSystemInfos.Select(fileSystemInfo => fileSystemInfo.FullName).ToList();
            }
        }

        public static void SetKeyValue(bool key)
        {
            SetAppSettings(nameof(key), key.ToString());
        }

        public static void SetAppSettings(string key, string value)
        {
            // refer to: https://stackoverflow.com/questions/179254/reloading-configuration-without-restarting-application-using-configurationmanage
            ConfigurationManager.AppSettings.Set(key, value);
        }

        // this one change the value in app settings but it doesn't take effect if the program is running, use SetAppSettings(string key, string value)
        public static void ChangeAppSettingsFile(string xmlFile, string key, string value)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFile);
            XmlNodeList xns = xmlDoc.SelectNodes($"/appSettings/add[@key='{key}']");
            if (xns != null)
            {
                foreach (XmlNode xn in xns)
                {
                    if (xn.Attributes != null)
                    {
                        if (xn.Attributes["value"].Value.Equals(value))
                            return;

                        xn.Attributes["value"].Value = value;
                    }
                }
                xmlDoc.Save(xmlFile);
            }

            //ConfigurationManager.RefreshSection("appSettings"); // this one doesn't work if the appsettings is external
        }

        public static void AreEqualIgnoreNullOrWhiteSpace(string expected, string actual)
        {
            if (string.IsNullOrWhiteSpace(expected) || string.IsNullOrWhiteSpace(actual))
                Assert.AreEqual(
                    string.IsNullOrWhiteSpace(expected),
                    string.IsNullOrWhiteSpace(actual));
            else Assert.AreEqual(expected, actual);
        }
    }
}
