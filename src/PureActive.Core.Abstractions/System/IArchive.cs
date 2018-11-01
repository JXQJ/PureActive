using System;
using System.Collections.Generic;

namespace PureActive.Core.Abstractions.System
{
    /// <summary>
    ///     An archive of files.
    /// </summary>
    public interface IArchive : IDisposable
    {
        /// <summary>
        ///     The files in the archive.
        /// </summary>
        IList<IArchiveFile> Files { get; }
    }
}