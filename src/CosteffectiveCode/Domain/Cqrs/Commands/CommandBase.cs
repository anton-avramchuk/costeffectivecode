using CosteffectiveCode.Metadata;
using Void = CosteffectiveCode.Metadata.Void;

namespace CostEffectiveCode.Domain.Cqrs.Commands
{
    public abstract class CommandBase<TInput> : ICommand<TInput>
    {
        protected abstract void DoExecute(TInput input);

        public Void Execute(TInput input)
        {
            DoExecute(input);
            return Void.Nothing;
        }

        void ICommand<TInput>.Execute(TInput input)
        {
            DoExecute(input);
        }
    }

    public abstract class CommandBase<TInput, TResult> : ICommand<TInput, TResult>, IOperation<TInput, TResult>
        where TResult: struct
    {
        protected abstract TResult DoExecute(TInput input);

        public abstract string Name { get; }

        public abstract string Description { get; }

        public TResult Execute(TInput input)
        {
            return DoExecute(input);
        }

        TResult ICommand<TInput, TResult>.Execute(TInput input)
        {
            return DoExecute(input);
        }
    }
}