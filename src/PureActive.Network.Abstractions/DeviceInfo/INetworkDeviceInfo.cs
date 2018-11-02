using System.Net;

namespace PureActive.Network.Abstractions.DeviceInfo
{
    public interface INetworkDeviceInfo : IDeviceInfo
    {
        IPAddress IpAddress { get; set; }
    }
}
