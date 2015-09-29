using JetBrains.Annotations;

namespace CostEffectiveCode.Domain.Ddd.Specifications
{
    public interface ISpecification<in T>
        //where T:IEntity
    {
        bool IsSatisfiedBy([NotNull]T o);
    }
}