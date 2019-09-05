using SharedService.Models;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SharedService
{
    public abstract class PdfServiceHelper
    {
        protected PdfServiceHelper(string patientDataFile)
        {
            PatientDataFile = patientDataFile;
            PatientFolder = OOPathUtilities.PathCaseFile.GetCaseDirectory(patientDataFile);
            PatientData = new OOPatientDataClass.OOPatientDataClass(patientDataFile);
        }

        public virtual AddressDetail AddressDetail => GetAddressDetail(PatientData.CountryCode);

        public virtual SymbolDetail SymbolDetail => GetSymbolDetail(PatientData.CountryCode);

        public virtual OOPatientDataClass.OOPatientDataClass PatientData { get; }

        public virtual string PatientDataFile { get; }

        public virtual string PatientFolder { get; }

        public abstract string TemplateFile { get; }

        public abstract string OutputReportFile { get; }

        public abstract string DefaultFont { get; }

        protected void ClonePatientBaseDetail(PatientBaseDetail target)
        {
            string today = DateTime.Now.ToString("dd-MMM-yyyy");
            string doct = PatientData.DateOfCT.ToString("dd-MMM-yyyy");
            var patientBaseDetail = new PatientBaseDetail
            {
                CaseId = PatientData.SimIDNo,
                DoCT = PatientData.DateOfCT.ToString("dd-MMM-yyyy"),
                DoB = PatientData.PatientDOB.ToString("dd-MMM-yyyy"),
                DoS = PatientData.DateOfSurgery?.ToString("dd-MMM-yyyy"),
                PatientName = PatientData.PatientFirstName + " " + PatientData.PatientLastName,
                Side = PatientData.SideString,
                SurgeonName = PatientData.Surgeon,
                FurtherInfo = $"Report created for {PatientData.Surgeon} on {today} using pre-operative imaging acquired on {doct}."
            };

            Type patientBaseDetailType = patientBaseDetail.GetType();
            PropertyInfo[] patientBaseDetailProperties = patientBaseDetailType.GetProperties();

            Type targetType = target.GetType();
            PropertyInfo[] targetProperties = targetType.GetProperties();

            foreach (PropertyInfo property in patientBaseDetailProperties)
            {
                PropertyInfo targetProperty = target.GetType().GetProperty(property.Name);
                var value = property.GetValue(patientBaseDetail, null);
                if (targetProperty != null) targetProperty.SetValue(target, value, null);
            }
        }

        private AddressDetail GetAddressDetail(string region)
        {
            var addressSettingData =
                SettingDataUtils.GetSettingData<AddressSettingData>(Constant.Configuration.AddressSettingData);
            var detail = new AddressDetail();

            Type type = detail.GetType();
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                var address = addressSettingData.Addresses.FirstOrDefault(a =>
                    a.Type.Equals(property.Name, StringComparison.CurrentCultureIgnoreCase) &&
                    a.Region.Equals(region, StringComparison.CurrentCultureIgnoreCase));
                if (address == null)
                {
                    address = addressSettingData.Addresses.FirstOrDefault(a =>
                        a.Type.Equals(property.Name, StringComparison.CurrentCultureIgnoreCase) &&
                        string.IsNullOrWhiteSpace(a.Region));
                    if (!string.IsNullOrWhiteSpace(address?.File))
                    {
                        var file = Path.Combine(addressSettingData.RootDir, address.File);
                        property.SetValue(detail, Convert.ChangeType(file, property.PropertyType), null);
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(address.File))
                    {
                        var file = Path.Combine(addressSettingData.RootDir, address.File);
                        property.SetValue(detail, Convert.ChangeType(file, property.PropertyType), null);
                    }
                }
            }

            return detail;
        }

        private SymbolDetail GetSymbolDetail(string region)
        {
            var symbolSettingData =
                SettingDataUtils.GetSettingData<SymbolSettingData>(Constant.Configuration.SymbolSettingData);

            var symbol =
                symbolSettingData.Symbols.First(
                    s => s.Region.Equals(region, StringComparison.CurrentCultureIgnoreCase));

            return new SymbolDetail
            {
                IconDetail = new SymbolDetail.Icon
                {
                    Symbol1Icon = Path.Combine(symbolSettingData.RootDir, symbol.Symbol1),
                    Symbol2Icon = Path.Combine(symbolSettingData.RootDir, symbol.Symbol2)
                },
                TextDetail = new SymbolDetail.Text
                {
                    Symbol1Text = symbol.Symbol1.Split('.').First(),
                    Symbol2Text = symbol.Symbol2.Split('.').First()
                }
            };
        }

    }
}
