using System;
using CostEffectiveCode.Cqrs.Queries;
using CostEffectiveCode.Ddd.Entities;
using CostEffectiveCode.Ddd.Specifications;
using CostEffectiveCode.Extensions;

namespace CostEffectiveCode.FactoryExtensions
{
    public static class QueryFactoryExtensions
    {
        //public static TResult Execute<TSpecification, TResult, TQuery>(
        //    this IQueryFactory queryFactory,
        //    TSpecification specification)
        //    where TQuery : IQuery<TSpecification, TResult>
        //=> queryFactory.GetQuery<TSpecification, TResult, TQuery>().Execute(specification);

        //public static TResult Get<TKey, TEntity, TResult>(
        //    this IQueryFactory queryFactory,
        //    TKey specification)
        //    where TKey : struct, IComparable, IComparable<TKey>, IEquatable<TKey>
        //    where TEntity : class, IEntityBase<TKey>
        //    where TResult : IEntityBase<TKey>
        //=> Execute<TKey, TResult, GetQuery<TKey, TEntity, TResult>>(queryFactory, specification);

        //public static IPagedEnumerable<TDto> Get<TPagedSpecification, TEntity, TDto, TQuery>(
        //    this IQueryFactory queryFactory,
        //    TPagedSpecification specification)
        //    where TPagedSpecification : IPagedSpecification<TDto>
        //    where TEntity : class, IEntity
        //    where TDto : class, IEntity
        //    where TQuery : PagedEntityToDtoQuery<TPagedSpecification, TEntity, TDto>
        //=> Execute<TPagedSpecification, IPagedEnumerable<TDto>, TQuery>(queryFactory, specification);
    }
}
