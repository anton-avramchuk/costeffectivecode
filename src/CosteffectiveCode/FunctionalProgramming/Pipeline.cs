using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace CosteffectiveCode.FunctionalProgramming
{
    public class Pipeline
    {
        private object _arg;

        private readonly List<IInvokable> _steps = new List<IInvokable>();

        private Pipeline(object arg)
        {
            _arg = arg;
        }

        public static Step Start(Action firstStep)
        {
            var p = new Pipeline(null);
            return new Step(p, firstStep);
        }

        public static Step<TInput, TOutput> Start<TInput, TOutput>(TInput firstArg, Func<TInput, TOutput> firstStep)
        {
            var p = new Pipeline(firstArg);
            // ReSharper disable once ObjectCreationAsStatement
            return new Step<TInput, TOutput>(p, firstStep);
        }

        public object Execute()
        {
            foreach (IInvokable t in _steps)
            {
                _arg = t.Invoke();
            }

            return _arg;
        }

        public class Step : IInvokable
        {
            private readonly Pipeline _pipe;

            private readonly Action _action;

            public Step(Pipeline pipe, Action action)
            {
                _pipe = pipe;
                _action = action;
                _pipe._steps.Add(this);
            }

            public Step Next([NotNull] Action action)
            {
                if (action == null) throw new ArgumentNullException(nameof(action));
                return new Step(_pipe, action);
            }

            public object Invoke()
            {
                _action.Invoke();
                return null;
            }

            public void Finish() => _pipe.Execute();
        }

        public class Step<TInput, TOutput> : IInvokable
        {
            private readonly Pipeline _pipe;

            private readonly Func<TInput, TOutput> _func;

            internal Step(Pipeline pipe, Func<TInput, TOutput> func)
            {
                _pipe = pipe;
                _func = func;
                _pipe._steps.Add(this);
            }

            object IInvokable.Invoke() => _func.Invoke((TInput) _pipe._arg);

            public Step<TOutput, TNext> Next<TNext>([NotNull] Func<TOutput, TNext> func)
            {
                if (func == null) throw new ArgumentNullException(nameof(func));
                return new Step<TOutput, TNext>(_pipe, func);
            }

            public Step Next([NotNull] Action action)
            {
                if (action == null) throw new ArgumentNullException(nameof(action));
                return new Step(_pipe, action);
            }

            public TOutput Finish() => (TOutput)_pipe.Execute();
        }

        internal interface IInvokable
        {
            object Invoke();
        }
    }


}
