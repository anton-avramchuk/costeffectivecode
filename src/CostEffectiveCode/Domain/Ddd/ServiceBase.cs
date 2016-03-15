using System;
using CostEffectiveCode.Common.Logger;
using CostEffectiveCode.Common.Scope;
using CostEffectiveCode.Domain.Cqrs.Commands;
using CostEffectiveCode.Domain.Cqrs.Queries;
using CostEffectiveCode.Domain.Ddd.UnitOfWork;

namespace CostEffectiveCode.Domain.Ddd
{
    public abstract class ServiceBase
    {
        protected readonly ICommandFactory CommandFactory;
        protected readonly IQueryFactory QueryFactory;
        protected readonly ILogger Logger;
        protected readonly IScope<IUnitOfWork> UowScope;

        protected ServiceBase(IScope<IUnitOfWork> uowScope, ICommandFactory commandFactory, IQueryFactory queryFactory, ILogger logger)
        {
            if (uowScope == null)
                throw new ArgumentNullException(nameof(uowScope));
            if (commandFactory == null)
                throw new ArgumentNullException(nameof(commandFactory));
            if (queryFactory == null)
                throw new ArgumentNullException(nameof(queryFactory));
            this.CommandFactory = commandFactory;
            this.QueryFactory = queryFactory;
            this.Logger = logger;
            this.UowScope = uowScope;
        }
    }
}
