using System.Windows.Forms;

namespace SharedService
{
    public static class WindowsDialogUtils
    {
        public static string ButtonArchive = "Archive";

        public static string ButtonReplace = "Replace";

        public static string ButtonCancel = "Cancel";

        public static void ShowError(string message)
        {
            MessageBox.Show(message, "Error");
        }

        public static void ShowWarning(string message)
        {
            MessageBox.Show(message, "Warning");
        }

        public static void ShowInfo(string message)
        {
            MessageBox.Show(message, "Info");
        }

        public static void ShowJobDone(string fileName)
        {
            var message = $"The pdf report is generated. Please find it at:\n\r{fileName}";
            MessageBox.Show(message, "Job Done!");
        }

        public static string ShowArchive()
        {
            var archiveInfo = $@"Please select any operation:

    {ButtonArchive}  - Archive the old report and generate a new one.
    {ButtonReplace}  - Replace the old report with new generated one.
    {ButtonCancel}   - Cancel generating report and keep the old one.
";
            var archive = new ArchiveForm(archiveInfo) { StartPosition = FormStartPosition.CenterParent };
            var result = archive.ShowDialog();
            return result.ToString();
        }
    }
}
