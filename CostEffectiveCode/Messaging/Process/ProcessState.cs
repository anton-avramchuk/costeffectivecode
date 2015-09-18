// ReSharper disable ConvertToAutoProperty
// ReSharper disable ConvertPropertyToExpressionBody

namespace CostEffectiveCode.Messaging.Process
{
    public class ProcessState
    {
        private readonly ushort _progress;
        private readonly ProcessStatusEnum _status;

        public ProcessState(ushort progress, ProcessStatusEnum status = ProcessStatusEnum.InProgress)
        {
            _status = status;
            _progress = progress;

            //switch (Status)
            //{
            //    case ProcessStatusEnum.Start:
            //        _progress = 0;
            //        break;
            //    case ProcessStatusEnum.Finished:
            //        _progress = 100;
            //        break;
            //    default:
            //        _progress = progress;
            //        break;
            //}
        }

        /// <summary>
        /// Progress is from 0 to 100 percents
        /// </summary>
        public ushort Progress
        {
            get { return _progress; }
        }

        public ProcessStatusEnum Status
        {
            get { return _status; }
        }
    }
}
