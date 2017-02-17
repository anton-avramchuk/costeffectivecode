using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CostEffectiveCode.Extensions
{
    public static class QueryableExtention
    {
        public static IQueryable<T> OrderIf<T, TKey>(this IQueryable<T> query, bool condition, bool acs, Expression<Func<T, TKey>> keySelector)
        {
            if (!condition)
                return query;
            return acs ? query.OrderBy(keySelector) : query.OrderByDescending(keySelector);
        }

        public static IQueryable<T> OrderThenIf<T, TKey>(this IQueryable<T> query, bool condition, bool acs, Expression<Func<T, TKey>> keySelector)
        {
            if (!condition)
                return query;
            return acs ? (IQueryable<T>)((IOrderedQueryable<T>)query).ThenBy<T, TKey>(keySelector) : (IQueryable<T>)((IOrderedQueryable<T>)query).ThenByDescending<T, TKey>(keySelector);
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            return condition ? query.Where<T>(predicate) : query;
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicateIfTrue, Expression<Func<T, bool>> predicateIfFalse)
        {
            return condition ? query.Where<T>(predicateIfTrue) : query.Where<T>(predicateIfFalse);
        }


        public static bool In<T>(this T value, params T[] values)
        {
            return values != null && ((IEnumerable<T>)values).Contains<T>(value);
        }

        public static bool In<T>(this T value, IQueryable<T> values)
        {
            return values != null && values.Contains<T>(value);
        }

        public static bool In<T>(this T value, IEnumerable<T> values)
        {
            return values != null && values.Contains<T>(value);
        }

        public static bool NotIn<T>(this T value, params T[] values)
        {
            return values == null || !((IEnumerable<T>)values).Contains<T>(value);
        }

        public static bool NotIn<T>(this T value, IQueryable<T> values)
        {
            return values == null || !values.Contains<T>(value);
        }

        public static bool NotIn<T>(this T value, IEnumerable<T> values)
        {
            return values == null || !values.Contains<T>(value);
        }
    }
}
