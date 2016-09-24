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
using CostEffectiveCode.Ddd.Specifications;
using CostEffectiveCode.Ddd.Specifications.UnitOfWork;
using JetBrains.Annotations;

namespace CostEffectiveCode.Extensions
{
    [PublicAPI]
    public static class Extensions
    {
        #region Dynamic Expression Compilation

        private static readonly ConcurrentDictionary<Expression, object> Cache
            = new ConcurrentDictionary<Expression, object>();

        public static Func<TEntity, bool> AsFunc<TEntity>(this TEntity entity, Expression<Func<TEntity, bool>> expr)
            where TEntity : class, IHasId
        {
            //@see http://sergeyteplyakov.blogspot.ru/2015/06/lazy-trick-with-concurrentdictionary.html
            return ((Lazy<Func<TEntity, bool>>)Cache.GetOrAdd(expr, id => new Lazy<object>(
                    () => Cache.GetOrAdd(id, expr.Compile())))).Value;
        }

        public static bool Is<TEntity>(this TEntity entity, Expression<Func<TEntity, bool>> expr)
            where TEntity : class, IHasId
        {
            return AsFunc(entity, expr).Invoke(entity);
        }

        #endregion

        #region Composite Specifications

        public static ISpecification<T> And<T>(this ISpecification<T> left, ISpecification<T> right)
             where T : class, IHasId
        {
            return new AndSpecification<T>(left, right);
        }
        public static ISpecification<T> Or<T>(this ISpecification<T> left, ISpecification<T> right)
             where T : class, IHasId
        {
            return new OrSpecification<T>(left, right);
        }
        public static ISpecification<T> Not<T>(this ISpecification<T> left)
             where T : class, IHasId
        {
            return new NotSpecification<T>(left);
        }

        #endregion

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> queryable, bool cnd, Expression<Func<T, bool>>  expr)
            => cnd
                ? queryable.Where(expr)
                : queryable;

        public static Func<TIn, TOut> ToFunc<TIn, TOut>(this IQuery<TIn, TOut> query)
            => x => query.Ask(x);

        public static Func<TIn, TOut> ToFunc<TIn, TOut>(this ICommandHandler<TIn, TOut> query)
            where TOut : struct
            => x => query.Handle(x);

        public static async Task<T> ToAsync<T>(Func<T> func)
            => await Task.Run(() => func.Invoke()).ConfigureAwait(false);

        public static async Task<TOut> ToAsync<TIn, TOut>(this IQuery<TIn, TOut> query, TIn message)
            => await Task.Run(() => query.Ask(message)).ConfigureAwait(false);

        public static async Task<TOut> ToAsync<TIn, TOut>(this ICommandHandler<TIn, TOut> handler, TIn message)
            where TOut : struct
            => await Task.Run(() => handler.Handle(message)).ConfigureAwait(false);


        public static IQueryable<T> Match<T, TPattern>(this IQueryable<T> source, object pattern,
            Func<IQueryable<T>, TPattern, IQueryable<T>> evaluator) where TPattern : class
            => pattern is TPattern
                ? evaluator.Invoke(source, (TPattern) pattern)
                : source;

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
    }
}
