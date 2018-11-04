using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PureActive.Core.Abstractions.Async;
using PureActive.Core.Abstractions.System;
using PureActive.Core.Extensions;
using PureActive.Hosting.Abstractions.System;
using PureActive.Hosting.Abstractions.Types;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Logging.Extensions.Types;
using PureActive.Network.Abstractions.ArpService;
using PureActive.Network.Abstractions.CommonNetworkServices;
using PureActive.Network.Abstractions.PingService;

namespace PureActive.Network.Services
{
    public class CommonNetworkServices : LoggableBase<CommonNetworkServices>, ICommonNetworkServices
    {
        // Implementation of ICommonServices
        public IProcessRunner ProcessRunner => CommonServices?.ProcessRunner;
        public IFileSystem FileSystem => CommonServices?.FileSystem;
        public IOperationRunner OperationRunner => CommonServices?.OperationRunner;
        public IOperatingSystem OperatingSystem => CommonServices?.OperatingSystem;

        // Implementation of ICommonNetworkServices specific items
        public ICommonServices CommonServices { get; }
        public IPingService PingService { get; }
        public IArpService ArpService { get; }

        public ServiceHostStatus ServiceHostStatus { get; internal set; } = ServiceHostStatus.Stopped;

        public CommonNetworkServices(ICommonServices commonServices, IPingService pingService, IArpService arpService) :
            base(commonServices?.LoggerFactory)
        {
            CommonServices = commonServices ?? throw new ArgumentNullException(nameof(commonServices));
            PingService = pingService ?? throw new ArgumentNullException(nameof(pingService));
            ArpService = arpService ?? throw new ArgumentNullException(nameof(arpService));
        }

        public static ICommonNetworkServices CreateInstance(IPureLoggerFactory loggerFactory, ICommonServices commonServices)
        {
            if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));
            if (commonServices == null) throw new ArgumentNullException(nameof(commonServices));

            // Common Network Services
            var pingService = new PingService.PingService(commonServices);
            var arpService = new ArpService.ArpService(commonServices, pingService);
  
            return new CommonNetworkServices(commonServices, pingService, arpService);
        }

        public static ICommonNetworkServices CreateInstance(IPureLoggerFactory loggerFactory, string appName)
        {
            return CreateInstance(loggerFactory, Hosting.CommonServices.CommonServices.CreateInstance(loggerFactory, appName));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            ServiceHostStatus = ServiceHostStatus.StartPending;
            var tasks = new List<Task>()
            {
                CommonServices.StartAsync(cancellationToken),
                PingService.StartAsync(cancellationToken),
                ArpService.StartAsync(cancellationToken),
            };

            var result = tasks.WaitForTasks(cancellationToken, Logger);

            if (result.IsCompleted && result.Status == TaskStatus.RanToCompletion)
                ServiceHostStatus = ServiceHostStatus.Running;

            return result;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            var tasks = new List<Task>()
            {
                ArpService.StopAsync(cancellationToken),
                PingService.StopAsync(cancellationToken),
                CommonServices.StopAsync(cancellationToken)
            };

            ServiceHostStatus = ServiceHostStatus.StopPending;

            var logger = CommonServices?.LoggerFactory?.CreatePureLogger<CommonNetworkServices>();
            var result = tasks.WaitForTasks(cancellationToken, logger);

            ServiceHostStatus = ServiceHostStatus.Stopped;

            return result;
        }

        // TODO: Implement ILogPropertyLevel
        //public override IEnumerable<ILogPropertyLevel> GetLogPropertyListLevel(LogLevel logLevel, LoggableFormat loggableFormat)
        //{
        //    var logPropertyLevels = loggableFormat.IsWithParents()
        //        ? base.GetLogPropertyListLevel(logLevel, loggableFormat)?.ToList()
        //        : new List<ILogPropertyLevel>();

        //    if (logLevel <= LogLevel.Information)
        //    {
        //        logPropertyLevels?.Add(new LogPropertyLevel("CommonNetworkServicesHostStatus", ServiceHostStatus, LogLevel.Information));
        //    }

        //    return logPropertyLevels;
        //}
    }
}
