using Akka.Actor;
using CostEffectiveCode.Akka.Actors;
using CostEffectiveCode.Processes.Akka.Helpers;
using CostEffectiveCode.Processes.EventArgs;
using CostEffectiveCode.Processes.State;

namespace CostEffectiveCode.Processes.Akka.Actors
{
    public class PubSubProcProtocolCoordinator<TProcessOptions, TProcessState, TProcessException, TProcessResult> : ReceiveActor
        where TProcessState : ProcessState
        where TProcessException : ProcessExceptionBase, new()     {
        public PubSubProcProtocolCoordinator()
        {
            CreatePubSubCoordinator<StartProcessEventArgs<TProcessOptions>>();
            CreatePubSubCoordinator<ProcessStateChangedEventArgs<TProcessState>>();
            CreatePubSubCoordinator<ProcessFailedEventArgs<TProcessException>>();
            CreatePubSubCoordinator<ProcessFinishedEventArgs<TProcessResult>>();
        }

        private IActorRef CreatePubSubCoordinator<TProcessEventArgs>()
            where TProcessEventArgs : ProcessEventArgsBase
        {
            return Context.ActorOf(Props.Create<PubSubCoordinatorActor<TProcessEventArgs>>(), ActorNamesHelper.GetActorName<TProcessEventArgs>());
        }
    }
}
