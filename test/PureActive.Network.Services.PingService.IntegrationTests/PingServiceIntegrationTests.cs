// ***********************************************************************
// Assembly         : PureActive.Network.Services.PingService.IntegrationTests
// Author           : SteveBu
// Created          : 11-17-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="PingServiceIntegrationTests.cs" company="BushChang Corporation">
//     � 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using PureActive.Hosting.Abstractions.Types;
using PureActive.Hosting.CommonServices;
using PureActive.Network.Abstractions.Extensions;
using PureActive.Network.Abstractions.Networking;
using PureActive.Network.Abstractions.PingService;
using PureActive.Network.Abstractions.PingService.Events;
using PureActive.Network.Abstractions.Types;
using PureActive.Network.Services.Networking;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.Services.PingService.IntegrationTests
{
    /// <summary>
    /// Class PingServiceIntegrationTests.
    /// Implements the <see cref="Serilog.Sink.Xunit.TestBase.TestBaseLoggable{PingServiceIntegrationTests}" />
    /// </summary>
    /// <seealso cref="Serilog.Sink.Xunit.TestBase.TestBaseLoggable{PingServiceIntegrationTests}" />
    /// <autogeneratedoc />
    [Trait("Category", "Integration")]
    public class PingServiceIntegrationTests : TestBaseLoggable<PingServiceIntegrationTests>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PingServiceIntegrationTests"/> class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        /// <autogeneratedoc />
        public PingServiceIntegrationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            var commonServices = CommonServices.CreateInstance(TestLoggerFactory, "PingServiceIntegrationTests");
            var networkingService = new NetworkingService(TestLoggerFactory.CreatePureLogger<NetworkingService>());
            _pingService = new PingService(commonServices, networkingService);
            _pingService.OnPingReplyService += PingReplyEventHandler;
            _networkingService = new NetworkingService(TestLoggerFactory.CreatePureLogger<NetworkingService>());
        }

        /// <summary>
        /// The ping service
        /// </summary>
        /// <autogeneratedoc />
        private readonly IPingService _pingService;
        /// <summary>
        /// The default network timeout
        /// </summary>
        /// <autogeneratedoc />
        private const int DefaultNetworkTimeout = 250;
        /// <summary>
        /// The default ping calls
        /// </summary>
        /// <autogeneratedoc />
        private const int DefaultPingCalls = 5;

        private readonly INetworkingService _networkingService;


        /// <summary>
        /// Pings the reply event handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="PingReplyEventArgs"/> instance containing the event data.</param>
        /// <autogeneratedoc />
        private void PingReplyEventHandler(object sender, PingReplyEventArgs args)
        {
            sender.Should().BeAssignableTo<IPingService>();

            if (args != null)
            {
                args.CancellationToken.Should().NotBeNull();
                TestOutputHelper.WriteLine(
                    $"Job: {args.PingJob.JobGuid}, TaskId: {args.PingJob.TaskId}, IPAddressSubnet: {args.PingJob.IPAddressSubnet}, Status: {args.PingReply.Status}");
            }
        }

        /// <summary>
        /// Defines the test method PingService Logging
        /// </summary>
        /// <returns>Task.</returns>
        /// <autogeneratedoc />
        [Fact]
        public async Task PingService_PingNetworkEvent_WithLogging()
        {
            var ipAddressSubnet = new IPAddressSubnet(_networkingService.GetDefaultLocalNetworkAddress(),
                IPAddressExtensions.SubnetClassC);

            await _pingService.PingNetworkAsync(ipAddressSubnet, CancellationToken.None, DefaultNetworkTimeout, DefaultPingCalls, 0, false);
        }

        /// <summary>
        /// Defines the test method PingService Delay between pings
        /// </summary>
        /// <returns>Task.</returns>
        /// <autogeneratedoc />
        [Fact]
        public async Task PingService_PingNetworkEvent_Delay()
        {
            var ipAddressSubnet = new IPAddressSubnet(_networkingService.GetDefaultLocalNetworkAddress(),
                IPAddressExtensions.SubnetClassC);

            await _pingService.PingNetworkAsync(ipAddressSubnet, CancellationToken.None, DefaultNetworkTimeout, DefaultPingCalls, 1, false);
        }


        /// <summary>
        ///  Defines the test method PingService cancellation
        /// </summary>
        /// <returns>Task.</returns>
        /// <autogeneratedoc />
        [Fact]
        public async Task PingService_PingNetworkEvent_Cancel()
        {
            var cancellationTokenSource = new CancellationTokenSource();

            var ipAddressSubnet = new IPAddressSubnet(_networkingService.GetDefaultLocalNetworkAddress(),
                IPAddressExtensions.SubnetClassC);

            var pingServiceTask = _pingService.PingNetworkAsync(ipAddressSubnet, cancellationTokenSource.Token, DefaultNetworkTimeout, DefaultPingCalls, 1, false);

            cancellationTokenSource.CancelAfter(200);

            await pingServiceTask;
        }

        /// <summary>
        /// Defines the test method PingService Shuffle parameter
        /// </summary>
        /// <returns>Task.</returns>
        /// <autogeneratedoc />
        [Fact]
        public async Task PingService_PingNetworkEvent_Shuffle()
        {
            var cancellationTokenSource = new CancellationTokenSource();

            var ipAddressSubnet = new IPAddressSubnet(_networkingService.GetDefaultLocalNetworkAddress(),
                IPAddressExtensions.SubnetClassC);

            var pingServiceTask = _pingService.PingNetworkAsync(ipAddressSubnet, cancellationTokenSource.Token, DefaultNetworkTimeout, DefaultPingCalls, 1, true);

            cancellationTokenSource.CancelAfter(2000);

            await pingServiceTask;
        }

        /// <summary>
        /// Defines the test method testing start/stop of PingService
        /// </summary>
        /// <returns>Task.</returns>
        /// <autogeneratedoc />
        [Fact]
        public async Task PingService_StartStop()
        {
            var cancellationTokenSource = new CancellationTokenSource();

            Assert.Equal(ServiceHostStatus.Stopped, _pingService.ServiceHostStatus);

            var startTask = _pingService.StartAsync(cancellationTokenSource.Token);
            startTask.Should().NotBeNull();

            Assert.Equal(ServiceHostStatus.Running, _pingService.ServiceHostStatus);
            await _pingService.StopAsync(cancellationTokenSource.Token);
            Assert.Equal(ServiceHostStatus.Stopped, _pingService.ServiceHostStatus);
        }


        /// <summary>
        /// Defines the test method testing start/stop of PingService
        /// </summary>
        /// <returns>Task.</returns>
        /// <autogeneratedoc />
        [Fact]
        public async Task PingService_StartStopAsync()
        {
            var cancellationTokenSource = new CancellationTokenSource();

            Assert.Equal(ServiceHostStatus.Stopped, _pingService.ServiceHostStatus);
            await _pingService.StartAsync(cancellationTokenSource.Token);
            Assert.Equal(ServiceHostStatus.Running, _pingService.ServiceHostStatus);
            await _pingService.StopAsync(cancellationTokenSource.Token);
            Assert.Equal(ServiceHostStatus.Stopped, _pingService.ServiceHostStatus);
        }


        /// <summary>
        /// Tests PingIpAddressAsync with default timeouts
        /// </summary>
        /// <returns>Task.</returns>
        /// <autogeneratedoc />
        [Fact]
        public async Task PingService_PingIpAddressAsync()
        {
            var ipAddress = _networkingService.GetDefaultLocalNetworkAddress();
            var pingReply = await _pingService.PingIpAddressAsync(ipAddress);

            pingReply.Should().NotBeNull();
        }

        /// <summary>
        /// Tests PingIpAddressAsync with a timeout
        /// </summary>
        /// <returns>Task.</returns>
        /// <autogeneratedoc />
        [Fact]
        public async Task PingService_PingIpAddressAsync_Timeout()
        {
            var ipAddress = _networkingService.GetDefaultLocalNetworkAddress();

            // Wait 5 seconds for a reply.
            int timeout = 500;
            var pingReply = await _pingService.PingIpAddressAsync(ipAddress, timeout);
            pingReply.Should().NotBeNull();
        }


        /// <summary>
        /// Tests PingIpAddressAsync with a timeout
        /// </summary>
        /// <returns>Task.</returns>
        /// <autogeneratedoc />
        [Fact]
        public async Task PingService_PingIpAddressAsync_BadAddress()
        {
            var ipAddress = IPAddress.Broadcast;

#pragma warning disable CS0618 // Type or member is obsolete
            long ipAddressLong = ipAddress.Address;

            ipAddress.Address = (ipAddressLong + 1);
#pragma warning restore CS0618 // Type or member is obsolete    

            var pingReply = await _pingService.PingIpAddressAsync(ipAddress);
            pingReply.Should().BeNull();
        }
    }
}