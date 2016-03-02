using System;
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
    public class QueryActor<TQuery, TEntity, TSpecification> : ReceiveActor
        where TQuery: class, IQuery<TEntity, TSpecification>
        where TEntity : class, IEntity
        where TSpecification : ISpecification<TEntity>
    {
        private readonly IScope<TQuery> _queryScope;
        private readonly IQueryFactory _queryFactory;

        private readonly ICanTell _receiver;
        private readonly ILogger _logger;

        public QueryActor([NotNull] IScope<TQuery> queryScope)
            : this(queryScope, null, null)
        {
        }

        public QueryActor([NotNull] IQueryFactory query)
            : this(query, null, null)
        {
        }

        public QueryActor([NotNull] IScope<TQuery> queryScope, [CanBeNull] ICanTell receiver,
            [CanBeNull] ILogger logger)
        {
            if (queryScope == null) throw new ArgumentNullException(nameof(queryScope));

            _queryScope = queryScope;
            _queryFactory = null;

            _receiver = receiver;
            _logger = logger;

            Receive<FetchRequestMessage<TEntity, TSpecification>>(x => Fetch(x));
        }

        public QueryActor([NotNull] IQueryFactory queryFactory, [CanBeNull] ICanTell receiver,
            [CanBeNull] ILogger logger)
        {
            if (queryFactory == null) throw new ArgumentNullException(nameof(queryFactory));

            _queryFactory = queryFactory;
            _queryScope = null;

            _receiver = receiver;
            _logger = logger;

            Receive<FetchRequestMessage<TEntity, TSpecification>>(x => Fetch(x));
        }


        protected virtual void Fetch(FetchRequestMessage<TEntity, TSpecification> requestMessage)
        {
            _logger?.Debug("Received fetch-message");

            IQuery<TEntity, TSpecification> query =
                _queryFactory != null
                ? _queryFactory.GetQuery<TEntity, TSpecification, TQuery>()
                : _queryScope.Instance;

            if (query == null)
                throw new InvalidOperationException("Query is unavailable");

            var dest = _receiver ?? Context.Sender;

            FetchResponseMessage<TEntity> responseMessage;

            if (requestMessage.Single)
                responseMessage = new FetchResponseMessage<TEntity>(
                    query.Single());
            else if (requestMessage.Limit != null && requestMessage.Page != null)
                responseMessage =
                    new FetchResponseMessage<TEntity>(
                        query.Paged(requestMessage.Page.Value, requestMessage.Limit.Value));
            else if (requestMessage.Limit != null)
                responseMessage =
                    new FetchResponseMessage<TEntity>(
                        query.Take(requestMessage.Limit.Value));
            else
                responseMessage = new FetchResponseMessage<TEntity>(
                    query.All());

            dest.Tell(responseMessage, Context.Self);
            _logger?.Debug($"Told the response to {dest}");
        }
    }
}
