using CostEffectiveCode.AutoMapper;
using CostEffectiveCode.Ddd;
using Moq;
using Xunit;

namespace CostEffectiveCode.Tests
{
    public class DtoToEntityConverterTests
    {
        [Fact]
        public void Convert_Success()
        {
            var uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x
                .Find<Category>(It.IsAny<object>()))
                .Returns(new Category(10, "Super") {Id = 1});

            var converter = new DtoEntityTypeConverter<int, ProductDto, Product>(uow.Object);
            var res = converter.Convert(new ProductDto() {CategoryId = 1, Price = 100500}, null, null);

            Assert.Equal(100500, res.Price);
        }
    }
}
