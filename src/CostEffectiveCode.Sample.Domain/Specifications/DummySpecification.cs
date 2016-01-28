using CostEffectiveCode.Domain.Ddd.Specifications;
using CostEffectiveCode.SampleProject.Domain.Shared.Entities;

namespace CostEffectiveCode.SampleProject.Domain.Shared.Specifications
{
    class DummySpecification : ISpecification<Product>
    {
        public bool IsSatisfiedBy(Product o)
        {
            return true;
        }
    }
}
