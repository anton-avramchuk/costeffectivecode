using JetBrains.Annotations;

namespace CosteffectiveCode.Ddd.Specifications
{
    public interface ISpecification<in T>
    {
        bool IsSatisfiedBy([NotNull]T o);
    }
}