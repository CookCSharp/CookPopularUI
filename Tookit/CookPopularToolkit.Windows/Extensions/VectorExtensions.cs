/*
 *Description: VectorExtensions
 *Author: Chance.zheng
 *Creat Time: 2023/8/30 15:44:47
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
    public static class VectorExtensions
    {
        public static Vector Normalized(this Vector vector)
        {
            Vector vector2 = new Vector(vector.X, vector.Y);
            double length = vector2.Length;
            if (MathHelper.IsVerySmall(length))
            {
                return new Vector(0.0, 1.0);
            }
            return vector2 / length;
        }

        /// <summary>
        /// Computes the normal direction vector of given line segments.
        /// </summary>
        internal static Vector Normal(this Point lhs, Point rhs)
        {
            return new Vector(lhs.Y - rhs.Y, rhs.X - lhs.X).Normalized();
        }

        /// <summary>
        /// Returns the dot product of two vectors.
        /// </summary>
        /// <param name="lhs">The first vector.</param>
        /// <param name="rhs">The second vector.</param>
        /// <returns>The dot product of <paramref name="lhs" /> and <paramref name="rhs" />.</returns>
        public static double Dot(this Vector lhs, Vector rhs)
        {
            return lhs.X * rhs.X + lhs.Y * rhs.Y;
        }

        public static Vector Lerp(this Vector vectorA, Vector vectorB, double alpha)
        {
            return new Vector(MathHelper.Lerp(vectorA.X, vectorB.X, alpha), MathHelper.Lerp(vectorA.Y, vectorB.Y, alpha));
        }
    }
}
