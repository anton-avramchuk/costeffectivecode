using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CostEffectiveCode.Domain.Ddd.Entities;
using CostEffectiveCode.Domain.Ddd.Specifications;
using JetBrains.Annotations;

namespace CostEffectiveCode.Domain.Cqrs.Queries
{
    [PublicAPI]
    public interface IQuery<TEntity, in TSpecification>
        where TEntity : class, IEntity
        where TSpecification : ISpecification<TEntity>
    {
        IQuery<TEntity, TSpecification> Where([NotNull] TSpecification specification);

        IQuery<TEntity, TSpecification> OrderBy<TProperty>(
           [NotNull] Expression<Func<TEntity, TProperty>> expression,
           SortOrder sortOrder = SortOrder.Asc);

        IQuery<TEntity, TSpecification> Include<TProperty>(
            [NotNull] Expression<Func<TEntity, TProperty>> expression);

        [NotNull]
        TEntity Single();

        [CanBeNull]
        TEntity FirstOrDefault();

        [NotNull]
        IEnumerable<TEntity> All();

        bool Any();

		/// <param name="pageNumber">starting 0</param>
		/// <param name="take"></param>
		/// <returns></returns>
        [NotNull]
        IPagedEnumerable<TEntity> Paged(int pageNumber, int take);       

	    long Count();
    }
}
