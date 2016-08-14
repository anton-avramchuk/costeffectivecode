using System;
using JetBrains.Annotations;

namespace CosteffectiveCode.Domain.Ddd.Specifications
{
    public class FuncSpecification<T> : ISpecification<T>
    {
        private readonly Func<T, bool> _func;

        public FuncSpecification([NotNull] Func<T, bool> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));
            _func = func;
        }

        public bool IsSatisfiedBy(T o) => _func.Invoke(o);
    }
}
