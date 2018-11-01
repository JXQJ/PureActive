using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions.Internal;
using PureActive.Core.Abstractions.System;

namespace PureActive.Core.System
{
    /// <summary>
    ///     Provides access to the file system.
    /// </summary>
    public class FileSystem : IFileSystem
    {
        /// <summary>
        ///     The buffer size for a file stream.
        /// </summary>
        private const int CBufferSize = 4000;
        public IOperatingSystem OperatingSystem { get; }

        public string AppFolderName { get; set; }

        public FileSystem(IConfigurationRoot configuration, IOperatingSystem operatingSystem) : this(
            configuration?.GetSection("AppSettings")?["AppFolderName"], operatingSystem)
        {

        }

        public FileSystem(string appFolderName, IOperatingSystem operatingSystem)
        {
            AppFolderName = appFolderName ?? throw new ArgumentNullException(nameof(appFolderName));
            OperatingSystem = operatingSystem ?? throw new ArgumentNullException(nameof(operatingSystem));
        }

        public FileSystem(Type type, IOperatingSystem operatingSystem)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            AppFolderName = TypeNameHelper.GetTypeDisplayName(type).Replace(".", "/");
            OperatingSystem = operatingSystem ?? throw new ArgumentNullException(nameof(operatingSystem));
        }

        public FileSystem(Type type) : this(type, new OperatingSystem())
        {

        }

        public FileSystem(string appFolderName) : this(appFolderName, new OperatingSystem())
        {

        }

        public FileSystem(IConfigurationRoot configuration) : this(configuration, new OperatingSystem())
        {

        }

        /// <summary>
        ///     Creates a new temporary file, and returns the corresponding
        ///     stream. The file is deleted when the stream is closed.
        /// </summary>
        public Stream CreateNewTempFile()
        {
            return new FileStream
            (
                Path.GetTempFileName(),
                FileMode.OpenOrCreate,
                FileAccess.ReadWrite,
                FileShare.ReadWrite,
                CBufferSize,
                FileOptions.DeleteOnClose
            );
        }

        /// <summary>
        ///     Reads the contents of a file.
        /// </summary>
        public async Task<string> ReadFileContentsAsync(string path)
        {
            var stream = new FileStream
            (
                path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read
            );

            using (var streamReader = new StreamReader(stream))
            {
                return await streamReader.ReadToEndAsync();
            }
        }

        /// <summary>
        ///     Writes contents to a file.
        /// </summary>
        public async Task WriteFileContentsAsync(string path, string contents)
        {
            var stream = new FileStream
            (
                path,
                FileMode.Create,
                FileAccess.Write,
                FileShare.Read
            );

            using (var streamWriter = new StreamWriter(stream))
            {
                await streamWriter.WriteAsync(contents);
            }
        }

        /// <summary>
        ///     Creates the given folder.
        /// </summary>
        public void CreateFolder(string path)
        {
            Directory.CreateDirectory(path);
        }

        /// <summary>
        ///     Deletes the given folder.
        /// </summary>
        public void DeleteFolder(string path)
        {
            Directory.Delete(path, true);
        }

        /// <summary>
        ///     Returns a Special folder path given a special folder type
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public string GetSpecialFolderPath(Environment.SpecialFolder folder)
        {
            return Environment.GetFolderPath(folder);
        }

        /// <summary>
        ///     Returns a Special folder path given a special folder type, allows option to verify or create
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public string GetSpecialFolderPath(Environment.SpecialFolder folder, Environment.SpecialFolderOption option)
        {
            return Environment.GetFolderPath(folder, option);
        }

        private string ProcessSpecialFolder(Environment.SpecialFolderOption option, string folderPath)
        {
            switch (option)
            {
                case Environment.SpecialFolderOption.DoNotVerify:
                    break;

                case Environment.SpecialFolderOption.None:
                    {
                        if (!Directory.Exists(folderPath))
                            return "";

                        break;
                    }

                case Environment.SpecialFolderOption.Create:
                    {
                        Directory.CreateDirectory(folderPath);

                        break;
                    }
            }

            return folderPath;
        }

        /// <summary>
        ///     Returns the path for storing application data common to all users, allows option to verify or create
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public string GetCommonApplicationDataFolderPath(Environment.SpecialFolderOption option)
        {

            if (OperatingSystem.IsOsx())
            {
                return ProcessSpecialFolder(option, "/Users/Shared/");
            }

            return GetSpecialFolderPath(Environment.SpecialFolder.CommonApplicationData, option);
        }

        /// <summary>
        ///     Returns the path for storing application data common to all users
        /// </summary>
        /// <returns></returns>
        public string GetCommonApplicationDataFolderPath()
        {
            return GetSpecialFolderPath(Environment.SpecialFolder.CommonApplicationData);
        }


        /// <summary>
        ///     Returns the path for storing application locally for the current user, allows option to verify or create
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public string GetLocalApplicationDataFolderPath(Environment.SpecialFolderOption option)
        {
            return GetSpecialFolderPath(Environment.SpecialFolder.LocalApplicationData, option);
        }

        /// <summary>
        ///     Returns the path for storing application locally for the current user
        /// </summary>
        /// <returns></returns>
        public string GetLocalApplicationDataFolderPath()
        {
            return GetSpecialFolderPath(Environment.SpecialFolder.LocalApplicationData);
        }


        public string GetCurrentApplicationDataFolderPath()
        {
            var commonApplicationFolder =
                GetCommonApplicationDataFolderPath(Environment.SpecialFolderOption.Create) + $"/{AppFolderName}/";

            Directory.CreateDirectory(commonApplicationFolder);

            return commonApplicationFolder;
        }


        /// <summary>
        ///     Returns folder path for logs
        /// </summary>
        /// <returns></returns>
        public string LogFolderPath()
        {
            var logFolderPath = GetCurrentApplicationDataFolderPath() + "/Logs/";

            Directory.CreateDirectory(logFolderPath);

            return logFolderPath;
        }

        /// <summary>
        ///     Returns folder path for test logs
        /// </summary>
        /// <returns></returns>
        public string TestLogFolderPath()
        {
            var logFolderPath = GetCurrentApplicationDataFolderPath() + "/Logs/Test/";

            Directory.CreateDirectory(logFolderPath);

            return logFolderPath;
        }

        /// <summary>
        ///     Returns folder path for data shared by all apps
        /// </summary>
        /// <returns></returns>
        public string DataFolderPath()
        {
            var databaseFolderPath = GetCurrentApplicationDataFolderPath() + "/Data/";

            Directory.CreateDirectory(databaseFolderPath);

            return databaseFolderPath;
        }

        public string ArpCommandPath()
        {
            if (OperatingSystem.IsWindows())
            {
                return GetSpecialFolderPath(Environment.SpecialFolder.System) + "\\arp.exe";
            }
            else
            {
                return "/usr/sbin/arp";
            }
        }
    }
}