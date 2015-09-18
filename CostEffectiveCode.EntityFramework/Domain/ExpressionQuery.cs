using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using CostEffectiveCode.Domain;
using CostEffectiveCode.Domain.Cqrs.Queries;
using CostEffectiveCode.Domain.Ddd;
using CostEffectiveCode.Domain.Ddd.Entities;
using CostEffectiveCode.Domain.Ddd.Specifications;
using CostEffectiveCode.Domain.Ddd.UnitOfWork;
using JetBrains.Annotations;

namespace CostEffectiveCode.EntityFramework.Domain
{
    public class ExpressionQuery<TEntity> : IQuery<TEntity, IExpressionSpecification<TEntity>>
        where TEntity : class, IEntity
    {
        #region Vars

        private readonly ILinqProvider _linqProvider;

        #endregion

        #region Constructor

        public ExpressionQuery(
            [NotNull] ILinqProvider linqProvider)
        {
            if (linqProvider == null) throw new ArgumentNullException("linqProvider");

            _linqProvider = linqProvider;
        }

        #endregion

        private IQueryable<TEntity> _queryable;

        private bool _isOrdered;

        private IQueryable<TEntity> GetQueryable(IExpressionSpecification<TEntity> spec = null)
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
            if (specification == null) throw new ArgumentNullException("specification");
            GetQueryable(specification);
            return this;
        }

        public IQuery<TEntity, IExpressionSpecification<TEntity>> OrderBy<TProperty>(
            Expression<Func<TEntity, TProperty>> expression,
            SortOrder sortOrder = SortOrder.Asc)
        {
            if (expression == null) throw new ArgumentNullException("expression");
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

        public IQuery<TEntity, IExpressionSpecification<TEntity>> Include<TProperty>(
            Expression<Func<TEntity, TProperty>> expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");
            GetQueryable().Include(expression);
            return this;
        }

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
            GetQueryable();          
            var res = new PagedList<TEntity>(_queryable.Count());
            var raw = _queryable
                .Skip(pageNumber*take)
                .Take(take)
                .ToArray();

            res.AddRange(raw);

            return res;
        }

        public IPagedEnumerable<TEntity> SkipTake(int skip, int take)
        {
            GetQueryable();          
            var res = new PagedList<TEntity>(_queryable.Count());
            var raw = _queryable
                .Skip(skip)
                .Take(take)
                .ToArray();

            res.AddRange(raw);

            return res;
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
