using Akka.Actor;
using CostEffectiveCode.Akka.Messages;
using CostEffectiveCode.Common;
using CostEffectiveCode.Common.Logger;
using CostEffectiveCode.Domain.Cqrs.Queries;
using CostEffectiveCode.Domain.Ddd.Entities;
using CostEffectiveCode.Domain.Ddd.Specifications;
using JetBrains.Annotations;

namespace CostEffectiveCode.Akka.Actors
{
    // ReSharper disable once PossibleInfiniteInheritance
    public class QueryActor<TEntity> : QueryActor<TEntity, IExpressionSpecification<TEntity>>
        where TEntity : class, IEntity
    {
        public QueryActor([NotNull] IScope<IQuery<TEntity, IExpressionSpecification<TEntity>>> queryScope) : base(queryScope)
        {
            ReceiveAdditional();
        }

        public QueryActor([NotNull] IQueryFactory query) : base(query)
        {
            ReceiveAdditional();
        }

        public QueryActor([NotNull] IScope<IQuery<TEntity, IExpressionSpecification<TEntity>>> queryScope, [CanBeNull] ICanTell receiver, [CanBeNull] ILogger logger) : base(queryScope, receiver, logger)
        {
            ReceiveAdditional();
        }

        public QueryActor([NotNull] IQueryFactory queryFactory, [CanBeNull] ICanTell receiver, [CanBeNull] ILogger logger) : base(queryFactory, receiver, logger)
        {
            ReceiveAdditional();
        }

        private void ReceiveAdditional()
        {
            Receive<FetchRequestMessage<TEntity>>(x => Fetch(x));
        }

        protected override IQuery<TEntity, IExpressionSpecification<TEntity>> ResolveQueryFromFactory(IQueryFactory queryFactory)
        {
            return queryFactory.GetQuery<TEntity>();
        }
    }

    public class QueryActor<TEntity, TSpecification> :
        QueryActor<TEntity, TSpecification, IQuery<TEntity, TSpecification>>
        where TEntity : class, IEntity
        where TSpecification : ISpecification<TEntity>
    {
        public QueryActor([NotNull] IScope<IQuery<TEntity, TSpecification>> queryScope) : base(queryScope)
        {
            ReceiveAdditional();
        }

        public QueryActor([NotNull] IQueryFactory query) : base(query)
        {
            ReceiveAdditional();
        }

        public QueryActor([NotNull] IScope<IQuery<TEntity, TSpecification>> queryScope, [CanBeNull] ICanTell receiver, [CanBeNull] ILogger logger) : base(queryScope, receiver, logger)
        {
            ReceiveAdditional();
        }

        public QueryActor([NotNull] IQueryFactory queryFactory, [CanBeNull] ICanTell receiver, [CanBeNull] ILogger logger) : base(queryFactory, receiver, logger)
        {
            ReceiveAdditional();
        }

        private void ReceiveAdditional()
        {
            Receive<FetchRequestMessage<TEntity, TSpecification>>(x => Fetch(x));
        }

        protected override IQuery<TEntity, TSpecification> ResolveQueryFromFactory(IQueryFactory queryFactory)
        {
            return queryFactory.GetQuery<TEntity, TSpecification>();
        }
    }

}
