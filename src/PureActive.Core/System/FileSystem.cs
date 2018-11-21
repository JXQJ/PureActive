﻿// ***********************************************************************
// Assembly         : PureActive.Core
// Author           : SteveBu
// Created          : 10-22-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="FileSystem.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions.Internal;
using PureActive.Core.Abstractions.System;

namespace PureActive.Core.System
{
    /// <summary>
    /// Provides access to the file system.
    /// Implements the <see cref="IFileSystem" />
    /// </summary>
    /// <seealso cref="IFileSystem" />
    public class FileSystem : IFileSystem
    {
        /// <summary>
        /// The buffer size for a file stream.
        /// </summary>
        private const int CBufferSize = 4000;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystem"/> class.
        /// </summary>
        /// <autogeneratedoc />
        public FileSystem() : this(new OperatingSystem())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystem"/> class.
        /// </summary>
        /// <param name="operatingSystem">The operating system.</param>
        /// <autogeneratedoc />
        public FileSystem(IOperatingSystem operatingSystem) : this(
            Assembly.GetExecutingAssembly().GetName().Name.Replace(".", "/"), operatingSystem)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystem"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="operatingSystem">The operating system.</param>
        /// <autogeneratedoc />
        public FileSystem(IConfigurationRoot configuration, IOperatingSystem operatingSystem) : this(
            configuration?.GetSection("AppSettings")?["AppFolderName"], operatingSystem)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystem"/> class.
        /// </summary>
        /// <param name="appFolderName">Name of the application folder.</param>
        /// <param name="operatingSystem">The operating system.</param>
        /// <exception cref="ArgumentNullException">
        /// appFolderName
        /// or
        /// operatingSystem
        /// </exception>
        /// <autogeneratedoc />
        public FileSystem(string appFolderName, IOperatingSystem operatingSystem)
        {
            AppFolderName = appFolderName ?? throw new ArgumentNullException(nameof(appFolderName));
            OperatingSystem = operatingSystem ?? throw new ArgumentNullException(nameof(operatingSystem));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystem"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="operatingSystem">The operating system.</param>
        /// <exception cref="ArgumentNullException">
        /// type
        /// or
        /// operatingSystem
        /// </exception>
        /// <autogeneratedoc />
        public FileSystem(Type type, IOperatingSystem operatingSystem)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            AppFolderName = TypeNameHelper.GetTypeDisplayName(type).Replace(".", "/");
            OperatingSystem = operatingSystem ?? throw new ArgumentNullException(nameof(operatingSystem));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystem"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <autogeneratedoc />
        public FileSystem(Type type) : this(type, new OperatingSystem())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystem"/> class.
        /// </summary>
        /// <param name="appFolderName">Name of the application folder.</param>
        /// <autogeneratedoc />
        public FileSystem(string appFolderName) : this(appFolderName, new OperatingSystem())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystem"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <autogeneratedoc />
        public FileSystem(IConfigurationRoot configuration) : this(configuration, new OperatingSystem())
        {
        }

        /// <summary>
        /// Gets the operating system.
        /// </summary>
        /// <value>The operating system.</value>
        /// <autogeneratedoc />
        public IOperatingSystem OperatingSystem { get; }

        /// <summary>
        /// Gets or sets the name of the application folder.
        /// </summary>
        /// <value>The name of the application folder.</value>
        /// <autogeneratedoc />
        public string AppFolderName { get; set; }

        /// <summary>
        /// Gets the current directory.
        /// </summary>
        /// <returns>System.String.</returns>
        /// <autogeneratedoc />
        public string GetCurrentDirectory() => Directory.GetCurrentDirectory();

        /// <summary>
        /// Creates a new temporary file, and returns the corresponding
        /// stream. The file is deleted when the stream is closed.
        /// </summary>
        /// <returns>Stream.</returns>
        /// <autogeneratedoc />
        public Stream CreateNewTempFile()
        {
            return new FileStream
            (
                GetTempFileName(),
                FileMode.OpenOrCreate,
                FileAccess.ReadWrite,
                FileShare.ReadWrite,
                CBufferSize,
                FileOptions.DeleteOnClose
            );
        }

        /// <summary>
        /// Gets the temporary folder path.
        /// </summary>
        /// <returns>System.String.</returns>
        /// <autogeneratedoc />
        public string GetTempFolderPath() => Path.GetTempPath();

        /// <summary>
        /// Gets the name of the temporary file.
        /// </summary>
        /// <returns>System.String.</returns>
        /// <autogeneratedoc />
        public string GetTempFileName() => Path.GetTempFileName();


        /// <summary>
        /// Reads the contents of a file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        /// <autogeneratedoc />
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
        /// Writes contents to a file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="contents">The contents.</param>
        /// <returns>Task.</returns>
        /// <autogeneratedoc />
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
        /// Creates the given folder.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <autogeneratedoc />
        public void CreateFolder(string path)
        {
            Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Deletes the given folder.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <autogeneratedoc />
        public void DeleteFolder(string path)
        {
            Directory.Delete(path, true);
        }

        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <autogeneratedoc />
        public void DeleteFile(string path) => File.Delete(path);

        /// <summary>
        /// Folders the exists.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <autogeneratedoc />
        public bool FolderExists(string path) => Directory.Exists(path);

        /// <summary>
        /// Files the exists.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <autogeneratedoc />
        public bool FileExists(string path) => File.Exists(path);

        /// <summary>
        /// Gets the file name without extension.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>System.String.</returns>
        /// <autogeneratedoc />
        public string GetFileNameWithoutExtension(string path) => Path.GetFileNameWithoutExtension(path);


        /// <summary>
        /// Gets the name of the folder.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>System.String.</returns>
        /// <autogeneratedoc />
        public string GetFolderName(string path) => Path.GetDirectoryName(path);

        /// <summary>
        /// Returns a Special folder path given a special folder type
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <returns>System.String.</returns>
        /// <autogeneratedoc />
        public string GetSpecialFolderPath(Environment.SpecialFolder folder)
        {
            return Environment.GetFolderPath(folder);
        }

        /// <summary>
        /// Returns a Special folder path given a special folder type, allows option to verify or create
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="option">The option.</param>
        /// <returns>System.String.</returns>
        public string GetSpecialFolderPath(Environment.SpecialFolder folder, Environment.SpecialFolderOption option)
        {
            return Environment.GetFolderPath(folder, option);
        }

        /// <summary>
        /// Returns the path for storing application data common to all users, allows option to verify or create
        /// </summary>
        /// <param name="option">The option.</param>
        /// <returns>System.String.</returns>
        /// <autogeneratedoc />
        public string GetCommonApplicationDataFolderPath(Environment.SpecialFolderOption option)
        {
            if (OperatingSystem.IsOsx())
            {
                return ProcessSpecialFolder(option, "/Users/Shared/");
            }

            return GetSpecialFolderPath(Environment.SpecialFolder.CommonApplicationData, option);
        }

        /// <summary>
        /// Returns the path for storing application data common to all users
        /// </summary>
        /// <returns>System.String.</returns>
        /// <autogeneratedoc />
        public string GetCommonApplicationDataFolderPath()
        {
            return GetSpecialFolderPath(Environment.SpecialFolder.CommonApplicationData);
        }


        /// <summary>
        /// Returns the path for storing application locally for the current user, allows option to verify or create
        /// </summary>
        /// <param name="option">The option.</param>
        /// <returns>System.String.</returns>
        /// <autogeneratedoc />
        public string GetLocalApplicationDataFolderPath(Environment.SpecialFolderOption option)
        {
            return GetSpecialFolderPath(Environment.SpecialFolder.LocalApplicationData, option);
        }

        /// <summary>
        /// Returns the path for storing application locally for the current user
        /// </summary>
        /// <returns>System.String.</returns>
        /// <autogeneratedoc />
        public string GetLocalApplicationDataFolderPath()
        {
            return GetSpecialFolderPath(Environment.SpecialFolder.LocalApplicationData);
        }


        /// <summary>
        /// Returns folder path for logs
        /// </summary>
        /// <returns>System.String.</returns>
        public string LogFolderPath()
        {
            var logFolderPath = GetCurrentApplicationDataFolderPath() + "Logs/";

            CreateFolder(logFolderPath);

            return logFolderPath;
        }

        /// <summary>
        /// Returns folder path for test logs
        /// </summary>
        /// <returns>System.String.</returns>
        public string TestLogFolderPath()
        {
            var logFolderPath = GetCurrentApplicationDataFolderPath() + "Logs/Test/";

            CreateFolder(logFolderPath);

            return logFolderPath;
        }

        /// <summary>
        /// Returns folder path for data shared by all apps
        /// </summary>
        /// <returns>System.String.</returns>
        public string DataFolderPath()
        {
            var databaseFolderPath = GetCurrentApplicationDataFolderPath() + "Data/";

            CreateFolder(databaseFolderPath);

            return databaseFolderPath;
        }

        /// <summary>
        /// Arps the command path.
        /// </summary>
        /// <returns>System.String.</returns>
        /// <autogeneratedoc />
        public string ArpCommandPath()
        {
            if (OperatingSystem.IsWindows())
            {
                return GetSpecialFolderPath(Environment.SpecialFolder.System) + "\\arp.exe";
            }

            return "/usr/sbin/arp";
        }

        /// <summary>
        /// Gets the assembly folder.
        /// </summary>
        /// <value>The assembly folder.</value>
        /// <autogeneratedoc />
        public string AssemblyFolder
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        /// <summary>
        /// Gets the settings folder.
        /// </summary>
        /// <value>The settings folder.</value>
        /// <autogeneratedoc />
        public string SettingsFolder => AssemblyFolder + "/Settings/";

        /// <summary>
        /// Processes the special folder.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="folderPath">The folder path.</param>
        /// <returns>System.String.</returns>
        /// <autogeneratedoc />
        private string ProcessSpecialFolder(Environment.SpecialFolderOption option, string folderPath)
        {
            switch (option)
            {
                case Environment.SpecialFolderOption.DoNotVerify:
                    break;

                case Environment.SpecialFolderOption.None:
                {
                    if (!FolderExists(folderPath))
                        return string.Empty;

                    break;
                }

                case Environment.SpecialFolderOption.Create:
                {
                    CreateFolder(folderPath);

                    break;
                }
            }

            return folderPath;
        }


        /// <summary>
        /// Gets the current application data folder path.
        /// </summary>
        /// <returns>System.String.</returns>
        /// <autogeneratedoc />
        public string GetCurrentApplicationDataFolderPath()
        {
            var commonApplicationFolder =
                GetCommonApplicationDataFolderPath(Environment.SpecialFolderOption.Create) + $"/{AppFolderName}/";

            CreateFolder(commonApplicationFolder);

            return commonApplicationFolder;
        }
    }
}