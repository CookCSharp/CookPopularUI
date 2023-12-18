/*
 *Description: ComboBoxItemPaddingConverter
 *Author: Chance.zheng
 *Creat Time: 2021-03-23 18:42:49
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
    [MarkupExtensionReturnType(typeof(Thickness))]
    [ValueConversion(typeof(Thickness), typeof(Thickness))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class ComboBoxItemPaddingConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Thickness thickness && int.TryParse(parameter?.ToString(), out int param))
            {
                return new Thickness(thickness.Left + param, thickness.Top, thickness.Right, thickness.Bottom);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
