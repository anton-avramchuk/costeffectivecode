using System;
using JetBrains.Annotations;

namespace CostEffectiveCode.Common
{
    [PublicAPI]
    public interface IScope<out T> : IDisposable
    {
        T Instance { get; }
    }
}
