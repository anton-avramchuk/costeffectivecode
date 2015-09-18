using System;
using JetBrains.Annotations;

namespace CostEffectiveCode.Common.Messaging.Process
{
    [PublicAPI]
    public class StartProcessEventArgs<TProcessOptions> : ProcessEventArgsBase
    {
        private TProcessOptions _options;

        //public StartProcessEventArgs([NotNull] TProcessOptions options, Guid processGuid) : base(processGuid)
        //{
        //    InitOptions(options);
        //}

        public StartProcessEventArgs([NotNull] TProcessOptions options) : base(Guid.NewGuid())
        {
            InitOptions(options);
        }

        private void InitOptions(TProcessOptions options)
        {
            if (options == null) throw new ArgumentNullException("options");
            _options = options;
        }

        public TProcessOptions Options
        {
            get { return _options; }
        }
    }
}
