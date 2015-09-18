using System;
using System.Web.Mvc;
using CostEffectiveCode.Domain.Cqrs.Queries;
using CostEffectiveCode.Domain.Ddd.Entities;
using CostEffectiveCode.Domain.Ddd.Specifications;
using CostEffectiveCode.Domain.Ddd.UnitOfWork;
using CostEffectiveCode.Extensions;

namespace CostEffectiveCode.EntityFramework.Domain
{
    public class ExpressionQueryFactory : IQueryFactory
    {
        private readonly ILinqProvider _linqProvider;

        public ExpressionQueryFactory(ILinqProvider linqProvider)
        {
            _linqProvider = linqProvider;
        }

        public IQuery<TEntity, IExpressionSpecification<TEntity>> GetQuery<TEntity>()
            where TEntity : class, IEntity
        {
            return new ExpressionQuery<TEntity>(_linqProvider);
        }

        public IQuery<TEntity, TSpecification> GetQuery<TEntity, TSpecification>() where TEntity : class, IEntity where TSpecification : ISpecification<TEntity>
        {
            throw new NotSupportedException();
        }

        public TQuery GetQuery<TEntity, TSpecification, TQuery>() where TEntity : class, IEntity where TSpecification : ISpecification<TEntity> where TQuery : IQuery<TEntity, TSpecification>
        {
            throw new NotSupportedException();
        }

        public TQuery GetSpecificQuery<TQuery>() where TQuery : ISpecificQuery
        {
            throw new NotSupportedException();
        }
    }
}
