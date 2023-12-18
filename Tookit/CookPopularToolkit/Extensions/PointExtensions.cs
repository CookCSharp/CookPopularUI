/*
 *Description: PointExtensions
 *Author: Chance.zheng
 *Creat Time: 2023/8/11 16:30:42
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
    public static class PointExtensions
    {
        [Pure]
        public static bool IsValid(this Point value) => ((double)value.X).IsValid() && ((double)value.Y).IsValid();

        /// <summary>
        /// Calculates the distance between two points.
        /// </summary>
        /// <param name="self">The first point.</param>
        /// <param name="other">The second point.</param>
        /// <returns>The distance of the points.</returns>
        public static double Distance(this Point self, Point other) => self.Distance(other.X, other.Y);

        /// <summary>
        /// Calculates the distance between a point and an x/y coordinate pair.
        /// </summary>
        /// <param name="self">The first point.</param>
        /// <param name="otherX">The x-coordinate of the second point.</param>
        /// <param name="otherY">The x-coordinate of the second point.</param>
        /// <returns>The distance of the points.</returns>
        public static double Distance(this Point self, double otherX, double otherY) => Math.Sqrt(Math.Pow(self.X - otherX, 2) + Math.Pow(self.Y - otherY, 2));
    }
}
