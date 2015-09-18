using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Akka.Actor;
using Akka.DI.Core;
using CostEffectiveCode.Akka.Actors;
using CostEffectiveCode.Akka.Messages;
// ReSharper disable UseNameofExpression

namespace CostEffectiveCode.Akka.Helpers
{
    public class ProcessesActorSystemBuilder
    {
        private readonly ActorSystem _system;
        private IActorRef _coordinator;

        public ProcessesActorSystemBuilder(ActorSystem system)
        {
            _system = system;
            _coordinator = null;
        }

        public ActorSystem System
        {
            // ReSharper disable once ConvertPropertyToExpressionBody
            get { return _system; }
        }

        public void Initialize()
        {
            if (_coordinator != null)
                return;

            _coordinator = CreateToplevelCoordinator();
        }

        private IActorRef CreateToplevelCoordinator()
        {
            if (_coordinator != null)
            {
                throw new InvalidOperationException("CEC processes support already added to actor system");
            }

            return _system.ActorOf(_system.DI().Props<ToplevelProcessesCoordinatorActor>(), ActorNamesHelper.ToplevelCoordinatorName);
        }

        public void AddProcess<TProcessActor>()
            where TProcessActor : ActorBase // here we would like to have more specific types, but constraining will lead to adding 4 extra generic type arguments
        {
            AddProcess(typeof(TProcessActor));
        }

        public void AddProcess<TProcessActor, TProcessCoordinatorActor>()
            where TProcessActor : ActorBase // here we would like to have more specific types, but constraining will lead to adding 4 extra generic type arguments
            where TProcessCoordinatorActor : ActorBase
        {
            AddProcess(typeof(TProcessActor), typeof(TProcessCoordinatorActor));
        }

        private void AddProcess(Type processActorType, Type processCoordinatorActorType = null, bool skipTypeCheck = false)
        {
            if (!skipTypeCheck)
            {
                var parseProcessActorTypeResult = ActorTypesHelper.ParseProcessActorType(processActorType);

                if (!parseProcessActorTypeResult.IsSubclass)
                    throw new ArgumentException(
                        "Given type of " + processActorType.Name +
                        " is incorrect. Only subclasses of ProcessActorBase<,,,> are supported to build a process actor hierarchy",
                        "processActorType");

                if (processCoordinatorActorType != null)
                {
                    var parseProcessCoordinatorActorTypeResult =
                        ActorTypesHelper.ParseProcessCoordinatorActorType(processCoordinatorActorType);

                    if (!parseProcessCoordinatorActorTypeResult.IsSubclass)
                        throw new ArgumentException(
                            "Given type of " + processCoordinatorActorType.Name
                            + " is incorrect. Only subclasses of ProcessCoordinatorActorBase<,,,,> are supported to build a process coordinator actor hierarchy",
                            "processCoordinatorActorType");

                    if (parseProcessCoordinatorActorTypeResult.ActorProcessType != processActorType)
                    {
                        throw new ArgumentException("Given type of " + processCoordinatorActorType.Name +
                            " is incompatible with given type of process actor type: "
                            + processActorType.Name,
                            "processCoordinatorActorType");
                    }
                }
            }

            if (_coordinator == null)
                throw new InvalidOperationException("Please add CEC processes support to actor system first");

            _coordinator.Tell(new CreateProcessMessage(processActorType, processCoordinatorActorType));
        }

        public void AddProcessesFromAssembly(Assembly assembly)
        {
            var actorTypes = assembly
                .GetTypes()
                .Where(x => !x.IsAbstract && x.IsSubclassOf(typeof(ActorBase)))
                .ToArray();

            foreach (var t in actorTypes)
            {
                var parseResult = ActorTypesHelper.ParseProcessActorType(t);

                if (!parseResult.IsSubclass)
                    continue;

                var processCoordinatorActorType = GetProcessCoordinatorInternal(t, actorTypes);

                AddProcess(t, processCoordinatorActorType, true);
            }
        }

        /// <summary>
        /// Searches for custom ProcessCoordinatorActor in specified assemlby for specified type of process actor.
        /// </summary>
        /// <param name="processActorType">Type of process</param>
        /// <param name="assembly">Assembly to search in</param>
        /// <returns>Type of custom ProcessCoordinatorActor or null if the default implementation should be used</returns>
        private Type GetProcessCoordinator(Type processActorType, Assembly assembly)
        {
            var actorTypes = assembly
                .GetTypes()
                .Where(x => !x.IsAbstract && x.IsSubclassOf(typeof(ActorBase)));

            return GetProcessCoordinatorInternal(processActorType, actorTypes);
        }

        private Type GetProcessCoordinatorInternal(Type processActorType, IEnumerable<Type> searchInTypes)
        {
            foreach (var t in searchInTypes)
            {
                var parseResult = ActorTypesHelper.ParseProcessCoordinatorActorType(t);

                if (parseResult.IsSubclass && parseResult.ActorProcessType == processActorType)
                    return t;
            }

            return null;
        }
    }
}