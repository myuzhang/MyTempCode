using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using GarminIntegration;

namespace WebApi
{
    public static class EngineContext
    {
        public static IContainer InitializeEngine()
        {
            var builder = new ContainerBuilder();
            // register your own business:
            builder.RegisterType<GarminConnector>().InstancePerRequest();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).InstancePerRequest();

            IContainer container = builder.Build();
            return container;
        }
    }
}