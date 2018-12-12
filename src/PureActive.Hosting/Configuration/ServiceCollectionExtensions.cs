// ***********************************************************************
// Assembly         : PureActive.Hosting
// Author           : SteveBu
// Created          : 11-03-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="ServiceCollectionExtensions.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PureActive.Hosting.Logging;
using PureActive.Logger.Provider.ApplicationInsights.Telemetry;
using PureActive.Logging.Abstractions.Interfaces;

namespace PureActive.Hosting.Configuration
{
    /// <summary>
    /// Extension methods for configuring the logger factory.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configures logging.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="telemetryInitializers">The telemetry initializers.</param>
        public static void AddTelemetry(
            this IServiceCollection services,
            IConfiguration configuration,
            params Type[] telemetryInitializers)
        {
            services.AddApplicationInsightsTelemetry(configuration);

            // Disable exception logging (as Serilog already sends exceptions
            // to Application Insights).
            var telemetryConfiguration = services.BuildServiceProvider()
                .GetService<TelemetryConfiguration>();
            var builder = telemetryConfiguration.TelemetryProcessorChainBuilder;
            builder.Use(next => new ExceptionFilterTelemetryProcessor(next));
            builder.Build();

            // Allow code to retrieve the current operation ID. This is useful
            // for sending the operation ID to backend requests.
            services.AddScoped<IOperationIdProvider, OperationIdProvider>();

            // Uses the operation ID in the request header, if any.
            services.AddSingleton(typeof(ITelemetryInitializer), typeof(OperationIdTelemetryInitializer));

            // Includes the hostname with each log entry.
            services.AddSingleton(typeof(ITelemetryInitializer), typeof(HostnameTelemetryInitializer));

            foreach (var type in telemetryInitializers) services.AddSingleton(typeof(ITelemetryInitializer), type);
        }
    }
}