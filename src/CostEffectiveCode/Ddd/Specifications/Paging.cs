using System;
using System.Linq;
using CostEffectiveCode.Cqrs.Queries;
using CostEffectiveCode.Ddd.Entities;

namespace CostEffectiveCode.Ddd.Specifications
{
    public class Paging<TEntity, TOrderKey> :
        IPaging<TEntity, TOrderKey>
        where TEntity : class, IHasId
    {
        public Paging(int page, int take, Sorting<TEntity, TOrderKey> orderBy)
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
            OrderBy = orderBy;
        }

        public Paging()
        {
            Take = 30;
        }


        public int Page { get; set; }
        public int Take { get; set; }

        public Sorting<TEntity, TOrderKey> OrderBy { get; set; }
    }
}
