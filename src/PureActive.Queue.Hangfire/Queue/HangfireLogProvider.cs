using Hangfire.Logging;
using PureActive.Logging.Abstractions.Interfaces;

namespace PureActive.Queue.Hangfire.Queue
{
    /// <summary>
    ///     A HangFire log provider that uses system logging.
    /// </summary>
    public class HangfireLogProvider : ILogProvider
    {
        /// <summary>
        ///     The logger factory.
        /// </summary>
        private readonly IPureLoggerFactory _loggerFactory;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public HangfireLogProvider(IPureLoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        /// <summary>
        ///     Returns a Hangfire logger.
        /// </summary>
        public ILog GetLogger(string name)
        {
            return new HangfireLogger(_loggerFactory?.CreatePureLogger(name));
        }
    }
}