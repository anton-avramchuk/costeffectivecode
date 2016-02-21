using System;
using System.Threading.Tasks;
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

    public class QueryActor<TEntity, TSpecification> : ReceiveActor
        where TEntity : class, IEntity
        where TSpecification : ISpecification<TEntity>
    {
        private readonly IQuery<TEntity, TSpecification> _query;
        private readonly ICanTell _receiver;
        private readonly ILogger _logger;

        public QueryActor([NotNull] IQuery<TEntity, TSpecification> query, [CanBeNull] ICanTell receiver,
            [CanBeNull] ILogger logger)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            _query = query;

            _receiver = receiver;
            _logger = logger;

            Receive<FetchRequestMessage>(x => Fetch(x));
        }

        private void Fetch(FetchRequestMessage requestMessage)
        {
            _logger.Debug("Received fetch-message");

            var dest = _receiver ?? Context.Sender;

            FetchResponseMessage<TEntity> responseMessage;

            if (requestMessage.Single)
                responseMessage = new FetchResponseMessage<TEntity>(
                    _query.Single());
            else if (requestMessage.Limit != null && requestMessage.Page != null)
                responseMessage =
                    new FetchResponseMessage<TEntity>(
                        _query.Paged(requestMessage.Page.Value, requestMessage.Limit.Value));
            else
                responseMessage = new FetchResponseMessage<TEntity>(
                    _query.All());

            _logger.Debug($"Told the response to {dest}");
            dest.Tell(responseMessage, Context.Self);
        }
    }
}
