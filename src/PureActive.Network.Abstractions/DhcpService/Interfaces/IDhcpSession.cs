using System;
using PureActive.Network.Abstractions.DhcpService.Types;

namespace PureActive.Network.Abstractions.DhcpService.Interfaces
{
    public interface IDhcpSession
    {
        RequestState RequestState { get; set; }

        DhcpSessionState DhcpSessionState { get; set; }


        DateTimeOffset CreatedTimestamp { get; }
        DateTimeOffset UpdatedTimestamp { get; set; }

        DateTimeOffset UpdateTimestamp();

        TimeSpan SessionTimeOut { get; set; }

        IDhcpDiscoveredDevice DhcpDiscoveredDevice { get; }

        bool HasSessionExpired(DateTimeOffset timeStamp, TimeSpan timeSpan);

        bool HasSessionExpired();

        DhcpMessageProcessed ProcessDiscover(IDhcpMessage dhcpMessage);


        DhcpMessageProcessed ProcessRequest(IDhcpMessage dhcpMessage);


        DhcpMessageProcessed ProcessDecline(IDhcpMessage dhcpMessage);


        DhcpMessageProcessed ProcessRelease(IDhcpMessage dhcpMessage);


        DhcpMessageProcessed ProcessInform(IDhcpMessage dhcpMessage);


        DhcpMessageProcessed ProcessAck(IDhcpMessage dhcpMessage);
    }
}