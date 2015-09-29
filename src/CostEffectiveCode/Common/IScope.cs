using System;
using JetBrains.Annotations;

namespace CostEffectiveCode.Common
{
    /// <summary>
    /// Represents object lifetime scope
    /// </summary>
    /// <typeparam name="T">instance type</typeparam>
    [PublicAPI]
    public interface IScope<out T>
    {
        T Instance { get; }
    }

    public class Scope<T> : IScope<T>
    {
        public Scope([NotNull] T subject)
        {
            if (subject == null) throw new ArgumentNullException(nameof(subject));
            Instance = subject;
        }

        public T Instance { get; }
    }
}
