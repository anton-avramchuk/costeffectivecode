using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace CosteffectiveCode.Ddd.Specifications
{
    public class PagedExpressionSpecification<TEntity> : ExpressionSpecification<TEntity>, IPagedSpecification<TEntity>
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
    }
}
