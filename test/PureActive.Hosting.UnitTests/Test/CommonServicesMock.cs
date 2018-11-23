using System;
using Moq;
using PureActive.Core.Abstractions.Async;
using PureActive.Core.Abstractions.System;
using PureActive.Hosting.Abstractions.System;
using PureActive.Logging.Abstractions.Interfaces;

namespace PureActive.Hosting.UnitTests.Test
{
    public class CommonServicesMock
    {
        [Flags]
        public enum CommonServicesToMock
        {
            ProcessRunner = 1 << 0,
            FileSystem = 1 << 1,
            OperationRunner = 1 << 2,
            OperatingSystem = 1 << 3,
            LoggerFactory = 1 << 4,
            Logger = 1 << 5,
            LoggerSettings = 1 << 6,
        }

        public Mock<ICommonServices> CommonServicesTest { get; internal set; }

        public ICommonServices CommonServices { get; internal set; }

        public CommonServicesMock(ICommonServices commonServices, CommonServicesToMock commonServicesToMock)
        {
            CommonServices = commonServices ?? throw new ArgumentNullException(nameof(commonServices));
            CommonServicesTest = new Mock<ICommonServices>();

   
            CommonServicesTest.Setup(cst => cst.ProcessRunner).Returns(commonServicesToMock.HasFlag(CommonServicesToMock.ProcessRunner) ? new Mock<IProcessRunner>().Object : CommonServices.ProcessRunner);
            CommonServicesTest.Setup(cst => cst.FileSystem).Returns(commonServicesToMock.HasFlag(CommonServicesToMock.FileSystem) ? new Mock<IFileSystem>().Object : CommonServices.FileSystem);
            CommonServicesTest.Setup(cst => cst.OperationRunner).Returns(commonServicesToMock.HasFlag(CommonServicesToMock.OperationRunner) ? new Mock<IOperationRunner>().Object : CommonServices.OperationRunner);
            CommonServicesTest.Setup(cst => cst.OperatingSystem).Returns(commonServicesToMock.HasFlag(CommonServicesToMock.OperatingSystem) ? new Mock<IOperatingSystem>().Object : CommonServices.OperatingSystem);
            CommonServicesTest.Setup(cst => cst.LoggerFactory).Returns(commonServicesToMock.HasFlag(CommonServicesToMock.LoggerFactory) ? new Mock<IPureLoggerFactory>().Object : CommonServices.LoggerFactory);
            CommonServicesTest.Setup(cst => cst.Logger).Returns(commonServicesToMock.HasFlag(CommonServicesToMock.Logger) ? new Mock<IPureLogger>().Object : CommonServices.Logger);
            CommonServicesTest.Setup(cst => cst.LoggerSettings).Returns(commonServicesToMock.HasFlag(CommonServicesToMock.LoggerSettings) ? new Mock<IPureLoggerSettings>().Object : CommonServices.LoggerSettings);
        }
    }
}