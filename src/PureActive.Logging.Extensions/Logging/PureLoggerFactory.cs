using System;
using Microsoft.Extensions.Logging;
using PureActive.Logging.Abstractions.Interfaces;

namespace PureActive.Logging.Extensions.Logging
{
    public class PureLoggerFactory : IPureLoggerFactory
    {
        ILoggerFactory WrappedLoggerFactory { get;}

        public PureLoggerFactory(ILoggerFactory loggerFactory)
        {
            WrappedLoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
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
            return new PureLogger(WrappedLoggerFactory.CreateLogger(categoryName));
        }

        public IPureLogger<T> CreatePureLogger<T>()
        {
            return new PureLogger<T>(WrappedLoggerFactory.CreateLogger<T>());
        }
    }
}
