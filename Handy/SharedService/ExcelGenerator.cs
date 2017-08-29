using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SharedService
{
    // this one calls vbs now
    public class ExcelGenerator
    {
        private readonly string _workingDirectory;

        private readonly string _caseId;

        public ExcelGenerator(string workingDirectory, string caseId)
        {
            _workingDirectory = workingDirectory;
            _caseId = caseId;
        }

        public string Build()
        {
            Process scriptProc = new Process
            {
                StartInfo =
                {
                    FileName = @"cscript",
                    WorkingDirectory = _workingDirectory,
                    Arguments = "//B //Nologo HipReportGen.vbs",
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            };

            scriptProc.Start();
            scriptProc.WaitForExit(); // <-- Optional if you want program running until your script exit
            scriptProc.Close();

            var hdDirectoryInWhichToSearch = new DirectoryInfo(_workingDirectory);
            string reportExcel = hdDirectoryInWhichToSearch
                .GetFiles($"DHA_{_caseId}_model_*.xlsx")
                .OrderByDescending(f => f.CreationTime)
                .FirstOrDefault()?.FullName;

            return reportExcel;
        }
    }
}
