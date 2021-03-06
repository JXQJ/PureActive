// ***********************************************************************
// Assembly         : PureActive.Network.Services.PingService.IntegrationTests
// Author           : SteveBu
// Created          : 11-17-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="PingTaskIngrationTests.cs" company="BushChang Corporation">
//     � 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using PureActive.Hosting.Abstractions.Extensions;
using PureActive.Hosting.Abstractions.System;
using PureActive.Hosting.CommonServices;
using PureActive.Network.Abstractions.PingService;
using PureActive.Network.Abstractions.PingService.Events;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.Services.PingService.IntegrationTests
{
    /// <summary>
    /// Class PingTaskIntegrationTests.
    /// Implements the <see cref="Serilog.Sink.Xunit.TestBase.TestBaseLoggable{PingTaskIntegrationTests}" />
    /// </summary>
    /// <seealso cref="Serilog.Sink.Xunit.TestBase.TestBaseLoggable{PingTaskIntegrationTests}" />
    /// <autogeneratedoc />
    [Trait("Category", "Integration")]
    public class PingTaskIntegrationTests : TestBaseLoggable<PingTaskIntegrationTests>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PingTaskIntegrationTests"/> class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        /// <autogeneratedoc />
        public PingTaskIntegrationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _commonServices = CommonServices.CreateInstance(TestLoggerFactory, "PingTaskUnitTests");

            _pingTask = new PingService.PingTaskImpl(_commonServices);
            _pingTask.OnPingReplyTask += PingReplyEventHandler;
            _pingOptions = new PingOptions(_pingTask.Ttl, _pingTask.DoNotFragment);
        }

        /// <summary>internal IPingTask service</summary>
        /// <autogeneratedoc />
        private readonly IPingTask _pingTask;

        /// <summary>Default ping options</summary>
        /// <autogeneratedoc />
        private readonly PingOptions _pingOptions;

        private readonly ICommonServices _commonServices;

        /// <summary>
        /// The default data buffer
        /// </summary>
        /// <autogeneratedoc />
        private readonly byte[] _defaultDataBuffer = Encoding.ASCII.GetBytes("abcdefghijklmnopqrstuvwxyz012345");


        /// <summary>
        /// Pings the reply event handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="PingReplyEventArgs"/> instance containing the event data.</param>
        /// <autogeneratedoc />
        private void PingReplyEventHandler(object sender, PingReplyEventArgs args)
        {
            sender.Should().BeAssignableTo<IPingTask>();

            if (args != null)
            {
                TestOutputHelper.WriteLine(
                    $"Job: {args.PingJob.JobGuid}, TaskId: {args.PingJob.TaskId}, IPAddressSubnet: {args.PingJob.IPAddressSubnet}, Status: {args.PingReply.Status}");
            }
        }

        /// <summary>
        /// Tests PingIpAddressAsync Default Network Gateway
        /// </summary>
        /// <returns>Task.</returns>
        /// <autogeneratedoc />
        [Fact]
        public async Task PingTask_PingIpAddressAsync_DefaultNetworkGateway()
        {
            var ipAddressSubnet = _commonServices.NetworkingSystem.GetDefaultGatewayAddress();

            var pingReply = await _pingTask.PingIpAddressAsync(ipAddressSubnet);
            pingReply.Should().NotBeNull();
            pingReply.Status.Should().Match<IPStatus>(ips => ips == IPStatus.Success || ips == IPStatus.TimedOut);
        }

        /// <summary>
        /// Tests PingIpAddressAsync with a null buffer
        /// </summary>
        /// <returns>Task.</returns>
        /// <autogeneratedoc />
        [Fact]
        public async Task PingTask_PingIpAddressAsync_Buffer_Null()
        {
            var ipAddressSubnet = _commonServices.NetworkingSystem.GetDefaultLocalNetworkAddress();

            var pingReply = await _pingTask.PingIpAddressAsync(ipAddressSubnet, _pingTask.DefaultTimeout, null, _pingOptions);
            pingReply.Should().BeNull();
        }

        /// <summary>
        /// Tests PingIpAddressAsync with a buffer to Small
        /// </summary>
        /// <returns>Task.</returns>
        /// <autogeneratedoc />
        [Fact]
        public async Task PingTask_PingIpAddressAsync_Buffer_Zero_Length()
        {
            var ipAddressSubnet = IPAddressExtensions.GooglePublicDnsServerAddress;
            byte[] pingBuffer = new byte[0];

            var pingReply = await _pingTask.PingIpAddressAsync(ipAddressSubnet, _pingTask.DefaultTimeout, pingBuffer, _pingOptions);
            pingReply.Should().NotBeNull();
            
        }

        /// <summary>
        /// Tests PingIpAddressAsync with expire TTl
        /// </summary>
        /// <returns>Task.</returns>
        /// <autogeneratedoc />
        [Fact]
        public async Task PingTask_PingIpAddressAsync_Ttl_Expired()
        {
            var ipAddressSubnet = IPAddressExtensions.GooglePublicDnsServerAddress;

            var pingReply = await _pingTask.PingIpAddressAsync(ipAddressSubnet, _pingTask.DefaultTimeout, _defaultDataBuffer, new PingOptions(1, true));
            pingReply.Should().NotBeNull();
        }

        /// <summary>
        /// Tests PingIpAddressAsync with expire TTl
        /// </summary>
        /// <returns>Task.</returns>
        /// <autogeneratedoc />
        [Fact]
        public async Task PingTask_PingIpAddressAsync_Fragment()
        {
            var ipAddressSubnet = IPAddressExtensions.GooglePublicDnsServerAddress;

            var pingReply = await _pingTask.PingIpAddressAsync(ipAddressSubnet, _pingTask.DefaultTimeout, _defaultDataBuffer, new PingOptions(2, false));
            pingReply.Should().NotBeNull();
        }
    }
}