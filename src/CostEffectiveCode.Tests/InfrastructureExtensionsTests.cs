using System;
using CostEffectiveCode.Extensions;
using CostEffectiveCode.Tests.Stubs;
using Xunit;

namespace CostEffectiveCode.Tests
{
    public class InfrastructureExtensionsTests
    {
        [Fact]
        public void Is_True()
        {
            Assert.True("123".Is(x => int.Parse(x) > 1));
        }

        [Fact]
        public void Is_False()
        {
            Assert.False("123".Is(x => int.Parse(x) > 1000));
        }

        [Fact]
        public void DoubleInsert_Are_Equal()
        {
            var first = "123".Is(x => int.Parse(x) > 1000);
            var second = "123".Is(x => int.Parse(x) > 1000);
            Assert.Equal(first, second);
        }

        [Fact]
        public void Do_123_NoException()
        {
            "123".Do(x => x + "!", () => new Exception("Excpetion"));
        }

        [Fact]
        public void Do_Null_NoException()
        {
            string a = null;
            Assert.Throws<Exception>(() => a.Do(x => x + "!", () => new Exception("Excpetion")));
        }

        [Fact]
        public void Forward_Func_Query_CommandSuccess()
        {
            var res = "123"
                .Forward(x => x)
                .Forward(new SimpleQuery())
                .Forward(new SimpleCommandHandler());

            Assert.Equal("123", res);
            res.Forward(new SimpleCommandHandler2());
        }

        [Fact]
        public void ToFunc_Query()
        {
            var q = new SimpleQuery();
            var func = q.ToFunc();
            Assert.Equal(q.Ask("123"), func.Invoke("123"));
        }

        [Fact]
        public void ToFunc_CommandHandler()
        {
            var q = new SimpleCommandHandler();
            var func = q.ToFunc();
            Assert.Equal(q.Handle("123"), func.Invoke("123"));
        }

        [Fact]
        public void Match_Success()
        {
            Assert.Equal("123456", "123".Match(x => x.StartsWith("123"), x => x + "456"));
        }

        [Fact]
        public void WithMatched_Success()
        {
            Assert.Equal("123456", "123".WithMatched<string, string>("456", (x,y) => x + y));
        }

        [Fact]
        public void WithMatched_Failure()
        {
            Assert.Equal("123", "123".WithMatched<string, string>(456, (x,y) => x + y));
        }
    }
}
