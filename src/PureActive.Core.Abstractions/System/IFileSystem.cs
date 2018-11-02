using System;
using System.IO;
using System.Threading.Tasks;

namespace PureActive.Core.Abstractions.System
{
    /// <summary>
    ///     Provides access to the file system.
    /// </summary>
    public interface IFileSystem
    {
        IOperatingSystem OperatingSystem { get; }

        /// <summary>
        ///     Creates a new temporary file, and returns the corresponding
        ///     stream. The file is deleted when the stream is closed.
        /// </summary>
        Stream CreateNewTempFile();

        /// <summary>
        ///     Reads the contents of a file.
        /// </summary>
        Task<string> ReadFileContentsAsync(string path);

        /// <summary>
        ///     Writes contents to a file.
        /// </summary>
        Task WriteFileContentsAsync(string path, string contents);

        /// <summary>
        ///     Creates the given folder.
        /// </summary>
        void CreateFolder(string path);

        /// <summary>
        ///     Deletes the given folder.
        /// </summary>
        void DeleteFolder(string path);

        /// <summary>
        ///     Returns a Special folder path given a special folder type
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        string GetSpecialFolderPath(Environment.SpecialFolder folder);

        /// <summary>
        ///     Returns a Special folder path given a special folder type and option
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        string GetSpecialFolderPath(Environment.SpecialFolder folder, Environment.SpecialFolderOption option);

        /// <summary>
        ///     Returns the path for storing application data common to all users
        /// </summary>
        /// <returns></returns>
        string GetCommonApplicationDataFolderPath();

        /// <summary>
        ///     Returns the path for storing application data common to all users, allows option to verify or create
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        string GetCommonApplicationDataFolderPath(Environment.SpecialFolderOption option);

        /// <summary>
        ///     Returns the path for storing application locally for the current user
        /// </summary>
        /// <returns></returns>
        string GetLocalApplicationDataFolderPath();

        /// <summary>
        ///     Returns the path for storing application locally for the current user, allows option to verify or create
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        string GetLocalApplicationDataFolderPath(Environment.SpecialFolderOption option);

        string LogFolderPath();

        string TestLogFolderPath();

        string DataFolderPath();

        string AppFolderName { get; set; }

        string ArpCommandPath();

        string AssemblyFolder { get; }

        string SettingsFolder { get; }
    }
}