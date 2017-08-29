using System.Xml.Serialization;

namespace SharedService.Models
{
    [XmlRoot(ElementName = "settingData")]
    public class AddressSettingData
    {
        [XmlElement(ElementName = "rootDir")]
        public string RootDir { get; set; }

        [XmlArray("addresses")]
        [XmlArrayItem("address")]
        public Address[] Addresses { get; set; }
    }

    public class Address
    {
        [XmlAttribute(AttributeName = "region")]
        public string Region { get; set; }

        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }

        [XmlAttribute(AttributeName = "file")]
        public string File { get; set; }
    }

    public class AddressTypeConstant
    {
        public const string Manufacturer = "manufacturer";

        public const string Sponsor = "sponsor";
    }
}
