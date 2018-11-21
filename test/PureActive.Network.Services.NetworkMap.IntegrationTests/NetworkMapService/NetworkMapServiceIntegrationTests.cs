// ***********************************************************************
// Assembly         : PureActive.Network.Services.NetworkMap.IntegrationTests
// Author           : SteveBu
// Created          : 11-17-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="NetworkMapServiceIntegrationTests.cs" company="BushChang Corporation">
//     � 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Threading;
using System.Threading.Tasks;
using PureActive.Network.Abstractions.CommonNetworkServices;
using PureActive.Network.Abstractions.NetworkMapService;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.Services.NetworkMap.IntegrationTests.NetworkMapService
{
    /// <summary>
    /// Class NetworkMapServiceIntegrationTests.
    /// Implements the <see cref="Serilog.Sink.Xunit.TestBase.TestBaseLoggable{NetworkMapServiceIntegrationTests}" />
    /// </summary>
    /// <seealso cref="Serilog.Sink.Xunit.TestBase.TestBaseLoggable{NetworkMapServiceIntegrationTests}" />
    /// <autogeneratedoc />
    [Trait("Category", "Integration")]
    public class NetworkMapServiceIntegrationTests : TestBaseLoggable<NetworkMapServiceIntegrationTests>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkMapServiceIntegrationTests"/> class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        /// <autogeneratedoc />
        public NetworkMapServiceIntegrationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _commonNetworkServices =
                CommonNetworkServices.CreateInstance(TestLoggerFactory, "NetworkMapServiceUnitTests");
        }

        /// <summary>
        /// The common network services
        /// </summary>
        /// <autogeneratedoc />
        private readonly ICommonNetworkServices _commonNetworkServices;

        /// <summary>
        /// Creates the network map service.
        /// </summary>
        /// <returns>INetworkMapService.</returns>
        /// <autogeneratedoc />
        private INetworkMapService CreateNetworkMapService()
        {
            // Common Network Services
            var dhcpService = new DhcpService.DhcpService(_commonNetworkServices);
            var networkMap = new Devices.Network.NetworkMap(_commonNetworkServices);

            return new NetworkMap.NetworkMapService(networkMap, dhcpService);
        }

        /// <summary>
        /// Defines the test method NetworkMapService_StartStop.
        /// </summary>
        /// <returns>Task.</returns>
        /// <autogeneratedoc />
        [Fact]
        public async Task NetworkMapService_StartStop()
        {
            var networkMapService = CreateNetworkMapService();

            if (networkMapService != null)
            {
                var cancellationTokenSource = new CancellationTokenSource();

                await networkMapService.StartAsync(cancellationTokenSource.Token);
                await networkMapService.StopAsync(cancellationTokenSource.Token);
            }

            Assert.NotNull(networkMapService);
        }
    }
}