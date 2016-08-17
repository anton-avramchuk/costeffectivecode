namespace CosteffectiveCode.Ddd.Specifications
{
    public interface IPagedSpecification<in T> : ISpecification<T>
    {
        int Page { get; }

        int Take { get; }
    }
}