using System.Linq;
using CosteffectiveCode.AutoMapper;
using CosteffectiveCode.Domain.Ddd.Entities;


namespace CosteffectiveCode.Tests
{
    public class Foo : EntityBase<int>
    {
        public Bar Bar { get; set; }
    }

    public class Bar : EntityBase<int>
    {
        public string Name { get; set; }
    }

    public class FooDto
    {
        public int Id { get; set; }

        public string BarName { get; set; }
    }
}
