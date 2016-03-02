using System;
using System.Linq;
using Akka.Actor;
using Akka.TestKit.Xunit2;
using CostEffectiveCode.Akka.Actors;
using CostEffectiveCode.Akka.Messages;
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
        public void FetchRequestMessageTold_FetchResponseMessageReceived()
        {
            // arrange
            var scopedExpressionQuery = new ScopedExpressionQuery<Product, SampleDbContext>(
                () => new SampleDbContext(_configuration["Data:DefaultConnection:ConnectionString"]), x => true);

            var queryActor = Sys.ActorOf(
                Props.Create(() => new QueryActor<Product, ExpressionSpecification<Product>>(scopedExpressionQuery, null, null)), "queryActorProduct");

            // act
            queryActor.Tell(new FetchRequestMessage(10));

            // assert
            var responseMessage = ExpectMsg<FetchResponseMessage<Product>>();

            Assert.NotNull(responseMessage.Entities);
            Assert.True(responseMessage.Entities.Count() > 1);
        }
    }
}
