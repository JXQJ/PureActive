namespace PureActive.Hosting.Abstractions.Types
{
    /// <summary>
    /// Service database configuration hosted on localhost or Docker container.
    /// </summary>
    public enum ServiceDatabaseConfig
    {
        /// <summary>
        /// Database service hosted on localhost
        /// </summary>
        LocalHost,
        /// <summary>
        /// Database service hosted in a Docker container
        /// </summary>
        Docker
    }
}