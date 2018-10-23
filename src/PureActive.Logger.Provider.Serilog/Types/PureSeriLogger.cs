using System;
using Microsoft.Extensions.Logging;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Logging.Extensions.Logging;
using ILoggerMsft = Microsoft.Extensions.Logging.ILogger;

namespace PureActive.Logger.Provider.Serilog.Types
{
    public class PureSeriLogger : PureLogger
    {
        public PureSeriLogger(ILoggerMsft logger) :base (logger)
        {

        }
    }
}
