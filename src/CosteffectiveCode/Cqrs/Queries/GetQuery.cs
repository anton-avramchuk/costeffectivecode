using System;
using System.Linq;
using CostEffectiveCode.Common;
using CostEffectiveCode.Ddd.Entities;
using CostEffectiveCode.Ddd.Specifications.UnitOfWork;
using JetBrains.Annotations;

namespace CostEffectiveCode.Cqrs.Queries
{
    public class GetQuery<TKey, TEntity, TResult> : IQuery<TKey, TResult>
        where TKey : struct, IComparable, IComparable<TKey>, IEquatable<TKey>
        where TEntity : class, IHasId<TKey>
        where TResult : IHasId<TKey>
    {
        protected readonly ILinqProvider LinqProvider;

        protected readonly IProjector Projector;

        public GetQuery([NotNull] ILinqProvider linqProvider, [NotNull] IProjector projector)
        {
            if (linqProvider == null) throw new ArgumentNullException(nameof(linqProvider));
            if (projector == null) throw new ArgumentNullException(nameof(projector));

            LinqProvider = linqProvider;
            Projector = projector;
        }

        public virtual TResult Ask(TKey specification) =>
            Projector.Project<TEntity, TResult>(LinqProvider
                .GetQueryable<TEntity>()
                .Where(x => specification.Equals(x.Id)))
            .SingleOrDefault();
    }
}
