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
                .Pipe(10, x => x + 6)
                .Pipe(x => x.ToString())
                .Pipe(int.Parse)
                .Pipe(x => Math.Sqrt(x))
                .Pipe(x => x*5)
                .Pipe(x => new Point((int)Math.Round(x), 120))
                .Execute();
            Assert.Equal(20, point.X);
            Assert.Equal(120, point.Y);
        }

        [Fact]
        public void Pipe_Start()
        {
            var point = Pipeline
                .Start(() => 10, x => x + 6)
                .Pipe(x => x.ToString())
                .Pipe(int.Parse)
                .Pipe(x => Math.Sqrt(x))
                .Pipe(x => x * 5)
                .Pipe(x => new Point((int)Math.Round(x), 120))
                .Execute();

            Assert.Equal(20, point.X);
            Assert.Equal(120, point.Y);
        }

        [Fact]
        public void Pipe_Compose_Func()
        {
            var composition = Pipeline
                .Pipe(10, x => x + 6)
                .Pipe(x => x.ToString())
                .Pipe(int.Parse)
                .Pipe(x => Math.Sqrt(x))
                .Pipe(x => x*5)
                .Pipe(x => new Point((int) Math.Round(x), 120));

            var point = composition.Execute();
            Assert.Equal(20, point.X);
            Assert.Equal(120, point.Y);
        }

        private static string Abc;

        [Fact]
        public void Pipe_Action()
        {
            var composition = Pipeline
                .Do(() => Debug.WriteLine("HoHo"))
                .Do(() => Abc = "Abc");

            composition.Execute();
            Assert.Equal("Abc", Abc);
        }

        [Fact]
        public void Pipe_Mix()
        {
            var pipeline = Pipeline
                .Start(() => 10, x => x + 6)
                .Pipe(x => x.ToString())
                .Pipe(int.Parse)
                .Pipe(x => Math.Sqrt(x))
                .Pipe(x => x*5)
                .Pipe(x => new Point((int) Math.Round(x), 120))
                .Finish(x => Debug.WriteLine($"{x.X}{x.Y}"))
                .Do(() => Debug.WriteLine("Point is so cool"));

            // do something else
            pipeline.Execute();
        }
    }
}
