using System;
using System.Collections.Generic;
using System.Linq;
using CosteffectiveCode.Common;
using JetBrains.Annotations;

namespace CosteffectiveCode.Messaging
{
    [PublicAPI]
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class Dispatcher<T> : IPublisher<T>
    {
        #region Vars

        private IMapper _mapper;

        protected Dictionary<Type, List<IRepublisher>> Publishers;

        [CanBeNull]
        public IMapper Mapper
        {
            get { return _mapper; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _mapper = value;
            }
        }

        #endregion

        #region Internal Helpers

        internal class Converter<TEventArgs, TPublisherEventArgs> : IRepublisher
            where TEventArgs : T
        {
            private readonly Func<TEventArgs, TPublisherEventArgs> _func;

            private readonly Func<TEventArgs, IPublisher<TPublisherEventArgs>> _outterPublisherFunc;

            private readonly IPublisher<TPublisherEventArgs> _outterPublisher;

            internal Converter(
                Func<TEventArgs, TPublisherEventArgs> func,
                Func<TEventArgs, IPublisher<TPublisherEventArgs>> outterPublisherFunc)
            {
                _func = func;
                _outterPublisherFunc = outterPublisherFunc;
            }


            internal Converter(
                Func<TEventArgs, TPublisherEventArgs> func,
                IPublisher<TPublisherEventArgs> outterPublisher)
            {
                _func = func;
                _outterPublisher = outterPublisher;
                _outterPublisherFunc = GetPublisher;
            }

            private IPublisher<TPublisherEventArgs> GetPublisher(TEventArgs ea)
            {
                return _outterPublisher;
            }

            public void Republish(T domainEventArgs)
            {
                // that's ok
                var args = (TEventArgs) domainEventArgs;
                var publisher = _outterPublisherFunc.Invoke(args);
                if (publisher == null)
                {
                    throw new InvalidOperationException($"Publisher for args \"{domainEventArgs}\" is not found");    
                }

                publisher.Publish(_func.Invoke(args));
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

        public Dispatcher([NotNull] IMapper mapper)
            : this()
        {
            Mapper = mapper;
        }

        #region Private

        private TPublisherEventArgs UseMapper<TEventArgs, TPublisherEventArgs>(TEventArgs ea)
            where TEventArgs : T
        {
            CheckMapper();
            return _mapper.Map<TPublisherEventArgs>(ea);
        }

        private void CheckMapper()
        {
            if (_mapper == null)
            {
                throw new InvalidOperationException("Mapper is not set");
            }
        }

        protected void InitIndex<TEventArgs>()
            where TEventArgs : T
        {
            var key = typeof (TEventArgs);
            if (!Publishers.ContainsKey(key))
            {
                Publishers[key] = new List<IRepublisher>();
            }
        }

        #endregion

        #region Mappings

        public Dispatcher<T> CreateMapping<TEventArgs, TPublisherEventArgs>(
            [NotNull]Func<TEventArgs, TPublisherEventArgs> map,
            [NotNull]Func<TEventArgs, IPublisher<TPublisherEventArgs>> selector)
            where TEventArgs : T
        {
            InitIndex<TEventArgs>();
            Publishers[typeof(TEventArgs)].Add(new Converter<TEventArgs, TPublisherEventArgs>(map, selector));
            return this;
        }

        public Dispatcher<T> CreateMapping<TEventArgs, TPublisherEventArgs>(
            [NotNull] Func<TEventArgs, TPublisherEventArgs> map,
            [NotNull] Func<TEventArgs, IPublisher<TPublisherEventArgs>> to,
            [CanBeNull] params Func<TEventArgs, bool>[] patterns)
            where TEventArgs : T
        {
            return CreateMapping(
                map,
                arg => patterns == null || !patterns.Any() || patterns.All(p => p.Invoke(arg))
                    ? to.Invoke(arg)
                    : null);
        }

        public Dispatcher<T> CreateMapping<TPublisherEventArgs>(
            [NotNull] Func<T, TPublisherEventArgs> map,
            [NotNull] Func<T, IPublisher<TPublisherEventArgs>> to,
            [CanBeNull] params Func<T, bool>[] patterns)
        {
            return CreateMapping<T, TPublisherEventArgs>(map, to, patterns);
;        }

        public void ClearMapping()
        {
            Publishers = new Dictionary<Type, List<IRepublisher>>();
        }

        #endregion

        public virtual void Publish([NotNull] T message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            var key = message.GetType();
            if (!Publishers.ContainsKey(key))
            {
                throw new InvalidOperationException($"Key of type \"{key}\" is not found in mapping");
            }

            foreach (var p in Publishers[key])
            {
                p.Republish(message);
            }
        }
    }
}
