using System.Collections.Generic;
using CosteffectiveCode.Domain.Ddd.Entities;
using CosteffectiveCode.Domain.Ddd.Specifications;
using JetBrains.Annotations;

namespace CosteffectiveCode.Domain.Cqrs.Queries
{
    [PublicAPI]
    public interface IQuery<out TResult>
    {
        TResult Execute();
    }

    [PublicAPI]
    public interface ISpecificationQuery<TSource, in TSpecification, out TResult> : IQuery<TResult>
        where TSpecification: ISpecification<TSource>
    {
        ISpecificationQuery<TSource, TSpecification, TResult> Where([NotNull] TSpecification specification);
    }

    [PublicAPI]
    public interface IEntityQuery<TEntity, in TSpecification, out TResult>
        : ISpecificationQuery<TEntity, TSpecification, IEnumerable<TResult>>
        where TEntity : class, IEntity
        where TSpecification : ISpecification<TEntity>
    {
        [NotNull]
        TResult Single();

        [CanBeNull]
        TResult FirstOrDefault();

        bool Any();

        /// <param name="pageNumber">starting 0</param>
        /// <param name="take"></param>
        /// <returns></returns>
        [NotNull]
        IPagedEnumerable<TResult> Paged(int pageNumber, int take);

        long Count();
    }
}
