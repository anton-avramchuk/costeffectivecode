using CostEffectiveCode.Common;
using CostEffectiveCode.Common.Logger;
using CostEffectiveCode.Domain.Cqrs.Commands;
using CostEffectiveCode.Domain.Cqrs.Queries;
using CostEffectiveCode.Sample.Domain.Entities;
using CostEffectiveCode.WebApi2.WebApi.Controller;

namespace CostEffectiveCode.WebApi2.Tests.Controllers
{
    public class ProductsController : EntityApiController<Product, long>
    {
        public ProductsController(IQueryFactory queryFactory, ICommandFactory commandFactory, IMapper mapper, ILogger logger)
            : base(queryFactory, commandFactory, mapper, logger)
        {
        }
    }
}
