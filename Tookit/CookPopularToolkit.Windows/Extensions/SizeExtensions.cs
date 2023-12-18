/*
 *Description: SizeExtensions
 *Author: Chance.zheng
 *Creat Time: 2023/8/23 15:24:47
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookPopularToolkit.Windows
{
    public static class SizeExtensions
    {
        public static bool IsNaN(this System.Windows.Size size)
            => double.IsNaN(size.Width) || double.IsNaN(size.Height);

        public static bool IsInfinity(this System.Windows.Size size)
            => double.IsInfinity(size.Width) || double.IsInfinity(size.Height);

        public static bool IsPositiveInfinity(this System.Windows.Size size)
            => double.IsPositiveInfinity(size.Width) || double.IsPositiveInfinity(size.Height);

        public static bool IsNegativeInfinity(this System.Windows.Size size)
            => double.IsNegativeInfinity(size.Width) || double.IsNegativeInfinity(size.Height);

        public static bool IsFiniteDouble(this System.Windows.Size size)
            => !double.IsInfinity(size.Width) && !double.IsNaN(size.Width) &&
               !double.IsInfinity(size.Height) && !double.IsNaN(size.Height);

        public static bool IsZero(this System.Windows.Size size)
            => MathHelper.IsZero(size.Width) || MathHelper.IsZero(size.Height);

        public static System.Windows.Rect Bounds(this System.Windows.Size size)
            => new System.Windows.Rect(0.0, 0.0, size.Width, size.Height);

        public static bool HasValidArea(this System.Windows.Size size)
            => size.Width > 0.0 && size.Height > 0.0 && !double.IsInfinity(size.Width) && !double.IsInfinity(size.Height);
    }
}
