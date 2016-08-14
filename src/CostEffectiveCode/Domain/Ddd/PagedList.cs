using System.Collections.Generic;
using JetBrains.Annotations;

namespace CosteffectiveCode.Domain.Ddd
{
    [PublicAPI]
    public class PagedList<T> : List<T>, IPagedEnumerable<T>
    {
        public long TotalCount { get; private set; }

        public PagedList(int totalCount)
        {
            TotalCount = totalCount;
        }

        public PagedList(int totalCount, params T[] values) : base(totalCount)
        {
            AddRange(values);
        }

    }
}
