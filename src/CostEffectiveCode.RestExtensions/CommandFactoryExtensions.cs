using CostEffectiveCode.Cqrs.Commands;

namespace CostEffectiveCode.FactoryExtensions
{
    public static class CommandFactoryExtensions
    {
        public static TKey Put<TInput, TKey, TCommand>(
            this ICommandFactory commandFactory,
            TInput specification)
            where TCommand : ICommand<TInput, TKey>
            where TKey : struct
        => commandFactory.GetCommand<TInput, TKey, TCommand>().Execute(specification);
    }
}
