﻿// ***********************************************************************
// Assembly         : PureActive.Logging.Abstractions
// Author           : SteveBu
// Created          : 10-30-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="NullPureLogger.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Abstractions.Internal;
using PureActive.Logging.Abstractions.Interfaces;

namespace PureActive.Logging.Abstractions.Types
{
    /// <summary>
    /// Class NullPureLogger.
    /// Implements the <see cref="IPureLogger" />
    /// </summary>
    /// <seealso cref="IPureLogger" />
    /// <autogeneratedoc />
    public class NullPureLogger : IPureLogger
    {
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        /// <autogeneratedoc />
        public static NullPureLogger Instance { get; } = new NullPureLogger();

        /// <summary>
        /// Writes a log entry.
        /// </summary>
        /// <typeparam name="TState">The type of the t state.</typeparam>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="eventId">Id of the event.</param>
        /// <param name="state">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a <c>string</c> message of the <paramref name="state" /> and <paramref name="exception" />.</param>
        /// <autogeneratedoc />
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            NullLogger.Instance.Log(logLevel, eventId, state, exception, formatter);
        }

        /// <summary>
        /// Checks if the given <paramref name="logLevel" /> is enabled.
        /// </summary>
        /// <param name="logLevel">level to be checked.</param>
        /// <returns><c>true</c> if enabled.</returns>
        /// <autogeneratedoc />
        public bool IsEnabled(LogLevel logLevel)
        {
            return NullLogger.Instance.IsEnabled(logLevel);
        }

        /// <summary>
        /// Begins a logical operation scope.
        /// </summary>
        /// <typeparam name="TState">The type of the t state.</typeparam>
        /// <param name="state">The identifier for the scope.</param>
        /// <returns>An IDisposable that ends the logical operation scope on dispose.</returns>
        /// <autogeneratedoc />
        public IDisposable BeginScope<TState>(TState state)
        {
            return NullLogger.Instance.BeginScope(state);
        }

        /// <summary>
        /// Pushes the property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <param name="destructureObjects">if set to <c>true</c> [destructure objects].</param>
        /// <returns>IDisposable.</returns>
        /// <autogeneratedoc />
        public IDisposable PushProperty(string propertyName, object value, bool destructureObjects = false)
        {
            return BeginScope(new Dictionary<string, object> {{propertyName, value}});
        }

        /// <summary>
        /// Pushes the property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <param name="destructureObjects">if set to <c>true</c> [destructure objects].</param>
        /// <returns>IDisposable.</returns>
        /// <autogeneratedoc />
        public IDisposable PushProperty<T>(string propertyName, T value, bool destructureObjects = false)
        {
            return BeginScope(new Dictionary<string, T> {{propertyName, value}});
        }

        /// <summary>
        /// Pushes the log properties.
        /// </summary>
        /// <param name="logPropertyList">The log property list.</param>
        /// <returns>IDisposable.</returns>
        /// <autogeneratedoc />
        public IDisposable PushLogProperties(IEnumerable<IPureLogProperty> logPropertyList) => PushEmptyLogProperty();

        /// <summary>
        /// Pushes the log properties.
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <param name="destructureObjects">if set to <c>true</c> [destructure objects].</param>
        /// <returns>IDisposable.</returns>
        /// <autogeneratedoc />
        public IDisposable PushLogProperties(IEnumerable<KeyValuePair<string, object>> properties,
            bool destructureObjects = false) => PushEmptyLogProperty();

        /// <summary>
        /// Pushes the log properties.
        /// </summary>
        /// <param name="logPropertyList">The log property list.</param>
        /// <param name="minimumLogLevel">The minimum log level.</param>
        /// <returns>IDisposable.</returns>
        /// <autogeneratedoc />
        public IDisposable PushLogProperties(IEnumerable<IPureLogPropertyLevel> logPropertyList,
            LogLevel minimumLogLevel) => PushEmptyLogProperty();

        /// <summary>
        /// Pushes the log properties.
        /// </summary>
        /// <param name="logPropertyList">The log property list.</param>
        /// <param name="includeLogProperty">The include log property.</param>
        /// <returns>IDisposable.</returns>
        /// <autogeneratedoc />
        public IDisposable PushLogProperties(IEnumerable<IPureLogPropertyLevel> logPropertyList,
            Func<IPureLogPropertyLevel, bool> includeLogProperty) => PushEmptyLogProperty();

        /// <summary>
        /// Pushes the empty log property.
        /// </summary>
        /// <returns>IDisposable.</returns>
        /// <autogeneratedoc />
        private IDisposable PushEmptyLogProperty() => NullScope.Instance;
    }

    /// <summary>
    /// Class NullPureLogger.
    /// Implements the <see cref="NullPureLogger" />
    /// Implements the <see cref="Interfaces.IPureLogger{TCategory}" />
    /// </summary>
    /// <typeparam name="TCategory">The type of the t category.</typeparam>
    /// <seealso cref="NullPureLogger" />
    /// <seealso cref="Interfaces.IPureLogger{TCategory}" />
    /// <autogeneratedoc />
    [ExcludeFromCodeCoverage]
    public class NullPureLogger<TCategory> : NullPureLogger, IPureLogger<TCategory>
    {
    }
}