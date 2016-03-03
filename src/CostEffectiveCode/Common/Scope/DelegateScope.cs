using System;

namespace CostEffectiveCode.Common.Scope
{
    public class DelegateScope<T> : IScope<T>
    {
        public Func<T> CtorFunc { get; protected set; }

        public DelegateScope(Func<T> ctorFunc)
        {
            CtorFunc = ctorFunc;
        }

        public T Instance => CtorFunc.Invoke();
    }
}