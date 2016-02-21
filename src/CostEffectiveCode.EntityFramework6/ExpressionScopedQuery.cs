using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CostEffectiveCode.Domain;
using CostEffectiveCode.Domain.Cqrs.Queries;
using CostEffectiveCode.Domain.Ddd.Entities;
using CostEffectiveCode.Domain.Ddd.Specifications;
using CostEffectiveCode.Domain.Ddd.UnitOfWork;

namespace CostEffectiveCode.EntityFramework6
{
    public class ExpressionScopedQuery<TEntity> : IQuery<TEntity, IExpressionSpecification<TEntity>>
        where TEntity : class, IEntity
    {
        private readonly Func<IDataContext> _dbContextFactoryMethod;
        private Expression<Func<TEntity, bool>> _filter;

        public ExpressionScopedQuery(Func<IDataContext> dbContextFactoryMethod, Expression<Func<TEntity, bool>> filter)
        {
            _dbContextFactoryMethod = dbContextFactoryMethod;
            _filter = filter;
        }

        public TEntity Single()
        {
            return WithDbContext(x => GetQuery(x).Single());
        }

        public IEnumerable<TEntity> All()
        {
            return WithDbContext(x => GetQuery(x).All());
        }

        private IQuery<TEntity, IExpressionSpecification<TEntity>> GetQuery(IDataContext x)
        {
            return new ExpressionQuery<TEntity>(x).Where(_filter);
        }

        public IPagedEnumerable<TEntity> Paged(int pageNumber, int take)
        {
            return WithDbContext(x => GetQuery(x).Paged(pageNumber, take));
        }

        public bool Any()
        {
            return WithDbContext(x => GetQuery(x).Any());
        }

        public TEntity FirstOrDefault()
        {
            return WithDbContext(x => GetQuery(x).FirstOrDefault());
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
            return WithDbContext(x => GetQuery(x).Count());
        }

        private TReturn WithDbContext<TReturn>(Func<IDataContext, TReturn> fetchFunc)
        {
            using (var dbContext = _dbContextFactoryMethod.Invoke())
            {
                return fetchFunc.Invoke(dbContext);
            }
        }

    }
}
