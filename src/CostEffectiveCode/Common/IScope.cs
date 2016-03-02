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

        // TODO: Possibly, BeginScope/EndScope methods should be added
    }

    /// <summary>
    /// Pass-through implementation of IScope&lt;&gt;
    /// Does nothing more than encapsulates given object's instance
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PassThroughScope<T> : IScope<T>
    {
        public PassThroughScope([NotNull] T subject)
        {
            if (subject == null) throw new ArgumentNullException(nameof(subject));
            Instance = subject;
        }

        public T Instance { get; }
    }
}
