using System.Web.Http;
using System.Web.Http.Description;
using Costeffectivecode.WebApi2.Example.Models;
using CostEffectiveCode.Cqrs.Commands;
using CostEffectiveCode.Cqrs.Queries;
using CostEffectiveCode.WebApi2;

namespace Costeffectivecode.WebApi2.Example.Controllers
{
    public class ProductController : CqrsController
    {
        public ProductController(ICommandFactory commandFactory, IQueryFactory queryFactory)
            : base(commandFactory, queryFactory)
        {
        }

        [ResponseType(typeof(ProductDto))]
        public IHttpActionResult Get(int id) => this.Get<int, Product, ProductDto>(id);

    }
}