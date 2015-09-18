using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CostEffectiveCode.Common
{
    public class SimpleCache : ICache
    {
        internal class CacheValue
        {
            public DateTime Expire;

            public object Value;

            public CacheValue(DateTime expire, object value)
            {
                Expire = expire;
                Value = value;
            }
        }

        private static readonly ConcurrentDictionary<string, CacheValue> Cache
            = new ConcurrentDictionary<string, CacheValue>();

        public bool Add<T>(string key, T obj, TimeSpan timeSpan)
        {
            return Cache.TryAdd(key, new CacheValue(DateTime.Now.Add(timeSpan), obj));
        }

        public T Get<T>(string key)
        {
            CacheValue result;
            return TryGetValue(key, out result)
                ? (T)result.Value
                : default(T);
        }

        public bool Contains(string key)
        {
            CacheValue result;
            return TryGetValue(key, out result);
        }

        private static bool TryGetValue(string key, out CacheValue result)
        {
            return Cache.TryGetValue(key, out result) && result.Expire < DateTime.Now;
        }
    }
}
