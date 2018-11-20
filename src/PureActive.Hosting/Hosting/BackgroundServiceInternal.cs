using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using PureActive.Hosting.Abstractions.System;
using PureActive.Hosting.Abstractions.Types;

namespace PureActive.Hosting.Hosting
{
    public abstract class BackgroundServiceInternal<T> : HostedServiceInternal<T>, IDisposable
    {
        private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();
        private Task _executingTask;

        protected BackgroundServiceInternal(ICommonServices commonServices, IApplicationLifetime applicationLifetime,
            ServiceHost serviceHost) :
            base(commonServices, applicationLifetime, serviceHost)
        {
        }

        public virtual void Dispose()
        {
            _stoppingCts.Cancel();
        }

        /// <summary>
        ///     This method is called when the <see cref="T:Microsoft.Extensions.Hosting.IHostedService" /> starts. The
        ///     implementation should return a task that represents
        ///     the lifetime of the long running operation(s) being performed.
        /// </summary>
        /// <param name="stoppingToken">
        ///     Triggered when
        ///     <see cref="M:Microsoft.Extensions.Hosting.IHostedService.StopAsync(System.Threading.CancellationToken)" /> is
        ///     called.
        /// </param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that represents the long running operations.</returns>
        protected abstract Task ExecuteAsync(CancellationToken stoppingToken);

        /// <summary>
        ///     Triggered when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            if (ServiceHostStatus != ServiceHostStatus.Stopped)
            {
                Logger?.LogDebug("{ServiceHost}:Started Called with {ServiceHostStatus}", ServiceHost,
                    ServiceHostStatus);
            }

            ServiceHostStatus = ServiceHostStatus.StartPending;
            _executingTask = ExecuteAsync(_stoppingCts.Token);

            _executingTask.ContinueWith(t =>
            {
                if (_executingTask.IsCompleted)
                {
                    ServiceHostStatus = ServiceHostStatus.Stopped;
                }
            }, cancellationToken);

            if (_executingTask.IsCompleted)
            {
                return _executingTask;
            }

            return Task.CompletedTask;
        }

        /// <summary>
        ///     Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            ServiceHostStatus = ServiceHostStatus.StopPending;

            if (_executingTask == null)
                return;
            try
            {
                _stoppingCts.Cancel();
            }
            finally
            {
                await Task.WhenAny(_executingTask, Task.Delay(-1, cancellationToken));
                ServiceHostStatus = ServiceHostStatus.Stopped;
            }
        }
    }
}