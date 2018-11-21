// ***********************************************************************
// Assembly         : PureActive.Email.Office365
// Author           : SteveBu
// Created          : 11-01-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-01-2018
// ***********************************************************************
// <copyright file="IEmailProvider.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace PureActive.Email.Office365.Interfaces
{
    /// <summary>
    /// Sends mail messages.
    /// Implements the <see cref="Microsoft.AspNetCore.Identity.UI.Services.IEmailSender" />
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Identity.UI.Services.IEmailSender" />
    public interface IEmailProvider : IEmailSender
    {
        /// <summary>
        /// Send a mail message.
        /// </summary>
        /// <param name="recipient">The recipient.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="isBodyHtml">if set to <c>true</c> [is body HTML].</param>
        /// <returns>Task.</returns>
        Task SendMessageAsync(
            MailAddress recipient,
            string subject,
            string body,
            bool isBodyHtml);

        /// <summary>
        /// Send a mail message using strings.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="isBodyHtml">if set to <c>true</c> [is body HTML].</param>
        /// <returns>Task.</returns>
        Task SendMessageAsync(
            string email,
            string displayName,
            string subject,
            string body,
            bool isBodyHtml);

        /// <summary>
        /// Send a mail message using strings.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="isBodyHtml">if set to <c>true</c> [is body HTML].</param>
        /// <returns>Task.</returns>
        Task SendMessageAsync(
            string email,
            string subject,
            string body,
            bool isBodyHtml);

        /// <summary>
        /// Send a mail message.
        /// </summary>
        /// <param name="recipients">The recipients.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="isBodyHtml">if set to <c>true</c> [is body HTML].</param>
        /// <returns>Task.</returns>
        Task SendMessageAsync(
            IList<MailAddress> recipients,
            string subject,
            string body,
            bool isBodyHtml);
    }
}