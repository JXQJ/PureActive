using PureActive.Core.Abstractions.Async;
using PureActive.Core.Abstractions.System;

namespace PureActive.Hosting.Abstractions.System
{
    public interface ICommonServices : IHostedServiceInternal
    {
        IProcessRunner ProcessRunner { get; }
        IFileSystem FileSystem { get; }
        IOperationRunner OperationRunner { get; }
        IOperatingSystem OperatingSystem { get; }
    }
}