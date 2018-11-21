// ***********************************************************************
// Assembly         : PureActive.Network.Services.DhcpService.UnitTests
// Author           : SteveBu
// Created          : 11-17-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="DhcpMessageUnitTests.cs" company="BushChang Corporation">
//     � 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Text;
using Microsoft.Extensions.Logging;
using PureActive.Logging.Abstractions.Types;
using PureActive.Logging.Extensions.Extensions;
using PureActive.Logging.Extensions.Types;
using PureActive.Network.Services.DhcpService.Message;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.Services.DhcpService.UnitTests.Message
{
    /// <summary>
    /// Class DhcpMessageUnitTests.
    /// Implements the <see cref="Serilog.Sink.Xunit.TestBase.TestBaseLoggable{DhcpMessageUnitTests}" />
    /// </summary>
    /// <seealso cref="Serilog.Sink.Xunit.TestBase.TestBaseLoggable{DhcpMessageUnitTests}" />
    /// <autogeneratedoc />
    [Trait("Category", "Unit")]
    public class DhcpMessageUnitTests : TestBaseLoggable<DhcpMessageUnitTests>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DhcpMessageUnitTests"/> class.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <autogeneratedoc />
        public DhcpMessageUnitTests(ITestOutputHelper output) : base(output)
        {
        }

        /// <summary>
        /// The i phone request DHCP MSG bytes
        /// </summary>
        /// <autogeneratedoc />
        private readonly byte[] _iPhoneRequestDhcpMsgBytes =
        {
            1, 1, 6, 0, 122, 6, 156, 94, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 64, 152, 173, 49,
            47, 138, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 99, 130, 83, 99, 53, 1, 3, 55, 7,
            1, 121, 3, 6, 15, 119, 252, 57, 2, 5, 220, 61, 7, 1, 64, 152, 173, 49, 47, 138, 50, 4, 10, 1, 10, 198, 51,
            4, 0, 118, 167, 0, 12, 15, 83, 116, 101, 118, 101, 66, 117, 115, 105, 80, 104, 111, 110, 101, 88, 255, 0, 0,
            0, 0, 0
        };

        /// <summary>
        /// The bc den discover DHCP MSG bytes
        /// </summary>
        /// <autogeneratedoc />
        private readonly byte[] _bcDenDiscoverDhcpMsgBytes =
        {
            1, 1, 6, 0, 136, 1, 157, 80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 21, 209, 16, 1,
            22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 99, 130, 83, 99, 53, 1, 1, 61, 7, 1, 2,
            21, 209, 16, 1, 22, 12, 5, 66, 67, 68, 101, 110, 60, 8, 77, 83, 70, 84, 32, 53, 46, 48, 55, 14, 1, 3, 6, 15,
            31, 33, 43, 44, 46, 47, 119, 121, 249, 252, 255, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
        };


        /// <summary>
        /// Defines the test method DhcpMessage_Deconstruct.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void DhcpMessage_Deconstruct()
        {
            DhcpMessage dhcpRequestMessage = new DhcpMessage(_iPhoneRequestDhcpMsgBytes, TestLoggerFactory);

            Logger?.LogDebug("DhcpRequest_WithDeconstruct {@DhcpMessage}", dhcpRequestMessage);
        }

        /// <summary>
        /// Defines the test method DhcpMessage_FormatPropertyList.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void DhcpMessage_FormatPropertyList()
        {
            DhcpMessage dhcpRequestMessage = new DhcpMessage(_bcDenDiscoverDhcpMsgBytes, TestLoggerFactory);

            var logLevel = LogLevel.Trace;
            var sb = new StringBuilder();

            PureLogPropertyLevel.FormatPropertyList(sb, LoggableFormat.ToLogWithParents,
                dhcpRequestMessage.GetLogPropertyListLevel(logLevel, LoggableFormat.ToStringWithParents), logLevel);

            TestOutputHelper.WriteLine(sb.ToString());
        }

        /// <summary>
        /// Defines the test method DhcpMessage_GetPropertyListLevel.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void DhcpMessage_GetPropertyListLevel()
        {
            DhcpMessage dhcpRequestMessage = new DhcpMessage(_iPhoneRequestDhcpMsgBytes, TestLoggerFactory);

            var logLevel = LogLevel.Information;

            using (Logger?.PushLogProperties(
                dhcpRequestMessage.GetLogPropertyListLevel(logLevel, LoggableFormat.ToLogWithParents)))
            {
                Logger?.LogDebug("DhcpRequest_GetPropertyListLevel by {LogLevel}", logLevel);
            }
        }

        /// <summary>
        /// Defines the test method DhcpMessage_ToString.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void DhcpMessage_ToString()
        {
            DhcpMessage dhcpRequestMessage = new DhcpMessage(_iPhoneRequestDhcpMsgBytes, TestLoggerFactory);

            TestOutputHelper.WriteLine(dhcpRequestMessage.ToString());
        }

        /// <summary>
        /// Defines the test method DhcpMessage_With.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void DhcpMessage_With()
        {
            DhcpMessage dhcpRequestMessage = new DhcpMessage(_iPhoneRequestDhcpMsgBytes, TestLoggerFactory);

            var logLevel = LogLevel.Debug;

            using (Logger?.PushLogProperties(
                dhcpRequestMessage.GetLogPropertyListLevel(logLevel, LoggableFormat.ToLogWithParents), logLevel))
            {
                Logger?.LogDebug("DhcpRequest_With by {LogLevel}", logLevel);
            }

            TestOutputHelper.WriteLine(Environment.NewLine + dhcpRequestMessage);
        }


        /// <summary>
        /// Defines the test method DhcpMessage_With_ILoggable.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void DhcpMessage_With_ILoggable()
        {
            DhcpMessage dhcpRequestMessage = new DhcpMessage(_iPhoneRequestDhcpMsgBytes, TestLoggerFactory);

            var logLevel = LogLevel.Debug;

            using (dhcpRequestMessage.PushLogProperties(logLevel))
            {
                Logger?.LogDebug("TestDhcpRequest_DhcpRequest_With_ILoggable by {LogLevel}", logLevel);
            }
        }


        /// <summary>
        /// Defines the test method DhcpMessage_With_None.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void DhcpMessage_With_None()
        {
            DhcpMessage dhcpRequestMessage = new DhcpMessage(_iPhoneRequestDhcpMsgBytes, TestLoggerFactory);

            var logLevel = LogLevel.None;

            using (Logger?.PushLogProperties(
                dhcpRequestMessage.GetLogPropertyListLevel(logLevel, LoggableFormat.ToLogWithParents), logLevel))
            {
                Logger?.LogDebug("DhcpRequest_With_None by {LogLevel}", logLevel);
            }
        }

        /// <summary>
        /// Defines the test method TestDhcpMessagingLogging.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void TestDhcpMessagingLogging()
        {
        }
    }
}