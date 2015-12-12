// ReSharper disable ConvertToAutoProperty
// ReSharper disable ConvertPropertyToExpressionBody

using JetBrains.Annotations;

namespace CostEffectiveCode.Processes.State
{
    [PublicAPI]
    public abstract class ProcessState
    {
        private readonly ushort _progress;
        private readonly ProcessStatusEnum _status;

        protected ProcessState(ushort progress, ProcessStatusEnum status = ProcessStatusEnum.InProgress)
        {
            _status = status;
            _progress = progress;
        }

        /// <summary>
        /// Progress is from 0 to 100 percents
        /// </summary>
        public ushort Progress => _progress;

        public ProcessStatusEnum Status => _status;
    }
}
