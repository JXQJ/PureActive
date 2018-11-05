using System.Collections.Generic;

namespace PureActive.Logging.Abstractions.Interfaces
{
    public interface IPureLogPropertyList: IEnumerable<IPureLogProperty>
    {
        IList<IPureLogProperty> GetLogPropertyList { get; }
    }
}
