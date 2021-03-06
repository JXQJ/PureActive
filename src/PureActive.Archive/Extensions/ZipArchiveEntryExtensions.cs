﻿using System.IO.Compression;

namespace PureActive.Archive.Extensions
{
    /// <summary>
    ///     Extension methods for the ZipArchiveEntry class.
    /// </summary>
    public static class ZipArchiveEntryExtensions
    {
        /// <summary>
        ///     Returns whether or not the zip archive entry represents a file.
        /// </summary>
        public static bool IsFile(this ZipArchiveEntry entry)
        {
            return !entry.FullName.EndsWith("/");
        }
    }
}