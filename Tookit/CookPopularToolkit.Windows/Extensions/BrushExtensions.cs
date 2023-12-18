/*
 *Description: BrushExtensions
 *Author: Chance.zheng
 *Creat Time: 2023/8/18 17:32:20
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CookPopularToolkit.Windows
{
    public static class BrushExtensions
    {
        /// <summary>
        /// 十六进制转<see cref="System.Windows.Media.Brush"/>
        /// </summary>
        /// <param name="hexadecimalString"></param>
        /// <returns></returns>
        public static System.Windows.Media.Brush ToBrush(this string hexadecimalString)
        {
            var brush = new BrushConverter().ConvertFromString(hexadecimalString);
            if (brush == null)
                return System.Windows.Media.Brushes.Transparent;
            else
                return (System.Windows.Media.Brush)brush;
        }

        /// <summary>
        /// 十六进制转<see cref="System.Windows.Media.Color"/>
        /// </summary>
        /// <param name="hexadecimalString"></param>
        /// <returns></returns>
        public static System.Windows.Media.Color ToColor(this string hexadecimalString) => (System.Windows.Media.Color)(System.Windows.Media.ColorConverter.ConvertFromString(hexadecimalString));
    }
}
