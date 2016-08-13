using CostEffectiveCode.Domain.Ddd.Specifications;
using CostEffectiveCode.Sample.Domain.Entities;

namespace CostEffectiveCode.Sample.Domain.Specifications
{
    class DummySpecification : ISpecification<Product>
    {
        public bool IsSatisfiedBy(Product o)
        {
            return true;
        }
    }
}
