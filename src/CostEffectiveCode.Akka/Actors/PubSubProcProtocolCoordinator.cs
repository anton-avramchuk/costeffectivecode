using Akka.Actor;
using CostEffectiveCode.Akka.Helpers;
using CostEffectiveCode.Processes.EventArgs;
using CostEffectiveCode.Processes.State;

namespace CostEffectiveCode.Akka.Actors
{
    public class PubSubProcProtocolCoordinator<TProcessOptions, TProcessState, TProcessException, TProcessResult> : ReceiveActor
        where TProcessState : ProcessState
        where TProcessException : ProcessExceptionBase, new()     {
        private readonly IActorRef _startCoordinator;
        private readonly IActorRef _stateChangedCoordinator;
        private readonly IActorRef _failedCoordinator;
        private readonly IActorRef _finishedCoordinator;

        public PubSubProcProtocolCoordinator()
        {
            _startCoordinator = CreatePubSubCoordinator<StartProcessEventArgs<TProcessOptions>>();
            _stateChangedCoordinator = CreatePubSubCoordinator<ProcessStateChangedEventArgs<TProcessState>>();
            _failedCoordinator = CreatePubSubCoordinator<ProcessFailedEventArgs<TProcessException>>();
            _finishedCoordinator = CreatePubSubCoordinator<ProcessFinishedEventArgs<TProcessResult>>();
        }

        private IActorRef CreatePubSubCoordinator<TProcessEventArgs>()
            where TProcessEventArgs : ProcessEventArgsBase
        {
            return Context.ActorOf(Props.Create<PubSubCoordinatorActor<TProcessEventArgs>>(), ActorNamesHelper.GetActorName<TProcessEventArgs>());
        }
    }
}
