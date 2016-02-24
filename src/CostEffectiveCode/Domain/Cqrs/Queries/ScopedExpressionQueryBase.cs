using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CostEffectiveCode.Domain.Ddd.Entities;
using CostEffectiveCode.Domain.Ddd.Specifications;
using CostEffectiveCode.Domain.Ddd.UnitOfWork;

namespace CostEffectiveCode.Domain.Cqrs.Queries
{
    [Obsolete("Use IQueryFactory instead")]
    public class ScopedExpressionQueryBase<TEntity, TLinqProvider> : IQuery<TEntity, IExpressionSpecification<TEntity>>
        where TEntity : class, IEntity
        where TLinqProvider: ILinqProvider, IDisposable
    {
        private readonly Func<TLinqProvider> _linqProviderFactoryMethod;
        private Expression<Func<TEntity, bool>> _filter;
        private readonly Func<TLinqProvider, ExpressionQueryBase<TEntity>> _expressionQueryFactoryMethod;

        public ScopedExpressionQueryBase(
            Func<TLinqProvider> linqProviderFactoryMethod,
            Expression<Func<TEntity, bool>> filter,
            Func<TLinqProvider, ExpressionQueryBase<TEntity>> expressionQueryFactoryMethod)
        {
            _linqProviderFactoryMethod = linqProviderFactoryMethod;
            _filter = filter;
            _expressionQueryFactoryMethod = expressionQueryFactoryMethod;
        }

        #region IQuery methods
        public TEntity Single()
        {
            return WithQueryInsideDbContext(x => x.Single());
        }

        public IEnumerable<TEntity> All()
        {
            return WithQueryInsideDbContext(x => x.All());
        }

        public IPagedEnumerable<TEntity> Paged(int pageNumber, int take)
        {
            return WithQueryInsideDbContext(x => x.Paged(pageNumber, take));
        }

        public IEnumerable<TEntity> Take(int count)
        {
            return WithQueryInsideDbContext(x => x.Take(count));
        }

        public bool Any()
        {
            return WithQueryInsideDbContext(x => x.Any());
        }

        public TEntity FirstOrDefault()
        {
            return WithQueryInsideDbContext(x => x.FirstOrDefault());
        }

        public IQuery<TEntity, IExpressionSpecification<TEntity>> Where(IExpressionSpecification<TEntity> specification)
        {
            _filter = specification.Expression;

            return this;
        }

        public IQuery<TEntity, IExpressionSpecification<TEntity>> OrderBy<TProperty>(Expression<Func<TEntity, TProperty>> expression, SortOrder sortOrder = SortOrder.Asc)
        {
            throw new NotImplementedException();
        }

        public IQuery<TEntity, IExpressionSpecification<TEntity>> Include<TProperty>(Expression<Func<TEntity, TProperty>> expression)
        {
            throw new NotImplementedException();
        }

        public long Count()
        {
            return WithQueryInsideDbContext(x => x.Count());
        }
        #endregion

        // decorator method
        private TReturn WithQueryInsideDbContext<TReturn>(Func<IQuery<TEntity, IExpressionSpecification<TEntity>>, TReturn> func)
        {
            using (var dbContext = _linqProviderFactoryMethod.Invoke())
            {
                return func.Invoke(GetQuery(dbContext));
            }
        }

        // factory method
        private IQuery<TEntity, IExpressionSpecification<TEntity>> GetQuery(TLinqProvider x)
        {
            return _expressionQueryFactoryMethod.Invoke(x).Where(_filter);
        }

    }
}
