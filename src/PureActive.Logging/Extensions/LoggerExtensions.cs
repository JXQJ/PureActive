using System;
using Microsoft.Extensions.Logging;

namespace PureActive.Logging.Extensions
{
    public static class LoggerExtensions
    {
        public static void AddTime(this ILogger logger, string msg)
        {
            logger.Log(LogLevel.Debug, $"[{DateTimeOffset.Now}] {msg}");
        }
    }
}
