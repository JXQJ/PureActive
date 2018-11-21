// ***********************************************************************
// Assembly         : PureActive.Hosting
// Author           : SteveBu
// Created          : 11-02-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-02-2018
// ***********************************************************************
// <copyright file="ErrorSettings.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace PureActive.Hosting.Settings
{
    /// <summary>
    /// Whether or not to show full error information.
    /// </summary>
    public class ErrorSettings
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="showExceptions">if set to <c>true</c> [show exceptions].</param>
        public ErrorSettings(bool showExceptions)
        {
            ShowExceptions = showExceptions;
        }

        /// <summary>
        /// Whether or not to show full exceptions for errors.
        /// </summary>
        /// <value><c>true</c> if [show exceptions]; otherwise, <c>false</c>.</value>
        public bool ShowExceptions { get; }
    }
}