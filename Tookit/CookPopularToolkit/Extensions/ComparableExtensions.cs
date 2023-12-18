/*
 *Description: ComparableExtensions
 *Author: Chance.zheng
 *Creat Time: 2023/8/11 16:02:31
 *.Net Version: 2.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Text;

namespace CookPopularToolkit
{
    public static class ComparableExtensions
    {
        /// <summary>
        /// Makes sure a comparable object is between a given range.
        /// </summary>
        public static T Clamp<T>(this T source, T min, T max) where T : IComparable
        {
            var isReversed = min.CompareTo(max) > 0;
            var smallest = isReversed ? max : min;
            var biggest = isReversed ? min : max;

            return source.CompareTo(smallest) < 0 ? smallest : source.CompareTo(biggest) > 0 ? biggest : source;
        }
    }
}
