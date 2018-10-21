namespace PureActive.Logger.Sink.Xunit
{
    public class XunitLoggingSink
    {
/*
        public static LoggerConfiguration CreateXUnitLoggerConfiguration(ITestOutputHelper testOutputHelper, IPureLoggerSettings loggerSettings, XUnitSerilogFormatter xUnitSerilogFormatter)
        {
            if (testOutputHelper == null) throw new ArgumentNullException(nameof(testOutputHelper));
            if (loggerSettings == null) throw new ArgumentNullException(nameof(loggerSettings));

            var loggerConfiguration = CreateDefaultLoggerConfiguration(loggerSettings);

            var jsonFormatter = GetXUnitSerilogFormatter(xUnitSerilogFormatter);

            if (jsonFormatter != null)
            {
                loggerConfiguration.WriteTo.XUnit(testOutputHelper, jsonFormatter, loggerSettings.Test.MinimumLevel, loggerSettings.Test.LoggingLevelSwitch);
            }
            else
            {
                loggerConfiguration.WriteTo.XUnit(testOutputHelper, loggerSettings.Test.MinimumLevel, XUnitLoggerConfigurationExtensions.DefaultOutputTemplate, null, loggerSettings.Test.LoggingLevelSwitch);
            }

            return loggerConfiguration;
        }

        public static ILoggerFactory CreateXUnitSerilogFactory(ITestOutputHelper testOutputHelper, IPureLoggerSettings loggerSettings,
            XUnitSerilogFormatter xUnitSerilogFormatter = XUnitSerilogFormatter.None)
        {
            if (testOutputHelper == null) throw new ArgumentNullException(nameof(testOutputHelper));
            if (loggerSettings == null) throw new ArgumentNullException(nameof(loggerSettings));

            var loggerFactory = CreateSerilogFactory(loggerSettings, CreateXUnitLoggerConfiguration(testOutputHelper, loggerSettings, xUnitSerilogFormatter));

            loggerFactory.AddDebug();

            return loggerFactory;
        }
        */
    }
}
