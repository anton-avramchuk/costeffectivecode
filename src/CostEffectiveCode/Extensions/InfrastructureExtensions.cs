using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CostEffectiveCode.Common;
using CostEffectiveCode.Cqrs;
using CostEffectiveCode.Ddd;
using CostEffectiveCode.Ddd.Entities;
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

        public static Func<TIn, TOut> ToFunc<TIn, TOut>(this IQuery<TIn, TOut> query)
            => x => query.Ask(x);

        public static Func<TIn, TOut> ToFunc<TIn, TOut>(this ICommandHandler<TIn, TOut> commandHandler)
            => x => commandHandler.Handle(x);


        #endregion

        #region FP
        
        public static Func<TSource, TResult> Compose<TSource, TIntermediate, TResult>(
            this Func<TIntermediate, TResult> func1, Func<TSource, TIntermediate> func2)
            => x => func1(func2(x));

        public static Func<TSource, TResult> Compose<TSource, TIntermediate, TResult>(
            this Func<TSource, TIntermediate> func1, Func<TIntermediate, TResult> func2)
            => x => func2(func1(x));        

        public static TResult Forward<TSource, TResult>(
            this TSource source, Func<TSource, TResult> func)
            => func(source);

        public static T Match<T>(this T source
            , Func<T, bool> pattern
            , Func<T, T> evaluator)
            where T : class
            => pattern.Invoke(source)
                ? evaluator.Invoke(source)
                : source;

        public static TReturn WithMatched<TReturn, TMatch>(this TReturn source
            , object match,
            Func<TReturn, TMatch, TReturn> evaluator)
            where TMatch : class
            => match is TMatch
                ? evaluator.Invoke(source, (TMatch)match)
                : source;

        public static TResult Return<TInput, TResult>(this TInput o,
            Func<TInput, TResult> evaluator, TResult failureValue)
            where TInput : class
            => o == null ? failureValue : evaluator(o);        

        public static TInput If<TInput>(this TInput o, Func<TInput, bool> evaluator, Func<TInput, TInput> fallBack)
            where TInput : class
        {
            if (o == null) return null;
            return evaluator(o) ? o : fallBack(o);
        }

        public static TInput Unless<TInput>(this TInput o, Func<TInput, bool> evaluator)
            where TInput : class
        {
            if (o == null) return null;
            return evaluator(o) ? null : o;
        }

        public static TInput Do<TInput>(this TInput o, Action<TInput> action, [CanBeNull]Func<Exception> ifNull = null)
            where TInput : class
        {
            if (o == null)
            {
                if (ifNull != null)
                {
                    throw ifNull.Invoke();
                }

                return null;
            }
            action(o);
            return o;
        }

        public static TOutput Do<TInput, TOutput>(this TInput o, Func<TInput, TOutput> func, Func<Exception> ifNull)
        {
            if (o == null)
            {
                throw ifNull.Invoke();
            }

            return func.Invoke(o);
        }

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


        public static TOut AskSync<TIn, TOut>(this IQuery<TIn, Task<TOut>> asyncQuery, TIn spec)
            => asyncQuery.Ask(spec).Result;

        #endregion

        #region Cqrs
        public static TResult Forward<TSource, TResult>(
            this TSource source, IQuery<TSource, TResult> query)
            => query.Ask(source);

        public static Task<TResult> Forward<TSource, TResult>(
            this TSource source, IQuery<TSource, Task<TResult>> query)
            => query.Ask(source);

        public static TResult Forward<TSource, TResult>(
            this TSource source, ICommandHandler<TSource, TResult> query)
            => query.Handle(source);

        public static void Forward<TSource>(
            this TSource source, ICommandHandler<TSource> query)
            => query.Handle(source);

        #endregion
    }
}
