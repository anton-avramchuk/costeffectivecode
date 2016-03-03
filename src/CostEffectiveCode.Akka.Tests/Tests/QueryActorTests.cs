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
        private const int MaxProducts = 6; // magic number assumed to present in db at least

        public QueryActorTests()
            : base(@"akka.loglevel = DEBUG")
        {
            _containerConfig = new ContainerConfig();
            _containerConfig.Configure();

            // ReSharper disable once ObjectCreationAsStatement
            new AutoFacDependencyResolver(_containerConfig.AutofacContainer, Sys);
        }


        [Fact]
        public void RequestedAll_ResponsedAllEntities()
        {
            GeneralCase(new FetchRequestMessageBase(), x => x.Entities.Count() >= MaxProducts);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(1)]
        [InlineData(0)]
        public void RequestedLimitedNumber_ToldLimitedNumber(int limit)
        {
            GeneralCase(new FetchRequestMessageBase(limit), x => x.Entities.Count() == limit);
        }

        [Fact]
        public void RequestedSingle_ResponsedFailure()
        {
            // assert
            var queryActor = GeneralArrange();

            // act
            GeneralAct(new FetchRequestMessageBase(true), queryActor);

            // assert
            var failureMessage = ExpectMsg<Failure>(new TimeSpan(0, 0, 10));
            Assert.NotNull(failureMessage.Exception);
            Assert.NotNull(failureMessage.Timestamp);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(1)]
        [InlineData(0)]
        public void CustomBaseQuery_RequestedLimited_ResponsedLimitedNumber(int limit)
        {
            var queryActor = Sys.ActorOf(Props.Create(() =>
                new QueryActor<Product>(
                    new DelegateScope<IQuery<Product, IExpressionSpecification<Product>>>(GetBaseQuery)
                )));

            GeneralAct(new FetchRequestMessageBase(3), queryActor);

            GeneralAssert(x => x.Entities.Count() == 3);
        }

        private IQuery<Product, IExpressionSpecification<Product>> GetBaseQuery()
        {
            return _containerConfig
                .Container
                .Resolve<IQuery<Product, IExpressionSpecification<Product>>>()
                .Where(x => x.Active);
        }


        private void GeneralCase(FetchRequestMessageBase request, Func<FetchResponseMessage<Product>, bool> assertFunc)
        {
            // arrange
            var queryActor = GeneralArrange();

            // act
            GeneralAct(request, queryActor);

            // assert
            GeneralAssert(assertFunc);
        }

        private IActorRef GeneralArrange()
        {
            var queryActorProps = Sys.DI().Props<QueryActor<Product>>();

            return Sys.ActorOf(queryActorProps, "queryActorProduct");
        }


        private static void GeneralAct(FetchRequestMessageBase request, IActorRef queryActor)
        {
            queryActor.Tell(request);
        }

        private void GeneralAssert(Func<FetchResponseMessage<Product>, bool> assertFunc)
        {
            var responseMessage = ExpectMsg<FetchResponseMessage<Product>>(new TimeSpan(0, 0, 10));

            Assert.NotNull(responseMessage.Entities);
            Assert.True(assertFunc(responseMessage));
        }
    }
}
