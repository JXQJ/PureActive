namespace PureActive.Logging.Abstractions.Interfaces
{
    public interface IPureLoggerGlobal
    {
        IPureLogger AppLogger { get; set; }
        IPureLogger TestLogger { get; set; }
    }
}