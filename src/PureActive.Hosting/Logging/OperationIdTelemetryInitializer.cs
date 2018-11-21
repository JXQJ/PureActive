// ***********************************************************************
// Assembly         : PureActive.Hosting
// Author           : SteveBu
// Created          : 11-03-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-05-2018
// ***********************************************************************
// <copyright file="OperationIdTelemetryInitializer.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Linq;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;

namespace PureActive.Hosting.Logging
{
    /// <summary>
    /// Initializes the operation ID from the request header, if any.
    /// Implements the <see cref="ITelemetryInitializer" />
    /// </summary>
    /// <seealso cref="ITelemetryInitializer" />
    public class OperationIdTelemetryInitializer : ITelemetryInitializer
    {
        /// <summary>
        /// The HTTP context accessor.
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public OperationIdTelemetryInitializer(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Initializes the initializer.
        /// </summary>
        /// <param name="telemetry">The telemetry.</param>
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