using System.Linq;
using CostEffectiveCode.Cqrs.Queries;
using CostEffectiveCode.Ddd.Entities;
using CostEffectiveCode.Ddd.Specifications;

namespace CostEffectiveCode.Tests
{
    public class ProductUberProductSpecification
        : IdPaging<ProductDto>, ILinqSpecification<Product>, ILinqSpecification<ProductDto>
    {
        public decimal Price = 5;

        public IQueryable<Product> Apply(IQueryable<Product> query)
            => query.Where(x => x.Price > Price);

        public IQueryable<ProductDto> Apply(IQueryable<ProductDto> query)
            => query.Where(x => x.CategoryName.StartsWith("S"));

    }
}
