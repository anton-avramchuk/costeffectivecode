using System;
using System.Linq;
using System.Linq.Expressions;
using Akka.Actor;
using CostEffectiveCode.Akka.Messages;
using CostEffectiveCode.Common;
using CostEffectiveCode.Common.Logger;
using CostEffectiveCode.Common.Scope;
using CostEffectiveCode.Domain.Cqrs.Queries;
using CostEffectiveCode.Domain.Ddd.Entities;
using CostEffectiveCode.Domain.Ddd.Specifications;
using JetBrains.Annotations;

namespace CostEffectiveCode.Akka.Actors
{
    public class QueryActor<TEntity, TSpecification, TQuery> : ReceiveActor
        where TQuery : class, IQuery<TEntity, TSpecification>
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
            Receive<FetchRequestMessageBase>(x => FetchBase(x));
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

            var query = ResolveQuery();
            if (query == null)
                throw new InvalidOperationException("Query is unavailable");

            query = AddConstraints(query, requestMessage);

            FetchInner(requestMessage, query);
        }

        protected virtual void FetchBase(FetchRequestMessageBase requestMessage)
        {
            _logger?.Debug("Received fetch-message");

            var query = ResolveQuery();
            if (query == null)
                throw new InvalidOperationException("Query is unavailable");

            // no constraints, Caaarl!

            FetchInner(requestMessage, query);
        }

        private void FetchInner(FetchRequestMessageBase requestMessage, IQuery<TEntity, TSpecification> query)
        {
            var dest = _receiver ?? Context.Sender;

            try
            {

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
            catch (Exception e)
            {
                dest.Tell(new Failure { Exception = e, Timestamp = DateTime.Now }, Context.Self);
                throw;
            }
        }

        private IQuery<TEntity, TSpecification> ResolveQuery()
        {
            IQuery<TEntity, TSpecification> query =
                _queryFactory != null
                    ? ResolveQueryFromFactory(_queryFactory)
                    : _queryScope.Instance;
            return query;
        }

        private IQuery<TEntity, TSpecification> AddConstraints(IQuery<TEntity, TSpecification> query, FetchRequestMessage<TEntity, TSpecification> requestMessage)
        {
            if (requestMessage.IncludeConstraints != null
                && requestMessage.IncludeConstraints.Any())
            {
                foreach (var include in requestMessage.IncludeConstraints)
                    AddIncludeConstraint(query, include);
            }

            if (requestMessage.WhereSpecificationConstraints != null
                && requestMessage.WhereSpecificationConstraints.Any())
            {
                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (var where in requestMessage.WhereSpecificationConstraints)
                    query = query.Where(where);
            }

            if (requestMessage.OrderByConstraints != null
                && requestMessage.OrderByConstraints.Count > 0)
            {
                foreach (var orderBy in requestMessage.OrderByConstraints)
                    AddOrderByConstraint(query, orderBy);
            }

            return query;
        }

        #region BEWARE! ANGRY REFLECTIONS!
        private void AddOrderByConstraint(IQuery<TEntity, TSpecification> query, object orderByConstraint)
        {
            var constraintType = orderByConstraint.GetType();
            var constraintGenericType = constraintType.GetGenericTypeDefinition();

            if (constraintGenericType != typeof(OrderByConstraint<,>) || constraintType.GenericTypeArguments.Length != 2)
                throw new InvalidMessageException("Wrong OrderByConstraint specified in FetchRequestMessage<,>");

            var propertyType = constraintType.GetGenericArguments().ElementAt(1);

            var expression = constraintType
                .GetProperty("Expression")
                .GetGetMethod()
                .Invoke(orderByConstraint, new object[] { });

            var sortOrder = constraintType
                .GetProperty("SortOrder")
                .GetGetMethod()
                .Invoke(orderByConstraint, new object[] { });

            var queryOrderByGenericMethodInfo = typeof(IQueryConstraints<,,>)
                .GetMethod("OrderBy")
                .MakeGenericMethod(propertyType);

            queryOrderByGenericMethodInfo.Invoke(query, new[] { expression, sortOrder });
        }

        private void AddIncludeConstraint(IQuery<TEntity, TSpecification> query, LambdaExpression includeExpression)
        {
            var constraintType = includeExpression.GetType();
            var constraintGenericType = constraintType.GetGenericTypeDefinition();

            if (constraintGenericType != typeof(Expression<>) || constraintType.GenericTypeArguments.Length != 1)
                throw new InvalidMessageException("Wrong IncludeConstraint specified in FetchRequestMessage<,>");

            var funcType = constraintType.GenericTypeArguments.Single();
            if (funcType.GetGenericTypeDefinition() != typeof(Func<,>) && funcType.GenericTypeArguments.Length != 2)
                throw new InvalidMessageException("Wrong delegate type in underlying Expression<> of IncludeConstraint specified in FetchRequestMessage<,>");

            var propertyType = funcType.GenericTypeArguments[1];

            var queryIncludeGenericMethodInfo = typeof(IQueryConstraints<,,>)
                .GetMethod("Include")
                .MakeGenericMethod(propertyType);

            queryIncludeGenericMethodInfo.Invoke(query, new object[] { includeExpression });
        }
        #endregion

        protected virtual TQuery ResolveQueryFromFactory(IQueryFactory queryFactory)
        {
            return _queryFactory.GetQuery<TEntity, TSpecification, TQuery>();
        }
    }
}
