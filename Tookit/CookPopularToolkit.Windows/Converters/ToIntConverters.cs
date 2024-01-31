/*
 *Description: ToIntConverters
 *Author: Chance.zheng
 *Creat Time: 2024/1/31 17:17:57
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace CookPopularToolkit.Windows
{
    [MarkupExtensionReturnType(typeof(int))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class StringToIntConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str && !string.IsNullOrEmpty(str))
            {
                return int.Parse(str);
            }

            return value;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString();
        }
    }
}
