// ***********************************************************************
// Assembly         : PureActive.Hosting
// Author           : SteveBu
// Created          : 11-02-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-02-2018
// ***********************************************************************
// <copyright file="WebAppEmail.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace PureActive.Hosting.Settings
{
    /// <summary>
    /// The e-mail address that messages will be sent from.
    /// (This is a separate type to enable dependency injection.)
    /// </summary>
    public class WebAppEmail
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        public WebAppEmail(string emailAddress)
        {
            EmailAddress = emailAddress;
        }

        /// <summary>
        /// The e-mail address that messages will be sent from.
        /// </summary>
        /// <value>The email address.</value>
        public string EmailAddress { get; }
    }
}