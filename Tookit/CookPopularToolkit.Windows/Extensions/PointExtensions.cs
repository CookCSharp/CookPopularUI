/*
 *Description: PointExtensions
 *Author: Chance.zheng
 *Creat Time: 2023/8/30 16:18:48
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Point = System.Windows.Point;

namespace CookPopularToolkit.Windows
{
    public static class PointExtensions
    {
        public static Point Lerp(this Point pointA, Point pointB, double alpha)
        {
            return new Point(MathHelper.Lerp(pointA.X, pointB.X, alpha), MathHelper.Lerp(pointA.Y, pointB.Y, alpha));
        }

        /// <summary>
        /// Returns the mid point of two points.
        /// </summary>
        /// <param name="lhs">The first point.</param>
        /// <param name="rhs">The second point.</param>
        /// <returns>The mid point between <paramref name="lhs" /> and <paramref name="rhs" />.</returns>
        public static Point Midpoint(this Point lhs, Point rhs)
        {
            return new Point((lhs.X + rhs.X) / 2.0, (lhs.Y + rhs.Y) / 2.0);
        }

        /// <summary>
        /// Memberwise plus for Point.
        /// </summary>
        public static Point Plus(this Point lhs, Point rhs)
        {
            return new Point(lhs.X + rhs.X, lhs.Y + rhs.Y);
        }

        /// <summary>
        /// Memberwise minus for Point.
        /// </summary>
        public static Point Minus(this Point lhs, Point rhs)
        {
            return new Point(lhs.X - rhs.X, lhs.Y - rhs.Y);
        }

        /// <summary>
        /// Gets the difference vector between two points.
        /// </summary>
        public static Vector Subtract(this Point lhs, Point rhs)
        {
            return new Vector(lhs.X - rhs.X, lhs.Y - rhs.Y);
        }

        /// <summary>
        /// Returns the distance between two points.
        /// </summary>
        /// <param name="lhs">The first point.</param>
        /// <param name="rhs">The second point.</param>
        /// <returns>The distance between <paramref name="lhs" /> and <paramref name="rhs" />.</returns>
        public static double Distance(this Point lhs, Point rhs)
        {
            double num = lhs.X - rhs.X;
            double num2 = lhs.Y - rhs.Y;
            return Math.Sqrt(num * num + num2 * num2);
        }

        /// <summary>
        /// Returns the square of the distance between two points.
        /// </summary>
        /// <param name="lhs">The first point.</param>
        /// <param name="rhs">The second point.</param>
        /// <returns>The square of the distance between <paramref name="lhs" /> and <paramref name="rhs" />.</returns>
        public static double SquaredDistance(this Point lhs, Point rhs)
        {
            double num = lhs.X - rhs.X;
            double num2 = lhs.Y - rhs.Y;
            return num * num + num2 * num2;
        }

        /// <summary>
        /// Returns the dot product of two points.
        /// </summary>
        public static double Dot(this Point lhs, Point rhs)
        {
            return lhs.X * rhs.X + lhs.Y * rhs.Y;
        }

        /// <summary>
        /// Determinant of the cross product. Equivalent to directional area.
        /// </summary>
        public static double Determinant(this Point lhs, Point rhs)
        {
            return lhs.X * rhs.Y - lhs.Y * rhs.X;
        }
    }
}
