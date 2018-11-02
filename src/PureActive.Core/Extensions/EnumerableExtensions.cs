using System.Collections.Generic;
using System.Linq;

namespace PureActive.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static int MaxStringLength<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.Select(item => item.ToString().Length).Concat(new[] {0}).Max();
        }
    }
}
