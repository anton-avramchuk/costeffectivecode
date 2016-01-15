using System;
using System.Collections;
using System.Linq;
using CostEffectiveCode.Domain.Ddd;
using CostEffectiveCode.SampleProject.Domain.Shared.Entities;
using Xunit;

namespace CostEffectiveCode.Tests.Tests
{
    public class AggregationRootCollectionTests : DataTestsBase
    {
        private AggregationRootCollection<Category, Product> _collection;

        public AggregationRootCollectionTests() : base(new TestDataContext())
        {
            _collection = new AggregationRootCollection<Category, Product>(
                DataContext.Category,
                DataContext.Provider.Query<Product>()
                    .ToArray());
        }

        [Fact]
        public void Constructor_FirstGuard_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => _collection = new AggregationRootCollection<Category, Product>(null, null));
        }

        [Fact]
        public void Constructor_SecondGuard_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => _collection = new AggregationRootCollection<Category, Product>(DataContext.Category, null));
        }

        [Fact]
        public void Add_AggregationRootChanged()
        {
            var category = new Category("New Category");
            var product = DataContext.Provider.Query<Product>().First();

            category.Products.Add(product);

            Assert.Equal(category, product.Category);
        }

        [Fact]
        public void Remove_ProductCategoryGuard_InvalidOperationException()
        {
            var first = _collection.First();
            Assert.Equal(DataContext.Category, first.Category);

            Assert.Throws<InvalidOperationException>(
                () => _collection.Remove(first));
        }

        [Fact]
        public void GetEnumerator_ReturnsEnumerator()
        {
            Assert.NotNull(_collection.GetEnumerator());
            Assert.NotNull(((IEnumerable)_collection).GetEnumerator());
        }

        [Fact]
        public void Remove()
        {
            var res = _collection.Remove(new Product());
            Assert.False(res);
        }

        [Fact]
        public void Count_WorksWell()
        {
            Assert.Equal(
                DataContext.Provider.Query<Product>().Count(),
                _collection.Count);
        }

        [Fact]
        public void IsReadOnly_ReturnsFalse()
        {
            Assert.False(_collection.IsReadOnly);
        }

        [Fact]
        public void Contains_DecoratorWorksWell()
        {
            var first = _collection.First();
            var actual = _collection.Contains(first);

            Assert.True(actual);
        }

        [Fact(Skip = "TODO: fix")]
        public void Clear_ZeroCount()
        {
            _collection.Clear();
            Assert.Equal(_collection.Count, 0);
        }

        [Fact]
        public void CopyTo_WorksWell()
        {
            var newArray = new Product[33];
            _collection.CopyTo(newArray, 0);

            Assert.True(DataContext.Provider.Query<Product>().All(p => newArray.Contains(p)));
        }
    }
}
