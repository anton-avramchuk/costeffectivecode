using System;
using System.Configuration;
using CostEffectiveCode.Common;
using CostEffectiveCode.Messaging;
using JetBrains.Annotations;
using Moq;
using NUnit.Framework;

namespace CostEffectiveCode.EntityFramework.Tests.Tests
{
    [TestFixture]
    public class DispatcherTests
    {
        private Dispatcher<EventArgs> _dispatcher;

        private Mock<IPublisher<string>> _rePublisherMock1;

        [SetUp]
        public void SetUp()
        {
            _dispatcher = new Dispatcher<EventArgs>();
            _rePublisherMock1 = new Mock<IPublisher<string>>();
        }

        [Test]
        public void CreateMapping_NoPatterMatching_MapsWell()
        {
            var res = "Hello";

            _dispatcher.CreateMapping(x => res, x => _rePublisherMock1.Object);
            _dispatcher.Publish(new EventArgs());
            _rePublisherMock1.Verify(p => p.Publish(res), Times.Once);
        }

        [Test]
        public void CreateMapping_PatterMatching_MapsWell()
        {
            _dispatcher.CreateMapping<StringEventArgs, string>(
                x => x.Str + "456",
                x => _rePublisherMock1.Object,
                x => x.Str == "123");

            _dispatcher.Publish(new StringEventArgs("123"));

            _rePublisherMock1.Verify(p => p.Publish("123456"), Times.Once);
        }

        [Test]
        //[Test, ExpectedException(typeof(ConfigurationErrorsException))]
        public void CreateMapping_PatterMatching_ConfigurationException()
        {
            _dispatcher.CreateMapping<StringEventArgs, string>(
                x => x.Str + "456",
                x => _rePublisherMock1.Object,
                x => x.Str != "123");

            _dispatcher.Publish(new StringEventArgs("123"));

            _rePublisherMock1.Verify(p => p.Publish("123456"), Times.Once);
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
