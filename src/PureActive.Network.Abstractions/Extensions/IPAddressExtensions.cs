using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Network.Abstractions.Types;

namespace PureActive.Network.Abstractions.Extensions
{
    public static class IPAddressExtensions
    {
        public const string StringSubnetClassA = "255.0.0.0";
        public const string StringSubnetClassB = "255.255.0.0";
        public const string StringSubnetClassC = "255.255.255.0";
        public static readonly IPAddress SubnetClassA = IPAddress.Parse(StringSubnetClassA);
        public static readonly IPAddress SubnetClassB = IPAddress.Parse(StringSubnetClassB);
        public static readonly IPAddress SubnetClassC = IPAddress.Parse(StringSubnetClassC);

        public static IPAddressSubnet GetDefaultLocalAddressSubnet(IPureLogger logger = null)
        {
            try
            {
                foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
                {
                    // Is the network up and have a Network Gateway
                    if (networkInterface.OperationalStatus == OperationalStatus.Up &&
                        networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                    {
                        if (networkInterface.GetIPProperties().GatewayAddresses.IPv4OrDefault() == null)
                            continue;

                        foreach (var ipAddress in networkInterface.GetIPProperties().UnicastAddresses)
                        {
                            if (ipAddress.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                return new IPAddressSubnet(ipAddress.Address, ipAddress.IPv4Mask);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "GetDefaultLocalAddressSubnet failed");
            }

            return new IPAddressSubnet(IPAddress.None, IPAddress.None);
        }

        public static IPAddress GetDefaultLocalAddress(IPureLogger logger = null) =>
            GetDefaultLocalAddressSubnet(logger)?.IPAddress;

        public static IPAddress GetDefaultLocalSubnet(IPureLogger logger = null) =>
            GetDefaultLocalAddressSubnet(logger)?.SubnetMask;


        public static IPAddressSubnet GetDefaultLocalNetworkAddressSubnet(IPureLogger logger = null)
        {
            var localIPAddressSubnet = GetDefaultLocalAddressSubnet(logger);

            return new IPAddressSubnet(localIPAddressSubnet.IPAddress.GetNetworkAddress(),
                localIPAddressSubnet.SubnetMask);
        }

        public static IPAddress GetDefaultLocalNetworkAddress(IPureLogger logger = null) =>
            GetDefaultLocalNetworkAddressSubnet(logger)?.IPAddress;

        public static IPAddress GetDefaultLocalNetworkSubnet(IPureLogger logger = null) =>
            GetDefaultLocalNetworkAddressSubnet(logger)?.SubnetMask;

        public static IPAddressSubnet GetDefaultGatewayAddressSubnet(IPureLogger logger = null)
        {
            try
            {
                foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
                {
                    // Is the network up and have a Network Gateway
                    if (networkInterface.OperationalStatus == OperationalStatus.Up &&
                        networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                    {
                        var defaultGateway = networkInterface.GetIPProperties().GatewayAddresses.IPv4OrDefault();

                        if (defaultGateway == null)
                            continue;

                        if (defaultGateway.Address.AddressFamily != AddressFamily.InterNetwork)
                        {
                            logger?.LogError("Default Gateway {GatewayIPAddress} is not IPv4", defaultGateway.Address);
                        }

                        foreach (var ipAddress in networkInterface.GetIPProperties().UnicastAddresses)
                        {
                            if (ipAddress.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                return new IPAddressSubnet(defaultGateway.Address, ipAddress.IPv4Mask);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "GetDefaultGatewayAddress failed");
            }

            return IPAddressSubnet.None;
        }

        public static IPAddress GetDefaultGatewayAddress(IPureLogger logger = null) =>
            GetDefaultGatewayAddressSubnet(logger).IPAddress;

        public static IPAddress GetDefaultGatewaySubnet(IPureLogger logger = null) =>
            GetDefaultGatewayAddressSubnet(logger).SubnetMask;


        /// <summary>
        ///     Returns broadcast address given an IPAddress and subnet Mask
        /// </summary>
        /// <param name="address"></param>
        /// <param name="subnetMask"></param>
        /// <returns></returns>
        public static IPAddress GetBroadcastAddress(this IPAddress address, IPAddress subnetMask)
        {
            var ipAddressBytes = address.GetAddressBytes();
            var subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAddressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match!");

            var broadcastAddress = new byte[ipAddressBytes.Length];

            for (var i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte) (ipAddressBytes[i] | (subnetMaskBytes[i] ^ 255));
            }

            return new IPAddress(broadcastAddress);
        }

        public static IPAddress GetBroadcastAddress(this IPAddress address) =>
            GetBroadcastAddress(address, SubnetClassC);


        /// <summary>
        ///     Returns Network Address given and IPAddress and subnetMask. Used to compare
        ///     two IPAddresses to see if they are on the same subnet
        /// </summary>
        /// <param name="address"></param>
        /// <param name="subnetMask"></param>
        /// <returns></returns>
        public static IPAddress GetNetworkAddress(this IPAddress address, IPAddress subnetMask)
        {
            var ipAddressBytes = address.GetAddressBytes();
            var subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAddressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match!");

            var networkAddress = new byte[ipAddressBytes.Length];

            for (var i = 0; i < networkAddress.Length; i++)
            {
                networkAddress[i] = (byte) (ipAddressBytes[i] & subnetMaskBytes[i]);
            }

            return new IPAddress(networkAddress);
        }

        public static IPAddress GetNetworkAddress(this IPAddress address) => GetNetworkAddress(address, SubnetClassC);

        public static bool IsAddressOnSameSubnet(this IPAddress address2, IPAddress address, IPAddress subnetMask)
        {
            IPAddress network1 = address.GetNetworkAddress(subnetMask);
            IPAddress network2 = address2.GetNetworkAddress(subnetMask);

            return network1.Equals(network2);
        }

        public static long ToLong(this IPAddress address) => BitConverter.ToUInt32(address.GetAddressBytes(), 0);

        public static long ToLongBackwards(this IPAddress address)
        {
            byte[] byteIP = address.GetAddressBytes();

            uint ip = (uint) byteIP[0] << 24;
            ip += (uint) byteIP[1] << 16;
            ip += (uint) byteIP[2] << 8;
            ip += byteIP[3];

            return ip;
        }

        public static IPAddress Increment(this IPAddress address)
        {
            byte[] bytes = address.GetAddressBytes();

            for (int k = bytes.Length - 1; k >= 0; k--)
            {
                if (bytes[k] == byte.MaxValue)
                {
                    bytes[k] = 0;
                    continue;
                }

                bytes[k]++;

                return new IPAddress(bytes);
            }

            // Un-incrementable, return the original address.
            return address;
        }


        public static IPAddress Decrement(this IPAddress address)
        {
            byte[] bytes = address.GetAddressBytes();

            for (int k = bytes.Length - 1; k >= 0; k--)
            {
                if (bytes[k] == byte.MinValue)
                {
                    bytes[k] = byte.MaxValue;
                    continue;
                }

                bytes[k]--;

                return new IPAddress(bytes);
            }

            // Un-decrementable, return the original address.
            return address;
        }

        public static int CompareTo(this IPAddress x, IPAddress y)
        {
            var result = x.AddressFamily.CompareTo(y.AddressFamily);

            if (result != 0)
                return result;

            var xBytes = x.GetAddressBytes();
            var yBytes = y.GetAddressBytes();

            var octets = Math.Min(xBytes.Length, yBytes.Length);

            for (var i = 0; i < octets; i++)
            {
                var octetResult = xBytes[i].CompareTo(yBytes[i]);
                if (octetResult != 0)
                    return octetResult;
            }

            return 0;
        }
    }
}