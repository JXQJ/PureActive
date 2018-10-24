using System;
using Microsoft.Extensions.Logging;
using PureActive.Logging.Abstractions.Interfaces;
using ILoggerMsft = Microsoft.Extensions.Logging.ILogger;

namespace PureActive.Logger.Provider.Serilog.Types
{
    public class PureSeriLogger : IPureLogger
    {
        public ILoggerMsft WrappedLogger { get; }

        public PureSeriLogger(ILoggerMsft logger)
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

    public class PureSeriLogger<T> : PureSeriLogger, IPureLogger<T>
    {
        public PureSeriLogger(ILoggerMsft logger) :base (logger)
        {

        }
    }


}
