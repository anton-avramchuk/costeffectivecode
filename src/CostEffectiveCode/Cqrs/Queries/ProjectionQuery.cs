using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CostEffectiveCode.Common;
using CostEffectiveCode.Ddd.Entities;
using CostEffectiveCode.Ddd.Specifications.UnitOfWork;
using JetBrains.Annotations;

namespace CostEffectiveCode.Cqrs.Queries
{
    public abstract class ProjectionQuery<TSpecification, TSource, TDest>
        : IQuery<TSpecification, IEnumerable<TDest>>, IQuery<TSpecification, int>
        where TSource : class, IEntity
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

        protected abstract IQueryable<TDest> GetQueryable(TSpecification specification);

        protected IQueryable<TDest> Project(IQueryable<TSource> queryable) => Projector.Project<TSource, TDest>(queryable);

        public IEnumerable<TDest> Execute(TSpecification specification) => GetQueryable(specification).ToArray();

        int IQuery<TSpecification, int>.Execute(TSpecification specification) => GetQueryable(specification).Count();
    }

    public class ProjectionQuery<TEntity, TResult> : ProjectionQuery<Expression<Func<TResult, bool>>, TEntity, TResult>
        where TEntity : class, IEntity
    {
        public ProjectionQuery([NotNull] ILinqProvider linqProvier, [NotNull] IProjector projector)
            : base(linqProvier, projector)
        {
        }

        protected override IQueryable<TResult> GetQueryable(Expression<Func<TResult, bool>> specification)
            => Project(LinqProvider.GetQueryable<TEntity>()).Where(specification);
    }
}
