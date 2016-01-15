using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using CostEffectiveCode.WebApi2.Tests.Config;
using Owin;

namespace CostEffectiveCode.WebApi2.Tests
{
    public class Startup
    {
        public static HttpConfiguration HttpConfiguration;

        public void Configuration(IAppBuilder app)
        {
            var container = IocConfig.Configure();
            ConfigureApp(container, app);
        }

        private void ConfigureApp(IContainer container, IAppBuilder app)
        {
            HttpConfiguration = new HttpConfiguration
            {
                DependencyResolver = new AutofacWebApiDependencyResolver(container)
            };

            HttpConfiguration.Formatters.Remove(HttpConfiguration.Formatters.XmlFormatter);
            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(HttpConfiguration);
            app.UseWebApi(HttpConfiguration);
            WebApiConfig.Register(HttpConfiguration);
        }

    }
}
