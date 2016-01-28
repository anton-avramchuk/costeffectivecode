using System.Data.Entity;
using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using CostEffectiveCode.AutoMapper;
using CostEffectiveCode.Common;
using CostEffectiveCode.Domain.Cqrs;
using CostEffectiveCode.Domain.Cqrs.Commands;
using CostEffectiveCode.Domain.Ddd.UnitOfWork;
using CostEffectiveCode.EntityFramework6;
using CostEffectiveCode.Sample.Data;
using CostEffectiveCode.WebApi2.Tests.Loggers;

namespace CostEffectiveCode.WebApi2.Tests.Config
{
    public class IocConfig
    {
        public static string ConnectionString = @"Data Source=.;Initial Catalog=CostEffectiveCode_WebApi2_Tests;Integrated Security=True;MultipleActiveResultSets=True";

        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            DbContext(builder);
            Cqrs(builder);
            DependencyResolving(builder);
            Mapping(builder);
            Logging(builder);

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            return builder.Build();
        }

        private static void Logging(ContainerBuilder builder)
        {
            builder.RegisterType<ConsoleLogger>()
                .As<ILogger>()
                .InstancePerRequest();
        }

        private static void DbContext(ContainerBuilder builder)
        {
            builder
                //.Register(x => new SampleProjectDbContext(ConnectionString))
                .RegisterType<SampleDbContext>()
                .As<SampleDbContext>()
                .As<DbContext>()
                .As<IDataContext>()
                .As<IUnitOfWork>()
                .As<ILinqProvider>()
                .InstancePerLifetimeScope();                ;
        }

        private static void Mapping(ContainerBuilder builder)
        {
            builder
                .RegisterType<AutoMapperWrapper>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }

        private static void DependencyResolving(ContainerBuilder builder)
        {
            builder.RegisterType<DiContainerScope<IUnitOfWork>>()
                .As<IScope<IUnitOfWork>>()
                .SingleInstance();

            builder.RegisterType<DiContainerScope<IDataContext>>()
                .As<IScope<IDataContext>>()
                .SingleInstance();

            builder.RegisterType<DependencyResolverDiContainer>()
                .As<IDiContainer>()
                .SingleInstance();
        }

        private static void Cqrs(ContainerBuilder builder)
        {
            builder.RegisterType<CommandQueryFactory>()
                .AsImplementedInterfaces()
                .InstancePerRequest();

            builder.RegisterGeneric(typeof (CreateEntityCommand<>))
                .InstancePerDependency();

            builder.RegisterGeneric(typeof (DeleteEntityCommand<>))
                .InstancePerDependency();

            builder.RegisterType<CommitCommand>()
                .InstancePerDependency();

            builder.RegisterGeneric(typeof (ExpressionQuery<>))
                .AsImplementedInterfaces()
                .InstancePerDependency();
        }
    }
}
