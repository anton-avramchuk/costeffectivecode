using System;
using CostEffectiveCode.Akka.Actors;
using JetBrains.Annotations;
// ReSharper disable UseNameofExpression
namespace CostEffectiveCode.Akka.Messages
{
    public class CreateProcessMessage
    {
        public CreateProcessMessage([NotNull] Type actorType, [CanBeNull] Type processCoordinatorType = null)
        {
            ProcessCoordinatorType = processCoordinatorType;
            if (actorType == null) throw new ArgumentNullException("actorType");

            ActorType = actorType;
        }

        public Type ActorType { get; private set; }
        public Type ProcessCoordinatorType { get; private set; }
    }
}
