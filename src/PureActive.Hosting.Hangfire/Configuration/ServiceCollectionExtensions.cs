// ***********************************************************************
// Assembly         : PureActive.Hosting
// Author           : SteveBu
// Created          : 11-03-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="ServiceCollectionExtensions.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Threading;
using Hangfire;
using Hangfire.SQLite;
using Microsoft.Extensions.DependencyInjection;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Queue.Hangfire.Queue;

namespace PureActive.Hosting.Hangfire.Configuration
{
    /// <summary>
    /// Extension methods for configuring the logger factory.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// How long to wait before retrying if the database does not exist.
        /// </summary>
        private static readonly TimeSpan CStorageRetryDelay = TimeSpan.FromMinutes(1);

        /// <summary>
        /// Registers the hangfire queue.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public static void AddHangfireQueue(
            this IServiceCollection services,
            string connectionString,
            IPureLoggerFactory loggerFactory)
        {
            services.AddHangfire
            (
                config =>
                {
                    var logProvider = new HangfireLogProvider(loggerFactory);
                    var storage = RetryGetHangfireStorage(connectionString);

                    config
                        .UseLogProvider(logProvider)
                        .UseStorage(storage);
                }
            );
        }


        /// <summary>
        /// Makes one attempt to retry connecting to the database after a failed attempt,
        /// before giving up.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>SQLiteStorage.</returns>
        private static SQLiteStorage RetryGetHangfireStorage(string connectionString)
        {
            try
            {
                return GetHangfireStorage(connectionString);
            }
            catch
            {
                // TODO: Fix logic for SQLite
                Thread.Sleep(CStorageRetryDelay);

                return GetHangfireStorage(connectionString);
            }
        }


        /// <summary>
        /// Returns the storage configuration for Hangfire.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>SQLiteStorage.</returns>
        private static SQLiteStorage GetHangfireStorage(string connectionString)
        {
            return new SQLiteStorage
            (
                connectionString,
                new SQLiteStorageOptions
                {
                    QueuePollInterval = TimeSpan.FromSeconds(1)
                }
            );
        }
    }
}