using System;

namespace PureActive.Core.Extensions
{
    /// <summary>
    /// Extension methods for Integers
    /// </summary>
    public static class IntegerExtensions
    {
        private static readonly string HexValues = "0123456789ABCDEF";

        /// <summary>
        /// Converts a Integer to Hex string
        /// </summary>
        /// <param name="value">Integer to convert</param>
        /// <param name="prefix">String'0x' to include before the Hex value.</param>
        /// <returns>Integer value in Hex</returns>
        public static string ToHexString(this Int16 value, string prefix)
        {
            char[] charValues = new char[4];
            charValues[0] = HexValues[(byte)((value >> 12) & 0x0F)];
            charValues[1] = HexValues[(byte)((value >> 8) & 0x0F)];
            charValues[2] = HexValues[(byte)((value >> 4) & 0x0F)];
            charValues[3] = HexValues[(byte)((value >> 0) & 0x0F)];
            return prefix + new string(charValues);
        }

        /// <summary>
        /// Converts a Integer to Hex string
        /// </summary>
        /// <param name="value">Integer to convert</param>
        /// <param name="prefix">String'0x' to include before the Hex value.</param>
        /// <returns>Integer value in Hex</returns>
        public static string ToHexString(this Int32 value, string prefix)
        {
            char[] charValues = new char[8];
            charValues[0] = HexValues[(byte)((value >> 28) & 0x0F)];
            charValues[1] = HexValues[(byte)((value >> 24) & 0x0F)];
            charValues[2] = HexValues[(byte)((value >> 20) & 0x0F)];
            charValues[3] = HexValues[(byte)((value >> 16) & 0x0F)];
            charValues[4] = HexValues[(byte)((value >> 12) & 0x0F)];
            charValues[5] = HexValues[(byte)((value >> 8) & 0x0F)];
            charValues[6] = HexValues[(byte)((value >> 4) & 0x0F)];
            charValues[7] = HexValues[(byte)((value >> 0) & 0x0F)];
            return prefix + new string(charValues);
        }

        /// <summary>
        /// Converts a Integer to Hex string
        /// </summary>
        /// <param name="value">Integer to convert</param>
        /// <param name="prefix">String'0x' to include before the Hex value.</param>
        /// <returns>Integer value in Hex</returns>
        public static string ToHexString(this UInt16 value, string prefix)
        {
            char[] charValues = new char[4];
            charValues[0] = HexValues[(byte)((value >> 12) & 0x0F)];
            charValues[1] = HexValues[(byte)((value >> 8) & 0x0F)];
            charValues[2] = HexValues[(byte)((value >> 4) & 0x0F)];
            charValues[3] = HexValues[(byte)((value >> 0) & 0x0F)];
            return prefix + new string(charValues);
        }

        /// <summary>
        /// Converts a Integer to Hex string
        /// </summary>
        /// <param name="value">Integer to convert</param>
        /// <param name="prefix">String'0x' to include before the Hex value.</param>
        /// <returns>Integer value in Hex</returns>
        public static string ToHexString(this UInt32 value, string prefix)
        {
            char[] charValues = new char[8];
            charValues[0] = HexValues[(byte)((value >> 28) & 0x0F)];
            charValues[1] = HexValues[(byte)((value >> 24) & 0x0F)];
            charValues[2] = HexValues[(byte)((value >> 20) & 0x0F)];
            charValues[3] = HexValues[(byte)((value >> 16) & 0x0F)];
            charValues[4] = HexValues[(byte)((value >> 12) & 0x0F)];
            charValues[5] = HexValues[(byte)((value >> 8) & 0x0F)];
            charValues[6] = HexValues[(byte)((value >> 4) & 0x0F)];
            charValues[7] = HexValues[(byte)((value >> 0) & 0x0F)];
            return prefix + new string(charValues);
        }
    }
}
