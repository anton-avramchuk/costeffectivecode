using System;
using Akka.Actor;
using CostEffectiveCode.Messaging.Process;

namespace CostEffectiveCode.Akka.Actors
{
    public abstract class ProcessActorBase<TProcessOptions, TProcessState, TProcessException, TProcessResult>
        : ReceiveActor, IWithUnboundedStash
        where TProcessState : ProcessState
        where TProcessException : ProcessExceptionBase, new()     {
        protected Guid ProcessGuid;
        protected TProcessOptions ProcessOptions;
        protected ProcessActorState State;

        protected readonly IProcessesProtocolServer<TProcessOptions, TProcessState, TProcessException, TProcessResult> PsppServer;

        protected ProcessActorBase(IProcessesProtocolServer<TProcessOptions, TProcessState, TProcessException, TProcessResult> psppServer)
        {
            // PSPP server
            PsppServer = psppServer;

            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
            Idle();
        }

        #region FSM states
        protected virtual void Idle()
        {
            Receive<StartProcessEventArgs<TProcessOptions>>(message => StartProcess(message));
            // unsupported yet
            //Receive<StopProcessEventArgs<TProcessOptions>>(message => CannotStopProcess(message));

            State = ProcessActorState.Idle;
            ProcessGuid = Guid.Empty;
            ProcessOptions = default(TProcessOptions);

            // ReSharper disable once UseNullPropagation
            if (Stash != null)
                Stash.UnstashAll();
        }

        protected virtual void Running()
        {
            Receive<StartProcessEventArgs<TProcessOptions>>(message => DeferStartProcess());
            // unsupported yet
            //Receive<StopProcessEventArgs<TProcessOptions>>(message => StopProcess(message));

            State = ProcessActorState.Running;
        }
        #endregion

        #region Message handlers
        private void StartProcess(StartProcessEventArgs<TProcessOptions> message)
        {
            // save message details first
            ProcessGuid = message.ProcessGuid;
            ProcessOptions = message.Options;

            Become(Running);

            try
            {
                // long blocking operation that should be overriden in derived actor class
                Execute(ProcessOptions);
            }
            catch (TProcessException e)
            {
                // catch TProcessExceptions to support throw-syntax inside of concrete actor
                Fail(e);
            }
            catch (Exception e)
            {
                // catch unhandled exceptions and publish them to PSPP error bus-broker
                Fail(new TProcessException { InnerException = e });
            }
        }

        private void DeferStartProcess()
        {
            Stash.Stash();
        }
        #endregion

        #region Syntax sugar-API for derived actor class to work on top of PSPP server object. These methods correspond to the simplified PSPP Server Session term.
        protected void ChangeState(TProcessState newState)
        {
            if (State != ProcessActorState.Running)
                throw new InvalidOperationException("Cannot change state of not running process");

            PsppServer.ChangeState(newState, ProcessGuid);

            // no need to become somewhat - here we stay Running
            //Become(Running);
        }

        protected void Fail(TProcessException error)
        {
            if (State != ProcessActorState.Running)
                throw new InvalidOperationException("Cannot break (with error) not running process");

            PsppServer.Fail(error, ProcessGuid);

            Become(Idle);
        }

        protected void Finish(TProcessResult result)
        {
            if (State != ProcessActorState.Running)
                throw new InvalidOperationException("Cannot finish not running process");

            PsppServer.Finish(result, ProcessGuid);

            Become(Idle);
        }
        #endregion

        protected abstract void Execute(TProcessOptions options);

        public IStash Stash { get; set; }
    }

    public enum ProcessActorState
    {
        Idle, Running
    }
}
