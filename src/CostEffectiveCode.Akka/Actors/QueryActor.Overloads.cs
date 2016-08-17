using Akka.Actor;
using CosteffectiveCode.Common;
using CosteffectiveCode.Domain.Cqrs.Queries;
using CosteffectiveCode.Domain.Ddd.Entities;
using CosteffectiveCode.Domain.Ddd.Specifications;
using CostEffectiveCode.Akka.Messages;
using JetBrains.Annotations;

namespace CostEffectiveCode.Akka.Actors
{
    // ReSharper disable once PossibleInfiniteInheritance
    //public class QueryActor<TEntity, TResult> : QueryActor<TEntity, ExpressionSpecification<TEntity>, TResult>
    //    where TEntity : class, IEntity
    //{
    //    public QueryActor([NotNull] IScope<ISpecificationQuery<TEntity, ExpressionSpecification<TEntity>, TResult>> queryScope) : base(queryScope)
    //    {
    //        ReceiveAdditional();
    //    }

    //    public QueryActor([NotNull] IQueryFactory query) : base(query)
    //    {
    //        ReceiveAdditional();
    //    }


    //    public QueryActor([NotNull] IQueryFactory queryFactory, [CanBeNull] ICanTell receiver, [CanBeNull] ILogger logger) : base(queryFactory, receiver, logger)
    //    {
    //        ReceiveAdditional();
    //    }

    //    private void ReceiveAdditional()
    //    {
    //        Receive<FetchRequestMessage<TEntity>>(x => Execute(x));
    //    }

    //    protected override IQuery<TEntity, ExpressionSpecification<TEntity>> ResolveQueryFromFactory(IQueryFactory queryFactory)
    //    {
    //        return queryFactory.GetQuery<TEntity>();
    //    }
    //}

    //public class QueryActor<TEntity, TSpecification> :
    //    QueryActor<TEntity, TSpecification, IQuery<TEntity, TSpecification>>
    //    where TEntity : class, IEntity
    //    where TSpecification : ISpecification<TEntity>
    //{
    //    public QueryActor([NotNull] IScope<IQuery<TEntity, TSpecification>> queryScope) : base(queryScope)
    //    {
    //        ReceiveAdditional();
    //    }

    //    public QueryActor([NotNull] IQueryFactory query) : base(query)
    //    {
    //        ReceiveAdditional();
    //    }

    //    public QueryActor([NotNull] IQuery<TEntity, TSpecification> query)
    //        : base(query)
    //    {
    //        ReceiveAdditional();
    //    }

    //    public QueryActor([NotNull] IScope<IQuery<TEntity, TSpecification>> queryScope, [CanBeNull] ICanTell receiver, [CanBeNull] ILogger logger) : base(queryScope, receiver, logger)
    //    {
    //        ReceiveAdditional();
    //    }

    //    public QueryActor([NotNull] IQueryFactory queryFactory, [CanBeNull] ICanTell receiver, [CanBeNull] ILogger logger) : base(queryFactory, receiver, logger)
    //    {
    //        ReceiveAdditional();
    //    }

    //    private void ReceiveAdditional()
    //    {
    //        Receive<FetchRequestMessage<TEntity, TSpecification>>(x => Fetch(x));
    //    }

    //    protected override IQuery<TEntity, TSpecification> ResolveQueryFromFactory(IQueryFactory queryFactory)
    //    {
    //        return queryFactory.GetQuery<TEntity, TSpecification>();
    //    }
    //}

}
