using System;
using System.Threading.Tasks;
using FluentAssertions;
using PureActive.Email.Office365.Providers;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Email.UnitTests
{
    [Trait("Category", "Integration")]
    public class EmailUnitTests : TestBaseLoggable<EmailUnitTests>
    {
        public EmailUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Fact]
        public async Task EmailUnitTests_Create_Provider()
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
