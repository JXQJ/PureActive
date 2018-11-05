using System.Linq;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;

namespace PureActive.Hosting.Logging
{
    /// <summary>
    ///     Initializes the operation ID from the request header, if any.
    /// </summary>
    public class OperationIdTelemetryInitializer : ITelemetryInitializer
    {
        /// <summary>
        ///     The HTTP context accessor.
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public OperationIdTelemetryInitializer(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        ///     Initializes the initializer.
        /// </summary>
        public void Initialize(ITelemetry telemetry)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var requestTelemetry = httpContext?.Features.Get<RequestTelemetry>();

            if (requestTelemetry == null) return;

            httpContext
                .Request
                ?.Headers
                ?.TryGetValue("X-Operation-Id", out var value);

            if (value.Count != 1) return;

            requestTelemetry.Id = value.First();
            telemetry.Context.Operation.Id = value.First();
        }
    }
}