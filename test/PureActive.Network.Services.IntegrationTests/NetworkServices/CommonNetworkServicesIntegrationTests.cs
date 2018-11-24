// ***********************************************************************
// Assembly         : PureActive.Network.Services.IntegrationTests
// Author           : SteveBu
// Created          : 11-23-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-23-2018
// ***********************************************************************
// <copyright file="CommonNetworkServicesIntegrationTests.cs" company="BushChang Corporation">
//     � 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using PureActive.Hosting.Abstractions.Types;
using PureActive.Hosting.CommonServices;
using PureActive.Network.Abstractions.CommonNetworkServices;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.Services.IntegrationTests.NetworkServices
{
    /// <summary>
    /// Class CommonNetworkServicesIntegrationTests.
    /// Implements the <see cref="CommonNetworkServicesIntegrationTests" />
    /// </summary>
    /// <seealso cref="CommonNetworkServicesIntegrationTests" />
    /// <autogeneratedoc />
    [Trait("Category", "Integration")]
    public class CommonNetworkServicesIntegrationTests : TestBaseLoggable<CommonNetworkServicesIntegrationTests>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommonNetworkServicesIntegrationTests"/> class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        /// <autogeneratedoc />
        public CommonNetworkServicesIntegrationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            var commonServices = CommonServices.CreateInstance(TestLoggerFactory, typeof(CommonNetworkServicesIntegrationTests));
            _commonNetworkServices = CommonNetworkServices.CreateInstance(TestLoggerFactory, commonServices);
        }

        private readonly ICommonNetworkServices _commonNetworkServices;

        [Fact]
        public void CommonNetworkServices_Constructor()
        {
            _commonNetworkServices.Should().NotBeNull().And.Subject.Should().BeAssignableTo<ICommonNetworkServices>();
        }

        /// <summary>
        /// arp service start stop as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        /// <autogeneratedoc />
        [Fact]
        public async Task CommonNetworkServices_StartStop()
        {
            var cancellationTokenSource = new CancellationTokenSource();

            Assert.Equal(ServiceHostStatus.Stopped, _commonNetworkServices.ServiceHostStatus);
            await _commonNetworkServices.StartAsync(cancellationTokenSource.Token);
            Assert.Equal(ServiceHostStatus.Running, _commonNetworkServices.ServiceHostStatus);
            await _commonNetworkServices.StopAsync(cancellationTokenSource.Token);
            Assert.Equal(ServiceHostStatus.Stopped, _commonNetworkServices.ServiceHostStatus);
        }

        /// <summary>
        /// Common Network Services start stop as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        /// <autogeneratedoc />
        [Fact]
        public async Task CommonNetworkServices_StartStopAsync()
        {
            var cancellationTokenSource = new CancellationTokenSource();

            Assert.Equal(ServiceHostStatus.Stopped, _commonNetworkServices.ServiceHostStatus);

            var startTask =_commonNetworkServices.StartAsync(cancellationTokenSource.Token);
            startTask.Should().NotBeNull();

            Assert.Equal(ServiceHostStatus.Running, _commonNetworkServices.ServiceHostStatus);
            await _commonNetworkServices.StopAsync(cancellationTokenSource.Token);
            Assert.Equal(ServiceHostStatus.Stopped, _commonNetworkServices.ServiceHostStatus);
        }

    }
}