using JetBrains.Annotations;

namespace CostEffectiveCode.Akka.Messages
{
    /// <summary>
    /// Empty strongly-typed message to execute encapsulated command inside CommandActor
    /// </summary>
    public class ExecuteCommandMessage
    {
    }

    public class ExecuteCommandMessage<TMessage>
    {
        public TMessage Message { get; private set; }

        public ExecuteCommandMessage([CanBeNull] TMessage message)
        {
            Message = message;
        }
    }
}