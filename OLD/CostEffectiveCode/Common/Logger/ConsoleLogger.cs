using System;

namespace CostEffectiveCode.Common.Logger
{
    public class ConsoleLogger : ILogger
    {
        public void Info(string message)
        {
            Console.WriteLine(message);
        }

        public void Debug(string message)
        {
            Console.WriteLine(message);
        }

        public void Warning(string message)
        {
            Console.WriteLine(message);
        }

        public void Error(Exception exception)
        {
            Console.WriteLine(exception.Message);
        }

        public void Fatal(Exception exception)
        {
            Console.WriteLine(exception.Message);
        }

        public void Error(object message)
        {
            Console.WriteLine(message);
        }

        public void ErrorFormat(string message, params object[] arguments)
        {
            Console.WriteLine(message);
        }

        public void DebugFormat(string message, params object[] arguments)
        {
            Console.WriteLine(message);
        }

        public void Error(string unexpextedError, Exception exception)
        {
            Console.WriteLine(unexpextedError);
        }
    }
}
