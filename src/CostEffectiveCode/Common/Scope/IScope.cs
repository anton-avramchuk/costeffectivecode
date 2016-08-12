using JetBrains.Annotations;

namespace CostEffectiveCode.Common.Scope
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
}
