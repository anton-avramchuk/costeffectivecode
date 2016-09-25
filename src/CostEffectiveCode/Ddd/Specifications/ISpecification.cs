using JetBrains.Annotations;

namespace CostEffectiveCode.Ddd.Specifications
{
    public interface ISpecification<in T>
    {
        bool IsSatisfiedBy([NotNull]T o);
    }
}