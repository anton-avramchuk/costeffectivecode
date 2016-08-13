using Xunit;

namespace CostEffectiveCode.Tests
{
    public class DataTestsBase : IClassFixture<TestDataContext>
    {
        public DataTestsBase(TestDataContext dataContext)
        {
            DataContext = dataContext;
        }

        public TestDataContext DataContext { get; set; }
    }
}