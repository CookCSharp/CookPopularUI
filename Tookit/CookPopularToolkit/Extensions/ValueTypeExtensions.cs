/*
 *Description: ValueTypeExtensions
 *Author: Chance.zheng
 *Creat Time: 2023/8/11 15:41:54
 *.Net Version: 2.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Text;

namespace CookPopularToolkit
{
    /// <summary>
    ///  Provides the extensions class for value types.
    /// </summary>
    public static class ValueTypeExtensions
    {
        public static bool ToBoolean(this int value) => value != 0;

        public static bool ToBoolean(this float value) => value != 0;

        public static bool ToBoolean(this double value) => value != 0;

        public static bool ToBoolean(this long value) => value != 0;

        /// <summary>
        /// Rounds the number down the the next even number.
        /// </summary>
        public static int Even(this int self) => self % 2 == 1 ? self - 1 : self;

        [Pure]
        public static bool IsValid(this double value) => !double.IsInfinity(value) && !double.IsNaN(value);

        public static int ToInt(this double value) => Convert.ToInt32(value);

        public static bool HasValue(this double value) => !double.IsNaN(value) && !double.IsInfinity(value);

        [Pure]
        public static bool IsValid(this Size value) => ((double)value.Width).IsValid() && ((double)value.Height).IsValid();

        [Pure]
        public static bool IsValid(this Point value)
        {
            return ((double)value.X).IsValid() && ((double)value.Y).IsValid();
        }

        public static int ToInt(this Enum value) => (int)(object)value;

        public static uint ToUInt(this Enum value) => (uint)(object)value;

        /// <summary>
        /// Gets a list of flags which are set in an <see cref="Enum"/>.
        /// </summary>
        public static IEnumerable<Enum> GetFlags(this Enum variable) => Enum.GetValues(variable.GetType()).Cast<Enum>().Where(variable.HasFlag);
    }
}
