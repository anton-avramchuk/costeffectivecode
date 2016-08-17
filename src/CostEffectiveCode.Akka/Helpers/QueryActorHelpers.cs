using System;
using System.Linq.Expressions;
using Akka.Actor;
using CosteffectiveCode.Common;
using CosteffectiveCode.Domain.Ddd.Entities;
using CosteffectiveCode.Domain.Ddd.Specifications;
using CostEffectiveCode.Akka.Actors;
using JetBrains.Annotations;

namespace CostEffectiveCode.Akka.Helpers
{
    public static class QueryActorHelpers
    {
        public static IActorRef QueryActorOf<TEntity, TResult>(
            this IActorRefFactory factory,
            IQuery<TEntity, ExpressionSpecification<TEntity>, TResult> query,
            string name = null)
            where TEntity : class, IEntity
        {
            return factory
                .ActorOf(Props.Create(() => new QueryActor<TEntity>(query)), name);
        }

        public static IActorRef QueryActorOf<TEntity, TSpecification>(
            this IActorRefFactory factory,
            IQuery<TEntity, TSpecification> query,
            string name = null)
            where TEntity : class, IEntity
            where TSpecification : ISpecification<TEntity>
        {
            return factory
                .ActorOf(Props.Create(() => new QueryActor<TEntity, TSpecification>(query)), name);
        }

        public static IActorRef QueryActorOf<TEntity>(
            this IActorRefFactory factory,
            IScope<IQuery<TEntity, ExpressionSpecification<TEntity>>> queryScope,
            string name = null)
            where TEntity : class, IEntity
        {
            return factory
                .ActorOf(Props.Create(() => new QueryActor<TEntity>(queryScope)), name);
        }

        public static IActorRef QueryActorOf<TEntity>(
            this IActorRefFactory factory,
            string name = null)
            where TEntity : class, IEntity
        {
            return factory
                .ActorOf(Props.Create(() => new QueryActor<TEntity>(
                    new DiContainerScope<IQuery<TEntity, ExpressionSpecification<TEntity>>>(
                        diContainer))), name);
        }

        public static IActorRef QueryActorOf<TEntity>(
            this IActorRefFactory factory, [NotNull] IDiContainer diContainer,
            [NotNull]Expression<Func<TEntity, bool>> baseFetchFilter,
            string actorName = null,
            [CanBeNull]ILogger logger = null)
            where TEntity : class, IEntityBase<long>
        {
            if (diContainer == null) throw new ArgumentNullException(nameof(diContainer));
            if (baseFetchFilter == null) throw new ArgumentNullException(nameof(baseFetchFilter));

            var queryScope = new DelegateScope<IQuery<TEntity, ExpressionSpecification<TEntity>>>(
                () => ResolveQuery(diContainer, baseFetchFilter));

            return factory
                .ActorOf(Props.Create(() => new QueryActor<TEntity>(
                    queryScope, null, logger)), actorName);
        }

        private static IQuery<TEntity, ExpressionSpecification<TEntity>> ResolveQuery<TEntity>(
            [NotNull] IDiContainer diContainer, [NotNull] Expression<Func<TEntity, bool>> baseFetchFilter)
            where TEntity : class, IEntityBase<long>
        {
            if (diContainer == null) throw new ArgumentNullException(nameof(diContainer));
            if (baseFetchFilter == null) throw new ArgumentNullException(nameof(baseFetchFilter));

            return diContainer
                .Resolve<IQuery<TEntity, ExpressionSpecification<TEntity>>>()
                .Where(baseFetchFilter);
        }
    }
}
