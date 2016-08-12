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
    public abstract class ExpressionQueryBase<TEntity, TResult>
        : IEntityQuery<TEntity, IExpressionSpecification<TEntity>, TResult>
        where TEntity : class, IEntity
    {
        #region Props

        private readonly ILinqProvider _linqProvider;

        protected IQueryable<TEntity> Queryable;

        private bool _isOrdered;

        #endregion

        #region Ctor

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
            {
                Queryable = _linqProvider.Query<TEntity>();
            }

            if (spec != null)
            {
                Queryable = Queryable.Where(spec.Expression);
            }

            return Queryable;
        }

        protected abstract IQueryable<TResult> Project(IQueryable<TEntity> queryable);

        public IEnumerable<TResult> Get()
        {
            return Project(Queryable).ToArray();
        }

        public ISpecificationQuery<TEntity, IExpressionSpecification<TEntity>, IEnumerable<TResult>> Where(IExpressionSpecification<TEntity> specification)
        {
            Queryable = Queryable.Where(specification.Expression);
            return this;
        }

        public TResult Single() => Project(Queryable).Single();
       
        public TResult FirstOrDefault() => Project(Queryable).FirstOrDefault();

        public bool Any() => Queryable.Any();

        public IPagedEnumerable<TResult> Paged(int pageNumber, int take)
        {
            var projection = Project(Queryable);
            var total = projection.Count();
            return new PagedList<TResult>(total, projection.Skip(pageNumber*take).Take(take).ToArray());
        } 

        public long Count()
        {
            return Queryable.Count();
        }
    }
}
