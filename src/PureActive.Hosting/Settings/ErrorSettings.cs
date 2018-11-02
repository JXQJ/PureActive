namespace PureActive.Hosting.Settings
{
    /// <summary>
    ///     Whether or not to show full error information.
    /// </summary>
    public class ErrorSettings
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        public ErrorSettings(bool showExceptions)
        {
            ShowExceptions = showExceptions;
        }

        /// <summary>
        ///     Whether or not to show full exceptions for errors.
        /// </summary>
        public bool ShowExceptions { get; }
    }
}