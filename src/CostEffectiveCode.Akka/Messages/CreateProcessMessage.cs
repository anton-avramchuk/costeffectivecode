using System;
using JetBrains.Annotations;
namespace CostEffectiveCode.Akka.Messages
{
    public class CreateProcessMessage
    {
        public CreateProcessMessage([NotNull] Type actorType, [CanBeNull] Type processCoordinatorType = null)
        {
            ProcessCoordinatorType = processCoordinatorType;
            if (actorType == null) throw new ArgumentNullException(nameof(actorType));

            ActorType = actorType;
        }

        public Type ActorType { get; private set; }
        public Type ProcessCoordinatorType { get; private set; }
    }
}
