using System;
using System.Collections.Generic;
using System.Linq;
using CosteffectiveCode.Domain.Ddd;
using CosteffectiveCode.Domain.Ddd.Entities;
using CosteffectiveCode.Domain.Ddd.Specifications;
using JetBrains.Annotations;
using Void = CosteffectiveCode.FunctionalProgramming.Void;

namespace CosteffectiveCode.Domain.Cqrs.Queries
{
    public abstract class ExpressionQueryBase<TEntity, TResult>
        : IEntityQuery<TEntity, ExpressionSpecification<TEntity>, TResult>
        where TEntity : class, IEntity
    {
        #region Props

        protected IQueryable<TEntity> Queryable;

        #endregion

        #region Ctor

        protected ExpressionQueryBase(
            [NotNull] IQueryable<TEntity> queryable)
        {
            if (queryable == null) throw new ArgumentNullException(nameof(queryable));
            Queryable = queryable;
        }

        #endregion

        protected IQueryable<TEntity> LoadQueryable(ExpressionSpecification<TEntity> spec = null)
        {        
            if (spec != null)
            {
                Queryable = Queryable.Where(spec.Expression);
            }

            return Queryable;
        }

        protected abstract IQueryable<TResult> Project(IQueryable<TEntity> queryable);

        public ISpecificationQuery<TEntity, ExpressionSpecification<TEntity>, IEnumerable<TResult>> Where(ExpressionSpecification<TEntity> specification)
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

        public IEnumerable<TResult> Execute()
        {
            return Project(Queryable).ToArray();
        }

        public IEnumerable<TResult> Execute(Void input)
        {
            return Execute();
        }
    }
}
