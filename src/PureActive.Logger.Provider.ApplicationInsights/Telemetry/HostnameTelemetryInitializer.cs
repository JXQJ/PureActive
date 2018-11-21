// ***********************************************************************
// Assembly         : PureActive.Logger.Provider.ApplicationInsights
// Author           : SteveBu
// Created          : 10-22-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 10-22-2018
// ***********************************************************************
// <copyright file="HostnameTelemetryInitializer.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Net;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace PureActive.Logger.Provider.ApplicationInsights.Telemetry
{
    /// <summary>
    /// Initializes the operation ID from the request header, if any.
    /// Implements the <see cref="Microsoft.ApplicationInsights.Extensibility.ITelemetryInitializer" />
    /// </summary>
    /// <seealso cref="Microsoft.ApplicationInsights.Extensibility.ITelemetryInitializer" />
    public class HostnameTelemetryInitializer : ITelemetryInitializer
    {
        /// <summary>
        /// The hostname.
        /// </summary>
        private readonly Lazy<string> _hostName = new Lazy<string>(Dns.GetHostName);

        /// <summary>
        /// Initializes the telemetry.
        /// </summary>
        /// <param name="telemetry">The telemetry.</param>
        public void Initialize(ITelemetry telemetry)
        {
            if (telemetry.Context.Cloud.RoleInstance == null) telemetry.Context.Cloud.RoleInstance = _hostName.Value;
        }
    }
}