using CostEffectiveCode.Domain.Ddd.Specifications;
using CostEffectiveCode.EntityFramework.Tests.Entities;

namespace CostEffectiveCode.EntityFramework.Tests.Specifications
{
    class DummySpecification : ISpecification<Product>
    {
        public bool IsSatisfiedBy(Product o)
        {
            return true;
        }
    }
}
