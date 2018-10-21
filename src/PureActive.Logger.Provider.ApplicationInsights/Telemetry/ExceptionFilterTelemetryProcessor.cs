using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace PureActive.Logger.Provider.ApplicationInsights.Telemetry
{
    /// <summary>
    ///     Filters out all exception logging handled at the platform level.
    ///     This avoids duplicate exception traces being sent to Application
    ///     Insights (as Serilog already sends enriched exception information).
    /// </summary>
    public class ExceptionFilterTelemetryProcessor : ITelemetryProcessor
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        public ExceptionFilterTelemetryProcessor(ITelemetryProcessor next)
        {
            Next = next;
        }

        /// <summary>
        ///     The next telemetry processor in the chain.
        /// </summary>
        private ITelemetryProcessor Next { get; }

        /// <summary>
        ///     Processes the telemetry item.
        /// </summary>
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