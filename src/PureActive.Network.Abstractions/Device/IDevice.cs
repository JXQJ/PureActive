using PureActive.Hosting.Abstractions.System;
using PureActive.Network.Abstractions.Object;
using PureActive.Network.Abstractions.Types;

namespace PureActive.Network.Abstractions.Device
{
    public interface IDevice : IObject
    {
        ICommonServices CommonServices { get; }
        DeviceType DeviceType { get; set; }
    }
}
