using JetBrains.Annotations;

namespace CostEffectiveCode.Domain.Ddd.Specifications
{
    public interface ISpecification<in T>
    {
        bool IsSatisfiedBy([NotNull]T o);
    }
}