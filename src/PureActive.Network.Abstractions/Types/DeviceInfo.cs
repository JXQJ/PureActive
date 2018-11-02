using System;
using PureActive.Network.Abstractions.DeviceInfo;

namespace PureActive.Network.Abstractions.Types
{
    public abstract class DeviceInfo : IDeviceInfo
    {
        public string Id { get; }
        public string Model { get; }
        public string Manufacturer { get; }
        public string DeviceName { get; }
        public string Version { get; }
        public Version VersionNumber { get; }
        public string AppVersion { get; }
        public string AppBuild { get; }
        public DevicePlatform DevicePlatform { get; }
        public DeviceType DeviceType { get; }
        public bool IsDevice { get; }
    }
}
