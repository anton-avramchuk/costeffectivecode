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

        private IQueryable<TEntity> _queryable;

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

        protected IQueryable<TEntity> GetQueryable(IExpressionSpecification<TEntity> spec = null)
        {
            if (_queryable == null)
            {
                _queryable = _linqProvider.Query<TEntity>();
            }

            if (spec != null)
            {
                _queryable = _queryable.Where(spec.Expression);
            }

            return _queryable;
        }

        public IQuery<TEntity, IExpressionSpecification<TEntity>> Where(
            IExpressionSpecification<TEntity> specification)
        {
            if (specification == null) throw new ArgumentNullException(nameof(specification));
            GetQueryable(specification);
            return this;
        }

        public IQuery<TEntity, IExpressionSpecification<TEntity>> OrderBy<TProperty>(
            Expression<Func<TEntity, TProperty>> expression,
            SortOrder sortOrder = SortOrder.Asc)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            var sorting = new Sorting<TEntity, TProperty>(expression, sortOrder);
            GetQueryable();

            if (_isOrdered)
            {
                var entities = (IOrderedQueryable<TEntity>)_queryable;
                _queryable = sorting.SortOrder == SortOrder.Asc
                    ? entities.ThenBy(sorting.Expression)
                    : entities.ThenByDescending(sorting.Expression);
            }
            else
            {
                _queryable = sorting.SortOrder == SortOrder.Asc
                    ? _queryable.OrderBy(sorting.Expression)
                    : _queryable.OrderByDescending(sorting.Expression);
            }

            _isOrdered = true;

            return this;
        }

        public abstract IQuery<TEntity, IExpressionSpecification<TEntity>> Include<TProperty>(
            Expression<Func<TEntity, TProperty>> expression);

        public TEntity Single()
        {
            return GetQueryable().Single();
        }

        public TEntity FirstOrDefault()
        {
            return GetQueryable().FirstOrDefault();
        }

        public IEnumerable<TEntity> All()
        {
            return GetQueryable().ToArray();
        }

        public bool Any()
        {
            return GetQueryable().Any();
        }

        public IPagedEnumerable<TEntity> Paged(int pageNumber, int take)
        {
            _queryable = GetQueryable();

            var result = new PagedList<TEntity>(_queryable.Count());
            var raw = _queryable
                .Skip(pageNumber * take)
                .Take(take)
                .ToArray();

            result.AddRange(raw);

            return result;
        }

        public IEnumerable<TEntity> Take(int count)
        {
            return GetQueryable().Take(count).ToArray();
        }

        public IQueryable<TResult> SelectTo<TResult>(Func<TEntity, TResult> selector)
        {
            return GetQueryable().Select(i => selector(i));
        }


        public long Count()
        {
            return GetQueryable().Count();
        }
    }
}
