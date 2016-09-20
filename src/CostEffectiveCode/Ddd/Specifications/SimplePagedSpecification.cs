using System;
using System.Linq;
using CostEffectiveCode.Cqrs.Queries;
using CostEffectiveCode.Ddd.Entities;

namespace CostEffectiveCode.Ddd.Specifications
{
    public class SimplePagedSpecification<TEntity> :
        IPagedSpecification<TEntity>,
        ILinqSpecification<TEntity> where TEntity : class, IEntity
    {
        public SimplePagedSpecification(int page, int take)
        {
            if (take <= 0)
            {
                throw new ArgumentException("take must be > 0", nameof(take));
            }

            if (page < 0)
            {
                throw new ArgumentException("page must be >= 0", nameof(page));
            }

            Page = page;
            Take = take;
        }

        [Obsolete("For Binding Only", true)]
        public SimplePagedSpecification()
        {

        }

        public bool IsSatisfiedBy(TEntity o) => true;

        public int Page { get; }
        public int Take { get; }

        public IQueryable<TEntity> Apply(IQueryable<TEntity> query)
            => query
            .OrderByDescending(x => x.Id)
            .Skip(Page * Take)
            .Take(Take);
    }
}
