using System;
using CostEffectiveCode.Common;
using CostEffectiveCode.Domain.Cqrs.Commands;
using CostEffectiveCode.Domain.Cqrs.Queries;
using CostEffectiveCode.Domain.Ddd.Entities;
using CostEffectiveCode.Domain.Ddd.Specifications;
using CostEffectiveCode.Extensions;
using JetBrains.Annotations;

namespace CostEffectiveCode.EntityFramework
{
    [UsedImplicitly]
    public class CommandQueryFactory : ICommandFactory, IQueryFactory
    {
        private readonly IDiContainer _container;

        #region ICommandFactory Implementation

        public CommandQueryFactory([NotNull] IDiContainer container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            _container = container;
        }


        public TCommand GetCommand<TEntity, TCommand>()
            where TCommand : ICommand<TEntity>
            where TEntity : IEntity
        {
            return _container.Resolve<TCommand>().CheckNotNull();
        }

        public T GetCommand<T>() where T : ICommand
        {
            return _container.Resolve<T>().CheckNotNull();
        }

        public CreateEntityCommand<T> GetCreateCommand<T>() where T : class, IEntity
        {
            var createEntityCommand = _container.Resolve<CreateEntityCommand<T>>();

            return createEntityCommand.CheckNotNull();
        }

        public CommitCommand GetCommitCommand()
        {
            return _container.Resolve<CommitCommand>().CheckNotNull();
        }

        public DeleteEntityCommand<T> GetDeleteCommand<T>() where T : class, IEntity
        {
            return _container.Resolve<DeleteEntityCommand<T>>().CheckNotNull();
        }

        #endregion

        #region IQueryFactory implementation

        public IQuery<TEntity, IExpressionSpecification<TEntity>> GetQuery<TEntity>()
            where TEntity : class, IEntity
        {
            return _container.Resolve<IQuery<TEntity, IExpressionSpecification<TEntity>>>()
                .CheckNotNull();
        }

        public IQuery<TEntity, TSpecification> GetQuery<TEntity, TSpecification>()
            where TEntity : class, IEntity
            where TSpecification : ISpecification<TEntity>
        {
            return _container.Resolve<IQuery<TEntity, TSpecification>>()
                .CheckNotNull();
        }

        public TQuery GetQuery<TEntity, TSpecification, TQuery>()
            where TEntity : class, IEntity
            where TSpecification : ISpecification<TEntity>
            where TQuery : IQuery<TEntity, TSpecification>
        {
            return _container.Resolve<TQuery>()
                .CheckNotNull();
        }

        #endregion
    }
}
