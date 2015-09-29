using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using CostEffectiveCode.Domain.Cqrs.Queries;
using CostEffectiveCode.Domain.Ddd.Entities;
using CostEffectiveCode.Domain.Ddd.Specifications;
using CostEffectiveCode.Extensions;
using JetBrains.Annotations;

namespace CostEffectiveCode.Domain
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
            return (Func<TEntity, bool>)Cache.GetOrAdd(expr, id => new Lazy<object>(
                    () => Cache.GetOrAdd(id, expr.Compile())));
        }

        public static bool Is<TEntity>(this TEntity entity, Expression<Func<TEntity, bool>> expr)
            where TEntity : class, IEntity
        {
            return AsFunc(entity, expr).Invoke(entity);
        }

        #endregion

        #region Validation

        public static void ValidateSelf([NotNull] this object obj, [CanBeNull]string memberName = null)
        {
            var context = new ValidationContext(obj);
            if (memberName != null)
            {
                context.MemberName = memberName;
            }

            Validator.ValidateObject(obj, context, true);
        }

        #endregion

        #region Specifications

        public static bool Decide<T>(
            this ISpecification<T> specification,
            T subject, [NotNull] Action<T> positive,
            Action<T> negative = null)
            where T : IEntity
        {
            if (positive == null) throw new ArgumentNullException(nameof(positive));
            if (specification.IsSatisfiedBy(subject))
            {
                positive.Invoke(subject);
                return true;
            }

            negative.Do(n => n.Invoke(subject));
            return false;
        }


        public static IQuery<T, ExpressionSpecification<T>> WhereIf<T>(
            this IQuery<T, ExpressionSpecification<T>> query, Func<bool> condition, Expression<Func<T, bool>> expression)
                where T : class, IEntity
        {
            if (condition.Invoke())
            {
                query.Where(new ExpressionSpecification<T>(expression));
            }

            return query;
        }

        #endregion

        #region Query

        public static IQuery<TEntity, TSpecification> By<TEntity, TSpecification>(
            this IQuery<TEntity, TSpecification> query,
            TSpecification spec)

            where TEntity : EntityBase<int>
            where TSpecification : ISpecification<TEntity>
        {
            return query.Where(spec);
        }

        public static IQuery<TEntity, IExpressionSpecification<TEntity>> Where<TEntity>(
            this IQuery<TEntity, IExpressionSpecification<TEntity>> query,
            Expression<Func<TEntity, bool>> expression)
            where TEntity : class, IEntity
        {
            return query
                .Where(new ExpressionSpecification<TEntity>(expression));
        }

        [CanBeNull]
        public static TEntity ById<TEntity>(
            this IQuery<TEntity, IExpressionSpecification<TEntity>> query,
            long id)
            where TEntity : class, IEntityBase<long>
        {
            return query
                .Where(new IdSpecification<TEntity>(id))
                .FirstOrDefault();
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
