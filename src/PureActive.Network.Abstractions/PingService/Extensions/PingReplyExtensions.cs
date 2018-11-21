﻿// ***********************************************************************
// Assembly         : PureActive.Network.Abstractions
// Author           : SteveBu
// Created          : 11-05-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="PingReplyExtensions.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Microsoft.Extensions.Logging;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Logging.Abstractions.Types;
using PureActive.Logging.Extensions.Types;

namespace PureActive.Network.Abstractions.PingService.Extensions
{
    /// <summary>
    /// Class PingReplyExtensions.
    /// </summary>
    /// <autogeneratedoc />
    public static class PingReplyExtensions
    {
        /// <summary>
        /// Gets the log property list level.
        /// </summary>
        /// <param name="pingReply">The ping reply.</param>
        /// <param name="logLevel">The log level.</param>
        /// <param name="loggableFormat">The loggable format.</param>
        /// <returns>IEnumerable&lt;IPureLogPropertyLevel&gt;.</returns>
        /// <autogeneratedoc />
        public static IEnumerable<IPureLogPropertyLevel> GetLogPropertyListLevel(this PingReply pingReply,
            LogLevel logLevel, LoggableFormat loggableFormat)
        {
            return new List<IPureLogPropertyLevel>
                {
                    new PureLogPropertyLevel("Status", pingReply.Status, LogLevel.Information),
                    new PureLogPropertyLevel("IPAddress", pingReply.Address, LogLevel.Information),
                    new PureLogPropertyLevel("RoundtripTime", pingReply.RoundtripTime, LogLevel.Trace)
                }
                .Where(p => p.MinimumLogLevel.CompareTo(logLevel) >= 0);
        }
    }
}