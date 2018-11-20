using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Microsoft.Extensions.Logging;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Logging.Abstractions.Types;
using PureActive.Logging.Extensions.Types;

namespace PureActive.Network.Abstractions.PingService.Extensions
{
    public static class PingReplyExtensions
    {
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