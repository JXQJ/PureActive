using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PureActive.Core.Utilities
{
    /// <summary>
    ///     Taken from http://www.codeproject.com/Articles/22517/Natural-Sort-Comparer
    /// </summary>
    public class NaturalComparer : Comparer<string>, IDisposable
    {
        private Dictionary<string, string[]> _table;

        public NaturalComparer()
        {
            _table = new Dictionary<string, string[]>();
        }

        public void Dispose()
        {
            _table.Clear();
            _table = null;
        }

        public override int Compare(string x, string y)
        {
            if (x == y || string.IsNullOrEmpty(x) || string.IsNullOrEmpty(y)) return 0;

            if (!_table.TryGetValue(x, out var x1))
            {
                x1 = Regex.Split(x.Replace(" ", ""), "([0-9]+)");
                _table.Add(x, x1);
            }

            if (!_table.TryGetValue(y, out var y1))
            {
                y1 = Regex.Split(y.Replace(" ", ""), "([0-9]+)");
                _table.Add(y, y1);
            }

            for (var i = 0; i < x1.Length && i < y1.Length; i++)
                if (x1[i] != y1[i])
                    return PartCompare(x1[i], y1[i]);
            if (y1.Length > x1.Length)
                return 1;
            if (x1.Length > y1.Length)
                return -1;
            return 0;
        }

        private static int PartCompare(string left, string right)
        {
            if (!int.TryParse(left, out var x)) return left.CompareTo(right);

            return !int.TryParse(right, out var y) ? left.CompareTo(right) : x.CompareTo(y);
        }
    }
}