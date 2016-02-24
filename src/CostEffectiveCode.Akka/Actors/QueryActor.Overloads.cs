using Akka.Actor;
using CostEffectiveCode.Common.Logger;
using CostEffectiveCode.Domain.Cqrs.Queries;
using CostEffectiveCode.Domain.Ddd.Entities;
using CostEffectiveCode.Domain.Ddd.Specifications;
using JetBrains.Annotations;

namespace CostEffectiveCode.Akka.Actors
{
    public class QueryActor<TEntity> : QueryActor<TEntity, IExpressionSpecification<TEntity>>
        where TEntity : class, IEntity
    {
        public QueryActor([NotNull] IQuery<TEntity, IExpressionSpecification<TEntity>> query, [CanBeNull] ICanTell receiver, [CanBeNull] ILogger logger)
            : base(query, receiver, logger)
        {
        }

        public QueryActor([NotNull] IQueryFactory queryFactory, [CanBeNull] ICanTell receiver, [CanBeNull] ILogger logger)
            : base(queryFactory, receiver, logger)
        {
        }
    }
}
