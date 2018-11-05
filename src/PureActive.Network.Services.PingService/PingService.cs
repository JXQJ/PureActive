using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using PureActive.Hosting.Abstractions.System;
using PureActive.Hosting.Abstractions.Types;
using PureActive.Hosting.Hosting;
using PureActive.Network.Abstractions.Extensions;
using PureActive.Network.Abstractions.PingService;
using PureActive.Network.Abstractions.PingService.Events;
using PureActive.Network.Abstractions.Types;

namespace PureActive.Network.Services.PingService
{
    public partial class PingService : BackgroundServiceInternal<PingService>, IPingService
    {
        public event PingReplyEventHandler OnPingReply;
        public bool EnableLogging { get; set; } = true;

        private readonly IPingTask _pingTask;

        private static readonly int DefaultTtl = 30;
        private static readonly int DefaultNetworkTimeout = 250;

        public PingService(ICommonServices commonServices, IApplicationLifetime applicationLifetime = null) :
            base(commonServices, applicationLifetime, ServiceHost.PingService)
        {
            if (commonServices == null) throw new ArgumentNullException(nameof(commonServices));

            OnPingReply += PingReplyLoggingEventHandler;

            _pingTask = new PingTaskImpl(commonServices);
            _pingTask.OnPingReply += OnPingReply;
        }

        private void PingReplyLoggingEventHandler(object sender, PingReplyEventArgs args)
        {
            if (args != null && EnableLogging)
            {
                using (Logger?.BeginScope("[{Timestamp}:{JobId}:{TaskId}]", args.PingJob.Timestamp, args.PingJob.JobGuid, args.PingJob.TaskId))
                {
                    var pingReply = args.PingReply;


                    // TODO: Fix PingReply Logging
                    //if (pingReply.Status == IPStatus.Success)
                    //{
                    //    using (Logger?.With(pingReply.GetLogPropertyListLevel(LogLevel.Debug, LoggableFormat.ToLog)))
                    //    {
                    //        Logger?.LogDebug("Ping {Status} to {IPAddressSubnet}", args.PingReply.Status,args.PingJob.IPAddressSubnet);
                    //    }
                    //}
                    //else if (pingReply.Status == IPStatus.TimedOut)
                    //{
                    //    using (Logger?.With(pingReply.GetLogPropertyListLevel(LogLevel.Trace, LoggableFormat.ToLog)))
                    //    {
                    //        Logger?.LogTrace("Ping {Status} for {IPAddressSubnet}", args.PingReply.Status, args.PingJob.IPAddressSubnet);
                    //    }
                    //}
                    //else
                    //{
                    //    using (Logger?.With(pingReply.GetLogPropertyListLevel(LogLevel.Debug, LoggableFormat.ToLog)))
                    //    {
                    //        Logger?.LogDebug("Ping {Status} for {IPAddressSubnet}", args.PingReply.Status, args.PingJob.IPAddressSubnet);
                    //    }
                    //}
                }
            }
        }


        public Task PingNetworkAsync(IPAddressSubnet ipAddressSubnet, CancellationToken cancellationToken, int timeout, int pingCallLimit, bool shuffle)
        {
            var iPAddressSubnet = IPAddressExtensions.GetDefaultGatewayAddressSubnet(Logger);

            return _pingTask.PingNetworkAsync(iPAddressSubnet, cancellationToken, timeout, pingCallLimit, shuffle);
        }

        public Task<PingReply> PingIpAddressAsync(IPAddress ipAddress, int timeout)
        {
            return _pingTask.PingIpAddressAsync(ipAddress, timeout);
        }

        public Task<PingReply> PingIpAddressAsync(IPAddress ipAddress) => PingIpAddressAsync(ipAddress, _pingTask.DefaultTimeout);

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var iPAddressSubnet = IPAddressExtensions.GetDefaultGatewayAddressSubnet(Logger);

                    await _pingTask.PingNetworkAsync(iPAddressSubnet, stoppingToken, DefaultNetworkTimeout, new PingOptions(DefaultTtl, true), Int32.MaxValue, true);
                }
            }, stoppingToken);
        }
    }
}
