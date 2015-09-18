using CostEffectiveCode.Domain.Ddd.Entities;
using CostEffectiveCode.Domain.Ddd.Specifications;
using JetBrains.Annotations;

namespace CostEffectiveCode.Domain.Cqrs.Queries
{
    [PublicAPI]
    public interface IQueryFactory
    {
        IQuery<TEntity, IExpressionSpecification<TEntity>> GetQuery<TEntity>()
            where TEntity : class, IEntity;

        IQuery<TEntity, TSpecification> GetQuery<TEntity, TSpecification>()
            where TEntity : class, IEntity
            where TSpecification : ISpecification<TEntity>;

        TQuery GetQuery<TEntity, TSpecification, TQuery>()
            where TEntity : class, IEntity
            where TSpecification: ISpecification<TEntity>
            where TQuery : IQuery<TEntity, TSpecification>;


        TQuery GetSpecificQuery<TQuery>()
            where TQuery : ISpecificQuery;
    }
}