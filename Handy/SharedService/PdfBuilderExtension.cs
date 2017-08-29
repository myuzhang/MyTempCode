using PdfMaker;
using SharedService.Models;

namespace SharedService
{
    public static class PdfBuilderExtension
    {
        public static void FillAddressDetail(this PdfBuilder builder, AddressDetail detaill)
        {
            builder.FillImageInAcroForm(detaill);
        }

        public static void FillSymbolDetail(this PdfBuilder builder, SymbolDetail detaill)
        {
            builder.FillImageInAcroForm(detaill.IconDetail);
            builder.FillFieldInAcroForm(detaill.TextDetail);
        }
    }
}
