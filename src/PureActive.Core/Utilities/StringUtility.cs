// ***********************************************************************
// Assembly         : PureActive.Core
// Author           : SteveBu
// Created          : 11-01-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="StringUtility.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections;
using System.Text;

namespace PureActive.Core.Utilities
{
    /// <summary>
    /// Provides additional string operations
    /// </summary>
    public static class StringUtility
    {
        /// <summary>
        /// Check if the provided string is either null or empty
        /// </summary>
        /// <param name="source">String to validate</param>
        /// <returns>True if the string is null or empty</returns>
        public static bool IsNullOrEmpty(string source)
        {
            return string.IsNullOrEmpty(source);
        }

        /// <summary>
        /// Check if the provided string is either null or white space
        /// </summary>
        /// <param name="source">String to validate</param>
        /// <returns>True if the string is null or white space</returns>
        public static bool IsNullOrWhiteSpace(string source)
        {
            return IsNullOrEmpty(source) || source.Trim().Length == 0;
        }

        /// <summary>
        /// Split a string by deliminator
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deliminator">The deliminator.</param>
        /// <returns>System.String[].</returns>
        public static string[] SplitComponents(string source, char deliminator)
        {
            int iStart = 0;
            string[] ret = null;
            string[] tmp;
            int i;
            string s;

            while (true)
            {
                // Find deliminator
                i = source.IndexOf(deliminator, iStart);

                if (InQuotes(source, i))
                    iStart = i + 1;
                else
                {
                    // Separate value
                    if (i < 0)
                        s = source;
                    else
                    {
                        s = source.Substring(0, i).Trim();
                        source = source.Substring(i + 1);
                    }

                    // Add value
                    if (ret == null)
                        ret = new[] {s};
                    else
                    {
                        tmp = new string[ret.Length + 1];
                        Array.Copy(ret, tmp, ret.Length);
                        tmp[tmp.Length - 1] = s;
                        ret = tmp;
                    }

                    iStart = 0;
                }

                // Break on last value
                if (i < 0 || source == string.Empty)
                    break;
            }

            return ret;
        }

        /// <summary>
        /// Determine if a specific character is inside of a quote string
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="position">The position.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool InQuotes(string source, int position)
        {
            int qcount = 0;
            int i;
            int iStart = 0;

            while (true)
            {
                // Find next instance of a quote
                i = source.IndexOf('"', iStart);

                // If not return our value
                if (i < 0 || i >= position)
                    return qcount % 2 != 0;

                // Check if it's a qualified quote
                if (i > 0 && source.Substring(i, 1) != "\\" || i == 0)
                    qcount++;

                iStart = i + 1;
            }
        }

        /// <summary>
        /// Determine if a string includes a pattern using "*" and "?" as wild cards
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="caseSensitive">if set to <c>true</c> [case sensitive].</param>
        /// <returns>True if pattern wild card matches</returns>
        public static bool MatchWildCard(string source, string pattern, bool caseSensitive)
        {
            if (!caseSensitive)
            {
                pattern = pattern.ToLower();
                source = source.ToLower();
            }

            int nText = 0;
            int nPattern = 0;
            //const char *cp = NULL, *mp = NULL;
            int mp = 0;
            int cp = 0;

            while (nText < source.Length && nPattern < pattern.Length && pattern[nPattern] != '*')
            {
                if (pattern[nPattern] != source[nText] && pattern[nPattern] != '?')
                {
                    return false;
                }

                nPattern++;
                nText++;
            }

            while (nText < source.Length)
            {
                if (pattern[nPattern] == '*')
                {
                    nPattern++;
                    if (nPattern >= pattern.Length)
                    {
                        return true;
                    }

                    mp = nPattern;
                    cp = nText + 1;
                }
                else if (pattern[nPattern] == source[nText] || pattern[nPattern] == '?')
                {
                    nPattern++;
                    nText++;
                }
                else
                {
                    nPattern = mp;
                    nText = cp++;
                }
            }

            while (nPattern < pattern.Length && pattern[nPattern] == '*')
            {
                nPattern++;
            }

            return nPattern >= pattern.Length;
        }

        /// <summary>
        /// Encodes a string according to the BASE64 standard
        /// </summary>
        /// <param name="value">The input string</param>
        /// <returns>The output string</returns>
        public static string Base64Encode(string value)
        {
            // Pairs of 3 8-bit bytes will become pairs of 4 6-bit bytes
            // That's the whole trick of base64 encoding :-)

            int blocks = value.Length / 3; // The amount of original pairs
            if (blocks * 3 < value.Length) ++blocks; // Fixes rounding issues; always round up
            int bytes = blocks * 4; // The length of the base64 output

            // These characters will be used to represent the 6-bit bytes in ASCII
            char[] base64Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=".ToCharArray();

            // Converts the input string to characters and creates the output array
            char[] inputChars = value.ToCharArray();
            char[] outputChars = new char[bytes];

            // Converts the blocks of bytes
            for (int block = 0; block < blocks; ++block)
            {
                // Fetches the input pairs
                byte input0 = (byte) (inputChars.Length > block * 3 ? inputChars[block * 3] : 0);
                byte input1 = (byte) (inputChars.Length > block * 3 + 1 ? inputChars[block * 3 + 1] : 0);
                byte input2 = (byte) (inputChars.Length > block * 3 + 2 ? inputChars[block * 3 + 2] : 0);

                // Generates the output pairs
                byte output0 = (byte) (input0 >> 2); // The first 6 bits of the 1st byte
                byte output1 =
                    (byte) (((input0 & 0x3) << 4) +
                            (input1 >>
                             4)); // The last 2 bits of the 1st byte followed by the first 4 bits of the 2nd byte
                byte output2 =
                    (byte) (((input1 & 0xf) << 2) +
                            (input2 >>
                             6)); // The last 4 bits of the 2nd byte followed by the first 2 bits of the 3rd byte
                byte output3 = (byte) (input2 & 0x3f); // The last 6 bits of the 3rd byte

                // This prevents 0-bytes at the end
                if (inputChars.Length < block * 3 + 2) output2 = 64;
                if (inputChars.Length < block * 3 + 3) output3 = 64;

                // Converts the output pairs to base64 characters
                outputChars[block * 4] = base64Characters[output0];
                outputChars[block * 4 + 1] = base64Characters[output1];
                outputChars[block * 4 + 2] = base64Characters[output2];
                outputChars[block * 4 + 3] = base64Characters[output3];
            }

            return new string(outputChars);
        }

        /// <summary>
        /// Return X.X Byte/KB/MB/GB/TB
        /// </summary>
        /// <param name="value">Size</param>
        /// <returns>System.String.</returns>
        public static string FormatDiskSize(long value)
        {
            double cur = value;
            string[] size = {"bytes", "kb", "mb", "gb", "tb"};
            int i = 0;

            while (cur > 1024 && i < 4)
            {
                cur /= 1024;
                i++;
            }

            return Math.Round(cur) + " " + size[i];
        }

        #region ZeroFill Method

        /// <summary>
        /// Changes a number into a string and add zeros in front of it, if required
        /// </summary>
        /// <param name="number">The input number</param>
        /// <param name="digits">The amount of digits it should be</param>
        /// <param name="character">The character to repeat in front (default: 0)</param>
        /// <returns>A string with the right amount of digits</returns>
        public static string ZeroFill(string number, int digits, char character = '0')
        {
            var negative = false;
            if (number.Substring(0, 1) == "-")
            {
                negative = true;
                number = number.Substring(1);
            }

            for (var counter = number.Length; counter < digits; ++counter)
            {
                number = character + number;
            }

            if (negative) number = "-" + number;

            return number;
        }

        /// <summary>
        /// Changes a number into a string and add zeros in front of it, if required
        /// </summary>
        /// <param name="number">The input number</param>
        /// <param name="minLength">The amount of digits it should be</param>
        /// <param name="character">The character to repeat in front (default: 0)</param>
        /// <returns>A string with the right amount of digits</returns>
        public static string ZeroFill(int number, int minLength, char character = '0')
        {
            return ZeroFill(number.ToString(), minLength, character);
        }

        #endregion ZeroFill Method

        #region Replace Method

        /// <summary>
        /// Replace all occurrences of the 'find' string with the 'replace' string.
        /// </summary>
        /// <param name="source">Original string</param>
        /// <param name="find">String to find within the original string</param>
        /// <param name="replace">String to be used in place of the find string</param>
        /// <returns>Final string after all instances have been replaced.</returns>
        public static string Replace(string source, string find, string replace)
        {
            var iStart = 0;

            if (string.IsNullOrEmpty(source) || find == string.Empty || find == null)
                return source;

            while (true)
            {
                var i = source.IndexOf(find, iStart, StringComparison.Ordinal);
                if (i < 0) break;

                if (i > 0)
                    source = source.Substring(0, i) + replace + source.Substring(i + find.Length);
                else
                    source = replace + source.Substring(i + find.Length);

                iStart = i + replace.Length;
            }

            return source;
        }

        /// <summary>
        /// Finds and replaces empty or null within a string
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="replaceWith">The replace with.</param>
        /// <returns>source</returns>
        public static string ReplaceEmptyOrNull(string source, string replaceWith)
        {
            return string.IsNullOrEmpty(source) ? replaceWith : source;
        }

        /// <summary>
        /// Finds and replaces empty or null within a string
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="replaceWith">The replace with.</param>
        /// <returns>Value</returns>
        public static string ReplaceEmptyOrNull(object source, string replaceWith)
        {
            if (source == null || source.ToString() == string.Empty)
                return replaceWith;

            return source.ToString();
        }

        #endregion Replace method

        #region Sort Method

        /// <summary>
        /// Sorts an array of strings.
        /// </summary>
        /// <param name="array">Array of string to be sorted.</param>
        /// <remarks>Original code by user "Jay Jay"
        /// http://www.tinyclr.com/codeshare/entry/475
        /// Modified to be specifically suites to sorting arrays of strings.</remarks>
        public static void Sort(string[] array)
        {
            Sort(array, 0, array.Length - 1);
        }

        /// <summary>
        /// This is a generic version of C.A.R Hoare's Quick Sort
        /// algorithm.  This will handle arrays that are already
        /// sorted, and arrays with duplicate keys.
        /// </summary>
        /// <param name="array">Array of string to be sorted.</param>
        /// <param name="l">Left boundary of array partition</param>
        /// <param name="r">Right boundary of array partition</param>
        /// <remarks>If you think of a one dimensional array as going from
        /// the lowest index on the left to the highest index on the right
        /// then the parameters to this function are lowest index or
        /// left and highest index or right.  The first time you call
        /// this function it will be with the parameters 0, a.length - 1.</remarks>
        private static void Sort(string[] array, int l, int r)
        {
            int M = 4;
            int i;
            int j;
            string v;

            if (r - l <= M)
            {
                InsertionSort(array, l, r);
            }
            else
            {
                i = (r + l) / 2;

                if (string.CompareOrdinal(array[l], array[i]) > 0)
                    Swap(array, l, i);

                if (string.CompareOrdinal(array[l], array[r]) > 0)
                    Swap(array, l, r);

                if (string.CompareOrdinal(array[i], array[r]) > 0)
                    Swap(array, i, r);

                j = r - 1;
                Swap(array, i, j);

                i = l;
                v = array[j];
                for (;;)
                {
                    while (string.CompareOrdinal(array[++i], v) < 0)
                    {
                    }

                    while (string.CompareOrdinal(array[--j], v) > 0)
                    {
                    }

                    if (j < i)
                        break;
                    Swap(array, i, j);
                }

                Swap(array, i, r - 1);

                Sort(array, l, j);
                Sort(array, i + 1, r);
            }
        }

        /// <summary>
        /// Insertions the sort.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="lo">The lo.</param>
        /// <param name="hi">The hi.</param>
        /// <autogeneratedoc />
        private static void InsertionSort(string[] array, int lo, int hi)
        {
            int i;
            int j;
            string v;

            for (i = lo + 1; i <= hi; i++)
            {
                v = array[i];
                j = i;
                while (j > lo && string.CompareOrdinal(array[j - 1], v) > 0)
                {
                    array[j] = array[j - 1];
                    --j;
                }

                array[j] = v;
            }
        }

        /// <summary>
        /// Swaps the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <autogeneratedoc />
        private static void Swap(IList list, int left, int right)
        {
            object swap = list[left];
            list[left] = list[right];
            list[right] = swap;
        }

        #endregion Sort Method

        #region Format Method

        /// <summary>
        /// Replaces one or more format items in a specified string with the string representation of a specified object.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg">The object to format.</param>
        /// <returns>A copy of format in which any format items are replaced by the string representation of arg0.</returns>
        /// <exception cref="FormatException">format is invalid, or the index of a format item is less than zero, or greater than
        /// or equal to the length of the args array.</exception>
        /// <exception cref="ArgumentNullException">format or args is null</exception>
        public static string Format(string format, object arg)
        {
            return Format(format, new[] {arg});
        }

        /// <summary>
        /// Format the given string using the provided collection of objects.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>A copy of format in which the format items have been replaced by the string representation of the
        /// corresponding objects in args.</returns>
        /// <exception cref="ArgumentNullException">format or args is null</exception>
        /// <exception cref="FormatException">
        /// </exception>
        /// <exception cref="FormatException">format is invalid, or the index of a format item is less than zero, or greater than
        /// or equal to the length of the args array.</exception>
        /// <example>
        /// x = StringUtility.Format("Quick brown {0}","fox");
        /// </example>
        public static string Format(string format, params object[] args)
        {
            if (format == null)
                throw new ArgumentNullException("format");

            if (args == null)
                throw new ArgumentNullException("args");

            // Validate the structure of the format string.
            ValidateFormatString(format);

            StringBuilder bld = new StringBuilder();

            int endOfLastMatch = 0;
            int starting = 0;

            while (starting >= 0)
            {
                starting = format.IndexOf('{', starting);

                if (starting >= 0)
                {
                    if (starting != format.Length - 1)
                    {
                        if (format[starting + 1] == '{')
                        {
                            // escaped starting bracket.
                            starting = starting + 2;
                        }
                        else
                        {
                            bool found = false;
                            int endsearch = format.IndexOf('}', starting);

                            while (endsearch > starting)
                            {
                                if (endsearch != format.Length - 1 && format[endsearch + 1] == '}')
                                {
                                    // escaped ending bracket
                                    endsearch = endsearch + 2;
                                }
                                else
                                {
                                    if (starting != endOfLastMatch)
                                    {
                                        string t = format.Substring(endOfLastMatch, starting - endOfLastMatch);
                                        t = Replace(t, "{{", "{"); // get rid of the escaped brace
                                        t = Replace(t, "}}", "}"); // get rid of the escaped brace
                                        bld.Append(t);
                                    }

                                    // we have a winner
                                    string fmt = format.Substring(starting, endsearch - starting + 1);

                                    if (fmt.Length >= 3)
                                    {
                                        fmt = fmt.Substring(1, fmt.Length - 2);

                                        string[] indexFormat = fmt.Split(':');

                                        string formatString = string.Empty;

                                        if (indexFormat.Length == 2)
                                        {
                                            formatString = indexFormat[1];
                                        }

                                        // no format, just number
                                        if (ParseUtility.TryParseInt(indexFormat[0], out int index))
                                        {
                                            bld.Append(FormatParameter(args[index], formatString));
                                        }
                                    }

                                    endOfLastMatch = endsearch + 1;

                                    found = true;
                                    starting = endsearch + 1;
                                    break;
                                }


                                endsearch = format.IndexOf('}', endsearch);
                            }
                            // need to find the ending point

                            if (!found)
                            {
                                throw new FormatException(FormatException.StringFormatErrorMessage);
                            }
                        }
                    }
                    else
                    {
                        // invalid
                        throw new FormatException(FormatException.StringFormatErrorMessage);
                    }
                }
            }

            // copy any additional remaining part of the format string.
            if (endOfLastMatch != format.Length)
            {
                bld.Append(format.Substring(endOfLastMatch, format.Length - endOfLastMatch));
            }

            return bld.ToString();
        }

        /// <summary>
        /// Validates the format string.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <autogeneratedoc />
        private static void ValidateFormatString(string format)
        {
            char expected = '{';

            int i = 0;

            while ((i = format.IndexOfAny(new[] {'{', '}'}, i)) >= 0)
            {
                if (i < format.Length - 1 && format[i] == format[i + 1])
                {
                    // escaped brace. continue looking.
                    i = i + 2;
                }
                else if (format[i] != expected)
                {
                    // badly formed string.
                    //throw new FormatException(FormatException.StringFormatErrorMessage);
                }
                else
                {
                    // move it along.
                    i++;

                    // expected it.
                    if (expected == '{')
                        expected = '}';
                    else
                        expected = '{';
                }
            }

            if (expected == '}')
            {
                // orpaned opening brace. Bad format.
                //throw new FormatException(FormatException.StringFormatErrorMessage);
            }
        }

        /// <summary>
        /// Formats the parameter.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="formatString">The format string.</param>
        /// <returns>System.String.</returns>
        /// <autogeneratedoc />
        private static string FormatParameter(object p, string formatString)
        {
            if (formatString == string.Empty)
                return p.ToString();

            if (p as IFormattable != null)
            {
                return ((IFormattable) p).ToString(formatString, null);
            }

            if (p is DateTime)
            {
                return ((DateTime) p).ToString(formatString);
            }

            if (p is double)
            {
                return ((double) p).ToString(formatString);
            }

            if (p is short)
            {
                return ((short) p).ToString(formatString);
            }

            if (p is int)
            {
                return ((int) p).ToString(formatString);
            }

            if (p is long)
            {
                return ((long) p).ToString(formatString);
            }

            if (p is sbyte)
            {
                return ((sbyte) p).ToString(formatString);
            }

            if (p is float)
            {
                return ((float) p).ToString(formatString);
            }

            if (p is ushort)
            {
                return ((ushort) p).ToString(formatString);
            }

            if (p is uint)
            {
                return ((uint) p).ToString(formatString);
            }

            if (p is ulong)
            {
                return ((ulong) p).ToString(formatString);
            }

            return p.ToString();
        }

        #endregion Format method
    }

    /// <summary>
    /// The exception that is thrown when the format of an argument does not meet the parameter specifications of the
    /// invoked method.
    /// Implements the <see cref="Exception" />
    /// </summary>
    /// <seealso cref="Exception" />
    public class FormatException : Exception
    {
        /// <summary>
        /// The string format error message
        /// </summary>
        /// <autogeneratedoc />
        internal const string StringFormatErrorMessage = "String format is not valid";

        /// <summary>
        /// Initializes a new instance of the FormatException class.
        /// </summary>
        public FormatException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the FormatException class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public FormatException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the FormatException class with a specified error message and a reference to the inner
        /// exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="ex">The exception that is the cause of the current exception. If the innerException parameter is not a
        /// null reference (Nothing in Visual Basic), the current exception is raised in a catch block that handles the inner
        /// exception.</param>
        public FormatException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}