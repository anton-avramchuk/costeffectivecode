using System;
using CostEffectiveCode.Processes.Akka.Actors;

namespace CostEffectiveCode.Processes.Akka.Helpers
{
    public class ActorTypesHelper
    {
        public static ParseProcessActorTypeResult ParseProcessActorType(Type actorType)
        {
            var actorTypeBase = GetBaseTypeDefinition(actorType, typeof(ProcessActorBase<,,,>));

            if (actorTypeBase == null)
            {
                return new ParseProcessActorTypeResult(false);
            }

#warning Here we have to use compile-unsafe construction to provide an easier API of ProcessesActorSystemBuilder (so the building process could be automated)

            var typeArguments = actorTypeBase.GetGenericArguments();

            return new ParseProcessActorTypeResult(
                true,
                typeArguments[0],
                typeArguments[1],
                typeArguments[2],
                typeArguments[3]);
        }

        public static ParseProcessCoordinatorActorTypeResult ParseProcessCoordinatorActorType(Type coordinatorActorType)
        {
            var coordinatorTypeBase = GetBaseTypeDefinition(coordinatorActorType, typeof(ProcessCoordinatorActor<,,,,>));

            if (coordinatorTypeBase == null)
            {
                return new ParseProcessCoordinatorActorTypeResult(false);
            }

            var typeArguments = coordinatorTypeBase.GetGenericArguments();
            return new ParseProcessCoordinatorActorTypeResult(
                true,
                typeArguments[0],
                typeArguments[1],
                typeArguments[2],
                typeArguments[3],
                typeArguments[4]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actorType">Type of actor</param>
        /// <param name="baseGenericType">Type of base type in inheritance hierarchy</param>
        /// <returns>Base type definition with generic arguments available if actor is a subclass of baseGenericType, or null otherwise</returns>
        private static Type GetBaseTypeDefinition(Type actorType, Type baseGenericType)
        {
            var actorTypeInternal = actorType;

            while (actorTypeInternal != null &&
                   !(actorTypeInternal.IsGenericType &&
                     actorTypeInternal.GetGenericTypeDefinition() == baseGenericType))
                actorTypeInternal = actorTypeInternal.BaseType;

            return actorTypeInternal;
        }
    }
}