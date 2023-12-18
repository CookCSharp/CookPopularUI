/*
 *Description: ItemControlTypeToVisibilityConverter
 *Author: Chance.zheng
 *Creat Time: 2023-02-20 14:12:24
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2021 All Rights Reserved.
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
using CookPopularToolkit.Windows;

namespace CookPopularUI.WPF
{
    [MarkupExtensionReturnType(typeof(Visibility))]
    [ValueConversion(typeof(ItemControlType), typeof(Visibility))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class ItemControlTypeToVisibilityConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.Equals(parameter))
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
