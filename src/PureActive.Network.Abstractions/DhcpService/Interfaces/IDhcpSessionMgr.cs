using System.Net.NetworkInformation;

namespace PureActive.Network.Abstractions.DhcpService.Interfaces
{
    public interface IDhcpSessionMgr
    {
        IDhcpSession FindOrCreateDhcpSession(PhysicalAddress physicalAddress);
    }
}
