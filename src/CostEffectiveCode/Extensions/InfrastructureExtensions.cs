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
using CostEffectiveCode.Ddd.Specifications;
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

        #region FP
        
        public static Func<TSource, TResult> Compose<TSource, TIntermediate, TResult>(
            this Func<TSource, TIntermediate> func1, Func<TIntermediate, TResult> func2)
            => x => func2(func1(x));        

        public static TResult PipeTo<TSource, TResult>(
            this TSource source, Func<TSource, TResult> func)
            => func(source);

        public static T PipeToIf<T>(this T source
            , Func<T, bool> condition
            , Func<T, T> evaluator)
            where T : class
            => condition(source)
                ? evaluator(source)
                : source;

        public static T ForwardIfNotNull<T>(this T source, Func<object, T> evaluator)
            where T : class
            => PipeToIf(source, x => x != null, x => evaluator(x));

        public static TOutput Either<TInput, TOutput>(this TInput o
            , Func<TInput, bool> condition
            , Func<TInput, TOutput> ifTrue
            , Func<TInput, TOutput> ifFalse)
            where TInput : class
            => condition(o) ? ifTrue(o) : ifFalse(o);

        #endregion

        #region Linq

        public static IQueryable<T> Where<T>(this IQueryable<T> source, ILinqSpecification<T> spec)
            where T : class
            => spec.Apply(source);

        public static IQueryable<T> MaybeSort<T>(this IQueryable<T> source, object sort)
        {
            var srt = sort as ILinqSorting<T>;
            return srt != null ? srt.Apply(source) : source;
        }

        public static IQueryable<T> MaybeWhere<T>(this IQueryable<T> source, object spec)
            where T : class
        {
            var specification = spec as ILinqSpecification<T>;
            if (specification != null)
            {
                source = specification.Apply(source);
            }

            var expr = spec as Expression<Func<T, bool>>;
            if (expr != null)
            {
                source = source.Where(expr);
            }

            var exprSpec = spec as ExpressionSpecification<T>;
            if (exprSpec != null)
            {
                source = source.Where(exprSpec.Expression);
            }
            return source;
        }

        public static T Do<T>(T obj, Action<T> action)
        {
            action(obj);
            return obj;
        }

        public static IQueryable<TDest> Project<TDest>(this IQueryable source, IProjector projector)
            => projector.Project<TDest>(source);

        public static TEntity ById<TEntity>(this ILinqProvider linqProvider, int id)
            where TEntity : class, IHasId<int>
            => linqProvider.Query<TEntity>().ById(id);

        public static TEntity ById<TEntity>(this IQueryable<TEntity> queryable, int id)
            where TEntity : class, IHasId<int>
            => queryable.SingleOrDefault(x => x.Id == id);

        #endregion

        #region Async

        public static Task<T> RunTask<T>(this Func<T> func)
            => Task.Run(func);


        public static TOut AskSync<TIn, TOut>(this IQuery<TIn, Task<TOut>> asyncQuery, TIn spec)
            => asyncQuery.Ask(spec).Result;

        #endregion

        #region Cqrs

        public static TResult PipeTo<TSource, TResult>(
            this TSource source, IQuery<TSource, TResult> query)
            => query.Ask(source);

        public static Task<TResult> PipeTo<TSource, TResult>(
            this TSource source, IQuery<TSource, Task<TResult>> query)
            => query.Ask(source);

        public static TResult PipeTo<TSource, TResult>(
            this TSource source, IHandler<TSource, TResult> query)
            => query.Handle(source);

        public static TSource PipeTo<TSource>(
            this TSource source, IHandler<TSource> query)
        {
            query.Handle(source);
            return source;
        }

        public static TOutput Ask<TInput, TOutput>(this IQuery<TInput, TOutput> query)
            where TInput : new()
            => query.Ask(new TInput());

        public static Func<TIn, TOut> ToFunc<TIn, TOut>(this IQuery<TIn, TOut> query)
            => x => query.Ask(x);

        public static Func<TIn, TOut> ToFunc<TIn, TOut>(this IHandler<TIn, TOut> handler)
            => x => handler.Handle(x);

        #endregion
    }
}
