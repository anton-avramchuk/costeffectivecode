using System.Linq;
using CostEffectiveCode.Extensions;
using CostEffectiveCode.Ddd.Specifications;
using CostEffectiveCode.Cqrs;
using CostEffectiveCode.Ddd.Pagination;

namespace CostEffectiveCode.Tests
{
    public class PagedQueryUser
    {
        private readonly IQuery<IdPaging<ProductDto>, IPagedEnumerable<ProductDto>> _query;
        public PagedQueryUser(IQuery<IdPaging<ProductDto>, IPagedEnumerable<ProductDto>> query)
        {
            _query = query;
        }
    }
}
