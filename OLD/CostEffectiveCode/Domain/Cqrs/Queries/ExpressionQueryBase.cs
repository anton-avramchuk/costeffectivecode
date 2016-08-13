using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CostEffectiveCode.Domain.Ddd;
using CostEffectiveCode.Domain.Ddd.Entities;
using CostEffectiveCode.Domain.Ddd.Specifications;
using CostEffectiveCode.Domain.Ddd.UnitOfWork;
using JetBrains.Annotations;

namespace CostEffectiveCode.Domain.Cqrs.Queries
{
    public abstract class ExpressionQueryBase<TEntity> : IQuery<TEntity, IExpressionSpecification<TEntity>>
        where TEntity : class, IEntity
    {
        #region Vars

        private readonly ILinqProvider _linqProvider;

        protected IQueryable<TEntity> Queryable;

        private bool _isOrdered;
        #endregion

        #region ctor

        protected ExpressionQueryBase(
            [NotNull] ILinqProvider linqProvider)
        {
            if (linqProvider == null) throw new ArgumentNullException(nameof(linqProvider));

            _linqProvider = linqProvider;
        }

        #endregion

        protected IQueryable<TEntity> LoadQueryable(IExpressionSpecification<TEntity> spec = null)
        {
            if (Queryable == null)
                Queryable = _linqProvider.Query<TEntity>();

            if (spec != null)
                Queryable = Queryable.Where(spec.Expression);

            return Queryable;
        }

        public IQuery<TEntity, IExpressionSpecification<TEntity>> Where(
            IExpressionSpecification<TEntity> specification)
        {
            if (specification == null) throw new ArgumentNullException(nameof(specification));
            LoadQueryable(specification);
            return this;
        }

        public IQuery<TEntity, IExpressionSpecification<TEntity>> OrderBy<TProperty>(
            Expression<Func<TEntity, TProperty>> expression,
            SortOrder sortOrder = SortOrder.Asc)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            var sorting = new Sorting<TEntity, TProperty>(expression, sortOrder);
            LoadQueryable();

            if (_isOrdered)
            {
                var entities = (IOrderedQueryable<TEntity>)Queryable;
                Queryable = sorting.SortOrder == SortOrder.Asc
                    ? entities.ThenBy(sorting.Expression)
                    : entities.ThenByDescending(sorting.Expression);
            }
            else
            {
                Queryable = sorting.SortOrder == SortOrder.Asc
                    ? Queryable.OrderBy(sorting.Expression)
                    : Queryable.OrderByDescending(sorting.Expression);
            }

            _isOrdered = true;

            return this;
        }

        public abstract IQuery<TEntity, IExpressionSpecification<TEntity>> Include<TProperty>(
            Expression<Func<TEntity, TProperty>> expression);

        public TEntity Single()
        {
            return LoadQueryable().Single();
        }

        public TEntity FirstOrDefault()
        {
            return LoadQueryable().FirstOrDefault();
        }

        public IEnumerable<TEntity> All()
        {
            return LoadQueryable().ToArray();
        }

        public bool Any()
        {
            return LoadQueryable().Any();
        }

        public IPagedEnumerable<TEntity> Paged(int pageNumber, int take)
        {
            Queryable = LoadQueryable();

            var result = new PagedList<TEntity>(Queryable.Count());
            var raw = Queryable
                .Skip(pageNumber * take)
                .Take(take)
                .ToArray();

            result.AddRange(raw);

            return result;
        }

        public IEnumerable<TEntity> Take(int count)
        {
            return LoadQueryable().Take(count).ToArray();
        }

        public IQueryable<TResult> SelectTo<TResult>(Func<TEntity, TResult> selector)
        {
            return LoadQueryable().Select(i => selector(i));
        }


        public long Count()
        {
            return LoadQueryable().Count();
        }
    }
}
