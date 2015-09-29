using System;
using Akka.Actor;
using Akka.DI.Core;
using CostEffectiveCode.Akka.Helpers;
using CostEffectiveCode.Akka.Messages;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace CostEffectiveCode.Akka.Actors
{
    public class ToplevelProcessesCoordinatorActor : ReceiveActor
    {

        public ToplevelProcessesCoordinatorActor()
        {
            Receive<CreateProcessMessage>(message => CreateProcess(message));

        }

        private void CreateProcess(CreateProcessMessage message)
        {
            var parseResult = ActorTypesHelper.ParseProcessActorType(message.ActorType);

            if (!parseResult.IsSubclass)
                throw new InvalidMessageException("Incorrect process actor type!");

            Type processCoordinatorActorType;

            if (message.ProcessCoordinatorType != null)
            {
                // use specific (custom) process coordinator
                // Hint: it should not be a generic-only type definition, 'cause we can't predict what generic type arguments should be specified
                processCoordinatorActorType = message.ProcessCoordinatorType;
            }
            else
            {
                // use default process coordinator
                processCoordinatorActorType = typeof(ProcessCoordinatorActor<,,,,>)
                    .MakeGenericType(message.ActorType, parseResult.OptionsType, parseResult.StateType, parseResult.ExceptionType, parseResult.ResultType);
            }

            Context.ActorOf(
                Context.DI()
                .Props(processCoordinatorActorType), ActorNamesHelper.GetProcessCoordinatorActorName(message.ActorType));
            // Independently of coordinator actor type, we are using the same naming conventions
        }
    }
}
