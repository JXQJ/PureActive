using System.IO;

namespace PureActive.Core.System
{
    /// <summary>
    ///     Manage.
    /// </summary>
    public static class ProcessUtils
    {
        public static bool CloseProcess(string processPath)
        {
            if (File.Exists(processPath)) return true;

            return false;
        }
    }
}