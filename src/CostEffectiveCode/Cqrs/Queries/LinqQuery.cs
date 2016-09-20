using System.Linq;
using CostEffectiveCode.Common;
using CostEffectiveCode.Ddd.Entities;
using CostEffectiveCode.Ddd.Specifications;
using CostEffectiveCode.Ddd.Specifications.UnitOfWork;
using JetBrains.Annotations;

namespace CostEffectiveCode.Cqrs.Queries
{
    public class LinqQuery<TSpecification, TEntity, TDto> : ProjectionQuery<TSpecification, TEntity, TDto>
        where TEntity : class, IEntity
        where TSpecification : ILinqSpecification<TDto>
        where TDto : IEntity
    {
        protected LinqQuery([NotNull] ILinqProvider linqProvider, [NotNull] IProjector projector)
            : base(linqProvider, projector) {}

        protected override IQueryable<TDto> GetQueryable(TSpecification specification)
            => specification.Apply(Project(LinqProvider.GetQueryable<TEntity>()));
    }

    public class LinqQuery<TEntity, TDto> :
        LinqQuery<PagedExpressionSpecification<TDto>, TEntity, TDto>
        where TEntity : class, IEntity
        where TDto : class, IEntity
    {
        public LinqQuery([NotNull] ILinqProvider linqProvider, [NotNull] IProjector projector)
            : base(linqProvider, projector) {}
    }
}
