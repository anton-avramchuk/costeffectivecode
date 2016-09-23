using System;
using JetBrains.Annotations;

namespace CostEffectiveCode.Common
{
    [PublicAPI]
    public interface ILogger
    {
        void Info(string message);
      
        void Debug(string message);

        void Warning(string message);

        void Error(Exception exception);
        
        void Fatal(Exception exception);

        void Error(object message);

        void ErrorFormat(string message, params object[] arguments);

        void DebugFormat(string message, params object[] arguments);

	    void Error(string unexpectedError, Exception exception);
    }
}
