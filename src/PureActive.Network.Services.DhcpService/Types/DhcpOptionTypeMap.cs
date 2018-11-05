using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using Microsoft.Extensions.Logging;
using PureActive.Core.Utilities;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Network.Abstractions.DhcpService.Types;
using PureActive.Network.Abstractions.Extensions;

namespace PureActive.Network.Services.DhcpService.Types
{
    public static class DhcpOptionTypeMap
    {
        public static Dictionary<DhcpOption, DhcpOptionType> DhcpOptionTypes => DhcpOptionTypesReadOnly.Value;

        private static readonly Lazy<Dictionary<DhcpOption, DhcpOptionType>> DhcpOptionTypesReadOnly =
            new Lazy<Dictionary<DhcpOption, DhcpOptionType>>(
                () => new Dictionary<DhcpOption, DhcpOptionType>()
                {
                    {DhcpOption.Pad, DhcpOptionType.SafeBytes},
                    {DhcpOption.NetMask, DhcpOptionType.SafeBytes},
                    {DhcpOption.TimeOffset, DhcpOptionType.SafeBytes},
                    {DhcpOption.Router, DhcpOptionType.SafeBytes},
                    {DhcpOption.TimeServer, DhcpOptionType.SafeBytes},
                    {DhcpOption.NameServer, DhcpOptionType.SafeBytes},
                    {DhcpOption.Dns, DhcpOptionType.SafeBytes},
                    {DhcpOption.LogServer, DhcpOptionType.SafeBytes},
                    {DhcpOption.CookieServer, DhcpOptionType.SafeBytes},
                    {DhcpOption.LprServer, DhcpOptionType.SafeBytes},
                    {DhcpOption.ImpressServer, DhcpOptionType.SafeBytes},
                    {DhcpOption.ReslocServer, DhcpOptionType.SafeBytes},
                    {DhcpOption.Hostname, DhcpOptionType.SafeString},
                    {DhcpOption.BootFileSize, DhcpOptionType.SafeBytes},
                    {DhcpOption.MeritDump, DhcpOptionType.SafeBytes},
                    {DhcpOption.DomainName, DhcpOptionType.SafeString},
                    {DhcpOption.SwapServer, DhcpOptionType.SafeBytes},
                    {DhcpOption.RootPath, DhcpOptionType.SafeBytes},
                    {DhcpOption.ExtsPath, DhcpOptionType.SafeBytes},
                    {DhcpOption.IpForward, DhcpOptionType.SafeBytes},
                    {DhcpOption.NonLocalsr, DhcpOptionType.SafeBytes},
                    {DhcpOption.PolicyFilter, DhcpOptionType.SafeBytes},
                    {DhcpOption.MaxReassemble, DhcpOptionType.SafeBytes},
                    {DhcpOption.IpTtl, DhcpOptionType.SafeBytes},
                    {DhcpOption.PathMtuAging, DhcpOptionType.SafeBytes},
                    {DhcpOption.PathMtuPlateau, DhcpOptionType.SafeBytes},
                    {DhcpOption.InterfaceMtu, DhcpOptionType.SafeBytes},
                    {DhcpOption.SubnetsLocal, DhcpOptionType.SafeBytes},
                    {DhcpOption.BCastAddress, DhcpOptionType.SafeBytes},
                    {DhcpOption.MaskDiscovery, DhcpOptionType.SafeBytes},
                    {DhcpOption.MaskSupplier, DhcpOptionType.SafeBytes},
                    {DhcpOption.RouterDiscovery, DhcpOptionType.SafeBytes},
                    {DhcpOption.RouterSolic, DhcpOptionType.SafeBytes},
                    {DhcpOption.StaticRoute, DhcpOptionType.SafeBytes},
                    {DhcpOption.TrailerEncaps, DhcpOptionType.SafeBytes},
                    {DhcpOption.ArpTimeout, DhcpOptionType.SafeBytes},
                    {DhcpOption.EthernetEncaps, DhcpOptionType.SafeBytes},
                    {DhcpOption.TcpTtl, DhcpOptionType.SafeBytes},
                    {DhcpOption.TcpKeepAliveInt, DhcpOptionType.SafeBytes},
                    {DhcpOption.TcpKeepAliveGrbg, DhcpOptionType.SafeBytes},
                    {DhcpOption.NisDomain, DhcpOptionType.SafeBytes},
                    {DhcpOption.NisServers, DhcpOptionType.SafeBytes},
                    {DhcpOption.NtpServers, DhcpOptionType.SafeBytes},
                    {DhcpOption.VendorSpecific, DhcpOptionType.SafeBytes},
                    {DhcpOption.NetBiosNameServ, DhcpOptionType.SafeBytes},
                    {DhcpOption.NetBiosDgDist, DhcpOptionType.SafeBytes},
                    {DhcpOption.NetBiosNodeType, DhcpOptionType.SafeBytes},
                    {DhcpOption.NetBiosScope, DhcpOptionType.SafeBytes},
                    {DhcpOption.X11Fonts, DhcpOptionType.SafeBytes},
                    {DhcpOption.X11DisplayMngr, DhcpOptionType.SafeBytes},
                    {DhcpOption.RequestedIpAddr, DhcpOptionType.IPAddress},
                    {DhcpOption.IpAddrLease, DhcpOptionType.UInt32},
                    {DhcpOption.Overload, DhcpOptionType.SafeBytes},
                    {DhcpOption.MessageType, DhcpOptionType.MessageTypeString},
                    {DhcpOption.ServerId, DhcpOptionType.SafeBytes},
                    {DhcpOption.ParamReqList, DhcpOptionType.SafeBytes},
                    {DhcpOption.Message, DhcpOptionType.SafeBytes},
                    {DhcpOption.MaxDhcpMsgSize, DhcpOptionType.UInt16},
                    {DhcpOption.RenewalTime, DhcpOptionType.SafeBytes},
                    {DhcpOption.RebindingTime, DhcpOptionType.SafeBytes},
                    {DhcpOption.VendorClassId, DhcpOptionType.SafeString},
                    {DhcpOption.ClientId, DhcpOptionType.PhysicalAddressSkip1ColonString},
                    {DhcpOption.NetwareIpDomain, DhcpOptionType.SafeBytes},
                    {DhcpOption.NetwareIpOption, DhcpOptionType.SafeBytes},
                    {DhcpOption.NisPlusDomain, DhcpOptionType.SafeBytes},
                    {DhcpOption.NisPlusServers, DhcpOptionType.SafeBytes},
                    {DhcpOption.TftpServer, DhcpOptionType.SafeBytes},
                    {DhcpOption.BootFile, DhcpOptionType.SafeBytes},
                    {DhcpOption.MobileIpHome, DhcpOptionType.SafeBytes},
                    {DhcpOption.SmtpServer, DhcpOptionType.SafeBytes},
                    {DhcpOption.Pop3Server, DhcpOptionType.SafeBytes},
                    {DhcpOption.NntpServer, DhcpOptionType.SafeBytes},
                    {DhcpOption.WwwServer, DhcpOptionType.SafeBytes},
                    {DhcpOption.FingerServer, DhcpOptionType.SafeBytes},
                    {DhcpOption.IrcServer, DhcpOptionType.SafeBytes},
                    {DhcpOption.StServer, DhcpOptionType.SafeBytes},
                    {DhcpOption.StdaServer, DhcpOptionType.SafeBytes},
                    {DhcpOption.UserClass, DhcpOptionType.SafeBytes},
                    {DhcpOption.SlpDirAgent, DhcpOptionType.SafeBytes},
                    {DhcpOption.SlpDirScope, DhcpOptionType.SafeBytes},
                    {DhcpOption.ClientFqdn, DhcpOptionType.SafeString},
                    {DhcpOption.RelayAgentFnfo, DhcpOptionType.SafeBytes},
                    {DhcpOption.ISns, DhcpOptionType.SafeBytes},
                    {DhcpOption.NdsServers, DhcpOptionType.SafeBytes},
                    {DhcpOption.NdsTreeName, DhcpOptionType.SafeBytes},
                    {DhcpOption.NdsContext, DhcpOptionType.SafeBytes},
                    {DhcpOption.Authentication, DhcpOptionType.SafeBytes},
                    {DhcpOption.ClientSystem, DhcpOptionType.SafeBytes},
                    {DhcpOption.ClientNdi, DhcpOptionType.SafeBytes},
                    {DhcpOption.Ldap, DhcpOptionType.SafeBytes},
                    {DhcpOption.UuidGuid, DhcpOptionType.SafeBytes},
                    {DhcpOption.UserAuth, DhcpOptionType.SafeBytes},
                    {DhcpOption.PCode, DhcpOptionType.SafeBytes},
                    {DhcpOption.TCode, DhcpOptionType.SafeBytes},
                    {DhcpOption.NetInfoAddress, DhcpOptionType.SafeBytes},
                    {DhcpOption.NetInfoTag, DhcpOptionType.SafeBytes},
                    {DhcpOption.Url, DhcpOptionType.SafeBytes},
                    {DhcpOption.AutoConfig, DhcpOptionType.SafeBytes},
                    {DhcpOption.NameServiceSearch, DhcpOptionType.SafeBytes},
                    {DhcpOption.SubNetSelection, DhcpOptionType.SafeBytes},
                    {DhcpOption.DomainSearch, DhcpOptionType.SafeBytes},
                    {DhcpOption.SipServersDhcp, DhcpOptionType.SafeBytes},
                    {DhcpOption.ClasslessStaticRoute, DhcpOptionType.SafeBytes},
                    {DhcpOption.Ccc, DhcpOptionType.SafeBytes},
                    {DhcpOption.GeoConf, DhcpOptionType.SafeBytes},
                    {DhcpOption.VIVendorClass, DhcpOptionType.SafeBytes},
                    {DhcpOption.VIVendorSpecific, DhcpOptionType.SafeBytes},
                    {DhcpOption.TfptServerIpAddress, DhcpOptionType.SafeBytes},
                    {DhcpOption.CallServerIpAddress, DhcpOptionType.SafeBytes},
                    {DhcpOption.DiscriminationString, DhcpOptionType.SafeBytes},
                    {DhcpOption.RemoteStatisticsServer, DhcpOptionType.SafeBytes},
                    {DhcpOption.W8021PVLanId, DhcpOptionType.SafeBytes},
                    {DhcpOption.W8021Ql2Priority, DhcpOptionType.SafeBytes},
                    {DhcpOption.DiffServCodePoint, DhcpOptionType.SafeBytes},
                    {DhcpOption.HttpProxyForPhoneSpec, DhcpOptionType.SafeBytes},
                    {DhcpOption.GeoLoc, DhcpOptionType.SafeBytes},
                    {DhcpOption.MsftClasslessStaticRoute, DhcpOptionType.SafeBytes},
                    {DhcpOption.Serial, DhcpOptionType.SafeBytes},
                    {DhcpOption.BpFile, DhcpOptionType.SafeBytes},
                    {DhcpOption.NextServer, DhcpOptionType.SafeBytes},
                    {DhcpOption.End, DhcpOptionType.SafeBytes},
                }
            );

        public enum DhcpRequestListFormat
        {
            StringCommaSeparated,
            StringNewlineSeparated,
            StringNewlineIndentedSeparated,
            HexValueCommaSeparated,
            HexValueSpaceSeparated,
            HexValueDashSeparated,
            DecimalValueCommaSeparated,
            DecimalValueSpaceSeparated
        }

        public static string GetDhcpRequestListString(byte[] dhcpOptionValue, DhcpRequestListFormat dhcpRequestListFormat, IPureLogger logger = null)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var requestOption in dhcpOptionValue)
            {
                DhcpOption dhcpRequestOption = (DhcpOption)requestOption;

                switch (dhcpRequestListFormat)
                {
                    case DhcpRequestListFormat.StringCommaSeparated:
                        sb.Append($"{dhcpRequestOption.ToString()}, ");
                        break;
                    case DhcpRequestListFormat.StringNewlineSeparated:
                        sb.Append($"{Environment.NewLine}{dhcpRequestOption.ToString()}");
                        break;
                    case DhcpRequestListFormat.StringNewlineIndentedSeparated:
                        sb.Append($"{Environment.NewLine}    {dhcpRequestOption.ToString()}");
                        break;
                    case DhcpRequestListFormat.HexValueCommaSeparated:
                        sb.Append($"{ByteUtility.ByteToHex((Byte)dhcpRequestOption)}, ");
                        break;
                    case DhcpRequestListFormat.HexValueSpaceSeparated:
                        sb.Append($"{ByteUtility.ByteToHex((Byte)dhcpRequestOption)} ");
                        break;
                    case DhcpRequestListFormat.HexValueDashSeparated:
                        sb.Append($"{ByteUtility.ByteToHex((Byte)dhcpRequestOption)}-");
                        break;
                    case DhcpRequestListFormat.DecimalValueCommaSeparated:
                        sb.Append($"{(Byte)dhcpRequestOption}, ");
                        break;
                    case DhcpRequestListFormat.DecimalValueSpaceSeparated:
                        sb.Append($"{(Byte)dhcpRequestOption} ");
                        break;
                }
            }
            
            // Remove trailing space
            if (sb.Length > 1)
            {
                if (sb[sb.Length - 1] == ' ' || sb[sb.Length - 1] == '-')
                    sb.Length--;
            }

            // Remove trailing comma
            if (sb.Length > 1)
            {
                if (sb[sb.Length - 1] == ',')
                    sb.Length--;
            }
        
            return sb.ToString();
        }

        public static string GetDhcpOptionString(
            DhcpOption dhcpOption, byte[] dhcpOptionValue, 
            DhcpRequestListFormat dhcpRequestListFormat = DhcpRequestListFormat.StringNewlineIndentedSeparated,
            IPureLogger logger = null
            )
        {
            try
            {
                if (dhcpOptionValue == null)
                    return "Null";

                if (dhcpOption == DhcpOption.ParamReqList)
                {
                    return GetDhcpRequestListString(dhcpOptionValue, dhcpRequestListFormat, logger);
                }

                switch (DhcpOptionTypes[dhcpOption])
                {
                    case DhcpOptionType.IPAddress:
                        return new IPAddress(dhcpOptionValue).ToString();
                    case DhcpOptionType.PhysicalAddressSkip1DashString:
                        return new PhysicalAddress(dhcpOptionValue.Skip(1).ToArray()).ToDashString();
                    case DhcpOptionType.PhysicalAddressSkip1ColonString:
                        return new PhysicalAddress(dhcpOptionValue.Skip(1).ToArray()).ToColonString();
                    case DhcpOptionType.PhysicalAddressDashString:
                        return new PhysicalAddress(dhcpOptionValue).ToDashString();
                    case DhcpOptionType.PhysicalAddressColonString:
                        return new PhysicalAddress(dhcpOptionValue).ToColonString();
                    case DhcpOptionType.MessageTypeString:
                        return MessageTypeString.GetName((MessageType) dhcpOptionValue[0]);
                    case DhcpOptionType.SafeString:
                        return ByteUtility.GetSafeString(dhcpOptionValue);
                    case DhcpOptionType.SafeBytes:
                        return ByteUtility.PrintSafeBytes(dhcpOptionValue);
                    case DhcpOptionType.UInt16:
                        return BitConverter.ToUInt16(dhcpOptionValue, 0).ToString();
                    case DhcpOptionType.UInt32:
                        return BitConverter.ToUInt32(dhcpOptionValue, 0).ToString();
                    case DhcpOptionType.UInt16Network:
                        return IPAddress.NetworkToHostOrder(BitConverter.ToUInt16(dhcpOptionValue, 0)).ToString();
                    case DhcpOptionType.UInt32Network:
                        return IPAddress.NetworkToHostOrder(BitConverter.ToUInt32(dhcpOptionValue, 0)).ToString();
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Invalid DhcpOption: {dhcpOption}", dhcpOption);
            }

            return string.Empty;
        }
    }
}