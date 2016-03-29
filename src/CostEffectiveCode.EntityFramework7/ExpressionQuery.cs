using System;
using System.Linq.Expressions;
using CostEffectiveCode.Domain.Cqrs.Queries;
using CostEffectiveCode.Domain.Ddd.Entities;
using CostEffectiveCode.Domain.Ddd.Specifications;
using CostEffectiveCode.Domain.Ddd.UnitOfWork;
using Microsoft.Data.Entity;

namespace CostEffectiveCode.EntityFramework7
{
    public class ExpressionQuery<TEntity> : ExpressionQueryBase<TEntity>
        where TEntity : class, IEntity
    {
        public ExpressionQuery(ILinqProvider linqProvider) : base(linqProvider)
        {
        }

        public override IQuery<TEntity, IExpressionSpecification<TEntity>> Include<TProperty>(
            Expression<Func<TEntity, TProperty>> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            Queryable = GetQueryable().Include(expression);

            return this;
        }
    }
}
