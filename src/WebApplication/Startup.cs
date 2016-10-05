using System.Reflection;
using System.Web.Mvc;
using CostEffectiveCode.AutoMapper;
using CostEffectiveCode.Common;
using CostEffectiveCode.Components;
using CostEffectiveCode.Ddd;
using Microsoft.Owin;
using Owin;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using WebApplication.Domain;

[assembly: OwinStartupAttribute(typeof(WebApplication.Startup))]
namespace WebApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            // scan assemblies for types
            var currentAssembly = typeof(Startup).Assembly;
            var componentMap = AutoRegistration.GetComponentMap(currentAssembly
                , x => typeof(IController).IsAssignableFrom(x)
                , currentAssembly
                , x => x.IsInterface);

            // init conventions for automapper
            StaticAutoMapperWrapper.Init(cfg =>
            {
                ConventionalProfile.Scan(currentAssembly);
                cfg.AddProfile<ConventionalProfile>();
            });

            // init IOC-container
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            var am = Lifestyle.Singleton.CreateRegistration(() => new StaticAutoMapperWrapper(), container);
            container.AddRegistration(typeof(IProjector), am);
            container.AddRegistration(typeof(IMapper), am);

            // Fake Context
            var reg = Lifestyle.Singleton.CreateRegistration(() => new FakeContext(new[] {
                new Product()
                {
                    Category = new Category(),
                    Id = 1,
                    Name = "1 Product",
                    Price = 100500
                }
                , new Product()
                {
                    Category = new Category(),
                    Id = 2,
                    Name = "2 Product",
                    Price = 200500
                }

            }), container);

            container.AddRegistration(typeof(ILinqProvider), reg);
            container.AddRegistration(typeof(IUnitOfWork), reg);

            foreach (var kv in componentMap)
            {
                container.Register(kv.Key, kv.Value, Lifestyle.Scoped);
            }

            // This is an extension method from the integration package.
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
    }
}
