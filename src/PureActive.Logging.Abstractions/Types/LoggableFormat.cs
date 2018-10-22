using System;

namespace PureActive.Logging.Abstractions.Types
{
    [Flags]
    public enum LoggableFormat : byte
    {
        None = 0,
        ToString = 1 << 0,
        ToLog = 1 << 1,
        WithParents = 1 << 2,
        ToLogWithParents = ToLog | WithParents,
        ToStringWithParents = ToString | WithParents,
    }

    public static class LoggableFormatUtils
    {
        public static bool IsWithParents(this LoggableFormat loggableFormat) =>
            (loggableFormat & LoggableFormat.WithParents) > 0;

        public static bool IsToString(this LoggableFormat loggableFormat) =>
            (loggableFormat & LoggableFormat.ToString) > 0;

        public static bool IsToLog(this LoggableFormat loggableFormat) =>
            (loggableFormat & LoggableFormat.ToLog) > 0;
    }
}
