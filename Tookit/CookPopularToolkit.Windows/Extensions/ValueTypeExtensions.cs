/*
 *Description: ValueTypeExtensions
 *Author: Chance.zheng
 *Creat Time: 2023/8/18 18:32:53
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace CookPopularToolkit.Windows
{
    public static class ValueTypeExtensions
    {
        [Pure]
        public static bool IsValid(this Vector value)
        {
            return value.X.IsValid() && value.Y.IsValid();
        }

        [Pure]
        public static bool IsValid(this System.Windows.Point value)
        {
            return value.X.IsValid() && value.Y.IsValid();
        }

        [Pure]
        public static bool IsValid(this System.Windows.Size value)
        {
            return value.Width.IsValid() && value.Height.IsValid();
        }

        [Pure]
        public static bool IsValid(this Point3D value)
        {
            return value.X.IsValid() && value.Y.IsValid() && value.Z.IsValid();
        }

        [Pure]
        public static bool IsValid(this Vector3D value)
        {
            return value.X.IsValid() && value.Y.IsValid() && value.Z.IsValid();
        }

        public static System.Windows.Point ToWpfPoint(this System.Windows.Point pixelPoint)
        {
            var dpi = DpiHelper.DeviceDpiX;
            var physicalUnitSize = 96d / dpi;
            var wpfPoint = new System.Windows.Point(physicalUnitSize * pixelPoint.X, physicalUnitSize * pixelPoint.Y);

            return wpfPoint;
        }
    }
}
