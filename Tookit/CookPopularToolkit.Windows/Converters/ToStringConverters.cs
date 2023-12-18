/*
 *Description: ToStringConverters
 *Author: Chance.zheng
 *Creat Time: 2023/12/15 16:17:20
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows;

namespace CookPopularToolkit.Windows
{
    [MarkupExtensionReturnType(typeof(string))]
    [ValueConversion(typeof(double), typeof(string), ParameterType = typeof(int))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class ToDecimalPlaceStringConverter : MarkupExtensionBase, IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double v && parameter is int p)
            {
                v.ToString($"F{p}");
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Windows.Data.Binding.DoNothing;
        }
    }
}
