using System.Web.Mvc;
using CostEffectiveCode.Domain.Cqrs.Commands;
using CostEffectiveCode.Domain.Cqrs.Queries;
using CostEffectiveCode.Domain.Ddd.Entities;
using CostEffectiveCode.Domain.Ddd.Specifications;
using CostEffectiveCode.Extensions;
using JetBrains.Annotations;

namespace CostEffectiveCode.EntityFramework.Domain
{
    [UsedImplicitly]
    public class CommandQueryFactory : ICommandFactory, IQueryFactory
    {
        #region ICommandFactory Implementation

        public TCommand GetCommand<TEntity, TCommand>() where TCommand : ICommand<TEntity>
        {
            return DependencyResolver.Current.GetService<TCommand>().CheckNotNull();
        }

        public T GetCommand<T>() where T:ICommand
        {
            return DependencyResolver.Current.GetService<T>().CheckNotNull();
        }

        public CreateEntityCommand<T> GetCreateCommand<T>() where T : class, IEntity
        {
            var createEntityCommand = DependencyResolver.Current.GetService<CreateEntityCommand<T>>();

            return createEntityCommand.CheckNotNull();
        }

        public DeleteEntityCommand<T> GetDeleteCommand<T>() where T : class, IEntity
        {
            return DependencyResolver.Current.GetService<DeleteEntityCommand<T>>().CheckNotNull();
        }

        #endregion

        #region IQueryFactory implementation

        public IQuery<TEntity,IExpressionSpecification<TEntity>> GetQuery<TEntity>()
            where TEntity : class, IEntity
        {
            return DependencyResolver.Current
                .GetService<IQuery<TEntity, IExpressionSpecification<TEntity>>>()
                .CheckNotNull();
        }

        public IQuery<TEntity, TSpecification> GetQuery<TEntity, TSpecification>()
            where TEntity : class, IEntity
            where TSpecification : ISpecification<TEntity>
        {
            return DependencyResolver.Current
                .GetService<IQuery<TEntity, TSpecification>>()
                .CheckNotNull();
        }

        public TQuery GetQuery<TEntity, TSpecification, TQuery>()
            where TEntity : class, IEntity
            where TSpecification : ISpecification<TEntity>
            where TQuery: IQuery<TEntity, TSpecification>
        {
            return DependencyResolver.Current
                .GetService<TQuery>()
                .CheckNotNull();
        }

        public ISpecificQuery<TResult, TFilter> GetSpecificQuery<TQuery, TResult, TFilter>() where TQuery : ISpecificQuery<TResult, TFilter>
        {
            return DependencyResolver.Current.GetService<TQuery>();
        }

        public TQuery GetSpecificQuery<TQuery>()
            where TQuery : ISpecificQuery
        {
            return DependencyResolver.Current.GetService<TQuery>().CheckNotNull();
        }


        #endregion
    }
}
