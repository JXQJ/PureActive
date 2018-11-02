using System;
using PureActive.Logging.Abstractions.Interfaces;

namespace PureActive.Network.Abstractions.Object
{
    public interface IObject : IComparable<IObject>, IPureLoggable
    {
        Guid ObjectId { get; set; }

        DateTimeOffset CreatedTimestamp { get; set; }

        DateTimeOffset ModifiedTimestamp { get; set; }

        ulong ObjectVersion { get; set; }
        
        IObject CopyInstance();

        IObject CloneInstance();

        IObject UpdateInstance(IObject objectUpdate);

        bool IsSameObjectId(IObject objectOther);

        bool IsSameObjectVersion(IObject objectOther);
    }
}
