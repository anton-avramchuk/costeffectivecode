using System;
using System.Linq;
using Akka.Actor;
using CostEffectiveCode.Akka.Messages;
using CostEffectiveCode.Common.Logger;
using CostEffectiveCode.Domain.Cqrs.Queries;
using CostEffectiveCode.Domain.Ddd.Entities;
using CostEffectiveCode.Domain.Ddd.Specifications;
using JetBrains.Annotations;

namespace CostEffectiveCode.Akka.Actors
{
    public class QueryActor<TEntity, TSpecification> : ReceiveActor
        where TEntity : class, IEntity
        where TSpecification : ISpecification<TEntity>
    {
        private IQuery<TEntity, TSpecification> _query;
        private readonly IQueryFactory _queryFactory;
        private readonly ICanTell _receiver;
        private readonly ILogger _logger;

        public QueryActor([NotNull] IQuery<TEntity, TSpecification> query, [CanBeNull] ICanTell receiver,
            [CanBeNull] ILogger logger)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            _query = query;
            _queryFactory = null;

            _receiver = receiver;
            _logger = logger;

            Receive<FetchRequestMessage>(x => Fetch(x));
        }

        public QueryActor([NotNull] IQueryFactory queryFactory, [CanBeNull] ICanTell receiver,
            [CanBeNull] ILogger logger)
        {
            if (queryFactory == null) throw new ArgumentNullException(nameof(queryFactory));

            _queryFactory = queryFactory;
            _query = null;

            _receiver = receiver;
            _logger = logger;

            Receive<FetchRequestMessage>(x => Fetch(x));
        }


        protected virtual void Fetch(FetchRequestMessage requestMessage)
        {
            _logger?.Debug("Received fetch-message");

            if (_queryFactory != null)
                _query = _queryFactory.GetQuery<TEntity, TSpecification>();

            if (_query == null)
                throw new InvalidOperationException("Query is unavailable");

            // TODO: construct query here using FetchRequestMessage's data

            var dest = _receiver ?? Context.Sender;

            FetchResponseMessage<TEntity> responseMessage;

            if (requestMessage.Single)
                responseMessage = new FetchResponseMessage<TEntity>(
                    _query.Single());
            else if (requestMessage.Limit != null && requestMessage.Page != null)
                responseMessage =
                    new FetchResponseMessage<TEntity>(
                        _query.Paged(requestMessage.Page.Value, requestMessage.Limit.Value));
            else if (requestMessage.Limit != null)
                responseMessage =
                    new FetchResponseMessage<TEntity>(
                        _query.Take(requestMessage.Limit.Value));
            else
                responseMessage = new FetchResponseMessage<TEntity>(
                    _query.All());

            dest.Tell(responseMessage, Context.Self);
            _logger?.Debug($"Told the response to {dest}");
        }
    }
}
