using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace PureActive.Logging.Abstractions.Interfaces
{
    public interface IPureLogPropertyLevelList: IEnumerable<IPureLogPropertyLevel>
    {
        IList<IPureLogPropertyLevel> GetLogPropertyLevelList(LogLevel minimumLogLevel);
    }
}
