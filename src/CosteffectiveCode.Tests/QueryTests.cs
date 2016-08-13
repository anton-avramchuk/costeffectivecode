using System;
using System.Linq;
using AutoMapper;
using CosteffectiveCode.AutoMapper;
using CostEffectiveCode.Domain.Ddd.Entities;
using Xunit;

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

    public class QueryTests
    {
        [Fact]
        public void Query_FirstOrDefault()
        {
            Mapper.Initialize(cfg => cfg.CreateMissingTypeMaps = true);

            var data = new[] {new Foo() {Bar = new Bar() {Name = "Super Bar"} } };
            var q = new EntityToDtoQuery<Foo,FooDto>(data.AsQueryable());
            var result = q.FirstOrDefault();
            Assert.Equal(data[0].Bar.Name, result?.BarName);
        }
    }
}
