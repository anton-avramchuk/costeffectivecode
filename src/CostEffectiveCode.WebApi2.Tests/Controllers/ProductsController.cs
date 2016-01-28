using System.Linq;
using System.Web.Http;
using CostEffectiveCode.Common;
using CostEffectiveCode.Domain.Cqrs.Commands;
using CostEffectiveCode.Domain.Cqrs.Queries;
using CostEffectiveCode.SampleProject.Domain.Shared.Entities;
using CostEffectiveCode.WebApi2.WebApi.Controller;
using Microsoft.Owin;

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
