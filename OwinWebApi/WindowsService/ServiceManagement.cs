using Autofac;

namespace WindowsService
{
    public class ServiceManagement
    {
        private readonly ILifetimeScope _lifetimeScope;

        private readonly System.Diagnostics.EventLog _eventLog;

        public ServiceManagement(System.Diagnostics.EventLog eventLog)
        {
            _eventLog = eventLog;
            var builder = new ContainerBuilder();
            Init(builder);
            var container = builder.Build();
            _lifetimeScope = container.BeginLifetimeScope();
        }
        
        private void Init(ContainerBuilder builder)
        {
            builder.RegisterType<Manager>().AsSelf().WithParameter("eventLog", _eventLog);
        }

        public Manager Manager => _lifetimeScope.Resolve<Manager>();
    }
}
