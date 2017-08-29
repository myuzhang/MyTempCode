using System;
using System.IO;

namespace SharedService
{
    public static class ConsoleApplicationUtils
    {
        public static void Run(string[] args, Func<string, IPdfService> pdfService, Predicate<string> fileCondition)
        {
            string fileName = null;
            try
            {
                string patientFile = GetPatientDataFileFromCli(args);

                IPdfService service = pdfService(patientFile);
                fileName = service.OutputReportFile;

                if (fileCondition(fileName))
                {
                    service.Run();
                    WindowsDialogUtils.ShowJobDone(fileName);
                }
            }
            catch (Exception e)
            {
                if (fileName != null && File.Exists(fileName)) File.Delete(fileName);
                WindowsDialogUtils.ShowError(e.Message);
            }
        }

        public static string GetPatientDataFileFromCli(string[] args)
        {
            if (args.Length > 1)
                throw new ArgumentException("The input parameter accepts only one argument for patient case folder or no argument for current path as case folder.");

            string patientFile;
            if (args.Length == 0)
            {
                var path = Directory.GetCurrentDirectory();
                patientFile = OOPathUtilities.PathCaseFile.GetPatientDataFile(path);
            }
            else
            {
                patientFile = OOPathUtilities.PathCaseFile.GetPatientDataFile(args[0]);
            }

            if (!File.Exists(patientFile))
                throw new ArgumentException($"No patient data file found! Invalid File:{patientFile}");

            return patientFile;
        }
    }
}
