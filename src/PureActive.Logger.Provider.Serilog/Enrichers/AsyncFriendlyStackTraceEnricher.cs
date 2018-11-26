// ***********************************************************************
// Assembly         : PureActive.Logger.Provider.Serilog
// Author           : SteveBu
// Created          : 10-22-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 10-22-2018
// ***********************************************************************
// <copyright file="AsyncFriendlyStackTraceEnricher.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using AsyncFriendlyStackTrace;
using Serilog.Core;
using Serilog.Events;

namespace PureActive.Logger.Provider.Serilog.Enrichers
{
    /// <summary>
    /// Adds a readable stack trace to all exceptions.
    /// Implements the <see>
    ///         <cref>Serilog.Core.ILogEventEnricher</cref>
    ///     </see>
    /// </summary>
    /// <seealso>
    ///     <cref>Serilog.Core.ILogEventEnricher</cref>
    /// </seealso>
    public class AsyncFriendlyStackTraceEnricher : ILogEventEnricher
    {
        /// <summary>Enriches the log event with a AsyncFriendlyStackTrace</summary>
        /// <param name="logEvent">The log event to enrich.  Does nothing if exception is null.</param>
        /// <param name="propertyFactory">The property factory.</param>
        /// <exception cref="ArgumentNullException">propertyFactory</exception>
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (logEvent == null) throw new ArgumentNullException(nameof(logEvent));
            if (propertyFactory == null) throw new ArgumentNullException(nameof(propertyFactory));

            if (logEvent.Exception == null)
                return;

            var readableStackTrace = propertyFactory.CreateProperty(
                "ReadableStackTrace",
                logEvent.Exception.ToAsyncString(),
                true);

            logEvent.AddPropertyIfAbsent(readableStackTrace);

            var fullExceptionString = propertyFactory.CreateProperty(
                "FullExceptionString",
                logEvent.Exception.ToString(),
                true);

            logEvent.AddPropertyIfAbsent(fullExceptionString);
        }
    }
}