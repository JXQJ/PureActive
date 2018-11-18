using System;
using System.Net.NetworkInformation;
using Microsoft.Extensions.Logging;
using PureActive.Core.Utilities;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Network.Abstractions.DhcpService.Interfaces;
using PureActive.Network.Abstractions.DhcpService.Types;
using PureActive.Network.Abstractions.Types;

namespace PureActive.Network.Services.DhcpService.Session
{
    public class DhcpSession : IDhcpSession
    {
        private readonly IDhcpService _dhcpService;
        public IPureLogger Logger { get; private set; }

        public RequestState RequestState { get; set; } = RequestState.Unknown;

        public DhcpSessionState DhcpSessionState { get; set; } = DhcpSessionState.Init;

        public PhysicalAddress PhysicalAddress { get; }

        private readonly DhcpSessionResult _dhcpSessionResult;

        public IDhcpSessionResult DhcpSessionResult => _dhcpSessionResult;

        public IDhcpDiscoveredDevice DhcpDiscoveredDevice => DhcpSessionResult?.DhcpDiscoveredDevice;


        public DateTimeOffset CreatedTimestamp { get; private set; }
        public DateTimeOffset UpdatedTimestamp { get; set; }

        public TimeSpan SessionTimeOut { get; set; }

        private static readonly TimeSpan DefaultSessionTimeOut = new TimeSpan(0, 30, 0); // 30 minutes


        public DhcpSession(IDhcpService dhcpService, IPureLogger<DhcpSession> logger, PhysicalAddress physicalAddress, TimeSpan sessionTimeOut)
        {
            _dhcpService = dhcpService ?? throw new ArgumentNullException(nameof(dhcpService));
            PhysicalAddress = physicalAddress;
            _dhcpSessionResult = new DhcpSessionResult(physicalAddress);
           
            Logger = logger;

            CreatedTimestamp = UpdatedTimestamp = DateTimeOffset.Now;
            SessionTimeOut = sessionTimeOut;
        }

        public DhcpSession(IDhcpService dhcpService, IPureLogger<DhcpSession> logger, PhysicalAddress physicalAddress) :
            this(dhcpService, logger, physicalAddress, DefaultSessionTimeOut)
        {

        }

        public DateTimeOffset UpdateTimestamp()
        {
            return UpdatedTimestamp = DateTimeOffset.Now;
        }


        public bool HasSessionExpired(DateTimeOffset timeStamp, TimeSpan timeSpan)
        {
            return timeStamp - UpdatedTimestamp > timeSpan;
        }

        public bool HasSessionExpired() => HasSessionExpired(DateTimeOffset.Now, DefaultSessionTimeOut);
        
        public DhcpMessageProcessed ProcessDiscover(IDhcpMessage dhcpMessage)
        {
            _dhcpSessionResult.UpdateSessionState(dhcpMessage.SessionId, DhcpSessionState.Discover, dhcpMessage.ClientHardwareAddress);
            UpdateTimestamp();

            return DhcpMessageProcessed.Success;
        }

        public DhcpMessageProcessed ProcessRequest(IDhcpMessage dhcpMessage)
        {
            if (_dhcpSessionResult.IsDuplicateRequest(dhcpMessage))
            {
                return DhcpMessageProcessed.Duplicate;
            }


            // Update Session State
            _dhcpSessionResult.UpdateSessionState(dhcpMessage.SessionId, DhcpSessionState.Request, dhcpMessage.ClientHardwareAddress);

            var addressRequest = dhcpMessage.GetOptionData(DhcpOption.RequestedIpAddr);

            var requestState = RequestState.Unknown;

            #region Pre-Processing
            //  Start pre-process validation
            //---------------------------------------------------------------------
            //|              |INIT-REBOOT  |SELECTING    |RENEWING     |REBINDING |
            //---------------------------------------------------------------------
            //|broad/unicast |broadcast    |broadcast    |unicast      |broadcast |
            //|server-ip     |MUST NOT     |MUST         |MUST NOT     |MUST NOT  |
            //|requested-ip  |MUST         |MUST         |MUST NOT     |MUST NOT  |
            //|ciaddr        |zero         |zero         |IP address   |IP address|
            //---------------------------------------------------------------------
            // first determine what KIND of request we are dealing with
            if (dhcpMessage.ClientAddress.Equals(InternetAddress.Any))
            {
                // the ciAddr MUST be 0.0.0.0 for Init-Reboot and Selecting
                requestState = addressRequest == null ?
                    RequestState.InitReboot : RequestState.Selecting;
            }
            else
            {
                // the ciAddr MUST NOT be 0.0.0.0 for Renew and Rebind
                if (!dhcpMessage.IsBroadcast)
                {
                    // renew is unicast
                    // NOTE: this will not happen if the v4 broadcast interface used at startup,
                    //		 but handling of DHCPv4 renew/rebind is the same anyway
                    requestState = RequestState.Renewing;
                }
                else
                {
                    // rebind is broadcast
                    requestState = RequestState.Rebinding;
                }
            }

            if (requestState == RequestState.InitReboot || requestState == RequestState.Selecting)
            {
                if (addressRequest == null)
                {
                    Logger?.LogDebug("Ignoring {DhcpMessageType} {DhcpRequestState} message: Requested IP option is null", MessageType.Request, RequestStateString.GetName(requestState));
                    //return; // if processing should not continue
                }
            }
            else
            {	// type == Renewing or Rebinding
                if (addressRequest != null)
                {
                    Logger?.LogDebug("Ignoring {DhcpMessageType} {DhcpRequestState} message: Requested IP option is not null", MessageType.Request, RequestStateString.GetName(requestState));
                    //return; // if processing should not continue
                }
            }

            //  End pre-process validation
            #endregion Pre-Processing

            Logger?.LogTrace("Processing {DhcpMessageType} {DhcpRequestState} message", MessageType.Request, RequestStateString.GetName(requestState));

            DhcpSessionState = DhcpSessionState.Request;
            RequestState = requestState;

            if (!dhcpMessage.ClientHardwareAddress.Equals(PhysicalAddress))
            {
                Logger?.LogError("ClientHardwareAddress {ClientHardwareAddress} does not equal PhysicalAddress {PhysicalAddress}", dhcpMessage.ClientHardwareAddress, PhysicalAddress);
            }

            // Update Hostname
            _dhcpSessionResult.HostName = ByteUtility.GetStringNullIfEmpty(dhcpMessage.GetOptionData(DhcpOption.Hostname));
            _dhcpSessionResult.VendorClassId = ByteUtility.GetStringNullIfEmpty(dhcpMessage.GetOptionData(DhcpOption.VendorClassId));

            bool routerDiscoveryParamExists = dhcpMessage.ParamOptionExists(DhcpOption.RouterDiscovery);

            // Figure out INetworkDeviceInfo


            // TODO: Handle DhcpRequest
            UpdateTimestamp();

            return DhcpMessageProcessed.Success;
        }


        public DhcpMessageProcessed ProcessDecline(IDhcpMessage dhcpMessage)
        {
            UpdateTimestamp();
            return DhcpMessageProcessed.Ignored;
        }


        public DhcpMessageProcessed ProcessRelease(IDhcpMessage dhcpMessage)
        {
            UpdateTimestamp();
            return DhcpMessageProcessed.Ignored;
        }


        public DhcpMessageProcessed ProcessInform(IDhcpMessage dhcpMessage)
        {
            UpdateTimestamp();
            return DhcpMessageProcessed.Ignored;
        }

        public DhcpMessageProcessed ProcessAck(IDhcpMessage dhcpMessage)
        {
            UpdateTimestamp();
            return DhcpMessageProcessed.Ignored;
        }
    }
}
