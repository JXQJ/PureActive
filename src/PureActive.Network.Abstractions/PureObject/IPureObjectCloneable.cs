using System;
using PureActive.Logging.Abstractions.Interfaces;

namespace PureActive.Network.Abstractions.PureObject
{
    public interface IPureObjectCloneable
    {
        IPureObject CopyInstance();

        IPureObject CloneInstance();

        IPureObject UpdateInstance(IPureObject pureObjectUpdate);
    }
}
