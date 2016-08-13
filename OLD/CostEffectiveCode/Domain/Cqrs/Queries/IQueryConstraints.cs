using System;
using System.Linq.Expressions;
using CostEffectiveCode.Domain.Ddd.Entities;
using CostEffectiveCode.Domain.Ddd.Specifications;
using JetBrains.Annotations;

namespace CostEffectiveCode.Domain.Cqrs.Queries
{
    // looks more like a queryable for now
    public interface IQueryConstraints<TEntity, in TSpecification, out TReturn>
        where TEntity : class, IEntity
        where TSpecification : ISpecification<TEntity>
    {
        TReturn Where([NotNull] TSpecification specification);

        TReturn OrderBy<TProperty>(
           [NotNull] Expression<Func<TEntity, TProperty>> expression,
           SortOrder sortOrder = SortOrder.Asc);

        TReturn Include<TProperty>(
            [NotNull] Expression<Func<TEntity, TProperty>> expression);
    }
}
