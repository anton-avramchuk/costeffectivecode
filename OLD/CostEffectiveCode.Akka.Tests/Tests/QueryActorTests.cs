using System;
using System.Linq;
using Akka.Actor;
using Akka.DI.AutoFac;
using Akka.DI.Core;
using Akka.TestKit.Xunit2;
using CostEffectiveCode.Akka.Actors;
using CostEffectiveCode.Akka.Messages;
using CostEffectiveCode.Common.Scope;
using CostEffectiveCode.Domain;
using CostEffectiveCode.Domain.Cqrs.Queries;
using CostEffectiveCode.Domain.Ddd.Specifications;
using CostEffectiveCode.Sample.Domain.Entities;
using Xunit;

namespace CostEffectiveCode.Akka.Tests.Tests
{
    public class QueryActorTests : TestKit
    {
        private readonly ContainerConfig _containerConfig;
        private const int MaxEntities = 6; // magic number assumed to present in db at least

        public QueryActorTests()
            : base(@"akka.loglevel = DEBUG")
        {
            _containerConfig = new ContainerConfig();
            _containerConfig.Configure();

            // ReSharper disable once ObjectCreationAsStatement
            new AutoFacDependencyResolver(_containerConfig.AutofacContainer, Sys);
        }

        [Fact]
        public void AskedForAllProducts_GotAllProducts()
        {
            GeneralCase(new FetchRequestMessageBase(), x => x.Entities.Count() >= MaxEntities);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(1)]
        [InlineData(0)]
        public void AskedForSeveralProducts_GotSeveralProducts(int limit)
        {
            GeneralCase(new FetchRequestMessageBase(limit), x => x.Entities.Count() == limit);
        }

        [Fact]
        public void AskedForSingleProduct_GotFailure()
        {
            // arrange
            var queryActor = GetEmptyQueryActor();

            // act
            queryActor.Tell(new FetchRequestMessageBase(true, false));

            // assert
            var failureMessage = ExpectMsg<Failure>(new TimeSpan(0, 0, 10));
            Assert.NotNull(failureMessage.Exception);
            Assert.NotNull(failureMessage.Timestamp);
        }

        [Fact]
        public void AskedForFirstProductWithCategory_GotOneProductWithCategory()
        {
            // arrange
            var queryActor = GetEmptyQueryActor();

            // act
            queryActor.Tell(new FetchRequestMessage<Product>(false, true)
                .Include(x => x.Category));

            // assert
            var responseMessage = ExpectMsg<FetchResponseMessage<Product>>(new TimeSpan(1, 0, 10));

            Assert.NotNull(responseMessage.Entities);
            Assert.NotNull(responseMessage.Entities.Single());
            Assert.NotNull(responseMessage.Entities.Single().Category);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(1)]
        [InlineData(0)]
        public void AskedForSeveralProductsWithCategories_GotSeveralProductsWithCategories(int limit)
        {
            // arrange
            var queryActor = GetEmptyQueryActor();

            // act
            queryActor.Tell(new FetchRequestMessage<Product>(limit)
                .Include(x => x.Category));

            // assert
            var responseMessage = ExpectMsg<FetchResponseMessage<Product>>(new TimeSpan(0, 0, 10));

            Assert.NotNull(responseMessage.Entities);
            Assert.NotNull(responseMessage.Entities.Count() == limit);

            foreach (var product in responseMessage.Entities)
            {
                Assert.NotNull(product.Category);
                Assert.NotNull(product.Category.Products.SingleOrDefault(x => x.Id == product.Id));
            }
        }

        [Fact(Skip = "not supported yet for some reason. TODO: investigate!")]
        public void AskedForFirstProductWithCategoryAndAllItsProducts_GotOneProductWithCategoryAndAllItsProducts()
        {
            // arrange
            var queryActor = GetEmptyQueryActor();

            // act
            queryActor.Tell(new FetchRequestMessage<Product>(false, true)
                .Include(x => x.Category.Products));
                // Hint: does not work: .Include(x => x.Category.Products));

            // assert
            var responseMessage = ExpectMsg<FetchResponseMessage<Product>>(new TimeSpan(1, 0, 10));

            Assert.NotNull(responseMessage.Entities);
            Assert.NotNull(responseMessage.Entities.Single());
            Assert.NotNull(responseMessage.Entities.Single().Category);
            Assert.NotNull(responseMessage.Entities.Single().Category.Products);
            Assert.True(responseMessage.Entities.Single().Category.Products.Count > 1);
        }



        [Fact]
        public void AskedForExpensiveProducts_GotOnlyExpensiveProducts()
        {
            GeneralCase(
                new FetchRequestMessage<Product>()
                    .Where(new ExpressionSpecification<Product>(x => x.Price >= 1000))
                    , x => x.Entities.Count() < MaxEntities && x.Entities.All(y => y.Price >= 1000));
        }

        [Fact]
        public void BaseQueryActiveProducts_AskedForAllProducts_GotAllProducts()
        {
            // arrange
            var queryActor = GetBaseQueryActor();

            // act
            queryActor.Tell(new FetchRequestMessageBase());

            // assert
            GeneralAssert(x => x.Entities.Count() >= MaxEntities);
        }

        [Fact]
        public void BaseQueryActiveProducts_AskedForExpensiveProducts_GotOnlyActiveExpensiveProducts()
        {
            var queryActor = GetBaseQueryActor();

            queryActor.Tell(
                new FetchRequestMessage<Product>()
                    .Where(new ExpressionSpecification<Product>(x => x.Price >= 1000)));

            GeneralAssert(x => x.Entities.Count() < MaxEntities
                && x.Entities.All(y => y.Active && y.Price >= 1000));
        }

        [Theory]
        [InlineData(3)]
        [InlineData(1)]
        [InlineData(0)]
        public void BaseQueryActiveProducts_AskedForSeveralProducts_GotSeveralProducts(int limit)
        {
            // arrange
            var queryActor = Sys.ActorOf(Props.Create(() =>
                new QueryActor<Product>(
                    new DelegateScope<IQuery<Product, IExpressionSpecification<Product>>>(() => BaseActiveProductsQuery)
                )));

            // act
            queryActor.Tell(new FetchRequestMessageBase(3));

            // assert
            GeneralAssert(x => x.Entities.Count() == 3);
        }

        private void GeneralCase(FetchRequestMessageBase requestMessage, Func<FetchResponseMessage<Product>, bool> assertFunc)
        {
            // arrange
            var queryActor = GetEmptyQueryActor();

            // act
            queryActor.Tell(requestMessage);

            // assert
            GeneralAssert(assertFunc);
        }

        private IActorRef GetEmptyQueryActor()
        {
            var queryActorProps = Sys.DI().Props<QueryActor<Product>>();

            return Sys.ActorOf(queryActorProps, "testedQueryActor");
        }

        private IActorRef GetBaseQueryActor()
        {
            var queryActor = Sys.ActorOf(Props.Create(() =>
                new QueryActor<Product>(
                    new DelegateScope<IQuery<Product, IExpressionSpecification<Product>>>(() => BaseActiveProductsQuery)
                    )), "testedQueryActor");
            return queryActor;
        }

        private IQuery<Product, IExpressionSpecification<Product>> BaseActiveProductsQuery =>
            _containerConfig
                .Container
                .Resolve<IQuery<Product, IExpressionSpecification<Product>>>()
                .Where(Product.ActiveRule);

        private void GeneralAssert(Func<FetchResponseMessage<Product>, bool> assertFunc)
        {
            var responseMessage = ExpectMsg<FetchResponseMessage<Product>>(new TimeSpan(0, 0, 10));

            Assert.NotNull(responseMessage.Entities);
            Assert.True(assertFunc(responseMessage));
        }
    }
}
