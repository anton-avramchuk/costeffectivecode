using System;

namespace CostEffectiveCode.Akka.Helpers
{
    public abstract class ParsePsppEntityTypeResultBase : ParseTypeResultBase
    {
        protected ParsePsppEntityTypeResultBase(bool isSubclass)
            : base(isSubclass)
        {
        }

        protected ParsePsppEntityTypeResultBase(bool isSubclass, Type optionsType, Type stateType, Type exceptionType, Type resultType)
            : base(isSubclass)
        {
            OptionsType = optionsType;
            StateType = stateType;
            ExceptionType = exceptionType;
            ResultType = resultType;
        }

        public Type OptionsType { get; private set; }
        public Type StateType { get; private set; }
        public Type ExceptionType { get; private set; }
        public Type ResultType { get; private set; }
    }
}