/*
 *Description: ItemsControlIsFirstConverter
 *Author: Chance.zheng
 *Creat Time: 2023/11/20 14:53:53
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows;
using CookPopularToolkit.Windows;
using System.Globalization;
using System.Windows.Controls.Primitives;

namespace CookPopularUI.WPF
{
    //[MarkupExtensionReturnType(typeof(bool))]
    //[Localizability(LocalizationCategory.NeverLocalize)]
    //public class ItemsControlIsFirstConverter : MarkupExtensionBase, IMultiValueConverter
    //{
    //    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (values.Length == 2 && values[0] is DependencyObject obj && values[1] is Selector selector)
    //        {
    //            return selector.ItemContainerGenerator.IndexFromContainer(obj) == 0;
    //        }

    //        return false;
    //    }

    //    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
