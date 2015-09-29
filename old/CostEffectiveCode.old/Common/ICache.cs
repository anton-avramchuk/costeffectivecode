using System;
using JetBrains.Annotations;

namespace CostEffectiveCode.Common
{
    [PublicAPI]
    public interface ICache
    {
        bool Add<T>(string key, T obj, TimeSpan timeSpan);

        T Get<T>(string key);

        bool Contains(string key);

    }
}
