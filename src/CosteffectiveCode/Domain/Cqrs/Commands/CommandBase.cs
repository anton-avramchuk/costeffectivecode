using CosteffectiveCode.FunctionalProgramming;
using Void = CosteffectiveCode.FunctionalProgramming.Void;

namespace CosteffectiveCode.Domain.Cqrs.Commands
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

    public abstract class CommandBase<TInput, TResult> : ICommand<TInput, TResult>, IMorphism<TInput, TResult>
        where TResult: struct
    {
        protected abstract TResult DoExecute(TInput input);

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