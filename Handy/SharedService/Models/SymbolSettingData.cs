using System.Xml.Serialization;

namespace SharedService.Models
{
    [XmlRoot(ElementName = "settingData")]
    public class SymbolSettingData
    {
        [XmlElement(ElementName = "rootDir")]
        public string RootDir { get; set; }

        [XmlArray("symbols")]
        [XmlArrayItem("symbol")]
        public Symbol[] Symbols { get; set; }
    }

    public class Symbol
    {
        [XmlAttribute(AttributeName = "region")]
        public string Region { get; set; }

        [XmlAttribute(AttributeName = "symbol1")]
        public string Symbol1 { get; set; }

        [XmlAttribute(AttributeName = "symbol2")]
        public string Symbol2 { get; set; }
    }
}
