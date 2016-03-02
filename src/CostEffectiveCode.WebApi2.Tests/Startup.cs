using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using CostEffectiveCode.WebApi2.Tests.Config;
using Microsoft.Extensions.Configuration;
using Owin;

namespace CostEffectiveCode.WebApi2.Tests
{
    public class Startup
    {
        public static HttpConfiguration HttpConfiguration;
        public static IConfigurationRoot Config;

        public void Configuration(IAppBuilder app)
        {
            LoadApplicationConfig();

            var container = IocConfig.Configure();
            ConfigureApp(container, app);
        }

        private static void LoadApplicationConfig()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json");

            //builder.AddEnvironmentVariables();

            Config = configurationBuilder.Build();
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
