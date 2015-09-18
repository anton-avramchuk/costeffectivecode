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
        T GetScoped();
    }

    public class Scope<T> : IScope<T>
    {
        T _subject;

        public Scope([NotNull] T subject)
        {
            if (subject == null) throw new ArgumentNullException("subject");
            _subject = subject;
        }

        public T GetScoped()
        {
            return _subject;
        }
    }
}
