﻿// ***********************************************************************
// Assembly         : PureActive.Database.Abstractions
// Author           : SteveBu
// Created          : 12-17-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 12-17-2018
// ***********************************************************************
// <copyright file="PureDbProviderFeature.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace PureActive.Database.Abstractions.Types
{
    /// <summary>
    /// Enum PureDbProviderFeature
    /// </summary>
    /// <autogeneratedoc />
    [Flags]
    public enum PureDbProviderFeature
    {
        /// <summary>
        /// No features
        /// </summary>
        /// <autogeneratedoc />
        None = 0,
        /// <summary>
        /// The default schema
        /// </summary>
        /// <autogeneratedoc />
        DefaultSchema = 1 << 0,
        /// <summary>
        /// The cascading updates
        /// </summary>
        /// <autogeneratedoc />
        CascadingUpdates = 1 << 1,



    }

    /// <summary>
    /// Class PureDbProviderFeatureExtensions.
    /// </summary>
    /// <autogeneratedoc />
    public static class PureDbProviderFeatureExtensions
    {
        /// <summary>
        /// Supportses the default schema.
        /// </summary>
        /// <param name="pureDbProviderFeature">The pure database provider feature.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <autogeneratedoc />
        public static bool SupportsDefaultSchema(this PureDbProviderFeature pureDbProviderFeature) =>
            pureDbProviderFeature.HasFlag(PureDbProviderFeature.DefaultSchema);

        /// <summary>
        /// Supportses the cascading updates.
        /// </summary>
        /// <param name="pureDbProviderFeature">The pure database provider feature.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <autogeneratedoc />
        public static bool SupportsCascadingUpdates(this PureDbProviderFeature pureDbProviderFeature) =>
            pureDbProviderFeature.HasFlag(PureDbProviderFeature.CascadingUpdates);
    }
}