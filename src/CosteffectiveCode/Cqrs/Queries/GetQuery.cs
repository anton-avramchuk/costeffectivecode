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
        where TEntity : class, IEntityBase<TKey>
        where TResult : IEntityBase<TKey>
    {
        private readonly ILinqProvider _linqProvider;

        private readonly IProjector _projector;

        public GetQuery([NotNull] ILinqProvider linqProvider, [NotNull] IProjector projector)
        {
            if (linqProvider == null) throw new ArgumentNullException(nameof(linqProvider));
            if (projector == null) throw new ArgumentNullException(nameof(projector));
            _linqProvider = linqProvider;
            _projector = projector;
        }

        public TResult Execute(TKey specification) =>
            _projector.Project<TEntity, TResult>(_linqProvider
                .GetQueryable<TEntity>()
                .Where(x => specification.Equals(x.Id)))
            .SingleOrDefault();
    }
}
