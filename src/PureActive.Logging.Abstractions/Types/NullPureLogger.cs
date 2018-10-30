using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using PureActive.Logging.Abstractions.Interfaces;

namespace PureActive.Logging.Abstractions.Types
{
    public class NullPureLogger : IPureLogger
    {
       public static NullPureLogger Instance { get; } = new NullPureLogger();

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            NullLogger.Instance.Log(logLevel, eventId, state, exception, formatter);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return NullLogger.Instance.IsEnabled(logLevel);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return NullLogger.Instance.BeginScope(state);
        }
    }

    public class NullPureLogger<T> : NullLogger<T>, IPureLogger<T>
    {
 
    }
}
