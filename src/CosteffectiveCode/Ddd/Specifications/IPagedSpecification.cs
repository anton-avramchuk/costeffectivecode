namespace CostEffectiveCode.Ddd.Specifications
{
    public interface IPagedSpecification<in T>
    {
        int Page { get; }

        int Take { get; }
    }
}