namespace SharedService
{
    public interface IPdfService
    {
        string OutputReportFile { get; set; }

        void Run();
    }
}
