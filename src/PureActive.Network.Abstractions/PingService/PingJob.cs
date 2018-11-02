using System;
using System.Net;
using System.Text;
using PureActive.Network.Abstractions.Types;

namespace PureActive.Network.Abstractions.PingService
{
    public struct PingJob
    {
        public Guid JobGuid;
        public int TaskId;
        public DateTimeOffset Timestamp;
        public IPAddressSubnet IPAddressSubnet;

        public PingJob(Guid jobGuid, int taskId, IPAddressSubnet iPAddressSubnet, DateTimeOffset timestamp)
        {
            JobGuid = jobGuid;
            TaskId = taskId;
            IPAddressSubnet = iPAddressSubnet;
            Timestamp = timestamp;
        }

        public int NextTask(IPAddress ipAddress)
        {
            IPAddressSubnet = new IPAddressSubnet(ipAddress, IPAddressSubnet.SubnetMask);
            return ++TaskId;
        }

        public byte[] ToBuffer()
        {
            byte[] buffer = Encoding.ASCII.GetBytes("abcdefghijklmnopqrstuvwxyz012345");

            return buffer;
        }
    }
}
