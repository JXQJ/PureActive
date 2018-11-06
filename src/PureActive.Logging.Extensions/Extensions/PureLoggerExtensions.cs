using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using PureActive.Logging.Abstractions.Interfaces;

namespace PureActive.Logging.Extensions.Extensions
{
    public static class PureLoggerExtensions
    {
        public static IDisposable BeginPropertyScope<T>(this IPureLogger logger, string propertyName, T value)
        {
            return logger?.BeginScope(new Dictionary<string, T> { { propertyName, value } });
        }
    }
}
