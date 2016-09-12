using System.Collections.Generic;
using CostEffectiveCode.Extensions;
using JetBrains.Annotations;

namespace CostEffectiveCode.Ddd
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
