using System.Collections.Generic;

namespace CostEffectiveCode.Domain.Ddd
{
    public class PagedList<T> : List<T>, IPagedEnumerable<T>
    {
        public long TotalCount { get; private set; }

        public PagedList(int totalCount)
        {
            TotalCount = totalCount;
        }
    }
}
