using CostEffectiveCode.Domain.Ddd.Specifications;
using CostEffectiveCode.Tests.Entities;

namespace CostEffectiveCode.Tests.Specifications
{
    class DummySpecification : ISpecification<Product>
    {
        public bool IsSatisfiedBy(Product o)
        {
            return true;
        }
    }
}
