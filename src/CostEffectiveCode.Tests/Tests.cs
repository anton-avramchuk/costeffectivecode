using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CostEffectiveCode.AutoMapper;
using CostEffectiveCode.Components;
using CostEffectiveCode.Cqrs;
using CostEffectiveCode.Cqrs.Queries;
using CostEffectiveCode.Ddd.Specifications;
using CostEffectiveCode.Extensions;
using Xunit;

namespace CostEffectiveCode.Tests
{
    public class Tests
    {
        static Tests()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Product, ProductDto>());
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
           var pagedQuery = new PagedQuery<ProductUberProductSpecification, Product, ProductDto, int>(
               LinqProvider(), new StaticAutoMapperWrapper()).AsPaged();

            var sw = new Stopwatch();

            sw.Start();
            var res = pagedQuery.Ask(new ProductUberProductSpecification() {Price = 3500});
            sw.Stop();
            
            Assert.Equal(100500, res.First().Price);
            Assert.Equal(1, res.TotalCount);
            Assert.True(sw.ElapsedMilliseconds < 120, $"Elapsed Miliseconds: {sw.ElapsedMilliseconds}");
        }

        private static InMemoryLinqProvider LinqProvider()
        {
            var category = new Category(100, "Smarphones");
            var linqProvider = new InMemoryLinqProvider(
                new[] {new Product(category, "iPhone", 100500), new Product(category, "Galaxy s7", 3500)});
            return linqProvider;
        }

        [Fact]
        public void Async_Result()
        {
            var res = new OptimizedQuery(LinqProvider()).ToAsync(new ProductUberProductSpecification()).Result;
            Assert.Equal(2, res.TotalCount);
        }

        [Fact]
        public async void Async_Await()
        {
            var res = await new OptimizedQuery(LinqProvider()).ToAsync(new ProductUberProductSpecification());
            Assert.Equal(2, res.TotalCount);
        }


        [Fact]
        public void OptimizedQuery_Ask()
        {
            var pagedQuery = new OptimizedQuery(LinqProvider());
            var sw = new Stopwatch();

            sw.Start();
            var res = pagedQuery.Ask(new ProductUberProductSpecification() { Price = 3500 });
            sw.Stop();

            Assert.Equal(100500, res.First().Price);
            Assert.Equal(1, res.TotalCount);
            Assert.True(sw.ElapsedMilliseconds < 120, $"Elapsed Miliseconds: {sw.ElapsedMilliseconds}");
        }

        [Fact]
        public void TestDbContext_PagedQuery_Ask()
        {
            using (var dbContext = new TestDbContext())
            {
                var pagedQuery = new PagedQuery<ProductUberProductSpecification, Product, ProductDto, int>(
                    dbContext, new StaticAutoMapperWrapper()).AsPaged();

                var optimizedQUery = new OptimizedQuery(dbContext);

                var sw = new Stopwatch();
                // WarmUp
                var pr = dbContext.Products.First();
                sw.Start();
                var res = pagedQuery.Ask(new ProductUberProductSpecification(1, 500) {Price = 3500});
                sw.Stop();

                //Assert.Equal(99900, res.First().Price);
                //Assert.Equal(964, res.TotalCount);
                Assert.True(sw.ElapsedMilliseconds < 300, $"Elapsed Miliseconds: {sw.ElapsedMilliseconds}");
            }
        }
    }
}
