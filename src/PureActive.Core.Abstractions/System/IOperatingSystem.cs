namespace PureActive.Core.Abstractions.System
{
    public interface IOperatingSystem
    {
        bool IsWindows();
        bool IsOsx();
        bool IsLinux();

    }
}
