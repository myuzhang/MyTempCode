using PdfMaker;
using PdfMaker.Models;

namespace SharedService.Models
{
    public class SymbolDetail
    {
        public Icon IconDetail = new Icon();

        public Text TextDetail = new Text();

        public class Icon
        {
            [PdfAction(ActionFlag.Required)]
            public string Symbol1Icon { get; set; }

            [PdfAction(ActionFlag.Required)]
            public string Symbol2Icon { get; set; }
        }

        public class Text
        {
            [PdfAction(ActionFlag.Required)]
            [PdfFont(Type = "helvetica", Size = 11)]
            public string Symbol1Text { get; set; }

            [PdfAction(ActionFlag.Required)]
            [PdfFont(Type = "helvetica", Size = 11)]
            public string Symbol2Text { get; set; }
        }
    }
}
