using CostEffectiveCode.Common;
using CostEffectiveCode.Ddd;
using CostEffectiveCode.Ddd.Entities;
using CostEffectiveCode.Ddd.Pagination;

namespace CostEffectiveCode.Cqrs.Queries
{
    public class PagedQuery<TSource, TDest, TSortKey>
        : ProjectionQuery<TSource, TDest>
        , IQuery<IPaging<TDest, TSortKey>, IPagedEnumerable<TDest>>

        where TSource : class, IHasId
        where TDest : class
    {
        public PagedQuery(ILinqProvider linqProvider, IProjector projector)
            : base(linqProvider, projector)
        {}

        public IPagedEnumerable<TDest> Ask(IPaging<TDest, TSortKey> spec)
            => Query(spec).ToPagedEnumerable(spec);
    }


    public class PagedQuery<TSource, TDest>
        : PagedQuery<TSource, TDest, int>
        , IQuery<IPaging, IPagedEnumerable<TDest>>
        where TSource : class, IHasId
        where TDest : class
    {
        public PagedQuery(ILinqProvider linqProvider, IProjector projector) : base(linqProvider, projector)
        {}

        public IPagedEnumerable<TDest> Ask(IPaging spec)
            => Query(spec).ToPagedEnumerable(spec);
    }
}
