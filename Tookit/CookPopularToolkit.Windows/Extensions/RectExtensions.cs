/*
 *Description: RectExtensions
 *Author: Chance.zheng
 *Creat Time: 2023/8/30 15:21:26
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
using System.Windows.Media;

namespace CookPopularToolkit.Windows
{
    public static class RectExtensions
    {
        public static Thickness Subtract(this Rect lhs, Rect rhs)
        {
            return new Thickness(rhs.Left - lhs.Left, rhs.Top - lhs.Top, lhs.Right - rhs.Right, lhs.Bottom - rhs.Bottom);
        }

        public static System.Windows.Point Center(this Rect rect)
        {
            return new System.Windows.Point(rect.X + rect.Width / 2.0, rect.Y + rect.Height / 2.0);
        }

        public static Rect GetStretchBound(this Rect logicalBound, Stretch stretch, System.Windows.Size aspectRatio)
        {
            if (stretch == Stretch.None)
            {
                stretch = Stretch.Fill;
            }
            if (stretch == Stretch.Fill || !aspectRatio.HasValidArea())
            {
                return logicalBound;
            }
            System.Windows.Point point = logicalBound.Center();
            if (stretch == Stretch.Uniform)
            {
                if (aspectRatio.Width * logicalBound.Height < logicalBound.Width * aspectRatio.Height)
                {
                    logicalBound.Width = logicalBound.Height * aspectRatio.Width / aspectRatio.Height;
                }
                else
                {
                    logicalBound.Height = logicalBound.Width * aspectRatio.Height / aspectRatio.Width;
                }
            }
            else if (stretch == Stretch.UniformToFill)
            {
                if (aspectRatio.Width * logicalBound.Height < logicalBound.Width * aspectRatio.Height)
                {
                    logicalBound.Height = logicalBound.Width * aspectRatio.Height / aspectRatio.Width;
                }
                else
                {
                    logicalBound.Width = logicalBound.Height * aspectRatio.Width / aspectRatio.Height;
                }
            }
            return new Rect(point.X - logicalBound.Width / 2.0, point.Y - logicalBound.Height / 2.0, logicalBound.Width, logicalBound.Height);
        }

        /// <summary>
        /// Resizes the rectangle to a relative size while keeping the center invariant.
        /// </summary>
        public static Rect Resize(this Rect rect, double ratio)
        {
            return rect.Resize(ratio, ratio);
        }

        public static Rect Resize(this Rect rect, double ratioX, double ratioY)
        {
            System.Windows.Point point = rect.Center();
            double num = rect.Width * ratioX;
            double num2 = rect.Height * ratioY;
            return new Rect(point.X - num / 2.0, point.Y - num2 / 2.0, num, num2);
        }

        public static Rect Inflate(this Rect rect, double offset)
        {
            return Inflate(rect, new Thickness(offset));
        }

        public static Rect Inflate(this Rect rect, Thickness thickness)
        {
            double num = rect.Width + thickness.Left + thickness.Right;
            double num2 = rect.Height + thickness.Top + thickness.Bottom;
            double num3 = rect.X - thickness.Left;
            if (num < 0.0)
            {
                num3 += num / 2.0;
                num = 0.0;
            }
            double num4 = rect.Y - thickness.Top;
            if (num2 < 0.0)
            {
                num4 += num2 / 2.0;
                num2 = 0.0;
            }
            return new Rect(num3, num4, num, num2);
        }
    }
}
