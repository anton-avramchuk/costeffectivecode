using System.Linq;
using AutoMapper;
using CosteffectiveCode.AutoMapper;
using Xunit;

namespace CosteffectiveCode.Tests
{    
    public class QueryTests
    {
        [Fact]
        public void Query_FirstOrDefault()
        {
            Mapper.Initialize(cfg => cfg.CreateMissingTypeMaps = true);

            var data = new[] {new Foo() {Bar = new Bar() {Name = "Super Bar"} } };
            var q = new EntityToDtoQuery<Foo,FooDto>(data.AsQueryable());
            var result = q.Execute(x => x.Id > 1).FirstOrDefault();
            Assert.Equal(data[0].Bar.Name, result?.BarName);
        }
    }
}
