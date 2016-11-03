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
            return acs ? (IQueryable<T>)query.OrderBy<T, TKey>(keySelector) : (IQueryable<T>)query.OrderByDescending<T, TKey>(keySelector);
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

        public static IQueryable<T> WhereIfContains<T, TProperty>(this IQueryable<T> query, bool condition, Expression<Func<T, TProperty>> propertyExpression, IEnumerable<TProperty> array, Expression<Func<T, bool>> additionalExpression = null)
        {
            return condition ? query.WhereContains<T, TProperty>(propertyExpression, array, additionalExpression) : query;
        }

        public static IQueryable<T> WhereContains<T, TProperty>(this IQueryable<T> query, Expression<Func<T, TProperty>> propertyExpression, IEnumerable<TProperty> array, Expression<Func<T, bool>> additionalExpression = null)
        {
            int count1 = 0;
            TProperty[] propertyArray = array as TProperty[] ?? array.ToArray<TProperty>();
            int num = ((IEnumerable<TProperty>)propertyArray).Count<TProperty>();
            ParameterExpression parameter = propertyExpression.Parameters[0];
            List<Expression> source = new List<Expression>();
            while (count1 < num)
            {
                int count2 = num - count1;
                if (num > 1000)
                    count2 = 1000;
                List<TProperty> list = ((IEnumerable<TProperty>)propertyArray).Skip<TProperty>(count1).Take<TProperty>(count2).ToList<TProperty>();
                source.Add((Expression)Expression.Call((Expression)Expression.Constant((object)list), list.GetType().GetTypeInfo().GetMethod("Contains"), new Expression[1]
                {
                    propertyExpression.Body
                }));
                count1 += count2;
            }

            if (source.Count > 0)
            {
                Expression wholeExpression = source.First<Expression>();
                foreach (var expr in source.Skip<Expression>(1))
                {
                    wholeExpression = Expression.MakeBinary(ExpressionType.OrElse, wholeExpression, expr);
                }

                Expression<Func<T, bool>> predicate = Expression.Lambda<Func<T, bool>>(wholeExpression, new ParameterExpression[1]
                {
                    parameter
                });

                if (additionalExpression != null)
                    predicate = Expression.Lambda<Func<T, bool>>((Expression)Expression.OrElse(additionalExpression.Body, wholeExpression), new ParameterExpression[1]
                    {
                        parameter
                    });
                query = query.Where<T>(predicate);
            }
            else
            {
                var queryable = additionalExpression != null
                    ? query.Where<T>(additionalExpression)
                    : query.Where<T>((Expression<Func<T, bool>>)(x => false));
                query = queryable;
            }
            return query;
        }

        public static IQueryable<T> WhereNotContains<T, TProperty>(this IQueryable<T> query, Expression<Func<T, TProperty>> propertyExpression, IEnumerable<TProperty> array)
        {
            int count1 = 0;
            TProperty[] propertyArray = array as TProperty[] ?? array.ToArray<TProperty>();
            int num = ((IEnumerable<TProperty>)propertyArray).Count<TProperty>();
            ParameterExpression parameter = propertyExpression.Parameters[0];
            List<Expression> source = new List<Expression>();
            while (count1 < num)
            {
                int count2 = num - count1;
                if (num > 1000)
                    count2 = 1000;
                List<TProperty> list = ((IEnumerable<TProperty>)propertyArray).Skip<TProperty>(count1).Take<TProperty>(count2).ToList<TProperty>();
                source.Add((Expression)Expression.Call((Expression)Expression.Constant((object)list), list.GetType().GetTypeInfo().GetMethod("Contains"), new Expression[1]
                {
          propertyExpression.Body
                }));
                count1 += count2;
            }
            if (source.Count > 0)
            {
                Expression wholeExpression = source.First<Expression>();
                foreach (var expr in source.Skip<Expression>(1))
                {
                    wholeExpression = (Expression)Expression.MakeBinary(ExpressionType.OrElse, wholeExpression, expr);
                }

                UnaryExpression unaryExpression = Expression.Not(wholeExpression);
                query = query.Where<T>(Expression.Lambda<Func<T, bool>>((Expression)unaryExpression, new ParameterExpression[1]
                {
                    parameter
                }));
            }
            return query;
        }
    }
}
