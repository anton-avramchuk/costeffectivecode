using CosteffectiveCode.Ddd.Entities;

namespace Costeffectivecode.WebApi2.Example.Models
{
    public class ProductDto : EntityBase<int>
    {
        public string InfoName { get; set; }
    }
}