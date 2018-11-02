using PureActive.Hosting.Abstractions.System;
using PureActive.Network.Abstractions.PureObject;
using PureActive.Network.Abstractions.Types;

namespace PureActive.Network.Abstractions.Device
{
    public interface IDevice : IPureObject
    {
        ICommonServices CommonServices { get; }
        DeviceType DeviceType { get; set; }
    }
}
