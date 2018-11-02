using System;
using PureActive.Logging.Abstractions.Interfaces;

namespace PureActive.Network.Abstractions.PureObject
{
    public interface IPureObject : IComparable<IPureObject>, IPureLoggable
    {
        Guid ObjectId { get; set; }

        DateTimeOffset CreatedTimestamp { get; set; }

        DateTimeOffset ModifiedTimestamp { get; set; }

        ulong ObjectVersion { get; set; }
        
        IPureObject CopyInstance();

        IPureObject CloneInstance();

        IPureObject UpdateInstance(IPureObject pureObjectUpdate);

        bool IsSameObjectId(IPureObject pureObjectOther);

        bool IsSameObjectVersion(IPureObject pureObjectOther);
    }
}
