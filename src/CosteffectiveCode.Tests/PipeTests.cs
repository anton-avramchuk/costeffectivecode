using System;
using System.Diagnostics;
using System.Drawing;
using CosteffectiveCode.FunctionalProgramming;
using Xunit;

namespace CosteffectiveCode.Tests
{
    public class PipeTests
    {
        [Fact]
        public void Pipe_Func()
        {
            var point = Pipeline
                .Start(10, x => x + 6)
                .Next(x => x.ToString())
                .Next(int.Parse)
                .Next(x => Math.Sqrt(x))
                .Next(x => x*5)
                .Next(x => new Point((int)Math.Round(x), 120))
                .Finish();

            Assert.Equal(20, point.X);
            Assert.Equal(120, point.Y);
        }

        private static string Abc;

        [Fact]
        public void Pipe_Action()
        {
            Pipeline
                .Start(() => Debug.WriteLine("HoHo"))
                .Next(() => Abc = "Abc")
                .Finish();

            Assert.Equal("Abc", Abc);
        }

        [Fact]
        public void Pipe_Mix()
        {
            Pipeline
                .Start(10, x => x + 6)
                .Next(x => x.ToString())
                .Next(int.Parse)
                .Next(() => Debug.WriteLine("HoHo"))
                .Finish();
        }
    }
}
