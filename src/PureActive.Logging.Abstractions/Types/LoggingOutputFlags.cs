using System;

namespace PureActive.Logging.Abstractions.Types
{
    [Flags]
    public enum LoggingOutputFlags
    {
        Default = 0,
        Console = 1 << 0,
        RollingFile = 1 << 1,
        AppInsights = 1 << 2,
        XUnitConsole = 1 << 3,
        TestCorrelator = 1 << 4,

        Testing = XUnitConsole | TestCorrelator,
        AppConsoleFile = Console | RollingFile,
        AppFull = AppConsoleFile | AppInsights,
        TestingAppConsoleFile = Console | RollingFile | Testing
    }
}