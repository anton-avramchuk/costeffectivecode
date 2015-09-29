using System;
using System.Configuration;
using JetBrains.Annotations;

namespace CostEffectiveCode.Extensions
{
    [PublicAPI]
    public static class Maybe
    {
        [NotNull]
        public static T CheckNotNull<T>(this T obj)
        {
            if (obj == null) throw new ConfigurationErrorsException($"instance of type \"{typeof (T)}\" is null");

            return obj;
        }

        public static TResult With<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator)
            where TResult : class where TInput : class
        {
            return o == null ? null : evaluator(o);
        }

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

        public static TInput Do<TInput>(this TInput o, Action<TInput> action)
            where TInput : class
        {
            if (o == null) return null;
            action(o);
            return o;
        }
    }
}