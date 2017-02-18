﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CostEffectiveCode.Extensions;
using JetBrains.Annotations;

namespace CostEffectiveCode.Ddd.Pagination
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
        public static IOrderedQueryable<TEntity> OrderBy<TEntity, TKey>(this IQueryable<TEntity> queryable
            , Sorting<TEntity, TKey> sorting)
            where TEntity : class
            => sorting.SortOrder == SortOrder.Asc
                ? queryable.OrderBy(sorting.Expression)
                : queryable.OrderByDescending(sorting.Expression);

        public static IOrderedQueryable<TEntity> ThenBy<TEntity, TKey>(this IOrderedQueryable<TEntity> queryable
            , Sorting<TEntity, TKey> sorting)
            where TEntity : class
            => sorting.SortOrder == SortOrder.Asc
                ? queryable.ThenBy(sorting.Expression)
                : queryable.ThenByDescending(sorting.Expression);

        public static IQueryable<T> Paginate<T, TKey>(this IQueryable<T> queryable, IPaging<T, TKey> paging)
            where T : class
        {
            if(!paging.OrderBy.Any())
            {
                throw new ArgumentException("OrderBy can't be null or empty", nameof(paging));
            }

            var ordered = queryable.OrderBy(paging.OrderBy.First());
            ordered = paging.OrderBy.Skip(1).Aggregate(ordered, (current, order) => current.ThenBy(order));

            return ordered
                .Skip((paging.Page - 1) * paging.Take)
                .Take(paging.Take);
        }

        public static IOrderedQueryable<T> Sort<T>(this IQueryable<T> queryable, ILinqSorting<T> paging)
            where T : class
            => paging.Apply(queryable);

        public static IPagedEnumerable<T> ToPagedEnumerable<T, TKey>(this IQueryable<T> queryable,
            IPaging<T, TKey> paging)
            where T : class
            => From(queryable.Paginate(paging).ToArray(), queryable.Count());

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
