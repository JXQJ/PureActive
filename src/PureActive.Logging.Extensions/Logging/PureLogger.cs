using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using PureActive.Logging.Abstractions.Interfaces;

namespace PureActive.Logging.Extensions.Logging
{
    public class PureLogger : IPureLogger
    {
        ILogger WrappedLogger { get;}

        public PureLogger(ILogger logger)
        {
            WrappedLogger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Wrap ILogger interface methods
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter) =>
            WrappedLogger?.Log(logLevel, eventId, state, exception, formatter);

        public bool IsEnabled(LogLevel logLevel) => WrappedLogger.IsEnabled(logLevel);

        public IDisposable BeginScope<TState>(TState state) => WrappedLogger.BeginScope(state);


    }
}
