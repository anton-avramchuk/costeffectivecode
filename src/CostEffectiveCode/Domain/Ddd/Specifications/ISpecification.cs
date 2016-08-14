using JetBrains.Annotations;

namespace CosteffectiveCode.Domain.Ddd.Specifications
{
    public interface ISpecification<in T>
    {
        bool IsSatisfiedBy([NotNull]T o);
    }
}