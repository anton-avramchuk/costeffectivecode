using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CostEffectiveCode.Ddd;
using CostEffectiveCode.Ddd.Specifications;
using JetBrains.Annotations;

namespace CostEffectiveCode.Extensions
{
    [PublicAPI]
    public interface IPagedEnumerable<out T> : IEnumerable<T>
    {
        /// <summary>
        /// Total number of entries across all pages.
        /// </summary>
        long TotalCount { get; }
    }

    [PublicAPI]
    public static class Paged
    {
        public static IOrderedQueryable<T> Paginate<T, TKey>(this IQueryable<T> queryable, IPaging<T, TKey> paging)
            where T : class
            => paging.OrderBy.SortOrder == SortOrder.Asc
                ? queryable.OrderBy(paging.OrderBy.Expression)
                : queryable.OrderByDescending(paging.OrderBy.Expression);

        public static IPagedEnumerable<T> From<T>(IEnumerable<T> inner, int totalCount)
            =>  new PagedEnumerable<T>(inner, totalCount);

        public static IPagedEnumerable<T> Empty<T>()
             =>  From(Enumerable.Empty<T>(), 0);
    }

    public class PagedEnumerable<T> : IPagedEnumerable<T>
    {
        private readonly IEnumerable<T> _inner;
        private readonly int _totalCount;

        public PagedEnumerable(IEnumerable<T> inner, int totalCount)
        {
            _inner = inner;
            _totalCount = totalCount;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public long TotalCount => _totalCount;
    }
}
