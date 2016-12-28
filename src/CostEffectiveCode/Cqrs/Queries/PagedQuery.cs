using System.Collections.Generic;
using System.Linq;
using CostEffectiveCode.Common;
using CostEffectiveCode.Ddd;
using CostEffectiveCode.Ddd.Entities;
using CostEffectiveCode.Ddd.Pagination;

namespace CostEffectiveCode.Cqrs.Queries
{
    public class PagedQuery<TSortKey, TSpec, TEntity, TDto> : ProjectionQuery<TSpec, TEntity, TDto>,
        IQuery<TSpec, IPagedEnumerable<TDto>> 
        where TEntity : class, IHasId
        where TDto : class, IHasId
        where TSpec : IPaging<TDto, TSortKey>
    {
        public PagedQuery(ILinqProvider linqProvider, IProjector projector)
            : base(linqProvider, projector)
        {
        }

        public override IEnumerable<TDto> Ask(TSpec spec)
            => GetQueryable(spec).Paginate(spec).ToArray();

        IPagedEnumerable<TDto> IQuery<TSpec, IPagedEnumerable<TDto>>.Ask(TSpec spec)
            => GetQueryable(spec).ToPagedEnumerable(spec);

        public IQuery<TSpec, IPagedEnumerable<TDto>> AsPaged() => this as IQuery<TSpec, IPagedEnumerable<TDto>>;
    }
}
