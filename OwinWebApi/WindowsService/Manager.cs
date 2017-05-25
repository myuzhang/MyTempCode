namespace WindowsService
{
    public class Manager
    {
        private System.Diagnostics.EventLog _eventLog;

        public Manager(System.Diagnostics.EventLog eventLog)
        {
            _eventLog = eventLog;
        }
    }
}
