using System;
using System.Diagnostics;
using Microsoft.Owin.Hosting;
using Topshelf;

namespace OwinService
{
    public class HostingConfiguration : ServiceControl
    {
        private IDisposable _webApplication;

        private readonly string _baseUrl;

        public HostingConfiguration()
        {
            _baseUrl = $"{PlatformManager.Instance.Platform.Protocal}://{PlatformManager.Instance.Platform.Host}:{PlatformManager.Instance.Platform.Port}";
        }

        public bool Start(HostControl hostControl)
        {
            Trace.WriteLine("Starting the patient service");
            _webApplication = WebApp.Start<Startup>(_baseUrl);
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _webApplication.Dispose();
            return true;
        }
    }
}
