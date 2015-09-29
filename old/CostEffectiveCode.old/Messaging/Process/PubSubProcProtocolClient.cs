using System;
using CostEffectiveCode.Domain.Cqrs.Commands;
using JetBrains.Annotations;

namespace CostEffectiveCode.Messaging.Process
{
    public class PubSubProcProtocolClient<TProcessOptions, TProcessState, TProcessException, TProcessResult>
        : IProcessesProtocolClient<TProcessOptions, TProcessState, TProcessException, TProcessResult>
        where TProcessState : ProcessState
        where TProcessException : ProcessExceptionBase, new()
    {
        private readonly IPublisher<StartProcessEventArgs<TProcessOptions>> _startPublisher;
        private readonly ISubscriber<ProcessStateChangedEventArgs<TProcessState>> _stateChangedSubscriber;
        private readonly ISubscriber<ProcessFailedEventArgs<TProcessException>> _failedSubscriber;
        private readonly ISubscriber<ProcessFinishedEventArgs<TProcessResult>> _finishedSubscriber;

        public PubSubProcProtocolClient(
            [NotNull] IPublisher<StartProcessEventArgs<TProcessOptions>> startPublisher,
            [CanBeNull] ISubscriber<ProcessStateChangedEventArgs<TProcessState>> stateChangedSubscriber,
            [CanBeNull] ISubscriber<ProcessFailedEventArgs<TProcessException>> failedSubscriber,
            [CanBeNull] ISubscriber<ProcessFinishedEventArgs<TProcessResult>> finishedSubscriber)
        {
            if (startPublisher == null) throw new ArgumentNullException(nameof(startPublisher));

            _startPublisher = startPublisher;
            _stateChangedSubscriber = stateChangedSubscriber;
            _failedSubscriber = failedSubscriber;
            _finishedSubscriber = finishedSubscriber;
        }

        public void StartProcess(StartProcessEventArgs<TProcessOptions> message)
        {
            System.Diagnostics.Debug.WriteLine("Process started");

            _startPublisher.Publish(message);
        }

        public void SubscribeOnStateChanged(ICommand<ProcessStateChangedEventArgs<TProcessState>> handler)
        {
            _stateChangedSubscriber.Subscribe(handler);
        }

        public void SubscribeOnFailure(ICommand<ProcessFailedEventArgs<TProcessException>> handler)
        {
            _failedSubscriber.Subscribe(handler);
        }

        public void SubscribeOnFinish(ICommand<ProcessFinishedEventArgs<TProcessResult>> handler)
        {
            _finishedSubscriber.Subscribe(handler);
        }
    }
}