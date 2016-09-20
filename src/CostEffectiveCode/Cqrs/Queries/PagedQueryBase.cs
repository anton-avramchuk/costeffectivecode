using System.Linq;
using CostEffectiveCode.Common;
using CostEffectiveCode.Ddd.Entities;
using CostEffectiveCode.Ddd.Specifications;
using CostEffectiveCode.Ddd.Specifications.UnitOfWork;
using CostEffectiveCode.Extensions;

namespace CostEffectiveCode.Cqrs.Queries
{
    public class PagedQueryBase<TSpec, TEntity, TDto> : LinqQuery<TSpec, TEntity, TDto>,
        IQuery<TSpec, IPagedEnumerable<TDto>> 
        where TEntity : class, IEntity<int>
        where TDto : class, IEntity<int>
        where TSpec : ILinqSpecification<TDto>, IPagedSpecification<TDto>
    {
        public PagedQueryBase(ILinqProvider linqProvier, IProjector projector)
            : base(linqProvier, projector)
        {
        }

        protected override IQueryable<TDto> GetQueryable(TSpec spec)
            => spec.Apply(Project(LinqProvider.GetQueryable<TEntity>())).OrderByDescending(x => x.Id);

        IPagedEnumerable<TDto> IQuery<TSpec, IPagedEnumerable<TDto>>.Execute(TSpec specification)
            => PagedEnumerable.From(Execute(specification), GetQueryable(specification).Count());
    }
}
