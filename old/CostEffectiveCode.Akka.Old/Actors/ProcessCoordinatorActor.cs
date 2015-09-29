using System;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.DI.Core;
using Akka.Routing;
using CostEffectiveCode.Akka.Cqrs;
using CostEffectiveCode.Akka.Helpers;
using CostEffectiveCode.Messaging.Process;

namespace CostEffectiveCode.Akka.Actors
{
    public class ProcessCoordinatorActor<TProcessActor, TProcessOptions, TProcessState, TProcessException, TProcessResult> : ReceiveActor
        where TProcessActor : ProcessActorBase<TProcessOptions, TProcessState, TProcessException, TProcessResult>
        where TProcessState : ProcessState
        where TProcessException : ProcessExceptionBase, new()     {
        private const int DefaultPoolSize = 1000; // TODO: move magic number to settings (HOCON?)
        private readonly IProcessesProtocolServer<TProcessOptions, TProcessState, TProcessException, TProcessResult> _psppServer;
        private readonly int _poolSize;

        // children
        private IActorRef _psppCoordinator;
        private readonly IActorRef _process; // in most common way - a RoundRobinPool

        public ProcessCoordinatorActor(IProcessesProtocolServer<TProcessOptions, TProcessState, TProcessException, TProcessResult> psppServer, int poolSize = DefaultPoolSize, bool createPsppCoordinator = true)
        {
            Receive<StartProcessEventArgs<TProcessOptions>>(message => StartProcess(message));

            _psppServer = psppServer;
            _poolSize = poolSize;

            var tellCommandSender = Self;
            if (createPsppCoordinator)
            {
                _psppCoordinator = CreatePsppCoordinator();
                tellCommandSender = _psppCoordinator;
            }

            _process = CreateProcessActor();

            // local subscription: When a process start is triggered by calling Publish() on another IBroker<> side
            // -- we should tell StartProcessEventArgs<> message to this actor
            // -- in order to start standard Akka pipeline
            _psppServer.OnProcessStart(
                new TellCommand<StartProcessEventArgs<TProcessOptions>>(Self, tellCommandSender));
        }

        private void StartProcess(StartProcessEventArgs<TProcessOptions> message)
        {
            _process.Tell(message);
        }

        private IActorRef CreateProcessActor()
        {
            return Context.ActorOf(
                Context.DI()
                    .Props<TProcessActor>()
                    .WithRouter(new RoundRobinPool(_poolSize))
                    .WithSupervisorStrategy(new OneForOneStrategy(decider => Directive.Restart)), // TODO: determine what error occured and publish CEC Proc Failure message!
                ActorNamesHelper.GetActorName<TProcessActor>()
            );
        }

        private IActorRef CreatePsppCoordinator()
        {
            return _psppCoordinator = Context
                .ActorOf<PubSubProcProtocolCoordinator
                    <TProcessOptions,
                    TProcessState,
                    TProcessException,
                    TProcessResult>>(ActorNamesHelper.PsppCoordinatorActorName);
        }
    }
}
