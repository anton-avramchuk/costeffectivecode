using CostEffectiveCode.Common.Scope;
using CostEffectiveCode.Domain.Ddd.UnitOfWork;

namespace CostEffectiveCode.Domain.Cqrs.Commands
{
    public class CommitCommand : ICommand
    {
        private readonly IScope<IUnitOfWork> _scope;

        public CommitCommand(IScope<IUnitOfWork> scope)
        {
            _scope = scope;
        }

        public void Execute()
        {
            _scope.Instance.Commit();
        }
    }
}
