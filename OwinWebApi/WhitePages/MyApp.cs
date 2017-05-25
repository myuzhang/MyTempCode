using System;
using Autofac;

namespace WhitePages
{
    public class MyApp
    {
        private static MyApp _instance = null;

        private ContainerBuilder _builder = new ContainerBuilder();

        private IContainer _container;

        private MyApp()
        {
            _builder.RegisterType<Application>().SingleInstance();

            _builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .Where(t => t.IsSubclassOf(typeof(BaseService)))
                .AsSelf()
                .SingleInstance();

            _container = _builder.Build();
        }

        public static MyApp Instance => _instance ?? (_instance = new MyApp());

        public Application MyAppApplication => _container.Resolve<Application>();

        public HomePage HomePage => _container.Resolve<HomePage>();
    }
}
