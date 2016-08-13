using System;
using System.Configuration;
using CostEffectiveCode.Messaging;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace CostEffectiveCode.Tests.Tests
{
    public class DispatcherTests
    {
        private readonly Dispatcher<EventArgs> _dispatcher;

        private readonly Mock<IPublisher<string>> _rePublisherMock;

        public DispatcherTests()
        {
            _dispatcher = new Dispatcher<EventArgs>();
            _rePublisherMock = new Mock<IPublisher<string>>();
        }

        [Fact]
        public void CreateMapping_NoPatterMatching_MapsWell()
        {
            var res = "Hello";

            _dispatcher.CreateMapping(x => res, x => _rePublisherMock.Object);
            _dispatcher.Publish(new EventArgs());
            _rePublisherMock.Verify(p => p.Publish(res), Times.Once);
        }

        [Fact]
        public void CreateMapping_PatterMatching_MapsWell()
        {
            _dispatcher.CreateMapping<StringEventArgs, string>(
                x => x.Str + "456",
                x => _rePublisherMock.Object,
                x => x.Str == "123");

            _dispatcher.Publish(new StringEventArgs("123"));

            _rePublisherMock.Verify(p => p.Publish("123456"), Times.Once);
        }

        [Fact]
        public void CreateMapping_PatternMatching_ConfigurationErrorsException()
        {
            _dispatcher.CreateMapping<StringEventArgs, string>(
                x => x.Str + "456",
                x => _rePublisherMock.Object,
                x => x.Str != "123");

            Assert.Throws<ConfigurationErrorsException>(() =>
            {
                _dispatcher.Publish(new StringEventArgs("123"));
            });

            //_rePublisherMock.Verify(p => p.Publish("123456"), Times.Once);
        }

        internal class StringEventArgs : EventArgs
        {
            public string Str { get; set; }

            public StringEventArgs([NotNull] string str)
            {
                if (str == null) throw new ArgumentNullException(nameof(str));
                Str = str;
            }
        }

    }
}
