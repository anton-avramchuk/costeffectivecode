using System;
using System.Text.RegularExpressions;
using Akka.Actor;
using CostEffectiveCode.Processes.EventArgs;

namespace CostEffectiveCode.Processes.Akka.Helpers
{
    /// <summary>
    /// A helper class to work with naming conventions of Process-based Actor system with support of PSPP
    /// </summary>
    public static class ActorNamesHelper
    {
        public const string ToplevelCoordinatorName = "processesCoordinator";
        public const string PsppCoordinatorActorName = "psppCoordinator";

        private static readonly Regex DeniedCharactersRegex = new Regex("[^a-zA-Z0-9]");

        public static string GetActorName<T>()
        {
            return GetActorName(typeof(T));
        }

        // Naming conventions hint: for the moment we specify a simple type name as an actor name (instead of assembly-qualified -- to make debugging easier)
        public static string GetActorName(Type t)
        {
            return DeniedCharactersRegex.Replace(t.Name, string.Empty);
        }

        public static string GetProcessCoordinatorActorName<TProcessActor>()
            where TProcessActor : ActorBase
        {
            return GetProcessCoordinatorActorName(typeof(TProcessActor));
        }

        public static string GetProcessCoordinatorActorName(Type processActorType)
        {
            return GetActorName(processActorType) + "Coordinator";
        }

        public static string GetAkkaBrokerPubSubCoordinatorPath<TProcessActor, TProcessEventArgs>()
            where TProcessActor : ActorBase
            where TProcessEventArgs : ProcessEventArgsBase
            // type-safety is hardly reached here for TProcessActor generic type argument (need to pass all it's generic arguments everywhere), so for now it's unconstrained
        {
            // ReSharper disable once UseStringInterpolation
            return string.Format("/user/{0}/{1}/{2}/{3}",
                ToplevelCoordinatorName,
                GetProcessCoordinatorActorName<TProcessActor>(),
                PsppCoordinatorActorName,
                GetActorName<TProcessEventArgs>());
        }
    }
}
