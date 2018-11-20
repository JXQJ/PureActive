using System;
using PureActive.Network.Abstractions.Types;

namespace PureActive.Network.Abstractions.DeviceInfo
{
    public interface IDeviceInfo
    {
        string Id { get; }

        string Model { get; }

        string Manufacturer { get; }

        string DeviceName { get; }

        string Version { get; }

        Version VersionNumber { get; }

        string AppVersion { get; }

        string AppBuild { get; }

        DevicePlatform DevicePlatform { get; }

        DeviceType DeviceType { get; }

        bool IsDevice { get; }
    }
}