using System;
using JetBrains.Annotations;

namespace CostEffectiveCode.Common.Scope
{
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