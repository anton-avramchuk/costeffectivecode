using System;
using System.Linq;
using Akka.Actor;
using Akka.DI.AutoFac;
using Akka.DI.Core;
using Akka.TestKit.Xunit2;
using CostEffectiveCode.Akka.Actors;
using CostEffectiveCode.Akka.Messages;
using CostEffectiveCode.Sample.Domain.Entities;
using Xunit;

namespace CostEffectiveCode.Akka.Tests.Tests
{
    public class QueryActorTests : TestKit
    {
        private const int MaxProducts = 6;

        public QueryActorTests()
            : base(@"akka.loglevel = DEBUG")
        {
            var containerConfig = new ContainerConfig();
            containerConfig.Configure();

            // ReSharper disable once ObjectCreationAsStatement
            new AutoFacDependencyResolver(containerConfig.AutofacContainer, Sys);
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
            var queryActor = GeneralCaseArrange();

            // act
            GeneralCaseAct(new FetchRequestMessageBase(true), queryActor);

            // assert
            var failureMessage = ExpectMsg<Failure>(new TimeSpan(0, 0, 10));
            Assert.NotNull(failureMessage.Exception);
            Assert.NotNull(failureMessage.Timestamp);
        }


        private void GeneralCase(FetchRequestMessageBase request, Func<FetchResponseMessage<Product>, bool> assertFunc)
        {
            // arrange
            var queryActor = GeneralCaseArrange();

            // act
            GeneralCaseAct(request, queryActor);

            // assert
            var responseMessage = ExpectMsg<FetchResponseMessage<Product>>(new TimeSpan(0, 0, 10));

            Assert.NotNull(responseMessage.Entities);
            Assert.True(assertFunc(responseMessage)); // magic number assumed to present in db at least
        }

        private static void GeneralCaseAct(FetchRequestMessageBase request, IActorRef queryActor)
        {
            queryActor.Tell(request);
        }

        private IActorRef GeneralCaseArrange()
        {
            var queryActorProps = Sys.DI().Props<QueryActor<Product>>();

            return Sys.ActorOf(queryActorProps, "queryActorProduct");
        }
    }
}
