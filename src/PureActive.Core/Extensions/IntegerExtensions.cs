// ***********************************************************************
// Assembly         : PureActive.Core
// Author           : SteveBu
// Created          : 10-31-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="IntegerExtensions.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace PureActive.Core.Extensions
{
    /// <summary>
    /// Extension methods for Integers
    /// </summary>
    public static class IntegerExtensions
    {
        /// <summary>
        /// Converts a Integer to Hex string
        /// </summary>
        /// <param name="value">Integer to convert</param>
        /// <param name="prefix">String'0x' to include before the Hex value.</param>
        /// <returns>Integer value in Hex</returns>
        public static string ToHexString(this short value, string prefix)
        {
            return $"{prefix}{value:X4}";
        }

        /// <summary>
        /// Converts a Integer to Hex string
        /// </summary>
        /// <param name="value">Integer to convert</param>
        /// <param name="prefix">String'0x' to include before the Hex value.</param>
        /// <returns>Integer value in Hex</returns>
        public static string ToHexString(this int value, string prefix)
        {
            return $"{prefix}{value:X8}";
        }

        /// <summary>
        /// Converts a Integer to Hex string
        /// </summary>
        /// <param name="value">Integer to convert</param>
        /// <param name="prefix">String'0x' to include before the Hex value.</param>
        /// <returns>Integer value in Hex</returns>
        public static string ToHexString(this ushort value, string prefix)
        {
            return $"{prefix}{value:X4}";
        }

        /// <summary>
        /// Converts a Integer to Hex string
        /// </summary>
        /// <param name="value">Integer to convert</param>
        /// <param name="prefix">String'0x' to include before the Hex value.</param>
        /// <returns>Integer value in Hex</returns>
        public static string ToHexString(this uint value, string prefix)
        {
            return $"{prefix}{value:X8}";
        }
    }
}