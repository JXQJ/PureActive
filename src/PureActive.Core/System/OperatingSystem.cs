// using System;

using System.Runtime.InteropServices;
using PureActive.Core.Abstractions.System;

namespace PureActive.Core.System
{
    public class OperatingSystem : IOperatingSystem
    {
        public bool IsLinux()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        }

        public bool IsOsx()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        }

        public bool IsWindows()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }
    }
}
