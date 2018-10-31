using System;

namespace PureActive.Core.Extensions
{
    public static class FileExtensions
    {

        public static string GetRandomFileName(string prefix, string ext)
        {
            return $"{prefix}{Guid.NewGuid().ToStringNoDashes()}{ext}";
        }

    }
}
