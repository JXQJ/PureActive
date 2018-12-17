﻿// ***********************************************************************
// Assembly         : PureActive.Database.SQLite
// Author           : SteveBu
// Created          : 12-17-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 12-17-2018
// ***********************************************************************
// <copyright file="PureDbContextSQLite.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using Microsoft.EntityFrameworkCore;
using PureActive.Core.Abstractions.System;
using PureActive.Core.System;
using PureActive.Database.Abstractions.Types;
using PureActive.Database.Core.Data;
using PureActive.Database.SQLite.Interfaces;
using PureActive.Hosting.Abstractions.Types;

namespace PureActive.Database.SQLite.Provider
{
    /// <summary>
    /// Class PureDbContextSQLite.
    /// Implements the <see cref="Core.Data.PureDbContextBase{TContext}" />
    /// </summary>
    /// <seealso cref="Core.Data.PureDbContextBase{TContext}" />
    /// <autogeneratedoc />
    public class PureDbContextSQLite<TContext> : PureDbContextBase<TContext>, IPureDbContextSQLite
    {
        /// <summary>
        /// The file system
        /// </summary>
        /// <autogeneratedoc />
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// The connection string
        /// </summary>
        /// <autogeneratedoc />
        private readonly string _connectionString;

        /// <summary>
        /// Constructor for PureDbContextSQLite
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <exception cref="ArgumentNullException">fileSystem</exception>
        public PureDbContextSQLite(IFileSystem fileSystem, string connectionString) : 
            base(PureDbProviderFeature.None)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _connectionString = connectionString;
        }

        /// <summary>
        /// Constructor for PureDbContextSQLite
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PureDbContextSQLite(string connectionString) : this(new FileSystem(), connectionString)
        {

        }

        /// <summary>
        /// Constructor for PureDbContextSQLite
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="serviceHost">The service host.</param>
        /// <param name="fileNameRoot">The file name root.</param>
        public PureDbContextSQLite(IFileSystem fileSystem, ServiceHost serviceHost, string fileNameRoot) : 
            this(fileSystem, GetConnectionString(fileSystem, serviceHost, fileNameRoot))
        {

        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="serviceHost">The service host.</param>
        /// <param name="fileNameRoot">The file name root.</param>
        /// <returns>System.String.</returns>
        /// <autogeneratedoc />
        public static string GetConnectionString(IFileSystem fileSystem, ServiceHost serviceHost, string fileNameRoot)
        {
            return GetConnectionString(fileSystem, serviceHost.ToString(), fileNameRoot);
        }

        /// <summary>Gets the connection string.</summary>
        /// <param name="serviceHost">The service host.</param>
        /// <param name="fileNameRoot">The file name root.</param>
        /// <returns>System.String.</returns>
        /// <autogeneratedoc />
        public string GetConnectionString(ServiceHost serviceHost, string fileNameRoot)
        {
            return GetConnectionString(_fileSystem, serviceHost.ToString(), fileNameRoot);
        }

        /// <summary>
        /// Gets the database path.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="folderName">Name of the folder.</param>
        /// <returns>System.String.</returns>
        /// <autogeneratedoc />
        public static string GetDatabasePath(IFileSystem fileSystem, string folderName)
        {
            var databasePath = $"{fileSystem.DataFolderPath()}/{folderName}";
            fileSystem.CreateFolder(databasePath);

            return databasePath;
        }

        /// <summary>
        /// Gets the name of the database file.
        /// </summary>
        /// <param name="fileNameRoot">The file name root.</param>
        /// <returns>System.String.</returns>
        /// <autogeneratedoc />
        public static string GetDatabaseFileName(string fileNameRoot)
        {
            return $"{fileNameRoot}.db";
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="folderName">Name of the folder.</param>
        /// <param name="fileNameRoot">The file name root.</param>
        /// <returns>System.String.</returns>
        /// <autogeneratedoc />
        public static string GetConnectionString(IFileSystem fileSystem, string folderName, string fileNameRoot)
        {
            return $"Data Source={GetDatabasePath(fileSystem, folderName)}/{GetDatabaseFileName(fileNameRoot)};";
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <returns>System.String.</returns>
        /// <inheritdoc />
        /// <autogeneratedoc />
        public override string GetConnectionString()
        {
            return _connectionString;
        }

        /// <summary>
        /// Truncates the table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="isCascading">if set to <c>true</c> [is cascading].</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <inheritdoc />
        /// <autogeneratedoc />
        public override int TruncateTable(string tableName, bool isCascading = false)
        {
            throw new NotImplementedException();
        }

        ///// <summary>
        ///// Truncates table with tableName
        ///// </summary>
        ///// <param name="tableName"></param>
        ///// <param name="isCascading">True if truncate is cascading</param>
        ///// <returns></returns>
        //protected int TruncateTable(string tableName, bool isCascading = false)
        //{
        //    var cascadeParam = isCascading ? " CASCADE" : string.Empty;

        //    return Database.ExecuteSqlCommand($"TRUNCATE TABLE \"{DefaultSchema}\".\"{tableName}\"{cascadeParam}");
        //}

        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(GetConnectionString());
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // builder.HasDefaultSchema("foo");

            base.OnModelCreating(builder);

        }
    }
}
