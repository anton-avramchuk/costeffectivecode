using CostEffectiveCode.Cqrs;

namespace CostEffectiveCode.Tests.Stubs
{
    public class SimpleCommandHandler : ICommandHandler<string, string>
    {
        public string Handle(string input)
        {
            return input;
        }
    }

    public class SimpleCommandHandler2 : ICommandHandler<string>
    {
        public void Handle(string input)
        {
        }
    }
}
