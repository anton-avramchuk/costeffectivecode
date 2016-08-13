using System;
using CostEffectiveCode.Domain.Cqrs.Commands;
using CostEffectiveCode.Messaging;
using CostEffectiveCode.Processes.EventArgs;
using CostEffectiveCode.Processes.State;
using JetBrains.Annotations;

namespace CostEffectiveCode.Processes.PubSubProcProtocol
{
    public class PubSubProcProtocolServer<TProcessOptions, TProcessState, TProcessException, TProcessResult>
        : IProcessesProtocolServer<TProcessOptions, TProcessState, TProcessException, TProcessResult>
        where TProcessState : ProcessState
        where TProcessException : ProcessExceptionBase, new()
    {
        private readonly ISubscriber<StartProcessEventArgs<TProcessOptions>> _startProcesSubscriber;
        private readonly IPublisher<ProcessStateChangedEventArgs<TProcessState>> _processStateChangedPublisher;
        private readonly IPublisher<ProcessFailedEventArgs<TProcessException>> _processFailedPublisher;
        private readonly IPublisher<ProcessFinishedEventArgs<TProcessResult>> _processFinishedPublisher;

        public PubSubProcProtocolServer(
            [NotNull] ISubscriber<StartProcessEventArgs<TProcessOptions>> startProcesSubscriber,
            [CanBeNull] IPublisher<ProcessStateChangedEventArgs<TProcessState>> processStateChangedPublisher,
            [CanBeNull] IPublisher<ProcessFailedEventArgs<TProcessException>> processFailedPublisher,
            [NotNull] IPublisher<ProcessFinishedEventArgs<TProcessResult>> processFinishedPublisher)
        {
            if (startProcesSubscriber == null) throw new ArgumentNullException(nameof(startProcesSubscriber));
            if (processFinishedPublisher == null) throw new ArgumentNullException(nameof(processFinishedPublisher));

            _startProcesSubscriber = startProcesSubscriber;
            _processStateChangedPublisher = processStateChangedPublisher;
            _processFailedPublisher = processFailedPublisher;
            _processFinishedPublisher = processFinishedPublisher;
        }

        public void OnProcessStart(ICommand<StartProcessEventArgs<TProcessOptions>> handler)
        {
            _startProcesSubscriber.Subscribe(handler);
        }

        public void ChangeState(ProcessStateChangedEventArgs<TProcessState> message)
        {
            _processStateChangedPublisher.Publish(message);
        }

        public void Fail(ProcessFailedEventArgs<TProcessException> message)
        {
            _processFailedPublisher.Publish(message);
        }

        public void Finish(ProcessFinishedEventArgs<TProcessResult> message)
        {
            _processFinishedPublisher.Publish(message);
        }
    }
}