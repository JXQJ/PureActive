using System;
using System.Collections.Generic;
using System.Text;
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
            return WrappedLoggerFactory.CreateLogger(categoryName);
        }

        public void AddProvider(ILoggerProvider provider)
        {
            WrappedLoggerFactory.AddProvider(provider);
        }
    }
}
