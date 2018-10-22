using System;
using Microsoft.Extensions.Logging;
using PureActive.Logging.Abstractions.Interfaces;

namespace PureActive.Logger.Provider.Serilog
{
    public class SerilogLoggerProvider : IPureLoggerProvider
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public ILogger CreateLogger(string categoryName)
        {
            throw new NotImplementedException();
        }
    }
}
