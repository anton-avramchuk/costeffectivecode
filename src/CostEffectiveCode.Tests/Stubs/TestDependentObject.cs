using System.Linq;
using CostEffectiveCode.Extensions;
using CostEffectiveCode.Ddd.Specifications;
using CostEffectiveCode.Cqrs;
using CostEffectiveCode.Ddd.Pagination;

namespace CostEffectiveCode.Tests
{
    public class TestDependentObject
    {
        public TestDependentObject(IQuery<IdPaging<ProductDto>, IPagedEnumerable<ProductDto>> pagedQuery
            , IQuery<int, ProductDto> getQuery
            , ICommandHandler<ProductDto, int> createOrUpdateCommandHandler)
        {
        }
    }
}
