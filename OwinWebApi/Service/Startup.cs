using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Owin;
using WebApi;

namespace Service
{
    public class Startup
    {
        // This method is required by Katana:
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            IContainer container = EngineContext.InitializeEngine();

            var dependencyResolver = new AutofacWebApiDependencyResolver(container);
            config.DependencyResolver = dependencyResolver;

            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);

            WebApiConfig.Register(config);
            app.UseWebApi(config);
        }
    }
}
