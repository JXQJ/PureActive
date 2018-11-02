using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Network.Abstractions.CommonNetworkServices;
using PureActive.Network.Abstractions.NetworkDevice;
using PureActive.Network.Abstractions.Types;
using PureActive.Network.Devices.Device;

namespace PureActive.Network.Devices.Network
{
    public class NetworkDeviceBase : DeviceBase, INetworkDevice
    {
        public ICommonNetworkServices CommonNetworkServices { get; }

        public NetworkDeviceBase(ICommonNetworkServices commonNetworkServices, DeviceType deviceType = DeviceType.UnknownDevice, IPureLogger logger = null) : 
            base(commonNetworkServices, deviceType, logger)
        {
            CommonNetworkServices = commonNetworkServices;
        }

        public static INetworkDevice NetworkDeviceFromType(ICommonNetworkServices commonNetworkServices,
            DeviceType deviceType)
        {
            switch (deviceType)
            {
                default:
                    break;
            }

            return new NetworkDeviceBase(commonNetworkServices, DeviceType.UnknownDevice, commonNetworkServices.LoggerFactory.CreatePureLogger<NetworkDeviceBase>());
        }
    }
}
