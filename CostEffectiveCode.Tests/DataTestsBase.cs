using NUnit.Framework;

namespace CostEffectiveCode.Tests
{
    public class DataTestsBase
    {
        protected TestDataContext DataContext;

        [SetUp]
        public virtual void SetUp()
        {
            DataContext = new TestDataContext();
        }
    }
}