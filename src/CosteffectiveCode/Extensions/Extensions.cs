using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CostEffectiveCode.Common;
using CostEffectiveCode.Cqrs;
using CostEffectiveCode.Cqrs.Commands;
using CostEffectiveCode.Cqrs.Queries;
using CostEffectiveCode.Ddd.Entities;
using CostEffectiveCode.Ddd.UnitOfWork;
using JetBrains.Annotations;

namespace CostEffectiveCode.Extensions
{
    [PublicAPI]
    public static class Extensions
    {
        #region Dynamic Expression Compilation

        private static readonly ConcurrentDictionary<Expression, object> Cache
            = new ConcurrentDictionary<Expression, object>();

        public static Func<TIn, TOut> AsFunc<TIn, TOut>(this Expression<Func<TIn, TOut>> expr)
            //@see http://sergeyteplyakov.blogspot.ru/2015/06/lazy-trick-with-concurrentdictionary.html
            => (Func<TIn, TOut>)((Lazy<object>)Cache.GetOrAdd(expr, id => new Lazy<object>(()
                => Cache.GetOrAdd(id, expr.Compile())))).Value;

        public static bool Is<T>(this T entity, Expression<Func<T, bool>> expr)
            => AsFunc(expr).Invoke(entity);

        public static Func<TIn, TOut> ToFunc<TIn, TOut>(this IQuery<TIn, TOut> query)
            => x => query.Ask(x);

        public static Func<TIn, TOut> ToFunc<TIn, TOut>(this ICommandHandler<TIn, TOut> query)
            where TOut : struct
            => x => query.Handle(x);


        #endregion

        #region FP

        public static Func<TArg1, Func<TArg2, Func<TArg3, TResult>>> Curry<TArg1, TArg2, TArg3, TResult>(
            this Func<TArg1, TArg2, TArg3, TResult> func)
            => arg1 => arg2 => arg3 => func(arg1, arg2, arg3);

        public static Func<TArg2, TArg3, TResult> Apply<TArg1, TArg2, TArg3, TResult>(
            this Func<TArg1, TArg2, TArg3, TResult> func, TArg1 arg1)
            =>  (arg2, arg3) => func(arg1, arg2, arg3);
        
        public static Func<TSource, TResult> Compose<TSource, TIntermediate, TResult>(
            this Func<TIntermediate, TResult> func1, Func<TSource, TIntermediate> func2)
            => x => func1(func2(x));

        public static Func<TSource, TResult> Compose<TSource, TIntermediate, TResult>(
            this Func<TSource, TIntermediate> func1, Func<TIntermediate, TResult> func2)
            => x => func2(func1(x));        

        public static TResult Forward<TSource, TResult>(
            this TSource source, Func<TSource, TResult> func)
        {
            return func(source);
        }

        public static TReturn Match<TReturn, TPattern>(this TReturn source, object pattern,
            Func<TReturn, TPattern, TReturn> evaluator) where TPattern : class
            => pattern is TPattern
                ? evaluator.Invoke(source, (TPattern) pattern)
                : source;

        #endregion

        #region Linq

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> queryable, bool cnd, Expression<Func<T, bool>> expr)
            => cnd
                ? queryable.Where(expr)
                : queryable;

        public static IQueryable<T> Apply<T>(this IQueryable<T> source, ILinqSpecification<T> spec)
            where T : class
            => spec.Apply(source);

        public static IQueryable<T> ApplyIfPossible<T>(this IQueryable<T> source, object spec)
            where T : class
            => spec is ILinqSpecification<T>
                ? ((ILinqSpecification<T>)spec).Apply(source)
                : source;

        public static IQueryable<TDest> Project<TSource, TDest>(this IQueryable<TSource> source, IProjector projector)
            => projector.Project<TSource, TDest>(source);

        public static TEntity ById<TEntity>(this ILinqProvider linqProvider, int id)
            where TEntity : class, IHasId<int>
            => linqProvider.GetQueryable<TEntity>().ById(id);

        public static TEntity ById<TEntity>(this IQueryable<TEntity> queryable, int id)
            where TEntity : class, IHasId<int>
            => queryable.SingleOrDefault(x => x.Id == id);
        public static TProjection ById<TEntity, TProjection>(this IQueryable<TEntity> queryable, int id, IProjector projector)
            where TEntity : class, IHasId<int>
            => projector
                .Project<TEntity, TProjection>(queryable.Where(x => x.Id == id))
                .SingleOrDefault();

        #endregion

        #region Async

        public static Task<T> RunTask<T>(this Func<T> func)
            => Task.Run(() => func.Invoke());


        public static TOut AskSync<TIn, TOut>(this IAsyncQuery<TIn, TOut> asyncQuery, TIn spec)
            => asyncQuery.Ask(spec).Result;

        #endregion
    }
}
