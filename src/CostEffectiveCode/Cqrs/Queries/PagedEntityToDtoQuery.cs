using System.Linq;
using CostEffectiveCode.Common;
using CostEffectiveCode.Ddd.Entities;
using CostEffectiveCode.Ddd.Specifications;
using CostEffectiveCode.Ddd.Specifications.UnitOfWork;
using CostEffectiveCode.Extensions;
using JetBrains.Annotations;

namespace CostEffectiveCode.Cqrs.Queries
{
    public abstract class PagedEntityToDtoQuery<TSpecification, TEntity, TDto> :
        EntityToDtoQuery<TSpecification, TEntity, TDto>,
        IQuery<TSpecification, IPagedEnumerable<TDto>>
        where TEntity : class, IEntity
        where TDto: class
        where TSpecification : IPagedSpecification<TDto>
    {
        protected PagedEntityToDtoQuery([NotNull] ILinqProvider linqProvider, [NotNull] IProjector projector)
            : base(linqProvider, projector)
        {}

        IPagedEnumerable<TDto>  IQuery<TSpecification, IPagedEnumerable<TDto>>.Execute(TSpecification specification)
        {
            var q = GetQueryable(specification);
            return PagedEnumerable.From(q
                .Skip(specification.Page * specification.Take)
                .Take(specification.Take)
                .ToArray(), q.Count());
        }
    }

    public class PagedEntityToDtoQuery<TEntity, TResult> :
        PagedEntityToDtoQuery<PagedExpressionSpecification<TResult>, TEntity, TResult>
        where TEntity : class, IEntity
        where TResult : class
    {
        public PagedEntityToDtoQuery([NotNull] ILinqProvider linqProvider, [NotNull] IProjector projector)
            : base(linqProvider, projector)
        {}

        protected override IQueryable<TResult> GetQueryable(PagedExpressionSpecification<TResult> specification)
            => Project(LinqProvider.GetQueryable<TEntity>()).Where(specification.Expression);
    }
}
