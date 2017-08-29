using PdfMaker;
using PdfMaker.Models;

namespace SharedService.Models
{
    public class PatientBaseDetail
    {
        [PdfAction(ActionFlag.Required)]
        public virtual string PatientName { get; set; }

        [PdfAction(ActionFlag.Required)]
        public virtual string Side { get; set; }

        [PdfAction(ActionFlag.Required)]
        public virtual string CaseId { get; set; }

        [PdfAction(ActionFlag.Required)]
        public virtual string DoB { get; set; }

        [PdfAction(ActionFlag.Optional)]
        public virtual string DoS { get; set; }

        [PdfAction(ActionFlag.Required)]
        public virtual string SurgeonName { get; set; }

        [PdfAction(ActionFlag.Required)]
        public virtual string DoCT { get; set; }

        [PdfAction(ActionFlag.Required)]
        public virtual string FurtherInfo { get; set; }
    }
}
