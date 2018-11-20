using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using PureActive.Core.Extensions;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Logging.Abstractions.Types;

namespace PureActive.Logging.Extensions.Types
{
    public class PureLogPropertyLevel : PureLogProperty, IPureLogPropertyLevel
    {
        public PureLogPropertyLevel(KeyValuePair<string, object> keyValuePair, LogLevel minimumLogLevel,
            bool destructureObjects = false) :
            base(keyValuePair, destructureObjects)
        {
            MinimumLogLevel = minimumLogLevel;
        }

        public PureLogPropertyLevel(string key, object value, LogLevel minimumLogLevel,
            bool destructureObjects = false) :
            this(new KeyValuePair<string, object>(key, value), minimumLogLevel, destructureObjects)
        {
        }

        public LogLevel MinimumLogLevel { get; }

        public static void FormatPropertyList(StringBuilder sb, LoggableFormat loggableFormat,
            IEnumerable<IPureLogPropertyLevel> logPropertyEnumerable, LogLevel minimumLogLevel = LogLevel.Debug)
        {
            if (sb == null) throw new ArgumentNullException(nameof(sb));
            if (logPropertyEnumerable == null) throw new ArgumentNullException(nameof(logPropertyEnumerable));

            var logPropertyList = logPropertyEnumerable.ToList();
            var maxLength = logPropertyList.Select(p => p.Key).MaxStringLength() + 2;

            foreach (var logPropertyListItem in logPropertyList)
            {
                if (logPropertyListItem.MinimumLogLevel.CompareTo(minimumLogLevel) >= 0)
                {
                    sb.AppendLine(
                        $"{logPropertyListItem.Key.PadWithDelim(": ", maxLength)}{logPropertyListItem.Value}");
                }
            }
        }
    }
}