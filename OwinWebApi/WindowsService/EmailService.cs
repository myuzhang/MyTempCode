using System;
using System.Configuration;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using Common.Enums;

namespace WindowsService
{
    public partial class EmailService : ServiceBase
    {
        private readonly EventLog _eventLog;

        private System.Timers.Timer _timer;

        private bool _serviceStarted;


        public EmailService()
        {
            InitializeComponent();
            InitializeService();
            _eventLog = new EventLog();
            if (!EventLog.SourceExists("MySource"))
            {
                EventLog.CreateEventSource(
                    "MySource", "MySentLog");
            }
            _eventLog.Source = "MySource";
            _eventLog.Log = "MySentLog";
        }

        protected override void OnStart(string[] args)
        {
            _eventLog.WriteEntry("Email Send In OnStart", EventLogEntryType.Information, (int)EventId.StartEmailService);

            ThreadStart start = RunService;
            Thread faxWorkerThread = new Thread(start);
            // start threads
            faxWorkerThread.Start();

            _timer.Start();
        }

        protected override void OnStop()
        {
            _eventLog.WriteEntry("Email Send In OnStop", EventLogEntryType.Information, (int)EventId.StartEmailService);
            _timer.Stop();
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            RunService();
        }

        private void InitializeService()
        {
            // Set up a timer according to the config.
            int interval = Int32.Parse(ConfigurationManager.AppSettings["interval"]);
            _timer = new System.Timers.Timer { Interval = interval * 1000 };
            _timer.Elapsed += OnTimer;
        }

        private void RunService()
        {
            if (_serviceStarted)
                return;

            try
            {
                _serviceStarted = true;
                ServiceManagement service = new ServiceManagement(_eventLog);
                // todo: run your own business
            }
            catch (Exception e)
            {
                _eventLog.WriteEntry($"Error in service: {e.Message}", EventLogEntryType.Error,
                    (int) EventId.ServiceInProgress);
            }
            finally
            {
                _serviceStarted = false;
            }
        }

        [Conditional("DEBUG_SERVICE")]
        private static void DebugMode()
        {
            Debugger.Break();
        }
    }
}
