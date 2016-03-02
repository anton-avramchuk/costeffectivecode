using System;
using System.Linq;
using Akka.Actor;
using Akka.TestKit.Xunit2;
using CostEffectiveCode.Akka.Actors;
using CostEffectiveCode.Akka.Messages;
using CostEffectiveCode.Common;
using CostEffectiveCode.Domain.Ddd.Specifications;
using CostEffectiveCode.EntityFramework6;
using CostEffectiveCode.Sample.Data;
using CostEffectiveCode.Sample.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace CostEffectiveCode.Akka.Tests.Tests
{
    public class QueryActorTests : TestKit
    {
        private const int MaxProducts = 6;
        private readonly IConfigurationRoot _configuration;

        public QueryActorTests()
            : base(@"akka.loglevel = DEBUG")
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");

            //builder.AddEnvironmentVariables();

            _configuration = builder.Build();
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
            // assert
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
// arrange
            var scopedExpressionQuery = new ScopedExpressionQuery<Product, SampleDbContext>(
                () => new SampleDbContext(_configuration["Data:DefaultConnection:ConnectionString"]), x => true);

            var queryActor = Sys.ActorOf(
                Props.Create(() => new QueryActor<Product, ExpressionSpecification<Product>>(
                    new PassThroughScope<ScopedExpressionQuery<Product, SampleDbContext>>(scopedExpressionQuery),
                    null, null)), "queryActorProduct");
            return queryActor;
        }
    }
}
