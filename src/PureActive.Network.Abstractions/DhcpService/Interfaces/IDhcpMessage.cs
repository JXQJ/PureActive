using System;
using System.Net.NetworkInformation;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Network.Abstractions.DhcpService.Types;
using PureActive.Network.Abstractions.Types;

namespace PureActive.Network.Abstractions.DhcpService.Interfaces
{
    public interface IDhcpMessage : IPureLoggable
    {
        /// <summary>
        ///     The timestamp when cached.
        /// </summary>
        DateTimeOffset CreatedTimestamp { get; set; }

        /// <summary>
        ///     The operation code of whatever last altered the packet (op).
        /// </summary>
        OperationCode Operation { get; set; }

        /// <summary>
        ///     The hardware address type (htype).
        /// </summary>
        HardwareType Hardware { get; set; }

        /// <summary>
        ///     The hardware address length (hlen).
        /// </summary>
        byte HardwareAddressLength { get; set; }

        /// <summary>
        ///     Optionally used by relay agents when booting via a relay agent (hops).
        /// </summary>
        byte Hops { get; set; }

        /// <summary>
        ///     A random number chosen by the client, to associate messages and responses between a client and a server (xid).
        /// </summary>
        uint SessionId { get; set; }

        /// <summary>
        ///     The seconds elapsed since client began address acquisition or renewal process (secs).
        /// </summary>
        ushort SecondsElapsed { get; set; }

        /// <summary>
        ///     The leftmost bit is defined as the BROADCAST (B) flag.   The remaining bits of the flags field are reserved for
        ///     future use.
        /// </summary>
        byte[] Flags { get; set; }

        /// <summary>
        ///     Is Broadcast(true) / Unicast(false) flag
        /// </summary>
        bool IsBroadcast { get; set; }

        /// <summary>
        ///     The client IP address (ciaddr).
        /// </summary>
        InternetAddress ClientAddress { get; set; }

        /// <summary>
        ///     The assigned client IP address (yiaddr).
        /// </summary>
        InternetAddress AssignedAddress { get; set; }

        /// <summary>
        ///     The server IP address (siaddr).
        /// </summary>
        InternetAddress NextServerAddress { get; set; }

        /// <summary>
        ///     The gateway IP address (giaddr);
        /// </summary>
        InternetAddress RelayAgentAddress { get; set; }

        /// <summary>
        ///     The client hardware address (chaddr).
        /// </summary>
        PhysicalAddress ClientHardwareAddress { get; set; }

        /// <summary>
        ///     The DNS hostname part that corresponds with the given IP address (sname).
        /// </summary>
        byte[] ServerName { get; set; }

        /// <summary>
        ///     The boot file name the clients may use (file).
        /// </summary>
        byte[] BootFileName { get; set; }

        /// <summary>
        ///     The option ordering parameter list.
        /// </summary>
        byte[] OptionOrdering { get; set; }

        /// <summary>
        ///     Get option data.
        /// </summary>
        byte[] GetOptionData(DhcpOption option);

        /// <summary>
        ///     Remove all options.
        /// </summary>
        void ClearOptions();

        /// <summary>
        ///     Add or define a new custom option type.
        /// </summary>
        void AddOption(DhcpOption option, params byte[] data);

        bool ParamOptionExists(DhcpOption paramOption);

        /// <summary>
        ///     Remove a custom option type.
        /// </summary>
        bool RemoveOption(DhcpOption option);

        /// <summary>
        ///     Converts dhcp message into a byte array.
        /// </summary>
        byte[] ToArray();

       
    }
}