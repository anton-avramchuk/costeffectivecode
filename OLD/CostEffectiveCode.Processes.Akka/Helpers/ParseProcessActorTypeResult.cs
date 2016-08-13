using System;

namespace CostEffectiveCode.Processes.Akka.Helpers
{
    public class ParseProcessActorTypeResult : ParsePsppEntityTypeResultBase
    {
        public ParseProcessActorTypeResult(bool isSubclass)
            : base(isSubclass)
        {
        }

        public ParseProcessActorTypeResult(bool isSubclass, Type optionsType, Type stateType, Type exceptionType, Type resultType)
            : base(isSubclass, optionsType, stateType, exceptionType, resultType)
        {
        }
    }
}