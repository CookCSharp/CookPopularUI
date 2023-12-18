/*
 *Description: TextFieldClearButtonVisibilityConverter
 *Author: Chance.zheng
 *Creat Time: 2021-03-19 17:27:36
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
using CookPopularToolkit.Windows;

namespace CookPopularUI.WPF
{
    /// <summary>
    /// 文本清除按钮
    /// </summary>
    [MarkupExtensionReturnType(typeof(Visibility))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class TextFieldClearButtonVisibilityConverter : MarkupExtensionBase, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Count() < 2) return Visibility.Collapsed;
            if ((bool)values[0] && !string.IsNullOrEmpty(values[1] == null ? string.Empty : values[1].ToString()))
                return ValueBoxes.VisibleBox;
            else
                return ValueBoxes.CollapsedBox;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
