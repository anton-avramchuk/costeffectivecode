using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using CosteffectiveCode.Domain.Cqrs.Queries;
using CosteffectiveCode.Domain.Ddd.Entities;
using CosteffectiveCode.Domain.Ddd.Specifications;
using JetBrains.Annotations;

namespace CosteffectiveCode.Domain
{
    [PublicAPI]
    public static class Extensions
    {
        #region Dynamic Expression Compilation

        private static readonly ConcurrentDictionary<Expression, object> Cache
            = new ConcurrentDictionary<Expression, object>();

        public static Func<TEntity, bool> AsFunc<TEntity>(this TEntity entity, Expression<Func<TEntity, bool>> expr)
            where TEntity : class, IEntity
        {
            //@see http://sergeyteplyakov.blogspot.ru/2015/06/lazy-trick-with-concurrentdictionary.html
            return ((Lazy<Func<TEntity, bool>>)Cache.GetOrAdd(expr, id => new Lazy<object>(
                    () => Cache.GetOrAdd(id, expr.Compile())))).Value;
        }

        public static bool Is<TEntity>(this TEntity entity, Expression<Func<TEntity, bool>> expr)
            where TEntity : class, IEntity
        {
            return AsFunc(entity, expr).Invoke(entity);
        }

        #endregion

        #region Specifications

        public static ISpecificationQuery<TSource, TSpecification, TResult> WhereIf<TSource, TSpecification, TResult>(
            this ISpecificationQuery<TSource, TSpecification, TResult> query, Func<bool> condition, TSpecification spec)
            where TSpecification: ISpecification<TSource>
        {
            if (condition.Invoke())
            {
                query = query.Where(spec);
            }

            return query;
        }

        #endregion

        #region Query

        public static ISpecificationQuery<TSource, ExpressionSpecification<TSource>, TResult> Where<TSource, TResult>(
            this ISpecificationQuery<TSource, ExpressionSpecification<TSource>, TResult> query,
            Expression<Func<TSource, bool>> expression)
        {
            return query.Where(new ExpressionSpecification<TSource>(expression));
        }


        #endregion

        #region Composite Specifications

        public static ISpecification<T> And<T>(this ISpecification<T> left, ISpecification<T> right)
             where T : class, IEntity
        {
            return new AndSpecification<T>(left, right);
        }
        public static ISpecification<T> Or<T>(this ISpecification<T> left, ISpecification<T> right)
             where T : class, IEntity
        {
            return new OrSpecification<T>(left, right);
        }
        public static ISpecification<T> Not<T>(this ISpecification<T> left)
             where T : class, IEntity
        {
            return new NotSpecification<T>(left);
        }

        #endregion
    }
}
