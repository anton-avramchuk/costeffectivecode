using System.Collections.Generic;
using JetBrains.Annotations;

namespace CostEffectiveCode.Domain.Ddd
{
    [PublicAPI]
    public class PagedList<T> : List<T>, IPagedEnumerable<T>
    {
        public long TotalCount { get; private set; }

        public PagedList(int totalCount)
        {
            TotalCount = totalCount;
        }
    }
}
