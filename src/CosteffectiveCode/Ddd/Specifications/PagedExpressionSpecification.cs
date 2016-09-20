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
        where TEntity : IEntity
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

            Page = page;
            Take = take;
        }

        public IQueryable<TEntity> Apply(IQueryable<TEntity> query) => query.Where(Expression);
    }
}
