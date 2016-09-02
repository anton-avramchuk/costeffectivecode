using CostEffectiveCode.Ddd.Entities;

namespace Costeffectivecode.WebApi2.Example.Models
{
    public class Info
    {
        public string Name { get; set; }
    }

    public class Product : EntityBase<int>
    {
        public Info Info { get; set; }
    }
}