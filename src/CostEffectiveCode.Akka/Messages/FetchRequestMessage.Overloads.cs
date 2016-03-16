using System;
using System.Linq.Expressions;
using CostEffectiveCode.Domain.Cqrs.Queries;
using CostEffectiveCode.Domain.Ddd.Entities;
using CostEffectiveCode.Domain.Ddd.Specifications;

namespace CostEffectiveCode.Akka.Messages
{
    public class FetchRequestMessage<TEntity> : FetchRequestMessage<TEntity, IExpressionSpecification<TEntity>>
        where TEntity : class, IEntity
    {
        public FetchRequestMessage(bool single, bool firstOrDefault) : base(single, firstOrDefault)
        {
        }

        public FetchRequestMessage(int page, int limit) : base(page, limit)
        {
        }

        public FetchRequestMessage(int limit) : base(limit)
        {
        }

        public FetchRequestMessage() : base()
        {
        }

        public FetchRequestMessage<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            var specification = new ExpressionSpecification<TEntity>(expression);

            return (FetchRequestMessage<TEntity>)base.Where(specification);
        }

        public new FetchRequestMessage<TEntity> OrderBy<TProperty>(Expression<Func<TEntity, TProperty>> expression, SortOrder sortOrder = SortOrder.Asc)
        {
            return (FetchRequestMessage<TEntity>)base.OrderBy(expression, sortOrder);
        }

        public new FetchRequestMessage<TEntity> Include<TProperty>(Expression<Func<TEntity, TProperty>> expression)
        {
            return (FetchRequestMessage<TEntity>)base.Include(expression);
        }
    }
}
