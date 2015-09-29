using System;

using CostEffectiveCode.Domain.Cqrs.Commands;
using CostEffectiveCode.Domain.Cqrs.Queries;
using CostEffectiveCode.Domain.Ddd.Entities;
using CostEffectiveCode.Domain.Ddd.Specifications;
using CostEffectiveCode.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace CostEffectiveCode.EntityFramework.Domain
{
    [UsedImplicitly]
    public class CommandQueryFactory : ICommandFactory, IQueryFactory
    {
        /// <summary>
        /// Used for DI container purposes at the moment
        /// </summary>
        private readonly OwinContext _context;

        #region ICommandFactory Implementation

        public CommandQueryFactory([NotNull] OwinContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            _context = context;
        }


        public TCommand GetCommand<TEntity, TCommand>() where TCommand : ICommand<TEntity>
        {
            return _context.Get<TCommand>().CheckNotNull();
        }

        public T GetCommand<T>() where T : ICommand
        {
            return _context.Get<T>().CheckNotNull();
        }

        public CreateEntityCommand<T> GetCreateCommand<T>() where T : class, IEntity
        {
            var createEntityCommand = _context.Get<CreateEntityCommand<T>>();

            return createEntityCommand.CheckNotNull();
        }

        public UpdateEntityCommand<T> GetUpdateCommand<T>() where T : class, IEntity
        {
            var updateEntityCommand = _context.Get<UpdateEntityCommand<T>>();

            return updateEntityCommand.CheckNotNull();
        }

        public DeleteEntityCommand<T> GetDeleteCommand<T>() where T : class, IEntity
        {
            return _context.Get<DeleteEntityCommand<T>>().CheckNotNull();
        }

        #endregion

        #region IQueryFactory implementation

        public IQuery<TEntity, IExpressionSpecification<TEntity>> GetQuery<TEntity>()
            where TEntity : class, IEntity
        {
            return _context.Get<IQuery<TEntity, IExpressionSpecification<TEntity>>>()
                .CheckNotNull();
        }

        public IQuery<TEntity, TSpecification> GetQuery<TEntity, TSpecification>()
            where TEntity : class, IEntity
            where TSpecification : ISpecification<TEntity>
        {
            return _context.Get<IQuery<TEntity, TSpecification>>()
                .CheckNotNull();
        }

        public TQuery GetQuery<TEntity, TSpecification, TQuery>()
            where TEntity : class, IEntity
            where TSpecification : ISpecification<TEntity>
            where TQuery : IQuery<TEntity, TSpecification>
        {
            return _context.Get<TQuery>()
                .CheckNotNull();
        }

        #endregion
    }
}
