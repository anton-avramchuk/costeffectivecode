using System;
using JetBrains.Annotations;
using NLog;

namespace CostEffectiveCode.NLog
{
    public class NLogLogger : Common.Logger.ILogger
    {
        private readonly ILogger _logger;

        public NLogLogger()
            : this(LogManager.GetCurrentClassLogger())
        {
        }

        public NLogLogger([NotNull] ILogger logger)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            _logger = logger;
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Warning(string message)
        {
            _logger.Warn(message);
        }

        public void Error(Exception exception)
        {
            _logger.Error(exception);
        }

        public void Fatal(Exception exception)
        {
            _logger.Fatal(exception);
        }

        public void Error(object message)
        {
            _logger.Error(message);
        }

        public void ErrorFormat(string message, params object[] arguments)
        {
            _logger.Debug(message, arguments);
        }

        public void DebugFormat(string message, params object[] arguments)
        {
            _logger.Debug(message, arguments);
        }

        public void Error(string unexpextedError, Exception exception)
        {
            _logger.Error(exception, unexpextedError);
        }
    }
}
