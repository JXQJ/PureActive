using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PureActive.Email.Office365.Interfaces;

namespace PureActive.Email.Office365.Providers
{
    /// <summary>
    ///     Sends mail messages.
    /// </summary>
    public class Office365EmailProvider : IEmailProvider
    {
        private readonly MailAddress _from;

        /// <summary>
        ///     The password for all messages
        /// </summary>
        private readonly string _password;

        /// <summary>
        ///     The username for all messages
        /// </summary>
        private readonly string _userName;


        /// <summary>
        ///     Constructor.
        /// </summary>
        public Office365EmailProvider(MailAddress from, string userName, string password)
        {
            _userName = userName;
            _password = password;
            _from = from;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Constructor.
        /// </summary>
        public Office365EmailProvider(string fromEmail, string fromName, string userName, string password) :
            this(new MailAddress(fromEmail, fromName), userName, password)

        {
        }

        public Task SendMessageAsync(MailAddress recipient, string subject, string body,
            bool isBodyHtml)
        {
            return SendMessageAsync(new List<MailAddress> {recipient}, subject, body, isBodyHtml);
        }

        public Task SendMessageAsync(string email, string displayName, string subject, string body,
            bool isBodyHtml)
        {
            return SendMessageAsync(new MailAddress(email, displayName), subject, body, isBodyHtml);
        }

        public Task SendMessageAsync(string email, string subject, string body,
            bool isBodyHtml)
        {
            return SendMessageAsync(new MailAddress(email), subject, body, isBodyHtml);
        }

        /// <summary>
        ///     Send a mail message.
        /// </summary>
        public async Task SendMessageAsync(
            IList<MailAddress> recipients,
            string subject,
            string body,
            bool isBodyHtml)
        {
            var tos = recipients.Select(r => new MailAddress(r.Address, r.DisplayName)).ToList();

            var client = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(_userName, _password)
            };

            var msg = new MailMessage
            {
                From = _from,
                Body = body,
                IsBodyHtml = isBodyHtml,
                BodyEncoding = Encoding.UTF8,
                Subject = subject,
                SubjectEncoding = Encoding.UTF8
            };

            // Add each to user
            foreach (var to in tos)
                if (to != null)
                    msg.To.Add(to);

            await client.SendMailAsync(msg);
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return SendMessageAsync(email, subject, htmlMessage, true);
        }

        /// <summary>
        ///     Creates a new Office365 mail provider.
        /// </summary>
        public static IEmailProvider CreateOffice365MailProvider(
            string fromEmail, string fromName, string userName, string password)
        {
            // string fromEmail, string fromName, string userName, string password

            return new Office365EmailProvider(fromEmail, fromName, userName, password);
        }

        /// <summary>
        ///     Creates a new Office365 mail provider from a configuration
        /// </summary>
        public static IEmailProvider CreateOffice365MailProvider(IConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return CreateOffice365MailProvider(
                configuration["Email:EmailAddress"],
                configuration["Email:EmailDisplayName"],
                configuration["Email:Providers:Office365:UserName"],
                configuration["Email:Providers:Office365:Password"]
            );
        }
    }
}