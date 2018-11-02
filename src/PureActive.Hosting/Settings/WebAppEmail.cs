namespace PureActive.Hosting.Settings
{
    /// <summary>
    ///     The e-mail address that messages will be sent from.
    ///     (This is a separate type to enable dependency injection.)
    /// </summary>
    public class WebAppEmail
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        public WebAppEmail(string emailAddress)
        {
            EmailAddress = emailAddress;
        }

        /// <summary>
        ///     The e-mail address that messages will be sent from.
        /// </summary>
        public string EmailAddress { get; }
    }
}