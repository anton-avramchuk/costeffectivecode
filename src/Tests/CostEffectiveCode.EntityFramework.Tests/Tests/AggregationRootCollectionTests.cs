using System.Collections;
using System.Linq;
using CostEffectiveCode.Domain.Ddd;
using CostEffectiveCode.SampleProject.Domain.Entities;
using NUnit.Framework;

namespace CostEffectiveCode.EntityFramework.Tests.Tests
{
    public class AggregationRootCollectionTests : DataTestsBase
    {
        private AggregationRootCollection<Category, Product> _collection;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _collection = new AggregationRootCollection<Category, Product>(
                DataContext.Category,
                DataContext.Provider.Query<Product>()
                    .ToArray());
        }

        //[Test, ExpectedException(typeof(ArgumentNullException))]
        [Test]
        public void Constructor_FirstGuard()
        {
            _collection = new AggregationRootCollection<Category, Product>(null, null);
        }

        //[Test, ExpectedException(typeof(ArgumentNullException))]
        [Test]
        public void Constructor_SecondGuard()
        {
            _collection = new AggregationRootCollection<Category, Product>(DataContext.Category, null);
        }

        [Test]
        public void Add_AggregationRootChanged()
        {
            var category = new Category("New Category");
            var product = DataContext.Provider.Query<Product>().First();

            category.Products.Add(product);

            Assert.AreEqual(category, product.Category);
        }

        //[Test, ExpectedException(typeof(ArgumentNullException))]
        [Test]
        public void Remove_ProductCategoryGuard()
        {
            var first = _collection.First();
            Assert.AreEqual(DataContext.Category, first.Category);

            _collection.Remove(first);
        }

        [Test]
        public void GetEnumerator_ReturnsEnumerator()
        {
            Assert.IsNotNull(_collection.GetEnumerator());
            Assert.IsNotNull(((IEnumerable)_collection).GetEnumerator());
        }

        [Test]
        public void Remove()
        {
            var res = _collection.Remove(new Product());
            Assert.IsFalse(res);
        }

        [Test]
        public void Count_WorksAsExpected()
        {
            Assert.AreEqual(
                DataContext.Provider.Query<Product>().Count(),
                _collection.Count);
        }

        [Test]
        public void IsReadOnly_ReturnsFalse()
        {
            Assert.IsFalse(_collection.IsReadOnly);
        }

        [Test]
        public void Contains_DecoratorksWorksAsExpected()
        {
            var first = _collection.First();
            var actual = _collection.Contains(first);

            Assert.IsTrue(actual);
        }

        //[Test, ExpectedException(typeof(ArgumentNullException))]
        [Test]
        public void Clear_DecoratorksWorksAsExpected()
        {
            _collection.Clear();
            Assert.IsTrue(_collection.Count == 0);
        }

        [Test]
        public void CopyTo_DecoratorksWorksAsExpected()
        {
            var newArray = new Product[33];
            _collection.CopyTo(newArray, 0);

            Assert.IsTrue(DataContext.Provider.Query<Product>().All(p =>  newArray.Contains(p)));
        }

    }
}
