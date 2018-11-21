// ***********************************************************************
// Assembly         : PureActive.Logger.Provider.ApplicationInsights
// Author           : SteveBu
// Created          : 10-22-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 10-22-2018
// ***********************************************************************
// <copyright file="ExceptionFilterTelemetryProcessor.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace PureActive.Logger.Provider.ApplicationInsights.Telemetry
{
    /// <summary>
    /// Filters out all exception logging handled at the platform level.
    /// This avoids duplicate exception traces being sent to Application
    /// Insights (as Serilog already sends enriched exception information).
    /// Implements the <see cref="Microsoft.ApplicationInsights.Extensibility.ITelemetryProcessor" />
    /// </summary>
    /// <seealso cref="Microsoft.ApplicationInsights.Extensibility.ITelemetryProcessor" />
    public class ExceptionFilterTelemetryProcessor : ITelemetryProcessor
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="next">The next.</param>
        public ExceptionFilterTelemetryProcessor(ITelemetryProcessor next)
        {
            Next = next;
        }

        /// <summary>
        /// The next telemetry processor in the chain.
        /// </summary>
        /// <value>The next.</value>
        private ITelemetryProcessor Next { get; }

        /// <summary>
        /// Processes the telemetry item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Process(ITelemetry item)
        {
            if (item is ExceptionTelemetry excTelem &&
                excTelem.Properties.TryGetValue("handledAt", out var handledAt) &&
                handledAt == "Platform")
                return;

            Next.Process(item);
        }
    }
}