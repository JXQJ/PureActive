using System;
using System.Collections.Generic;

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

        public static List<T> AddItem<T>(this List<T> list, T item)
        {
            list.Add(item);
            return list;
        }

        public static ICollection<T> AddItem<T>(this ICollection<T> list, T item)
        {
            list.Add(item);
            return list;
        }
    }
}
