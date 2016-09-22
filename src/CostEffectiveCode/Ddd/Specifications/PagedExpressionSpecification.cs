using System;
using System.Linq;
using System.Linq.Expressions;
using CostEffectiveCode.Cqrs.Queries;
using CostEffectiveCode.Ddd.Entities;
using JetBrains.Annotations;

namespace CostEffectiveCode.Ddd.Specifications
{
    public class PagedExpressionSpecification<TEntity> : ExpressionSpecification<TEntity>,
        IPagedSpecification<TEntity>, ILinqSpecification<TEntity>
        where TEntity : class, IEntity
    {
        // ReSharper disable once StaticMemberInGenericType
        public static int DefaultTake { set; get; } = 20;

        public int Page { get; protected set; }

        public int Take { get; protected set; }

        public PagedExpressionSpecification([NotNull] Expression<Func<TEntity, bool>> expression, int page, int take)
            : base(expression)
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

        public IQueryable<TEntity> Apply(IQueryable<TEntity> query)
            => query.Where(Expression)
            .OrderByDescending(x => x.Id)
            .Skip(Page * Take)
            .Take(Take);
    }
}
