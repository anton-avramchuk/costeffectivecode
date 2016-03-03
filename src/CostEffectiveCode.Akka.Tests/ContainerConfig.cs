using Autofac;
using CostEffectiveCode.Akka.Actors;
using CostEffectiveCode.Autofac;
using CostEffectiveCode.Common;
using CostEffectiveCode.Common.Scope;
using CostEffectiveCode.Domain.Cqrs.Queries;
using CostEffectiveCode.Domain.Ddd.Specifications;
using CostEffectiveCode.Domain.Ddd.UnitOfWork;
using CostEffectiveCode.EntityFramework6;
using CostEffectiveCode.Sample.Data;
using CostEffectiveCode.Sample.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace CostEffectiveCode.Akka.Tests
{
    public class ContainerConfig
    {
        public IContainer AutofacContainer { get; private set; }

        private IDiContainer _container;
        public IDiContainer Container =>
            _container ?? (_container = new AutofacDiContainer(AutofacContainer));

        public IConfigurationRoot Configuration { get; private set; }

        public void Configure()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");

            //builder.AddEnvironmentVariables();

            Configuration = builder.Build();

            AutofacContainer = ConfigureContainer();
        }

        private IContainer ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            builder
                .Register(x => Container)
                .As<IDiContainer>()
                .SingleInstance();

            builder
                .RegisterType<ExpressionQuery<Product>>()
                .As(typeof(IQuery<Product, IExpressionSpecification<Product>>))
                .InstancePerDependency();

            builder
                .RegisterType<ExpressionSpecification<Product>>()
                .As<IExpressionSpecification<Product>>()
                .InstancePerDependency();

            builder
                .RegisterGeneric(typeof(DiContainerScope<>))
                .As(typeof(IScope<>))
                .SingleInstance();

            builder
                .Register(x =>
                    new QueryActor<Product>(
                        x.Resolve<IScope<IQuery<Product, IExpressionSpecification<Product>>>>()))
                .As<QueryActor<Product>>()
                .InstancePerDependency();

            builder
                .Register(x => new SampleDbContext(Configuration["Data:DefaultConnection:ConnectionString"]))
                .As<SampleDbContext>()
                .As<ILinqProvider>()
                .As<IUnitOfWork>()
                .As<IDataContext>();

            return builder.Build();
        }
    }
}