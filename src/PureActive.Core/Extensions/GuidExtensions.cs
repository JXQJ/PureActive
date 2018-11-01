using System;

namespace PureActive.Core.Extensions
{
    public static class GuidExtensions
    {
        public static string ToStringNoDashes(this Guid guid)
        {
            return guid.ToString("N").ToUpper();
        }
    }
}
