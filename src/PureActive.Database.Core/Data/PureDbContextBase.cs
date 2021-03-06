﻿// ***********************************************************************
// Assembly         : PureActive.Database.Core
// Author           : SteveBu
// Created          : 12-16-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 12-16-2018
// ***********************************************************************
// <copyright file="PureDbContextBase.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Microsoft.EntityFrameworkCore;
using PureActive.Database.Abstractions.Data;
using PureActive.Database.Abstractions.Types;

namespace PureActive.Database.Core.Data
{
    /// <summary>
    /// Class PureDbContextBase.
    /// Implements the <see cref="Microsoft.EntityFrameworkCore.DbContext" />
    /// </summary>
    /// <typeparam name="TContext">The type of the t context.</typeparam>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    /// <autogeneratedoc />
    public abstract class PureDbContextBase<TContext> : DbContext, IPureDbContext
    {
        /// <inheritdoc />
        public PureDbProviderFeature PureDbProviderFeatures { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PureDbContextBase{TContext}"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="pureDbProviderFeatures">features supported by database provider</param>
        /// <autogeneratedoc />
        protected PureDbContextBase(DbContextOptions<PureDbContextBase<TContext>> options, PureDbProviderFeature pureDbProviderFeatures)
            : base(options)
        {
            PureDbProviderFeatures = pureDbProviderFeatures;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="PureDbContextBase{TContext}"/> class.
        /// </summary>
        /// <param name="pureDbProviderFeatures">features supported by database provider</param>
        protected PureDbContextBase(PureDbProviderFeature pureDbProviderFeatures)
        : this(new DbContextOptions<PureDbContextBase<TContext>>(), pureDbProviderFeatures)
        {
   
        }

        /// <inheritdoc />
        public abstract string GetConnectionString();

        /// <inheritdoc />
        public abstract int TruncateTable(string tableName, bool isCascading = false);

        /// <inheritdoc />
        public virtual void ApplyDatabaseMigrations()
        {
            Database.Migrate();
        }
    }
}
