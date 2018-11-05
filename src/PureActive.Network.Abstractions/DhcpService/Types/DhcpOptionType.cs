
namespace PureActive.Network.Abstractions.DhcpService.Types
{
    /// <summary>
    /// The dhcp defined options.
    /// </summary>
    public enum DhcpOptionType : byte
    {
        SafeString,
        SafeBytes,
        MessageTypeString,
        PhysicalAddressSkip1ColonString,
        PhysicalAddressSkip1DashString,
        PhysicalAddressColonString,
        PhysicalAddressDashString,

        IPAddress,
        UInt16,
        UInt32,
        UInt16Network,
        UInt32Network,
    }
}
