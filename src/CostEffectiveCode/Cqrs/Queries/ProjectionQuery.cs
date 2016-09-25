using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CostEffectiveCode.Common;
using CostEffectiveCode.Ddd.Entities;
using CostEffectiveCode.Ddd.UnitOfWork;
using CostEffectiveCode.Extensions;
using JetBrains.Annotations;

namespace CostEffectiveCode.Cqrs.Queries
{
    public class ProjectionQuery<TSpecification, TSource, TDest>
        : IQuery<TSpecification, IEnumerable<TDest>>, IQuery<TSpecification, int>
        where TSource : class, IHasId
        where TDest : class
    {
        protected readonly ILinqProvider LinqProvider;
        protected readonly IProjector Projector;

        protected ProjectionQuery([NotNull] ILinqProvider linqProvier, [NotNull] IProjector projector)
        {
            if (linqProvier == null) throw new ArgumentNullException(nameof(linqProvier));
            if (projector == null) throw new ArgumentNullException(nameof(projector));

            LinqProvider = linqProvier;
            Projector = projector;
        }

        protected virtual IQueryable<TDest> GetQueryable(TSpecification spec)
        => LinqProvider
            .GetQueryable<TSource>()
            .ApplyIfPossible(spec)
            .Project<TSource, TDest>(Projector)
            .ApplyIfPossible(spec);

        public virtual IEnumerable<TDest> Ask(TSpecification specification) => GetQueryable(specification).ToArray();

        int IQuery<TSpecification, int>.Ask(TSpecification specification) => GetQueryable(specification).Count();
    }
}
