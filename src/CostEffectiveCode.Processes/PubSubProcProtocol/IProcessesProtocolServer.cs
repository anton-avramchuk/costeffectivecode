using System;
using CostEffectiveCode.Domain.Cqrs.Commands;
using CostEffectiveCode.Processes.EventArgs;
using CostEffectiveCode.Processes.State;
using JetBrains.Annotations;

namespace CostEffectiveCode.Processes.PubSubProcProtocol
{
    /// <summary>
    /// Here is where <b>PubSubProc [PSP] protocol</b> appears.
    /// It's a way of communication between subsystems using Pub/sub interfaces of CEC framework in terms of Processes of CEC framework.
    /// 
    /// This interface represents server-side of PSP protocol.
    /// </summary>
    /// <typeparam name="TProcessOptions"></typeparam>
    /// <typeparam name="TProcessState"></typeparam>
    /// <typeparam name="TProcessException"></typeparam>
    /// <typeparam name="TProcessResult"></typeparam>
    [PublicAPI]
    public interface IProcessesProtocolServer<TProcessOptions, TProcessState, TProcessException, TProcessResult>
        where TProcessState : ProcessState
        where TProcessException : ProcessExceptionBase, new()
    {
        void OnProcessStart(ICommand<StartProcessEventArgs<TProcessOptions>> handler);

        void ChangeState(ProcessStateChangedEventArgs<TProcessState> message);

        void Fail(ProcessFailedEventArgs<TProcessException> message);

        void Finish(ProcessFinishedEventArgs<TProcessResult> message);
    }

    [PublicAPI]
    public static class PubSubProcProtocolServerExtensions
    {
        public static void OnProcessStart<TProcessOptions, TProcessState, TProcessException, TProcessResult>(
            this IProcessesProtocolServer<TProcessOptions, TProcessState, TProcessException, TProcessResult> server,
            Action<StartProcessEventArgs<TProcessOptions>> handler)
            where TProcessState : ProcessState
            where TProcessException : ProcessExceptionBase, new()
        {
            server.OnProcessStart(new ActionCommand<StartProcessEventArgs<TProcessOptions>>(handler));
        }

        public static void ChangeState<TProcessOptions, TProcessState, TProcessException, TProcessResult>(
            this IProcessesProtocolServer<TProcessOptions, TProcessState, TProcessException, TProcessResult> server,
            TProcessState processState,
            Guid processGuid)
            where TProcessState : ProcessState
            where TProcessException : ProcessExceptionBase, new()
        {
            server.ChangeState(new ProcessStateChangedEventArgs<TProcessState>(processState, processGuid));
        }

        public static void Fail<TProcessOptions, TProcessState, TProcessException, TProcessResult>(
            this IProcessesProtocolServer<TProcessOptions, TProcessState, TProcessException, TProcessResult> server,
            TProcessException error, Guid processGuid)
            where TProcessState : ProcessState
            where TProcessException : ProcessExceptionBase, new()
        {
            server.Fail(new ProcessFailedEventArgs<TProcessException>(error, processGuid));
        }

        public static void Finish<TProcessOptions, TProcessState, TProcessException, TProcessResult>(
            this IProcessesProtocolServer<TProcessOptions, TProcessState, TProcessException, TProcessResult> server,
            TProcessResult result, Guid processGuid)
            where TProcessState : ProcessState
            where TProcessException : ProcessExceptionBase, new()
        {
            server.Finish(new ProcessFinishedEventArgs<TProcessResult>(result, processGuid));
        }
    }
}