using System;
using System.Collections.Generic;
using System.Configuration;
using CostEffectiveCode.Common;
using JetBrains.Annotations;

namespace CostEffectiveCode.Messaging
{
    public class Dispatcher<T> : IPublisher<T>
    {
        #region Vars

        private IMapper _mapper;

        protected Dictionary<Type, List<IRepublisher>> _publishers;

        [CanBeNull]
        public IMapper Mapper
        {
            get { return _mapper; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                _mapper = value;
            }
        }

        #endregion

        #region Internal Helpers

        internal class Converter<TPublisherEventArgs> : IRepublisher
        {
            private readonly Func<T, TPublisherEventArgs> _func;

            private readonly Func<T, IPublisher<TPublisherEventArgs>> _outterPublisherFunc;

            private readonly IPublisher<TPublisherEventArgs> _outterPublisher;

            internal Converter(Func<T, TPublisherEventArgs> func,
                Func<T, IPublisher<TPublisherEventArgs>> outterPublisherFunc)
            {
                _func = func;
                _outterPublisherFunc = outterPublisherFunc;
            }

            
            internal Converter(Func<T, TPublisherEventArgs> func, IPublisher<TPublisherEventArgs> outterPublisher)
            {
                _func = func;
                _outterPublisher = outterPublisher;
                _outterPublisherFunc = GetPublisher;
            }

            public IPublisher<TPublisherEventArgs> GetPublisher(T ea)
            {
                return _outterPublisher;
            }

            public void Republish(T domainEventArgs)
            {
                _outterPublisherFunc
                    .Invoke(domainEventArgs)
                    .Publish(_func.Invoke(domainEventArgs));
            }
        }

        protected interface IRepublisher
        {
            void Republish(T domainEventArgs);
        }


        #endregion

        public Dispatcher()
        {
            ClearMapping();
        }

        public Dispatcher([NotNull] IMapper mapper):this()
        {
            Mapper = mapper;
        }

        #region Private

        private TPublisherEventArgs UseMapper<TPublisherEventArgs>(T ea)
        {
            CheckMapper();
            return _mapper.Map<TPublisherEventArgs>(ea);
        }

        private void CheckMapper()
        {
            if (_mapper == null)
            {
                throw new ConfigurationErrorsException("Mapper is not set");
            }
        }

        protected void InitIndex<TPublisherEventArgs>()
        {
            var key = typeof(TPublisherEventArgs);
            if (!_publishers.ContainsKey(key))
            {
                _publishers[key] = new List<IRepublisher>();
            }
        }

        #endregion

        #region Mappings

        public Dispatcher<T> CreateMapping<TPublisherEventArgs, TPublisher>(
            Func<T, TPublisherEventArgs> mapFunc, TPublisher publisher)
            where TPublisher : IPublisher<TPublisherEventArgs>
        {
            InitIndex<T>();
            _publishers[typeof(TPublisherEventArgs)].Add(new Converter<TPublisherEventArgs>(mapFunc, publisher));
            return this;
        }


        public Dispatcher<T> CreateMapping<TPublisherEventArgs>(
            Func<T, TPublisherEventArgs> mapFunc, Func<T, IPublisher<TPublisherEventArgs>> publisherSelector)
        {
            InitIndex<T>();
            _publishers[typeof(TPublisherEventArgs)].Add(new Converter<TPublisherEventArgs>(mapFunc, publisherSelector));
            return this;
        }

        public Dispatcher<T> CreateMapping<TPublisherEventArgs>(Func<T, IPublisher<TPublisherEventArgs>> publisherSelector)
        {
            return CreateMapping(UseMapper<TPublisherEventArgs>, publisherSelector);
        }

        public Dispatcher<T> CreateMapping<TPublisherEventArgs, TPublisher>(TPublisher publisher)
            where TPublisher : IPublisher<TPublisherEventArgs>
        {
            CheckMapper();
            return CreateMapping(UseMapper<TPublisherEventArgs>, publisher);
        }

        public Dispatcher<T> CreateMapping<TPublisherEventArgs, TPublisher>(
            Func<T, TPublisherEventArgs> mapFunc)
            where TPublisher : IPublisher<TPublisherEventArgs>, new()
        {
            return CreateMapping(mapFunc, new TPublisher());
        }

        public Dispatcher<T> CreateMapping<TPublisherEventArgs, TPublisher>()
            where TPublisher : IPublisher<TPublisherEventArgs>, new()
        {
            return CreateMapping<TPublisherEventArgs, TPublisher>(new TPublisher());
        }

        public void ClearMapping()
        {
            _publishers = new Dictionary<Type, List<IRepublisher>>();
        }

        #endregion

        public virtual void Publish([NotNull] T message)
        {
            if (message == null) throw new ArgumentNullException("message");
            var key = message.GetType();
            if (!_publishers.ContainsKey(key))
            {
                throw new ConfigurationErrorsException();
            }

            foreach (var p in _publishers[key])
            {
                p.Republish(message);
            }
        }
    }
}
