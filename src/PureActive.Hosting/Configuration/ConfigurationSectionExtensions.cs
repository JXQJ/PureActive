using Microsoft.Extensions.Configuration;
using PureActive.Hosting.Settings;

namespace PureActive.Hosting.Configuration
{
    public static class ConfigurationSectionExtensions
    {
        /// <summary>
        ///     Returns the hostname of the web app.
        /// </summary>
        public static WebAppHost GetHostName(IConfigurationSection webAppSettings)
        {
            return new WebAppHost(webAppSettings["HostName"]);
        }

        /// <summary>
        ///     Returns the from address for e-mails sent from the web-app.
        /// </summary>
        public static WebAppEmail GetEmailAddress(IConfigurationSection webAppSettings)
        {
            return new WebAppEmail(webAppSettings["EmailAddress"]);
        }

        /// <summary>
        ///     Returns settings about how to display errors to users.
        /// </summary>
        public static ErrorSettings GetErrorSettings(IConfigurationSection webAppSettings)
        {
            return new ErrorSettings(webAppSettings.GetValue<bool>("ShowExceptions"));
        }
    }
}