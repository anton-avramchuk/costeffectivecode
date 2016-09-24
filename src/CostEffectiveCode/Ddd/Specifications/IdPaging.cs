using CostEffectiveCode.Ddd.Entities;

namespace CostEffectiveCode.Ddd.Specifications
{
    public class IdPaging<TEntity> : IdPaging<TEntity, int>
        where TEntity : class, IHasId<int>
    {
        public IdPaging(int page, int take) : base(page, take)
        {
        }
        
        public IdPaging()
        {
            OrderBy = new Sorting<TEntity, int>(x => x.Id, SortOrder.Desc);
        }
    }

    public class IdPaging<TEntity, TKey> : Paging<TEntity, TKey> 
        where TEntity : class, IHasId<TKey>
    {
        public IdPaging(int page, int take) : base(page, take, new Sorting<TEntity, TKey>(x => x.Id, SortOrder.Desc))
        {
        }

        public IdPaging()
        {
            OrderBy = new Sorting<TEntity, TKey>(x => x.Id, SortOrder.Desc);
        }
    }
}
