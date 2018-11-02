using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PureActive.Core.Abstractions.Async;
using PureActive.Core.Abstractions.System;
using PureActive.Core.Async;
using PureActive.Core.System;
using PureActive.Hosting.Abstractions.System;
using PureActive.Hosting.Abstractions.Types;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Logging.Extensions.Types;

namespace PureActive.Hosting.CommonServices
{
    public class CommonServices : LoggableBase<CommonServices>, ICommonServices
    {
        public IProcessRunner ProcessRunner { get; }
        public IFileSystem FileSystem { get; }
        public IOperationRunner OperationRunner { get; }
        public IOperatingSystem OperatingSystem { get; }

        public ServiceHostStatus ServiceHostStatus { get; internal set; } = ServiceHostStatus.Stopped;

        public CommonServices(IProcessRunner processRunner, IFileSystem fileSystem, IOperatingSystem operatingSystem, 
            IOperationRunner operationRunner, IPureLoggerFactory loggerFactory) :
            base(loggerFactory)
        {
            ProcessRunner = processRunner ?? throw new ArgumentNullException(nameof(processRunner));
            FileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            OperatingSystem = operatingSystem ?? throw new ArgumentNullException(nameof(operatingSystem));
            OperationRunner = operationRunner ?? throw new ArgumentNullException(nameof(operationRunner));
        }

        private static ICommonServices CreateInstanceCore(IPureLoggerFactory loggerFactory, IFileSystem fileSystem)
        {
            if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));

            var processRunner = new ProcessRunner(loggerFactory.CreatePureLogger<ProcessRunner>());
            var operationRunner = new OperationRunner(loggerFactory.CreatePureLogger<OperationRunner>());

            return new CommonServices(processRunner, fileSystem, fileSystem.OperatingSystem, operationRunner, loggerFactory);
        }

        public static ICommonServices CreateInstance(IPureLoggerFactory loggerFactory, string appFolderName)
        {
            return CreateInstanceCore(loggerFactory, new FileSystem(appFolderName));
        }

        public static ICommonServices CreateInstance(IPureLoggerFactory loggerFactory, Type type)
        {
            return CreateInstanceCore(loggerFactory, new FileSystem(type));
        }

        public static ICommonServices CreateInstance(IPureLoggerFactory loggerFactory, IConfigurationRoot configuration)
        {
            return CreateInstanceCore(loggerFactory, new FileSystem(configuration));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            ServiceHostStatus = ServiceHostStatus.Running;

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            ServiceHostStatus = ServiceHostStatus.Stopped;

            return Task.CompletedTask;
        }

        // TODO: ILogPropertyLevel
        //public override IEnumerable<ILogPropertyLevel> GetLogPropertyListLevel(LogLevel logLevel, LoggableFormat loggableFormat)
        //{
        //    var logPropertyLevels = loggableFormat.IsWithParents()
        //        ? base.GetLogPropertyListLevel(logLevel, loggableFormat)?.ToList()
        //        : new List<ILogPropertyLevel>();

        //    if (logLevel <= LogLevel.Information)
        //    {
        //        logPropertyLevels?.Add(new LogPropertyLevel("CommonServicesHostStatus", ServiceHostStatus, LogLevel.Information));
        //    }

        //    return logPropertyLevels;
        //}
    }
}
