using CostEffectiveCode.Domain.Ddd.Specifications;
using CostEffectiveCode.SampleProject.Domain.Entities;

namespace CostEffectiveCode.SampleProject.Domain.Specifications
{
    class DummySpecification : ISpecification<Product>
    {
        public bool IsSatisfiedBy(Product o)
        {
            return true;
        }
    }
}
