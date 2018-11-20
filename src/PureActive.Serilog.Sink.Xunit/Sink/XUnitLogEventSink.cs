using System;
using System.IO;
using System.Text;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using Xunit.Abstractions;

namespace PureActive.Serilog.Sink.Xunit.Sink
{
    public class XUnitLogEventSink : ILogEventSink
    {
        const int DefaultWriteBuffer = 256;

        private readonly ITextFormatter _formatter;
        private readonly ITestOutputHelper _testOutputHelper;

        public XUnitLogEventSink(ITestOutputHelper testOutputHelper, string outputTemplate,
            IFormatProvider formatProvider)
        {
            _testOutputHelper = testOutputHelper;
            _formatter = new MessageTemplateTextFormatter(outputTemplate, formatProvider);
        }

        public XUnitLogEventSink(ITestOutputHelper testOutputHelper, ITextFormatter formatter)
        {
            _testOutputHelper = testOutputHelper;
            _formatter = formatter;
        }

        public void Emit(LogEvent logEvent)
        {
            var writer = new StringWriter(new StringBuilder(DefaultWriteBuffer)) {NewLine = ""};

            _formatter.Format(logEvent, writer);

            _testOutputHelper.WriteLine(writer.ToString());
        }
    }
}