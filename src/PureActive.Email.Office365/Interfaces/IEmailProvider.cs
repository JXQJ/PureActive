using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace PureActive.Email.Office365.Interfaces
{
    /// <summary>
    ///     Sends mail messages.
    /// </summary>
    public interface IEmailProvider : IEmailSender
    {
        /// <summary>
        ///     Send a mail message.
        /// </summary>
        Task SendMessageAsync(
            MailAddress recipient,
            string subject,
            string body,
            bool isBodyHtml);

        /// <summary>
        ///     Send a mail message using strings.
        /// </summary>
        Task SendMessageAsync(
            string email,
            string displayName,
            string subject,
            string body,
            bool isBodyHtml);

        /// <summary>
        ///     Send a mail message using strings.
        /// </summary>
        Task SendMessageAsync(
            string email,
            string subject,
            string body,
            bool isBodyHtml);

        /// <summary>
        ///     Send a mail message.
        /// </summary>
        Task SendMessageAsync(
            IList<MailAddress> recipients,
            string subject,
            string body,
            bool isBodyHtml);
    }
}