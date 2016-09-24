using System.Linq;
using CostEffectiveCode.Common;
using CostEffectiveCode.Ddd.Entities;
using CostEffectiveCode.Ddd.Specifications;
using CostEffectiveCode.Ddd.Specifications.UnitOfWork;
using CostEffectiveCode.Extensions;

namespace CostEffectiveCode.Cqrs.Queries
{
    public class PagedQuery<TSpec, TEntity, TDto, TSortKey> : ProjectionQuery<TSpec, TEntity, TDto>,
        IQuery<TSpec, IPagedEnumerable<TDto>> 
        where TEntity : class, IHasId
        where TDto : class, IHasId
        where TSpec : IPaging<TDto, TSortKey>
    {
        public PagedQuery(ILinqProvider linqProvier, IProjector projector)
            : base(linqProvier, projector)
        {
        }

        IPagedEnumerable<TDto> IQuery<TSpec, IPagedEnumerable<TDto>>.Ask(TSpec spec)
            => GetQueryable(spec).ToPagedEnumerable(spec);

        public IQuery<TSpec, IPagedEnumerable<TDto>> AsPaged() => this as IQuery<TSpec, IPagedEnumerable<TDto>>;
    }
}
