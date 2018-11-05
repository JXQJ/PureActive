using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using PureActive.Core.Extensions;
using PureActive.Core.Utilities;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Logging.Extensions.Types;
using PureActive.Network.Abstractions.DhcpService.Interfaces;
using PureActive.Network.Abstractions.DhcpService.Types;
using PureActive.Network.Abstractions.Extensions;
using PureActive.Network.Abstractions.Types;
using PureActive.Network.Extensions.IO;
using PureActive.Network.Services.DhcpService.Types;

namespace PureActive.Network.Services.DhcpService.Message
{
    #region RFC Specification

    /* Description:  Object that represents a DHCPv4 message as defined in
    *               RFC 2131.
    *               
    *  The following diagram illustrates the format of DHCP messages sent
    *  between server and clients:
    *
    *    0                   1                   2                   3
    *    0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
    *   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    *   |     op (1)    |   htype (1)   |   hlen (1)    |   hops (1)    |
    *   +---------------+---------------+---------------+---------------+
    *   |                            xid (4)                            |
    *   +-------------------------------+-------------------------------+
    *   |           secs (2)            |           flags (2)           |
    *   +-------------------------------+-------------------------------+
    *   |                          ciaddr  (4)                          |
    *   +---------------------------------------------------------------+
    *   |                          yiaddr  (4)                          |
    *   +---------------------------------------------------------------+
    *   |                          siaddr  (4)                          |
    *   +---------------------------------------------------------------+
    *   |                          giaddr  (4)                          |
    *   +---------------------------------------------------------------+
    *   |                                                               |
    *   |                          chaddr  (16)                         |
    *   |                                                               |
    *   |                                                               |
    *   +---------------------------------------------------------------+
    *   |                                                               |
    *   |                          sname   (64)                         |
    *   +---------------------------------------------------------------+
    *   |                                                               |
    *   |                          file    (128)                        |
    *   +---------------------------------------------------------------+
    *   |                                                               |
    *   |                          options (variable)                   |
    *   +---------------------------------------------------------------+
    */

    #endregion RFC Specification

    /// <summary>
    /// A class that represents a DHCP packet.
    /// See http://www.faqs.org/rfcs/rfc2131.html for full details of protocol.
    /// </summary>
    public class DhcpMessage : LoggableBase<DhcpMessage>, IDhcpMessage
    {
        #region Private Properties

        private static readonly Random Random = new Random();

        private byte _op;
        private byte _htype;
        private byte _hlen;
        private byte _hops;
        private uint _xid;
        private ushort _secs;
        private byte[] _flags = new byte[2];
        private byte[] _ciaddr = new byte[4];
        private byte[] _yiaddr = new byte[4];
        private byte[] _siaddr = new byte[4];
        private byte[] _giaddr = new byte[4];
        private byte[] _chaddr = new byte[16];
        private byte[] _sname = new byte[64];
        private byte[] _file = new byte[128];

        private Dictionary<DhcpOption, byte[]> _options = new Dictionary<DhcpOption, byte[]>();
        private byte[] _optionOrdering = new Byte[] { };
        private int _optionDataSize;

        #endregion Private Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DhcpMessage"/> class.
        /// </summary>
        public DhcpMessage(IPureLoggerFactory loggerFactory, IPureLogger logger = null) :
            base (loggerFactory, logger)
        {
            CreatedTimestamp = DateTimeOffset.Now;

            _op = (byte)OperationCode.BootReply;
            _htype = (byte)HardwareType.Ethernet;
            _hlen = 0;
            _hops = 0;
            _xid = (ushort)Random.Next(ushort.MaxValue);
            _secs = 0;
            Array.Clear(_flags, 0, _flags.Length);
            Array.Clear(_ciaddr, 0, _ciaddr.Length);
            Array.Clear(_yiaddr, 0, _yiaddr.Length);
            Array.Clear(_siaddr, 0, _siaddr.Length);
            Array.Clear(_giaddr, 0, _giaddr.Length);
            Array.Clear(_chaddr, 0, _chaddr.Length);
            Array.Clear(_sname, 0, _sname.Length);
            Array.Clear(_file, 0, _file.Length);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DhcpMessage"/> class.
        /// <param name="data">Array containing the data to decode.</param>
        /// </summary>
        public DhcpMessage(byte[] data, IPureLoggerFactory loggerFactory, IPureLogger logger = null)
            :base(loggerFactory, logger)
        {
            ByteReader byteReader = new ByteReader(data, ByteOrder.Network);

            CreatedTimestamp = DateTimeOffset.Now;

            _op = byteReader.ReadByte();
            _htype = byteReader.ReadByte();
            _hlen = byteReader.ReadByte();
            _hops = byteReader.ReadByte();
            _xid = byteReader.ReadUInt32();
            _secs = byteReader.ReadUInt16();
            _flags = byteReader.ReadBytes(2);
            _ciaddr = byteReader.ReadBytes(4);
            _yiaddr = byteReader.ReadBytes(4);
            _siaddr = byteReader.ReadBytes(4);
            _giaddr = byteReader.ReadBytes(4);
            _chaddr = byteReader.ReadBytes(16);
            _sname = byteReader.ReadBytes(64);
            _file = byteReader.ReadBytes(128);

            byte[] rawOptions = byteReader.ReadBytes(byteReader.AvailableBytes);
            int offset = 0;

            // if magic number is valid then process options
            if (offset + 4 < rawOptions.Length &&
                (BitConverter.ToUInt32(rawOptions, offset) == DhcpConstants.DhcpOptionsMagicNumber
                || BitConverter.ToUInt32(rawOptions, offset) == DhcpConstants.DhcpWinOptionsMagicNumber))
            {
                offset += 4;
                Boolean end = false;

                while (!end && offset < rawOptions.Length)
                {
                    DhcpOption option = (DhcpOption)rawOptions[offset];
                    offset++;

                    int optionLen;

                    switch (option)
                    {
                        case DhcpOption.Pad:
                            continue;
                        case DhcpOption.End:
                            end = true;
                            continue;
                        default:
                            optionLen = rawOptions[offset];
                            offset++;
                            break;
                    }

                    byte[] optionData = new byte[optionLen];

                    Array.Copy(rawOptions, offset, optionData, 0, optionLen);
                    offset += optionLen;

                    _options.Add(option, optionData);
                    _optionDataSize += optionLen;
                }
            }
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// The timestamp when cached.
        /// </summary>
        public DateTimeOffset CreatedTimestamp { get; set; }

        ///<summary>
        /// The operation code of whatever last altered the packet (op).
        ///</summary>
        public OperationCode Operation
        {
            get { return (OperationCode)_op; }
            set { _op = (byte)value; }
        }

        ///<summary>
        /// The hardware address type (htype).
        ///</summary>
        public HardwareType Hardware
        {
            get { return (HardwareType)_htype; }
            set { _htype = (byte)value; }
        }

        ///<summary>
        /// The hardware address length (hlen).
        ///</summary>
        public byte HardwareAddressLength
        {
            get { return _hlen; }
            set { _hlen = value; }
        }

        ///<summary>
        ///  Optionally used by relay agents when booting via a relay agent (hops).
        ///</summary>
        public byte Hops
        {
            get { return _hops; }
            set { _hops = value; }
        }

        ///<summary>
        /// A random number chosen by the client, to associate messages and responses between a client and a server (xid).
        ///</summary>
        public uint SessionId
        {
            get { return _xid; }
            set { _xid = value; }
        }

        ///<summary>
        /// The seconds elapsed since client began address acquisition or renewal process (secs).
        ///</summary>
        public ushort SecondsElapsed
        {
            get { return _secs; }
            set { _secs = value; }
        }

        ///<summary>
        /// The leftmost bit is defined as the BROADCAST (B) flag.   The remaining bits of the flags field are reserved for future use.
        ///</summary>
        public byte[] Flags
        {
            get { return _flags; }
            set { _flags = value; }
        }

        /// <summary>
        /// Is Broadcast(true) / Unicast(false) flag 
        /// </summary>
        public bool IsBroadcast
        {
            get => ByteUtility.GetBits(_flags[0], 7, 1) == 1;
            set
            {
                if (value)
                {
                    _flags[0] = ByteUtility.SetBits(ref _flags[0], 15, 1, 1);
                }
                else
                {
                    _flags[0] = ByteUtility.SetBits(ref _flags[0], 15, 1, 0);
                }
            }
        }

        ///<summary>
        /// The client IP address (ciaddr).
        ///</summary>
        public InternetAddress ClientAddress
        {
            get { return new InternetAddress(_ciaddr); }
            set { CopyData(value.ToArray(), _ciaddr); }
        }

        ///<summary>
        /// The assigned client IP address (yiaddr).
        ///</summary>
        public InternetAddress AssignedAddress
        {
            get { return new InternetAddress(_yiaddr); }
            set { CopyData(value.ToArray(), _yiaddr); }
        }

        ///<summary>
        /// The server IP address (siaddr).
        ///</summary>
        public InternetAddress NextServerAddress
        {
            get { return new InternetAddress(_siaddr); }
            set { CopyData(value.ToArray(), _siaddr); }
        }

        ///<summary>
        /// The gateway IP address (giaddr);
        ///</summary>
        public InternetAddress RelayAgentAddress
        {
            get { return new InternetAddress(_giaddr); }
            set { CopyData(value.ToArray(), _giaddr); }
        }

        ///<summary>
        /// The client hardware address (chaddr).
        ///</summary>
        public PhysicalAddress ClientHardwareAddress
        {
            get
            {
                Byte[] hardwareAddress = new Byte[_hlen];
                Array.Copy(_chaddr, hardwareAddress, _hlen);
                return new PhysicalAddress(hardwareAddress);
            }

            set
            {
                CopyData(value.ToArray(), _chaddr);
                _hlen = (Byte)value.ToArray().Length;

                for (Int32 i = value.ToArray().Length; i < 16; i++)
                {
                    _chaddr[i] = 0;
                }
            }
        }

        /// <summary>
        /// The DNS hostname part that corresponds with the given IP address (sname).
        /// </summary>
        public byte[] ServerName
        {
            get { return _sname; }
            set { CopyData(value, _sname); }
        }

        /// <summary>
        /// The boot file name the clients may use (file).
        /// </summary>
        public byte[] BootFileName
        {
            get { return _file; }
            set { CopyData(value, BootFileName); }
        }

        /// <summary>
        /// The option ordering parameter list.
        /// </summary>
        public byte[] OptionOrdering
        {
            get { return _optionOrdering; }
            set { _optionOrdering = value; }
        }

        #endregion Public Properties

        #region Methods

        /// <summary>
        /// Get option data.
        /// </summary> 
        public byte[] GetOptionData(DhcpOption option)
        {
            if (_options.ContainsKey(option))
            {
                return _options[option];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Remove all options.
        /// </summary> 
        public void ClearOptions()
        {
            _optionDataSize = 0;
            _options.Clear();
        }

        /// <summary>
        /// Add or define a new custom option type.
        /// </summary>   
        public void AddOption(DhcpOption option, params Byte[] data)
        {
            if (option == DhcpOption.Pad || option == DhcpOption.End)
            {
                throw new ArgumentException("The Pad and End DhcpOptions cannot be added explicitly", "option");
            }

            _options.Add(option, data);
            _optionDataSize += data.Length;
        }

        public bool ParamOptionExists(DhcpOption paramOption)
        {
            var paramOptions = GetOptionData(DhcpOption.ParamReqList);

            if (paramOptions == null) return false;

            foreach (var paramOptionItem in paramOptions)
            {
                if (paramOptionItem == (byte)paramOption)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Remove a custom option type.
        /// </summary>  
        public bool RemoveOption(DhcpOption option)
        {
            if (_options.ContainsKey(option))
            {
                byte[] optionValue = _options[option];
                _optionDataSize -= optionValue.Length;
                _options.Remove(option);
                return true;
            }
            return false;
        }

        public IEnumerable<DhcpOption> DhcpOptionKeys() => _options.Keys;
        public IEnumerable<byte[]> DhcpOptionValues() => _options.Values;

        /// <summary>
        /// Converts dhcp message into a byte array.
        /// </summary>
        public byte[] ToArray()
        {
            ByteWriter byteWriter = new ByteWriter(DhcpConstants.DhcpMinMessageSize, ByteOrder.Network);

            byteWriter.Write(_op);
            byteWriter.Write(_htype);
            byteWriter.Write(_hlen);
            byteWriter.Write(_hops);
            byteWriter.Write(_xid);
            byteWriter.Write(_secs); // (ReverseByteOrder(BitConverter.GetBytes(this._secs))) ??
            byteWriter.Write(_flags);
            byteWriter.Write(_ciaddr);
            byteWriter.Write(_yiaddr);
            byteWriter.Write(_siaddr);
            byteWriter.Write(_giaddr);
            byteWriter.Write(_chaddr);
            byteWriter.Write(_sname);
            byteWriter.Write(_file);

            byte[] data = new byte[(_options.Count > 0 ? 4 + _options.Count * 2 + _optionDataSize + 1 : 0)];

            int offset = 0;
            if (_options.Count > 0)
            {
                BitConverter.GetBytes(DhcpConstants.DhcpWinOptionsMagicNumber).CopyTo(data, offset);
                offset += 4;

                foreach (Byte optionId in _optionOrdering)
                {
                    DhcpOption option = (DhcpOption)optionId;
                    if (_options.ContainsKey(option))
                    {
                        data[offset++] = optionId;
                        byte[] optionValue = _options[option];

                        if (_options[option] != null && optionValue.Length > 0)
                        {
                            data[offset++] = (Byte)optionValue.Length;
                            Array.Copy(_options[option], 0, data, offset, optionValue.Length);
                            offset += optionValue.Length;
                        }
                    }
                }

                foreach (DhcpOption option in _options.Keys)
                {
                    if (Array.IndexOf(_optionOrdering, (Byte)option) == -1)
                    {
                        data[offset++] = (Byte)option;
                        byte[] optionValue = _options[option];

                        if (_options[option] != null && optionValue.Length > 0)
                        {
                            data[offset++] = (Byte)optionValue.Length;
                            Array.Copy(_options[option], 0, data, offset, optionValue.Length);
                            offset += optionValue.Length;
                        }
                    }
                }

                data[offset] = (Byte)DhcpOption.End;
            }
            byteWriter.Write(data);

            return byteWriter.GetBytes();
        }
        
        private string GetDhcpOptionString(DhcpOption dhcpOption, DhcpOptionTypeMap.DhcpRequestListFormat dhcpRequestListFormat = DhcpOptionTypeMap.DhcpRequestListFormat.StringNewlineIndentedSeparated)
        {
            return DhcpOptionTypeMap.GetDhcpOptionString(dhcpOption, GetOptionData(dhcpOption), dhcpRequestListFormat, Logger);
        }

        // TODO: ILogPropertyLevel
        //public override IEnumerable<ILogPropertyLevel> GetLogPropertyListLevel(LogLevel logLevel, LoggableFormat loggableFormat)
        //{
        //    var logPropertyLevels = loggableFormat.IsWithParents()
        //        ? base.GetLogPropertyListLevel(logLevel, loggableFormat)?.ToList()
        //        : new List<ILogPropertyLevel>();

        //    if (logLevel <= LogLevel.Information)
        //    {
        //        logPropertyLevels?.Add(new LogPropertyLevel("MessageTimeStamp", CreatedTimestamp.ToLocalTime(), LogLevel.Information));
        //        logPropertyLevels?.Add(new LogPropertyLevel("MessageOp", OperationString.GetName(this.Operation), LogLevel.Information));
        //        logPropertyLevels?.Add(new LogPropertyLevel("HardwareType)", HardwareString.GetName(this.Hardware), LogLevel.Trace));
        //        logPropertyLevels?.Add(new LogPropertyLevel("Hops", ByteUtility.PrintByte(this.Hops), LogLevel.Trace));
        //        logPropertyLevels?.Add(new LogPropertyLevel("TrxId", this.SessionId.ToHexString("0x"), LogLevel.Information));
        //        logPropertyLevels?.Add(new LogPropertyLevel("SecondsElapsed", this.SecondsElapsed.ToString(), LogLevel.Trace));
        //        logPropertyLevels?.Add(new LogPropertyLevel("Flags", ByteUtility.PrintBytes(this._flags), LogLevel.Trace));
        //        logPropertyLevels?.Add(new LogPropertyLevel("IsBroadcast", this.IsBroadcast.ToString(), LogLevel.Trace));
        //        logPropertyLevels?.Add(new LogPropertyLevel("ClientAddress", this.ClientAddress.ToString(), LogLevel.Debug));
        //        logPropertyLevels?.Add(new LogPropertyLevel("AssignedAddress", this.AssignedAddress.ToString(), LogLevel.Debug));
        //        logPropertyLevels?.Add(new LogPropertyLevel("NextServerAddress", this.NextServerAddress.ToString(), LogLevel.Trace));
        //        logPropertyLevels?.Add(new LogPropertyLevel("RelayAgentAddress", this.RelayAgentAddress.ToString(), LogLevel.Trace));
        //        logPropertyLevels?.Add(new LogPropertyLevel("HardwareAddress", this.ClientHardwareAddress.ToString(), LogLevel.Information));
        //        logPropertyLevels?.Add(new LogPropertyLevel("ServerName", ByteUtility.GetSafeString(this.ServerName), LogLevel.Trace));
        //        logPropertyLevels?.Add(new LogPropertyLevel("BootFileName", ByteUtility.GetSafeString(this.BootFileName), LogLevel.Trace));

        //        logPropertyLevels?.Add(new LogPropertyLevel("OptMessageType", GetDhcpOptionString(DhcpOption.MessageType, DhcpOptionTypeMap.DhcpRequestListFormat.StringNewlineIndentedSeparated), LogLevel.Information));
        //        logPropertyLevels?.Add(new LogPropertyLevel("OptClientId", GetDhcpOptionString(DhcpOption.ClientId, DhcpOptionTypeMap.DhcpRequestListFormat.StringNewlineIndentedSeparated), LogLevel.Trace));
        //        logPropertyLevels?.Add(new LogPropertyLevel("OptVendorClassId", GetDhcpOptionString(DhcpOption.VendorClassId, DhcpOptionTypeMap.DhcpRequestListFormat.StringNewlineIndentedSeparated), LogLevel.Trace));
        //        logPropertyLevels?.Add(new LogPropertyLevel("OptHostName", GetDhcpOptionString(DhcpOption.Hostname, DhcpOptionTypeMap.DhcpRequestListFormat.StringNewlineIndentedSeparated), LogLevel.Information));
        //        logPropertyLevels?.Add(new LogPropertyLevel("OptAddressRequest", GetDhcpOptionString(DhcpOption.RequestedIpAddr, DhcpOptionTypeMap.DhcpRequestListFormat.StringNewlineIndentedSeparated), LogLevel.Information));
        //        logPropertyLevels?.Add(new LogPropertyLevel("OptServerIdentifier", GetDhcpOptionString(DhcpOption.ServerId, DhcpOptionTypeMap.DhcpRequestListFormat.StringNewlineIndentedSeparated), LogLevel.Trace));
        //        logPropertyLevels?.Add(new LogPropertyLevel("OptParamReqList", GetDhcpOptionString(DhcpOption.ParamReqList, DhcpOptionTypeMap.DhcpRequestListFormat.StringNewlineIndentedSeparated), LogLevel.Trace));
        //    }

        //    return logPropertyLevels?.Where(p => p.MinimumLogLevel.CompareTo(logLevel) >= 0);
        //}
        
        private void FormatDhcpOptionColon(StringBuilder sb, DhcpOption dhcpOption, string dhcpOptionString, int maxLength = 0)
        {
            sb.Append(maxLength > 0
                ? $"{dhcpOption.ToString().PadWithDelim(": ", maxLength)}{dhcpOptionString}"
                : $"{dhcpOption.ToString()}: {dhcpOptionString}");
        }

        /// <summary>
        /// Copy byte array object.
        /// </summary>
        private void CopyData(byte[] source, byte[] dest)
        {
            if (source.Length > dest.Length)
            {
                throw new ArgumentException("Values must be no more than " + dest.Length + " bytes.");
            }

            source.CopyTo(dest, 0);
        }

        #endregion Methods
    }
}
