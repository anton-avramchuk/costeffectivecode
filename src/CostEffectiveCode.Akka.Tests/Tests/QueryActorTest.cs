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
using Xunit;

namespace CostEffectiveCode.Akka.Tests.Tests
{
    public class QueryActorTest : TestKit
    {
        public static string ConnectionString = @"Data Source=.;Initial Catalog=CostEffectiveCode_Sample_Data_Tests;Integrated Security=False;MultipleActiveResultSets=True;User ID=superuser;Password=Devpas123";

        public QueryActorTest()
            : base(@"akka.loglevel = DEBUG")
        {
        }

        [Fact]
        public void FetchRequestMessageTold_FetchResponseMessageReceived()
        {
            // arrange
            var scopedExpressionQuery = new ScopedExpressionQuery<Product, SampleDbContext>(
                () => new SampleDbContext(ConnectionString), x => true);

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
