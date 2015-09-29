using System;
using JetBrains.Annotations;

namespace CostEffectiveCode.Messaging.Process
{
    [PublicAPI]
    public class StartProcessEventArgs<TProcessOptions> : ProcessEventArgsBase
    {
        private TProcessOptions _options;

        public StartProcessEventArgs([NotNull] TProcessOptions options) : base(Guid.NewGuid())
        {
            InitOptions(options);
        }

        private void InitOptions(TProcessOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            _options = options;
        }

        public TProcessOptions Options => _options;
    }
}
