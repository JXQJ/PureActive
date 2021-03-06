﻿// ***********************************************************************
// Assembly         : PureActive.Serilog.Sink.Xunit
// Author           : SteveBu
// Created          : 10-23-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="XUnitLogEventSink.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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
    /// <summary>
    /// Class XUnitLogEventSink.
    /// Implements the <see cref="ILogEventSink" />
    /// </summary>
    /// <seealso cref="ILogEventSink" />
    /// <autogeneratedoc />
    public class XUnitLogEventSink : ILogEventSink
    {
        /// <summary>
        /// The default write buffer
        /// </summary>
        /// <autogeneratedoc />
        const int DefaultWriteBuffer = 256;

        /// <summary>
        /// The formatter
        /// </summary>
        /// <autogeneratedoc />
        private readonly ITextFormatter _formatter;
        /// <summary>
        /// The test output helper
        /// </summary>
        /// <autogeneratedoc />
        private readonly ITestOutputHelper _testOutputHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="XUnitLogEventSink"/> class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        /// <param name="outputTemplate">The output template.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <autogeneratedoc />
        public XUnitLogEventSink(ITestOutputHelper testOutputHelper, string outputTemplate,
            IFormatProvider formatProvider)
        {
            _testOutputHelper = testOutputHelper;
            _formatter = new MessageTemplateTextFormatter(outputTemplate, formatProvider);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XUnitLogEventSink"/> class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        /// <param name="formatter">The formatter.</param>
        /// <autogeneratedoc />
        public XUnitLogEventSink(ITestOutputHelper testOutputHelper, ITextFormatter formatter)
        {
            _testOutputHelper = testOutputHelper;
            _formatter = formatter;
        }

        /// <summary>
        /// Emits the specified log event.
        /// </summary>
        /// <param name="logEvent">The log event.</param>
        /// <autogeneratedoc />
        public void Emit(LogEvent logEvent)
        {
            var writer = new StringWriter(new StringBuilder(DefaultWriteBuffer)) {NewLine = ""};

            _formatter.Format(logEvent, writer);

            _testOutputHelper.WriteLine(writer.ToString());
        }
    }
}