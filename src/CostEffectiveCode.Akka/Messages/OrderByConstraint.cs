using System;
using System.Linq.Expressions;
using CosteffectiveCode.Domain.Cqrs.Queries;
using CosteffectiveCode.Domain.Ddd.Entities;

namespace CostEffectiveCode.Akka.Messages
{
    public class OrderByConstraint<TEntity, TProperty>
        where TEntity : IEntity
    {
        public OrderByConstraint(
            Expression<Func<TEntity, TProperty>> expression,
            SortOrder sortOrder = SortOrder.Asc)
        {
            Expression = expression;
            SortOrder = sortOrder;
        }

        public Expression<Func<TEntity, TProperty>> Expression { get; private set; }

        public SortOrder SortOrder { get; private set; }
    }
}