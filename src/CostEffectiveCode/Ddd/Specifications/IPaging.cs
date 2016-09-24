namespace CostEffectiveCode.Ddd.Specifications
{
    public interface IPaging<TEntity, TSortKey>
        where TEntity : class
    {
        int Page { get; }

        int Take { get; }

        Sorting<TEntity, TSortKey> OrderBy { get; set; }
    }
}