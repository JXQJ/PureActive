using System;
using System.Collections.Generic;
using System.Linq;
using PureActive.Logging.Abstractions.Interfaces;
using Serilog.Context;
using Serilog.Core;
using Serilog.Core.Enrichers;

namespace PureActive.Logger.Provider.Serilog.Types
{
    /// <summary>
    ///     Allows for adding additional properties to all log messages
    ///     logged within newly created scopes.
    /// </summary>
    public class PureSeriLogContext : IPureLogContext
    {
        /// <summary>
        ///     Creates a new log scope, with the given properties.
        /// </summary>
        public IDisposable CreateLogScope(IList<KeyValuePair<string, string>> properties)
        {
            return LogContext.Push
            (
                properties.Select
                    (
                        p => new PropertyEnricher(p.Key, p.Value)
                    ).Cast<ILogEventEnricher>()
                    .ToArray()
            );
        }
    }
}