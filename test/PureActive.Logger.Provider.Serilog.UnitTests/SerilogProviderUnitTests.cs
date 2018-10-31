using System.IO;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using PureActive.Core.Extensions;
using PureActive.Core.System;
using PureActive.Logger.Provider.Serilog.Configuration;
using PureActive.Logger.Provider.Serilog.Settings;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Logging.Abstractions.Types;
using Serilog.Events;
using Xunit;

namespace PureActive.Logger.Provider.Serilog.UnitTests
{
    public class SerilogProviderUnitTests
    {
        private void AssertLogFileEntry(IPureLoggerSettings loggerSettings, LogLevel logLevel, string msg, string logFileName)
        {
            string partialName = Path.GetFileNameWithoutExtension(logFileName);
            DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(loggerSettings.LogFolderPath);
            FileInfo[] filesInDir = hdDirectoryInWhichToSearch.GetFiles("*" + partialName + "*.*");

            foreach (FileInfo foundFile in filesInDir)
            {
                if (foundFile.Name.StartsWith(partialName))
                {
                    if (File.Exists(foundFile.FullName))
                    {
                        using (var sr = new StreamReader(foundFile.FullName))
                        {
                            var logContents = sr.ReadToEnd();

                            logContents.Should().EndWith($"{msg}\r\n");
                            logContents.Should().Contain($"[{logLevel}]");
                        }
                    }
                }
            }
        }

        private IPureLoggerFactory CreatePureLoggerFactory(LogEventLevel logEventLevel, LoggingOutputFlags loggingOutputFlags, string logFileName)
        {
            var fileSystem = new FileSystem(typeof(SerilogProviderUnitTests));

            var loggerSettings = new SerilogLoggerSettings(fileSystem, LogEventLevel.Debug, LoggingOutputFlags.AppConsoleFile);
            var loggerConfiguration = LoggerConfigurationFactory.CreateLoggerConfiguration((string)null, logFileName, loggerSettings, b => true);

            return LoggerConfigurationFactory.CreatePureSeriLoggerFactory(loggerSettings, loggerConfiguration);
        }

        [Fact]
        public void SerilogProvider_CreateLogger_AppConsoleFile()
        {
            var logFileName = FileExtensions.GetRandomFileName("", ".log");
            var loggerFactory = CreatePureLoggerFactory(LogEventLevel.Debug, LoggingOutputFlags.AppConsoleFile, logFileName);
            var logger = loggerFactory.CreatePureLogger<SerilogProviderUnitTests>();

            const string msg = "Test";
            logger.LogDebug(msg);

            // Dispose Logger Factory so we can access log file
            loggerFactory.Dispose();

            AssertLogFileEntry(loggerFactory.PureLoggerSettings, LogLevel.Debug, msg, logFileName);
        }
    }
}
