using PdfMaker;
using PdfMaker.Models;

namespace SharedService.Models
{
    public class AddressDetail
    {
        [PdfAction(ActionFlag.Required)]
        public string Manufacturer { get; set; }

        [PdfAction(ActionFlag.Optional)]
        public string Sponsor { get; set; }
    }
}
