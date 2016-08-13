using System;
using JetBrains.Annotations;

namespace CosteffectiveCode.Common
{
    /// <summary>
    /// Represents object lifetime scope
    /// </summary>
    /// <typeparam name="T">instance type</typeparam>
    [PublicAPI]
    public interface IScope<out T> : IDisposable
    {
        T Instance { get; }
    }
}
