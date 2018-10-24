using System;
using Microsoft.Extensions.Logging;
using PureActive.Logging.Abstractions.Interfaces;

namespace PureActive.Logging.Extensions.Logging
{
    public class PureLoggableBase<T> : IPureLoggable
    {
        private IPureLogger Logger { get; }
        private IPureLoggerFactory LoggerFactory { get; }

        public PureLoggableBase(IPureLoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            Logger = loggerFactory.CreatePureLogger<T>();
        }
    }
}
