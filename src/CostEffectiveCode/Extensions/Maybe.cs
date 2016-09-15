using System;
using JetBrains.Annotations;

namespace CostEffectiveCode.Extensions
{
    [PublicAPI]
    public static class Maybe
    {
        public static TResult Return<TInput, TResult>(this TInput o,
            Func<TInput, TResult> evaluator, TResult failureValue) where TInput : class
        {
            return o == null ? failureValue : evaluator(o);
        }

        public static TInput If<TInput>(this TInput o, Func<TInput, bool> evaluator)
            where TInput : class
        {
            if (o == null) return null;
            return evaluator(o) ? o : null;
        }

        public static TInput Unless<TInput>(this TInput o, Func<TInput, bool> evaluator)
            where TInput : class
        {
            if (o == null) return null;
            return evaluator(o) ? null : o;
        }

        public static TInput Do<TInput>(this TInput o, Action<TInput> action, [CanBeNull]Func<Exception> ifNull = null)
            where TInput : class
        {
            if (o == null)
            {
                if (ifNull != null)
                {
                    throw ifNull.Invoke();
                }

                return null;
            }
            action(o);
            return o;
        }


        public static TOutput Do<TInput, TOutput>(this TInput o, Func<TInput, TOutput> func, Func<Exception> ifNull)
        {
            if (o == null)
            {
                throw ifNull.Invoke();
            }

            return func.Invoke(o);
        }
    }
}