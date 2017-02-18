using System;
using System.Linq;
using CostEffectiveCode.Common;
using CostEffectiveCode.Ddd;
using CostEffectiveCode.Ddd.Entities;
using CostEffectiveCode.Extensions;

namespace CostEffectiveCode.Cqrs.Queries
{
    public class GetByIdQuery<TKey, TEntity, TProjection> : IQuery<TKey, TProjection>
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
        where TEntity : class, IHasId<TKey>
        where TProjection : IHasId<TKey>
    {
        protected readonly ILinqProvider LinqProvider;

        protected readonly IProjector Projector;

        public GetByIdQuery(ILinqProvider linqProvider, IProjector projector)
        {
            if (linqProvider == null) throw new ArgumentNullException(nameof(linqProvider));
            if (projector == null) throw new ArgumentNullException(nameof(projector));

            LinqProvider = linqProvider;
            Projector = projector;
        }

        public virtual TProjection Ask(TKey specification) =>
            LinqProvider
                .Query<TEntity>()
                .Where(x => specification.Equals(x.Id))
                .Project<TProjection>(Projector)
                .SingleOrDefault();
    }
}
