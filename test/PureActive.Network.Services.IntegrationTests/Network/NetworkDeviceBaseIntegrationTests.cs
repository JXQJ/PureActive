// ***********************************************************************
// Assembly         : PureActive.Network.Services.IntegrationTests
// Author           : SteveBu
// Created          : 11-17-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="NetworkDeviceBaseIntegrationTests.cs" company="BushChang Corporation">
//     � 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using FluentAssertions;
using PureActive.Network.Abstractions.CommonNetworkServices;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.Services.IntegrationTests.Network
{
    /// <summary>
    /// Class NetworkDeviceBaseIntegrationTests.
    /// Implements the <see cref="Serilog.Sink.Xunit.TestBase.TestBaseLoggable{NetworkDeviceBaseIntegrationTests}" />
    /// </summary>
    /// <seealso cref="Serilog.Sink.Xunit.TestBase.TestBaseLoggable{NetworkDeviceBaseIntegrationTests}" />
    /// <autogeneratedoc />
    [Trait("Category", "Integration")]
    public class NetworkDeviceBaseIntegrationTests : TestBaseLoggable<NetworkDeviceBaseIntegrationTests>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkDeviceBaseIntegrationTests"/> class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        /// <autogeneratedoc />
        public NetworkDeviceBaseIntegrationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _commonNetworkServices =
                CommonNetworkServices.CreateInstance(TestLoggerFactory, "NetworkDeviceBaseIntegrationTests");
        }

        /// <summary>
        /// The common network services
        /// </summary>
        /// <autogeneratedoc />
        private readonly ICommonNetworkServices _commonNetworkServices;

        /// <summary>
        /// Defines the test method NetworkDeviceBase_Constructor.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void NetworkDeviceBase_Constructor()
        {
            _commonNetworkServices.Should().NotBeNull();
        }
    }
}