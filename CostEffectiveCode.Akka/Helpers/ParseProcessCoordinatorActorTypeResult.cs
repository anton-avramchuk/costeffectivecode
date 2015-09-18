using System;

namespace CostEffectiveCode.Akka.Helpers
{
    public class ParseProcessCoordinatorActorTypeResult: ParseProcessActorTypeResult
    {
        public ParseProcessCoordinatorActorTypeResult(bool isSubclass)
            : base(isSubclass)
        {
        }

        public ParseProcessCoordinatorActorTypeResult(bool isSubclass, Type actorProcessType, Type optionsType, Type stateType, Type exceptionType, Type resultType)
            : base(isSubclass, optionsType, stateType, exceptionType, resultType)
        {
            ActorProcessType = actorProcessType;
        }
        public Type ActorProcessType { get; private set;  }
    }
}