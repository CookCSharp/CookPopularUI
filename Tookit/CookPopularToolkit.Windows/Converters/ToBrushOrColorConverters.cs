/*
 * Description：ToBrushOrColorConverters 
 * Author： Chance.Zheng
 * Create Time: 2022-08-28 15:58:08
 * .Net Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2020-2022 All Rights Reserved.
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
using System.Windows.Media;

namespace CookPopularToolkit.Windows
{
    [MarkupExtensionReturnType(typeof(System.Windows.Media.Color))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class BrushToColorConverter : MarkupExtensionBase, IValueConverter
    {
        public static readonly BrushToColorConverter Instance = new BrushToColorConverter();

        private BrushToColorConverter()
        {
        }

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var brush = value as SolidColorBrush;
            if (brush != null)
                return brush.Color;

            return default;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new SolidColorBrush((System.Windows.Media.Color)value);
        }
    }

    [MarkupExtensionReturnType(typeof(System.Windows.Media.Brush))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class ColorToBrushConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new SolidColorBrush((System.Windows.Media.Color)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((SolidColorBrush)value).Color;
        }
    }
}
