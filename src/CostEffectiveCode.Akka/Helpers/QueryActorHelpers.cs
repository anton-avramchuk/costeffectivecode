using System;
using System.Linq.Expressions;
using Akka.Actor;
using CostEffectiveCode.Akka.Actors;
using CostEffectiveCode.Common;
using CostEffectiveCode.Common.Logger;
using CostEffectiveCode.Common.Scope;
using CostEffectiveCode.Domain;
using CostEffectiveCode.Domain.Cqrs.Queries;
using CostEffectiveCode.Domain.Ddd.Entities;
using CostEffectiveCode.Domain.Ddd.Specifications;
using JetBrains.Annotations;

namespace CostEffectiveCode.Akka.Helpers
{
    public static class QueryActorHelpers
    {
        public static IActorRef QueryActorOf<TEntity>(
            this IActorRefFactory factory,
            IQuery<TEntity, IExpressionSpecification<TEntity>> query,
            string name = null)
            where TEntity : class, IEntity
        {
            return factory
                .ActorOf(Props.Create(() => new QueryActor<TEntity>(query)), name);
        }

        public static IActorRef QueryActorOf<TEntity>(
            this IActorRefFactory factory,
            IScope<IQuery<TEntity, IExpressionSpecification<TEntity>>> queryScope,
            string name = null)
            where TEntity : class, IEntity
        {
            return factory
                .ActorOf(Props.Create(() => new QueryActor<TEntity>(queryScope)), name);
        }

        public static IActorRef QueryActorOf<TEntity>(
            this IActorRefFactory factory,
            IDiContainer diContainer,
            string name = null)
            where TEntity : class, IEntity
        {
            return factory
                .ActorOf(Props.Create(() => new QueryActor<TEntity>(
                    new DiContainerScope<IQuery<TEntity, IExpressionSpecification<TEntity>>>(
                        diContainer))), name);
        }

        public static IActorRef QueryActorOf<TEntity>(
            this IActorRefFactory factory,
            IDiContainer diContainer,
            Expression<Func<TEntity, bool>> baseFetchFilter = null,
            string actorName = null,
            [CanBeNull]ILogger logger = null)
            where TEntity : class, IEntityBase<long>
        {
            var queryScope = new DelegateScope<IQuery<TEntity, IExpressionSpecification<TEntity>>>(
                () => ResolveQuery(diContainer, baseFetchFilter));

            return factory
                .ActorOf(Props.Create(() => new QueryActor<TEntity>(
                    queryScope, null, logger)), actorName);
        }

        private static IQuery<TEntity, IExpressionSpecification<TEntity>> ResolveQuery<TEntity>(
            IDiContainer diContainer, Expression<Func<TEntity, bool>> baseFetchFilter = null)
            where TEntity : class, IEntityBase<long>
        {
            var query = diContainer
                .Resolve<IQuery<TEntity, IExpressionSpecification<TEntity>>>();

            if (baseFetchFilter != null)
                query = query.Where(baseFetchFilter);

            return query;
        }
    }
}
