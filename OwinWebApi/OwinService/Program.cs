using System;
using Topshelf;

namespace OwinService
{
    class Program
    {
        static void Main(string[] args)
        {
            // start scheduled task


            // start owin
            var exitCode = HostFactory.Run(x =>
                {
                    x.SetStartTimeout(TimeSpan.FromSeconds(PlatformManager.Instance.Platform.Timeout));
                    x.SetStopTimeout(TimeSpan.FromSeconds(PlatformManager.Instance.Platform.Timeout));
                    x.Service<HostingConfiguration>();
                    x.RunAsLocalSystem();
                    x.StartAutomatically();
                    //x.RunAsNetworkService();
                    x.SetDescription("Patient Service : webapi service");
                    x.SetDisplayName("My Service");
                    x.SetServiceName("My Service");
                }
            );
        }
    }
}
