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
        : IQueryConstraints<TEntity, TSpecification, IQuery<TEntity, TSpecification>>
        where TEntity : class, IEntity
        where TSpecification : ISpecification<TEntity>
    {

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

        [NotNull]
        IEnumerable<TEntity> Take(int count); 

	    long Count();
    }
}
