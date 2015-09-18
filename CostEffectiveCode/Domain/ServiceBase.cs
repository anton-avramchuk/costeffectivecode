using System;
using CostEffectiveCode.Common;
using CostEffectiveCode.Domain.Cqrs.Commands;
using CostEffectiveCode.Domain.Cqrs.Queries;
using CostEffectiveCode.Domain.Ddd.UnitOfWork;
using JetBrains.Annotations;

namespace CostEffectiveCode.Domain
{
    public abstract class ServiceBase
    {
        protected readonly ICommandFactory CommandFactory;
        protected readonly IQueryFactory QueryFactory;
        protected readonly ILogger Logger;
        protected readonly IScope<IUnitOfWork> UowScope;

        protected ServiceBase(
            [NotNull] IScope<IUnitOfWork> uowScope,
            [NotNull] ICommandFactory commandFactory,
            [NotNull] IQueryFactory queryFactory,
            [NotNull] ILogger logger)
        {
            if (uowScope == null) throw new ArgumentNullException("uowScope");
            if (commandFactory == null) throw new ArgumentNullException("commandFactory");
            if (queryFactory == null) throw new ArgumentNullException("queryFactory");

            CommandFactory = commandFactory;
            QueryFactory = queryFactory;
            Logger = logger;
            UowScope = uowScope;
        }
    }
}
