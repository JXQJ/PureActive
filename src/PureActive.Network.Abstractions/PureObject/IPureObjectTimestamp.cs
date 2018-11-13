using System;
using PureActive.Logging.Abstractions.Interfaces;

namespace PureActive.Network.Abstractions.PureObject
{
    public interface IPureObjectTimestamp
    {
        DateTimeOffset CreatedTimestamp { get; set; }
        DateTimeOffset ModifiedTimestamp { get; set; }
    }
}
