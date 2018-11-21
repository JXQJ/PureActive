// ***********************************************************************
// Assembly         : PureActive.Serilog.Sink.Xunit
// Author           : SteveBu
// Created          : 11-21-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-21-2018
// ***********************************************************************
// <copyright file="TestLoggerObserver.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary>
//      Reactive Observer that uses the ITestOutputHelper to write to the 
//      xUnit console.
// </summary>
// ***********************************************************************
using System;
using Microsoft.Extensions.Logging;
using PureActive.Logging.Abstractions.Interfaces;

namespace PureActive.Serilog.Sink.Xunit.Observers
{
    /// <summary>Class TestLoggerObserver.
    /// Implements the <see cref="T:System.IObserver`1"/> to log to xUnit console</summary>
    /// <typeparam name="T">Type of observed object</typeparam>
    /// <seealso cref="System.IObserver{T}"/>
    public class TestLoggerObserver<T> : IObserver<T>
    {
        /// <summary>
        /// The optional name of the observable
        /// </summary>
        private readonly string _name;

        /// <summary>IPureLogger based on ITestOutputHelper interface</summary>
        private readonly IPureLogger _logger;

        /// <summary>
        /// Message template string for OnNext event
        /// </summary>
        public const string OnNextMsgTemplate = "{0} - OnNext({1})";

        /// <summary>
        /// Message template string for OnCompleted event
        /// </summary>
        public const string OnCompletedMsgTemplate = "{0} - OnCompleted()";

        /// <summary>
        /// Message template string for OnError event
        /// </summary>
        public const string OnErrorMsgTemplate = "{0} - OnError:";

        /// <summary>
        /// Initializes a new instance of the <see cref="TestLoggerObserver{T}"/> class.
        /// </summary>
        /// <param name="logger">test logger using ITestOutputHelper</param>
        /// <param name="name">optional name of the observable.</param>
        /// <exception cref="System.ArgumentNullException">logger</exception>
        public TestLoggerObserver(IPureLogger logger, string name = "")
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _name = name;
        }

        /// <summary>
        /// Provides the observer with new data.
        /// </summary>
        /// <param name="value">The current notification information.</param>
        public void OnNext(T value)
        {
            _logger.LogDebug(OnNextMsgTemplate, _name, value);
        }

        /// <summary>
        /// Notifies the observer that the provider has experienced an error condition.
        /// </summary>
        /// <param name="error">An object that provides additional information about the error.</param>
        public void OnError(Exception error)
        {
            _logger.LogDebug(OnErrorMsgTemplate, _name);
            _logger.LogDebug("\t {0}", error);
        }

        /// <summary>
        /// Notifies the observer that the provider has finished sending push-based notifications.
        /// </summary>
        public void OnCompleted()
        {
            _logger.LogDebug(OnCompletedMsgTemplate, _name);
        }
    }
}