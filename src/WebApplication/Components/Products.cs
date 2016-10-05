using System.Web.Mvc;
using CostEffectiveCode.AutoMapper;
using CostEffectiveCode.Components.Cqrs;
using CostEffectiveCode.Cqrs;
using CostEffectiveCode.Ddd.Entities;
using CostEffectiveCode.Ddd.Pagination;
using CostEffectiveCode.Ddd.Specifications;
using WebApplication.Domain;

namespace WebApplication.Components
{
    public class ProductController : Controller
    {
        private readonly IQuery<int, ProductDto> _byId;
        private readonly IQuery<ProductSpec, IPagedEnumerable<ProductDto>> _bySpec;

        public ProductController(IQuery<int, ProductDto> byId
            , IQuery<ProductSpec, IPagedEnumerable<ProductDto>> bySpec)
        {
            _byId = byId;
            _bySpec = bySpec;
        }

        public ActionResult ById(int id) => Json(_byId.Ask(id), JsonRequestBehavior.AllowGet);

        public ActionResult BySpec(ProductSpec spec) => Json(_bySpec.Ask(spec), JsonRequestBehavior.AllowGet);
    }

    public class ProductSpec : IdPaging<ProductDto>
    {
        
    }

    [DtoFor(typeof(Product)), ConventionalMap(typeof(Product))]
    public class ProductDto : HasIdBase<int>
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public decimal Price { get; set; }
    }
}