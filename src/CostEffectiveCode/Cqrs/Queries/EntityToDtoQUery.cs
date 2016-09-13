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
    public abstract class EntityToDtoQuery<TSpecification, TEntity, TDto>
        : IQuery<TSpecification, IEnumerable<TDto>>, IQuery<TSpecification, int>
        where TEntity : class, IEntity
    {
        protected readonly ILinqProvider LinqProvider;
        protected readonly IProjector Projector;


        protected EntityToDtoQuery([NotNull] ILinqProvider linqProvier, [NotNull] IProjector projector)
        {
            if (linqProvier == null) throw new ArgumentNullException(nameof(linqProvier));
            if (projector == null) throw new ArgumentNullException(nameof(projector));

            LinqProvider = linqProvier;
            Projector = projector;
        }

        protected abstract IQueryable<TDto> GetQueryable(TSpecification specification);

        protected IQueryable<TDto> Project(IQueryable<TEntity> queryable) => Projector.Project<TEntity, TDto>(queryable);

        public IEnumerable<TDto> Execute(TSpecification specification) => GetQueryable(specification).ToArray();

        int IQuery<TSpecification, int>.Execute(TSpecification specification) => GetQueryable(specification).Count();
    }

    public class EntityToDtoQuery<TEntity, TResult> : EntityToDtoQuery<Expression<Func<TResult, bool>>, TEntity, TResult>
        where TEntity : class, IEntity
    {
        public EntityToDtoQuery([NotNull] ILinqProvider linqProvier, [NotNull] IProjector projector)
            : base(linqProvier, projector)
        {
        }

        protected override IQueryable<TResult> GetQueryable(Expression<Func<TResult, bool>> specification)
            => Project(LinqProvider.GetQueryable<TEntity>()).Where(specification);
    }
}
