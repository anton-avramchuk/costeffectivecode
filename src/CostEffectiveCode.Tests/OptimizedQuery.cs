using System.Linq;
using CostEffectiveCode.Common;
using CostEffectiveCode.Cqrs.Queries;
using CostEffectiveCode.Ddd.Specifications.UnitOfWork;
using CostEffectiveCode.Extensions;

namespace CostEffectiveCode.Tests
{
    public class OptimizedQuery: IQuery<ProductUberProductSpecification, IPagedEnumerable<ProductDto>>
    {
        private readonly ILinqProvider _linqProvider;

        public OptimizedQuery(ILinqProvider linqProvider)
        {
            _linqProvider = linqProvider;
        }

        public IPagedEnumerable<ProductDto> Ask(ProductUberProductSpecification spec)
            => _linqProvider
                .GetQueryable<Product>()
                //.Apply(spec)
                .Select(x => new ProductDto()
                {
                    CategoryName = x.Category.Name,
                    Price = 100500,
                    Id = x.Id
                })
                .Apply(spec)
                .ToPagedEnumerable(spec);

    }
}
