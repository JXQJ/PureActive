using System;
using Microsoft.Extensions.Logging;
using PureActive.Logger.Provider.Serilog.Interfaces;
using PureActive.Logging.Abstractions.Interfaces;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace PureActive.Logger.Provider.Serilog.Types
{
    public class PureSeriLoggerFactory : IPureLoggerFactory
    {
        public ILoggerFactory WrappedLoggerFactory { get; }
        public ISerilogLoggerSettings SerilogLoggerSettings { get; }

        public IPureLoggerSettings PureLoggerSettings => SerilogLoggerSettings;

        public PureSeriLoggerFactory(ILoggerFactory loggerFactory, ISerilogLoggerSettings loggerSettings)
        {
            WrappedLoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            SerilogLoggerSettings = loggerSettings ?? throw new ArgumentNullException(nameof(loggerSettings));
        }

        public void Dispose()
        {
            WrappedLoggerFactory.Dispose();
        }

        public ILogger CreateLogger(string categoryName)
        {
            return CreatePureLogger(categoryName);
        }

        public ILogger<T> CreateLogger<T>()
        {
            return CreatePureLogger<T>();
        }

        public void AddProvider(ILoggerProvider provider)
        {
            WrappedLoggerFactory.AddProvider(provider);
        }

        public IPureLogger CreatePureLogger(string categoryName)
        {
            return new PureSeriLogger(WrappedLoggerFactory.CreateLogger(categoryName));
        }

        public IPureLogger<T> CreatePureLogger<T>()
        {
            return new PureSeriLogger<T>(WrappedLoggerFactory.CreateLogger<T>());
        }
    }
}
