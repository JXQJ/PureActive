using System;
using PureActive.Logging.Abstractions.Interfaces;

namespace PureActive.Network.Abstractions.PureObject
{
    public interface IPureObject : IComparable<IPureObject>, IPureLoggable, IPureObjectCloneable, IPureObjectTimestamp
    {
        Guid ObjectId { get; set; }

        ulong ObjectVersion { get; set; }

        bool IsSameObjectId(IPureObject pureObjectOther);

        bool IsSameObjectVersion(IPureObject pureObjectOther);
    }
}