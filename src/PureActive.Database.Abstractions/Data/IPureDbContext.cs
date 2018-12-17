﻿// ***********************************************************************
// Assembly         : PureActive.Core.Abstractions
// Author           : SteveBu
// Created          : 12-16-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 12-16-2018
// ***********************************************************************
// <copyright file="IPureDbContext.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using PureActive.Database.Abstractions.Types;

namespace PureActive.Database.Abstractions.Data
{
    /// <summary>
    /// Interface IPureDbContext
    /// </summary>
    /// <autogeneratedoc />
    public interface IPureDbContext
    {

        /// <summary>
        /// Returns the Connection String associated with this database context
        /// </summary>
        /// <returns>Connection String</returns>
        string GetConnectionString();

        /// <summary>
        /// Truncates tableName
        /// </summary>
        /// <param name="tableName">Name of table to truncate</param>
        /// <param name="isCascading">True to perform a cascading update</param>
        /// <returns></returns>
        int TruncateTable(string tableName, bool isCascading = false);

        /// <summary>
        /// Features supported by database provider
        /// </summary>
        PureDbProviderFeature PureDbProviderFeatures { get; }

        /// <summary>
        ///     Applies any new database migrations, if needed (creating
        ///     and initializing the database if it does not yet exist).
        /// </summary>
        void ApplyDatabaseMigrations();
    }
}