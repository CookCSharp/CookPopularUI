/*
 *Description: GeometryHelper
 *Author: Chance.zheng
 *Creat Time: 2023/8/30 15:37:53
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CookPopularUI.WPF.Controls
{
    public class GeometryHelper
    {
        /// <summary>
        /// Gets the normalized arc in a (0,0)(1,1) box.
        /// Zero degrees is mapped to [0.5, 0] (up), and clockwise.
        /// </summary>
        internal static Point GetArcPoint(double degree)
        {
            double num = degree * 3.141592653589793 / 180.0;
            return new Point(0.5 + 0.5 * Math.Sin(num), 0.5 - 0.5 * Math.Cos(num));
        }

        /// <summary>
        /// Maps a relative point to an absolute point using the mapping from a given bound to a (0,0)(1,1) box.
        /// </summary>
        internal static Point RelativeToAbsolutePoint(Rect bound, Point relative)
        {
            return new Point(bound.X + relative.X * bound.Width, bound.Y + relative.Y * bound.Height);
        }

        /// <summary>
        /// Gets the absolute arc point in a given bound with a given relative radius.
        /// </summary>
        internal static Point GetArcPoint(double degree, Rect bound)
        {
            Point arcPoint = GetArcPoint(degree);
            return RelativeToAbsolutePoint(bound, arcPoint);
        }

        /// <summary>
        /// Ensures the value is an instance of result type (T). If not, replace with a new instance of type (T).
        /// </summary>
        internal static bool EnsureGeometryType<T>(out T result, ref Geometry value, Func<T> factory) where T : Geometry
        {
            result = value as T;
            if (result == null)
            {
                value = (result = factory());
                return true;
            }
            return false;
        }

        /// <summary>
        /// Ensures the list[index] is an instance of result type (T). If not, replace with a new instance of type (T).
        /// </summary>
        internal static bool EnsureSegmentType<T>(out T result, IList<PathSegment> list, int index, Func<T> factory) where T : PathSegment
        {
            result = (T)list[index];
            if (result == null)
            {
                list[index] = (result = factory());
                return true;
            }
            return false;
        }
    }
}
