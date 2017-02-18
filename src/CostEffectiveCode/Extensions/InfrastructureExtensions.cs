using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CostEffectiveCode.Cqrs;
using JetBrains.Annotations;

namespace CostEffectiveCode.Extensions
{
    [PublicAPI]
    public static class InfrastructureExtensions
    {
        #region Dynamic Expression Compilation

        private static readonly ConcurrentDictionary<Expression, object> Cache
            = new ConcurrentDictionary<Expression, object>();

        public static Func<TIn, TOut> AsFunc<TIn, TOut>(this Expression<Func<TIn, TOut>> expr)
            //@see http://sergeyteplyakov.blogspot.ru/2015/06/lazy-trick-with-concurrentdictionary.html
            => (Func<TIn, TOut>)((Lazy<object>)Cache
                .GetOrAdd(expr, id => new Lazy<object>(expr.Compile))).Value;

        public static bool Is<T>(this T entity, Expression<Func<T, bool>> expr)
            => AsFunc(expr).Invoke(entity);

        #endregion

        #region Async

        public static Task<T> RunTask<T>(this Func<T> func)
            => Task.Run(func);


        public static TOut AskSync<TIn, TOut>(this IQuery<TIn, Task<TOut>> asyncQuery, TIn spec)
            => asyncQuery.Ask(spec).Result;

        #endregion

        public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> me, IDictionary<TKey, TValue> merge)
        {
            foreach (var item in merge)
            {
                me[item.Key] = item.Value;
            }
        }
    }
}
