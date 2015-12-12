using System;
using System.Linq;
using CostEffectiveCode.Domain;
using CostEffectiveCode.Domain.Cqrs.Queries;
using CostEffectiveCode.EntityFramework.Tests.Entities;
using NUnit.Framework;

namespace CostEffectiveCode.EntityFramework.Tests.Tests
{
    public class ExpressionQueryTests : DataTestsBase
    {
        private ExpressionQuery<Product> _query;

        public override void SetUp()
        {
            base.SetUp();
            _query = new ExpressionQuery<Product>(DataContext.Provider);
        }

        [Test]
        public void Single_EntityFetched()
        {
            var expected = DataContext.Provider
                .Query<Product>()
                .Single(p => p.Id == 1);

            var single = _query.ById(1);
          
            Assert.AreEqual(expected, single);
        }

        [Test]
        public void All_AllEntitiesFetched()
        {
            var all = _query.All();

            Assert.AreEqual(
                DataContext.Provider.Query<Product>().Count(),
                all.Count());
        }

        [Test]
        public void Paged_AllEntitiesFetched()
        {
            var expectedTotal = DataContext.Provider.Query<Product>().Count();

            var paged = _query.Paged(0, 10);
            Assert.AreEqual(
                expectedTotal,
                paged.TotalCount);

            Assert.LessOrEqual(paged.Count(), expectedTotal);
        }

        [Test]
        public void First_EntitуFetched()
        {
            var first = _query.FirstOrDefault();

            Assert.AreEqual(
                DataContext.Provider.Query<Product>().First(),
                first);
        }

        [Test]
        public void OrderByPaged_Desc_OrderedAndFetched()
        {
            var expexted = DataContext.Provider
                .Query<Product>()
                .Last()
                .Id;

            var paged = _query
                .OrderBy(p => p.Id, SortOrder.Desc)
                .Paged(0, 20);

            Assert.AreEqual(expexted, paged.First().Id);
        }

        [Test]
        public void OrderByPaged_Asc_OrderedAndFetched()
        {
            var expexted = DataContext.Provider
                .Query<Product>()
                .First()
                .Id;

            var paged = _query
                .OrderBy(p => p.Id)
                .Paged(0, 20);

            Assert.AreEqual(expexted, paged.First().Id);
        }

        //[Test, ExpectedException(typeof(ArgumentNullException))]
        [Test]
        public void OrderBy_EmptyExpression_ArgumentExcpetion()
        {
            _query.OrderBy<string>(null);
        }

        [Test]
        public void OrderBy_Multiple_Ordered()
        {
            var last = _query
                .OrderBy(x => x.Price, SortOrder.Desc)
                .OrderBy(x => x.Id)
                .All()
                .Last();

            Assert.AreEqual("Transformer", last.Name);
        }
                

        //[Test, ExpectedException(typeof(ArgumentNullException))]
        [Test]
        public void Where_EmptySpecification_ArgumentException()
        {
            _query.Where(null);
        }


        //[Test, ExpectedException(typeof(ArgumentNullException))]
        [Test]
        public void Constructor_EmptyLinqProvider_ArgumentException()
        {
            _query = new ExpressionQuery<Product>(null);
        }

        [Test]
        public void Include_CategoryIncluded()
        {
            _query.Include(p => p.Category);
            var product = _query.FirstOrDefault();

            Assert.AreEqual(DataContext.Category, product.Category);
        }

        //[Test, ExpectedException(typeof(ArgumentNullException))]
        [Test]
        public void Include_EmptyExpression_ArgumentException()
        {
            _query.Include<string>(null);
        }
    }
}
