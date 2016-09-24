using System.Collections;
using System.Diagnostics;
using System.Linq;
using AutoMapper;
using CostEffectiveCode.AutoMapper;
using CostEffectiveCode.Components;
using CostEffectiveCode.Cqrs.Queries;
using CostEffectiveCode.Ddd.Specifications;
using CostEffectiveCode.Extensions;
using Xunit;

namespace CostEffectiveCode.Tests
{
    public class Test
    {
        static Test()
        {
            Mapper.Initialize(cfg => cfg.CreateMissingTypeMaps = true);
        }

        [Fact]
        public void AutoRegistration_PagedQuery()
        {
            var sw = new Stopwatch();
            sw.Start();
            var assembly = GetType().Assembly;
            var res = AutoRegistration.GetComponentMap(assembly, t => t == typeof(PagedQueryUser), assembly, t => true);
            sw.Stop();

            Assert.Equal(
                typeof(PagedQuery<IdPaging<ProductDto>, Product, ProductDto, int>),
                res[typeof(IQuery<IdPaging<ProductDto>, IPagedEnumerable<ProductDto>>)]);

            Assert.True(sw.ElapsedMilliseconds < 50);
        }

        [Fact]
        public void PagedQuery_Execute()
        {
            var category = new Category(100, "Smarphones");

            IQuery<ProductUberProductSpecification, IPagedEnumerable<ProductDto>> pagedQuery =
                new PagedQuery<ProductUberProductSpecification, Product, ProductDto, int>(
                    new InMemoryLinqProvider(
                        new [] {new Product(category, "iPhone", 100500), new Product(category, "Galaxy s7", 3500) }),
                        new StaticAutoMapperWrapper());

            var res = pagedQuery.Execute(new ProductUberProductSpecification() {Price = 3500});

            Assert.Equal(100500, res.First().Price);
            Assert.Equal(1, res.TotalCount);
        }
    }
}
