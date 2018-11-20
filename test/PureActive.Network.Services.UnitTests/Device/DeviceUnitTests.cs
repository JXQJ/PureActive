using Microsoft.Extensions.Logging;
using PureActive.Hosting.Abstractions.System;
using PureActive.Hosting.CommonServices;
using PureActive.Logging.Abstractions.Types;
using PureActive.Network.Abstractions.Types;
using PureActive.Network.Devices.Device;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.Services.UnitTests.Device
{
    [Trait("Category", "Unit")]
    public class DeviceUnitTests : TestBaseLoggable<DeviceUnitTests>
    {
        public DeviceUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _commonServices = CommonServices.CreateInstance(TestLoggerFactory, "DeviceUnitTests");
        }

        private readonly ICommonServices _commonServices;

        private class DeviceTest : DeviceBase
        {
            public DeviceTest(ICommonServices commonServices, DeviceType deviceType) :
                base(commonServices, deviceType)
            {
            }
        }

        [Fact]
        public void Device_ToLog()
        {
            var deviceTest = new DeviceTest(_commonServices, DeviceType.UnknownDevice);

            TestOutputHelper.WriteLine(deviceTest.ToString(LogLevel.Debug, LoggableFormat.ToLog));
        }

        [Fact]
        public void Device_ToLogParents()
        {
            var deviceTest = new DeviceTest(_commonServices, DeviceType.UnknownDevice);

            TestOutputHelper.WriteLine(deviceTest.ToString(LogLevel.Debug, LoggableFormat.ToLogWithParents));
        }


        [Fact]
        public void Device_ToString()
        {
            var deviceTest = new DeviceTest(_commonServices, DeviceType.UnknownDevice);

            TestOutputHelper.WriteLine(deviceTest.ToString(LogLevel.Debug, LoggableFormat.ToString));
        }

        [Fact]
        public void Device_ToStringParents()
        {
            var deviceTest = new DeviceTest(_commonServices, DeviceType.UnknownDevice);

            TestOutputHelper.WriteLine(deviceTest.ToString(LogLevel.Debug, LoggableFormat.ToStringWithParents));
        }
    }
}