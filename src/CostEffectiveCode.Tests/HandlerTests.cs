using CostEffectiveCode.AutoMapper;
using CostEffectiveCode.Cqrs.Commands;
using CostEffectiveCode.Ddd;
using Moq;
using Xunit;

namespace CostEffectiveCode.Tests
{
    public class HandlerTests
    {
        static HandlerTests()
        {
            StaticAutoMapperWrapper.Init(cfg => cfg.CreateMissingTypeMaps = true);
        }

        [Fact]
        public void CreateOrUpdateHandler_AddAndSave()
        {
            var uow = new Mock<IUnitOfWork>();
            var createOrUpdateCommandHandler = new CreateOrUpdateHandler<int, ProductDto, Product>(
                uow.Object, new StaticAutoMapperWrapper());

            
            createOrUpdateCommandHandler.Handle(new ProductDto()
            {
                CategoryId = 1,
                Price = 100500
            });

            uow.Verify(x => x.Add(It.Is<Product>(y => y.Price == 100500)), Times.Once);
            uow.Verify(x => x.Commit(), Times.Once);
        }
    }
}
