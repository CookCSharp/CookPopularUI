/*
 *Description: ValueBoxes
 *Author: Chance.zheng
 *Creat Time: 2023/8/17 17:47:29
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

namespace CookPopularToolkit.Windows
{
    public static class ValueBoxes
    {
        public static readonly object VisibleBox = Visibility.Visible;
        public static readonly object HiddenBox = Visibility.Hidden;
        public static readonly object CollapsedBox = Visibility.Collapsed;

        public static readonly object CornerRadius0Box = new CornerRadius(0);
        public static readonly object CornerRadius2Box = new CornerRadius(2);
        public static readonly object CornerRadius10Box = new CornerRadius(10);
        public static readonly object MarginRight10Box = new Thickness(0, 0, 10, 0);

        public static object VisibilityBox(this Visibility visibility)
        {
            object v;
            switch (visibility)
            {
                case Visibility.Visible:
                    v = VisibleBox;
                    break;
                case Visibility.Hidden:
                    v = HiddenBox;
                    break;
                case Visibility.Collapsed:
                    v = CollapsedBox;
                    break;
                default:
                    v = default(Visibility);
                    break;
            }

            return v;
        }
    }
}
