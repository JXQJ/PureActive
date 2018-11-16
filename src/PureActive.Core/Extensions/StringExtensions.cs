using System;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace PureActive.Core.Extensions
{
    /// <summary>
    ///     Extension methods for the string class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        ///     Changes the first character of a string to lower-case,
        ///     if it is not already.
        /// </summary>
        public static string ToCamelCase(this string str) => string.IsNullOrEmpty(str) ? str : str.Substring(0, 1).ToLower() + str.Substring(1);

        /// <summary>
        ///     Changes the first character of a string to upper-case,
        ///     if it is not already.
        /// </summary>
        public static string ToPascalCase(this string str) => string.IsNullOrEmpty(str) ? str : str.Substring(0, 1).ToUpper() + str.Substring(1);

        /// <summary>
        ///     Returns a version of the string with non-alpha-numeric
        ///     characters stripped out.
        /// </summary>
        public static string ToAlphaNumeric(this string str) => string.IsNullOrEmpty(str) ? str: new string(str.Where(char.IsLetterOrDigit).ToArray());

        public static string RemoveWhitespace(this string str) => string.IsNullOrEmpty(str) ? str : new string(str.Where(c => !char.IsWhiteSpace(c)).ToArray());

        /// <summary>
        ///     Returns a version of the string with non-numeric
        ///     characters stripped out.
        /// </summary>
        public static string ToNumeric(this string str) => string.IsNullOrEmpty(str) ? str : new string(str.Where(char.IsDigit).ToArray());


        /// <summary>
        ///     Returns a string where every line in the string has all
        ///     whitespace stripped from either end.
        /// </summary>
        public static string TrimEveryLine(this string str)
        {
            var lines = str.Trim().Split
            (
                new[] {"\r\n", "\n"},
                StringSplitOptions.None
            );

            return string.Join
            (
                "\n",
                lines.Select(line => line.Trim())
            ).Trim();
        }

        private static string[] ProcessSplits(string str, int indexDelim)
        {
            var strings = new string[2];

            if (string.IsNullOrEmpty(str))
                return strings;

            if (indexDelim != -1 && indexDelim <= str.Length - 1)
            {
                strings[0] = str.Substring(0, indexDelim).Trim();
                strings[1] = str.Substring(indexDelim + 1).Trim();
            }
            else
            {
                strings[0] = str.Trim();
                strings[1] = string.Empty;
            }

            return strings;
        }

        public static string[] SplitOnFirstDelim(this string str, char chDelim)
        {
            return string.IsNullOrEmpty(str) ? new string[2]: ProcessSplits(str, str.IndexOf(chDelim));
        }

        public static string[] SplitOnLastDelim(this string str, char chDelim)
        {
            return string.IsNullOrEmpty(str) ? new string[2] : ProcessSplits(str, str.LastIndexOf(chDelim));
        }

        public static string StringAfterDelim(this string str, char chDelim)
        {
            var strings = SplitOnFirstDelim(str, chDelim);

            return strings.Length == 2 ? strings[1] : null;
        }

        public static string StringBeforeDelim(this string str, char chDelim)
        {
            var strings = SplitOnFirstDelim(str, chDelim);

            return strings.Length == 2 ? strings[0] : null;
        }

        public static bool? ParseYesNo(this string str)
        {
            if (!string.IsNullOrWhiteSpace(str) && str.Length > 0)
            {
                return char.ToUpper(str.Trim()[0]) == 'Y';
            }

            return null;
        }

        public static bool? ParseYesNoStrict(this string str)
        {
            if (string.IsNullOrWhiteSpace(str) || str.Length <= 0) return null;

            var strNormalized = str.Trim().ToUpper();

            if (strNormalized.Equals("YES"))
                return true;

            if (strNormalized.Equals("NO"))
                return false;

            return null;
        }

        public static double? ParseDoubleOrNull(this string input)
        {
            if (!string.IsNullOrEmpty(input) && double.TryParse(input, out var result))
                return result;

            return null;
        }

        public static long? ParseLongOrNull(this string input)
        {
            if (!string.IsNullOrEmpty(input) && long.TryParse(input, out var result))
                return result;

            return null;
        }

        public static int? ParseIntOrNull(this string input)
        {
            if (!string.IsNullOrEmpty(input) && int.TryParse(input, out var result))
                return result;

            return null;
        }

        public static string NullIfWhitespace(this string input)
        {
            return string.IsNullOrEmpty(input = input?.Trim()) ? null : input;
        }

        public static string PadWithDelim(this string input, string delim, int length)
        {
            var sb = new StringBuilder(input).Append(delim);

            var paddingLen = length - sb.Length;

            if (paddingLen > 0)
                sb.Append(' ', paddingLen);

            return sb.ToString();
        }

        public static string ToDoubleQuoted(this string input)
        {
            return $"\"{input}\"";
        }
    }
}