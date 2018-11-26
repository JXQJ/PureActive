using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using PureActive.Core.Extensions;
using PureActive.Logger.Provider.Serilog.Types;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Logging.Extensions.Types;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Serilog.Sinks.TestCorrelator;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Logger.Provider.Serilog.UnitTests.Types
{

    [Trait("Category", "Unit")]
    public class PureSeriLoggerFactoryUnitTests : TestBaseLoggable<PureSeriLoggerFactoryUnitTests>
    {
        public PureSeriLoggerFactoryUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _pureSeriLoggerFactory = new PureSeriLoggerFactory(TestLoggerFactory, LoggerSettings);
        }

        private readonly IPureLoggerFactory _pureSeriLoggerFactory;

        [Fact]
        public void PureSeriLoggerFactory_Constructor()
        {
           _pureSeriLoggerFactory.Should().NotBeNull().And.Subject.Should().BeAssignableTo<IPureLoggerFactory>();
        }


        [Fact]
        public void PureSeriLoggerFactory_CreateLogger()
        {
            var logger = _pureSeriLoggerFactory.CreateLogger(nameof(PureSeriLoggerFactoryUnitTests));
            logger.Should().NotBeNull().And.Subject.Should().BeAssignableTo<ILogger>();
        }

        [Fact]
        public void PureSeriLoggerFactory_CreateLogger_Type()
        {
            var logger = _pureSeriLoggerFactory.CreateLogger<PureSeriLoggerFactoryUnitTests>();
            logger.Should().NotBeNull().And.Subject.Should().BeAssignableTo<ILogger<PureSeriLoggerFactoryUnitTests>>();
        }


        [Fact]
        public void PureSeriLoggerFactory_AddProvider_Null()
        {
           _pureSeriLoggerFactory.AddProvider(null);
        }
    }
}