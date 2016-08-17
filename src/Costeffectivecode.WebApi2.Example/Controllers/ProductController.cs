using System.Web.Http;
using System.Web.Http.Description;
using CosteffectiveCode.Cqrs.Commands;
using CosteffectiveCode.Cqrs.Queries;
using Costeffectivecode.WebApi2.Example.Models;

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