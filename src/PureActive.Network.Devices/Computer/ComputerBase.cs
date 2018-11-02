using Microsoft.Extensions.Logging;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Network.Abstractions.CommonNetworkServices;
using PureActive.Network.Abstractions.Computer;
using PureActive.Network.Abstractions.Types;
using PureActive.Network.Devices.Network;

namespace PureActive.Network.Devices.Computer
{
    public abstract class ComputerBase : NetworkDeviceBase, IComputer
    {
        protected ComputerBase(ICommonNetworkServices commonNetworkServices, DeviceType deviceType, IPureLogger logger = null) : 
            base(commonNetworkServices, deviceType, logger)
        {

        }

        protected ComputerBase(ICommonNetworkServices commonNetworkServices) : this(commonNetworkServices, DeviceType.Computer)
        {

        }

    }
}
