using System;
using System.Threading.Tasks;
using FluentAssertions;
using PureActive.Email.Office365.Providers;
using Xunit;

namespace PureActive.Email.UnitTests
{
    public class EmailUnitTests
    {
        [Fact]
        public async Task Email_Create_Provider()
        {
            try
            {
                var office365EmailProvider = new Office365EmailProvider("stevebu@bushchang.com", "Steve Bush", "stevebu@bushchang.com", "*");

                await office365EmailProvider.SendEmailAsync("stevebu@stevebu.com", "Testing Email", "<HTML><BODY>Testing</BODY></HTML>");
            }
            catch (Exception e)
            {
                e.Should().BeOfType<System.Net.Mail.SmtpException>();
            }
        }
    }
}
