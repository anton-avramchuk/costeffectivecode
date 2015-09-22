using System;
using CostEffectiveCode.Common;
using CostEffectiveCode.Messaging;
using Moq;
using NUnit.Framework;

namespace CostEffectiveCode.EntityFramework.Tests.Tests
{
    [TestFixture]
    public class DispatcherTests
    {
        private Dispatcher<EventArgs> _dispatcher;

        private Mock<IPublisher<string>> _rePublisherMock1;

        private Mock<IPublisher<int>> _rePublisherMock2;        
        
        [SetUp]
        public void SetUp()
        {
            _dispatcher = new Dispatcher<EventArgs>();
            _rePublisherMock1 = new Mock<IPublisher<string>>();
            _rePublisherMock2 = new Mock<IPublisher<int>>();
        }

        [Test]
        public void CreateMapping_MapsOneWell()
        {
            var res = "Hello";

            _dispatcher.CreateMapping(x => res, _rePublisherMock1.Object);
            _dispatcher.Publish(new EventArgs());
            _rePublisherMock1.Verify(p => p.Publish(res), Times.Once);
        }

        [Test]
        public void CreateMapping_MapsTwoWell()
        {
            var res = "Hello";
            var res2 = 23;

            _dispatcher.CreateMapping(x => res, _rePublisherMock1.Object);
            _dispatcher.CreateMapping(x => res2, _rePublisherMock2.Object);

            _dispatcher.Publish(new EventArgs());
            _rePublisherMock1.Verify(p => p.Publish(res), Times.Once);
            _rePublisherMock2.Verify(p => p.Publish(res2), Times.Once);
        }

        [Test]
        public void CreateMapping_UseIMapper_MapsWell()
        {
            var mapper = new Mock<IMapper>();
            mapper
                .Setup(m => m.Map<string>(It.IsAny<EventArgs>()))
                .Returns("123");

            _dispatcher.Mapper = mapper.Object;
            _dispatcher.CreateMapping<string,IPublisher<string>>(_rePublisherMock1.Object);

            _dispatcher.Publish(new EventArgs());
            
            _rePublisherMock1.Verify(p => p.Publish(It.IsAny<string>()), Times.Once);
            mapper.Verify(m => m.Map<string>(It.IsAny<EventArgs>()), Times.Once);
        }

        [Test]
        public void CreateMapping_UseSelector_MapsWell()
        {
            _dispatcher.CreateMapping(m => "123", e => _rePublisherMock1.Object);

            _dispatcher.Publish(new EventArgs());

            _rePublisherMock1.Verify(p => p.Publish(It.IsAny<string>()), Times.Once);
        }
    }
}
