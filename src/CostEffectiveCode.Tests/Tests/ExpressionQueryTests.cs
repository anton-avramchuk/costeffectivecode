using System;
using System.Linq;
using CostEffectiveCode.Domain;
using CostEffectiveCode.Domain.Cqrs.Queries;
using CostEffectiveCode.EntityFramework6;
using CostEffectiveCode.SampleProject.Domain.Entities;
using Xunit;

namespace CostEffectiveCode.Tests.Tests
{
    public class ExpressionQueryTests : DataTestsBase
    {
        private ExpressionQuery<Product> _query;

        public ExpressionQueryTests() : base(new TestDataContext())
        {
            _query = new ExpressionQuery<Product>(DataContext.Provider);
        }

        [Fact]
        public void Single_EntityFetched()
        {
            var expected = DataContext.Provider
                .Query<Product>()
                .Single(p => p.Id == 1);

            var single = _query.ById(1);

            Assert.Equal(expected, single);
        }

        [Fact]
        public void All_AllEntitiesFetched()
        {
            var all = _query.All();

            Assert.Equal(
                DataContext.Provider.Query<Product>().Count(),
                all.Count());
        }

        [Fact]
        public void Paged_AllEntitiesFetched()
        {
            var expectedTotal = DataContext.Provider.Query<Product>().Count();

            var paged = _query.Paged(0, 10);
            Assert.Equal(
                expectedTotal,
                paged.TotalCount);

            Assert.True(paged.Count() <= expectedTotal);
        }

        [Fact]
        public void First_EntitуFetched()
        {
            var first = _query.FirstOrDefault();

            Assert.Equal(
                DataContext.Provider.Query<Product>().First(),
                first);
        }

        [Fact]
        public void OrderByPaged_Desc_OrderedAndFetched()
        {
            var expexted = DataContext.Provider
                .Query<Product>()
                .Last()
                .Id;

            var paged = _query
                .OrderBy(p => p.Id, SortOrder.Desc)
                .Paged(0, 20);

            Assert.Equal(expexted, paged.First().Id);
        }

        [Fact]
        public void OrderByPaged_Asc_OrderedAndFetched()
        {
            var expexted = DataContext.Provider
                .Query<Product>()
                .First()
                .Id;

            var paged = _query
                .OrderBy(p => p.Id)
                .Paged(0, 20);

            Assert.Equal(expexted, paged.First().Id);
        }

        [Fact]
        public void OrderBy_EmptyExpression_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => _query.OrderBy<string>(null));
        }

        [Fact]
        public void OrderBy_Multiple_Ordered()
        {
            var last = _query
                .OrderBy(x => x.Price, SortOrder.Desc)
                .OrderBy(x => x.Id)
                .All()
                .Last();

            Assert.Equal("Transformer", last.Name);
        }


        [Fact]
        public void Where_EmptySpecification_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => _query.Where(null));
        }


        [Fact]
        public void Constructor_EmptyLinqProvider_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => _query = new ExpressionQuery<Product>(null));
        }

        [Fact]
        public void Include_CategoryIncluded()
        {
            _query.Include(p => p.Category);
            var product = _query.FirstOrDefault();

            Assert.Equal(DataContext.Category, product.Category);
        }

        [Fact]
        public void Include_EmptyExpression_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => _query.Include<string>(null));
        }
    }
}
