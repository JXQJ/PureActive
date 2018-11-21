﻿// ***********************************************************************
// Assembly         : PureActive.Logging.Abstractions
// Author           : SteveBu
// Created          : 10-20-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="IPureLoggerFactory.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Microsoft.Extensions.Logging;

namespace PureActive.Logging.Abstractions.Interfaces
{
    /// <summary>
    /// Interface IPureLoggerFactory
    /// Implements the <see cref="ILoggerFactory" />
    /// </summary>
    /// <seealso cref="ILoggerFactory" />
    /// <autogeneratedoc />
    public interface IPureLoggerFactory : ILoggerFactory
    {
        /// <summary>
        /// Gets the wrapped logger factory.
        /// </summary>
        /// <value>The wrapped logger factory.</value>
        /// <autogeneratedoc />
        ILoggerFactory WrappedLoggerFactory { get; }

        /// <summary>
        /// Gets the pure logger settings.
        /// </summary>
        /// <value>The pure logger settings.</value>
        /// <autogeneratedoc />
        IPureLoggerSettings PureLoggerSettings { get; }

        /// <summary>
        /// Creates a new <see cref="T:PureActive.Logging.Abstractions.Interfaces.IPureLogger" /> instance.
        /// </summary>
        /// <param name="categoryName">The category name for messages produced by the logger.</param>
        /// <returns>The <see cref="T:PureActive.Logging.Abstractions.Interfaces.IPureLogger" />.</returns>
        IPureLogger CreatePureLogger(string categoryName);

        /// <summary>
        /// Creates the pure logger.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>IPureLogger&lt;T&gt;.</returns>
        /// <autogeneratedoc />
        IPureLogger<T> CreatePureLogger<T>();

        /// <summary>
        /// Creates the logger.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>ILogger&lt;T&gt;.</returns>
        /// <autogeneratedoc />
        ILogger<T> CreateLogger<T>();
    }
}