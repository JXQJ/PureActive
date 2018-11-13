using System;
using System.Collections.Generic;
using System.Linq;

namespace PureActive.Core.Extensions
{
    public static class ListExtensions
    {
        private static readonly Random Rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            if (list == null)
                return;

            var n = list.Count;

            while (n > 1)
            {
                n--;

                var k = Rng.Next(n + 1);

                // Swap values
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
       }

        public static IList<T> AddItem<T>(this IList<T> list, T item)
        {
            list.Add(item);
            return list;
        }

        public static ICollection<T> AddItem<T>(this ICollection<T> list, T item)
        {
            list.Add(item);
            return list;
        }

        public static IList<T> CloneList<T>(this IEnumerable<T> list) where T : ICloneable
        {
            return list.Select(item => (T)item.Clone()).ToList();
        }
    }
}
