using CostEffectiveCode.Domain.Ddd.Entities;
using CostEffectiveCode.Domain.Ddd.Specifications;
using JetBrains.Annotations;

namespace CostEffectiveCode.Domain.Cqrs.Queries
{
    [PublicAPI]
    public interface IQueryFactory
    {
        ISpecificationQuery<TEntity, TSpecification, TResult> GetQuery<TEntity, TSpecification, TResult>()
            where TEntity : class, IEntity
            where TSpecification : ISpecification<TEntity>;

        TQuery GetQuery<TEntity, TSpecification, TResult, TQuery>()
            where TSpecification: ISpecification<TEntity>
            where TQuery : ISpecificationQuery<TEntity, TSpecification, TResult>;
    }
}