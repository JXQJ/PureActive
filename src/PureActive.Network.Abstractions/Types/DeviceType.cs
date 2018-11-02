using System;

namespace PureActive.Network.Abstractions.Types
{
    [Flags]
    public enum DeviceType : ulong
    {
        UnknownDevice = 0,
        LocalDevice = 1 << 1,
        NetworkDevice = 1 << 2,
        Computer = 1 << 3,
        Phone  = 1 << 4,
        Tablet = 1 << 4,
        MediaDevice = 1 << 5,
        SmartTv = 1 << 6,
        GameConsole = 1 << 6,
        Car = 1 << 7,
        Watch = 1 << 8,
        Storage = 1 << 7,
        Printer = 1 << 8,
        Gateway = 1 << 9,
        AccessPoint = 1 << 10,
        Network = 1 << 11,

        LocalNetworkDevice = LocalDevice | NetworkDevice,

        LocalComputer = LocalNetworkDevice | Computer,
        RemoteComputer = NetworkDevice | Computer,

        LocalCellPhone = LocalNetworkDevice | Phone,
        RoamingCellPhone = NetworkDevice | Phone,

        NetworkGateway = NetworkDevice | Gateway,
        NetworkAccessPoint = NetworkDevice | AccessPoint,

        LocalNetwork = LocalDevice | Network,
        Internet = Network + 1,

        NetworkAdapter = LocalNetworkDevice + 1,


    }
}
