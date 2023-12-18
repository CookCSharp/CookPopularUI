/*
 *Description: ToCornerRadiusConverter
 *Author: Chance.zheng
 *Creat Time: 2023/9/4 11:13:32
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
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace CookPopularToolkit.Windows
{
    [MarkupExtensionReturnType(typeof(CornerRadius))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class BorderCircularToCornerRadiusConverter : MarkupExtensionBase, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is double width && values[1] is double height)
            {
                if (width < double.Epsilon || height < double.Epsilon)
                {
                    return new CornerRadius();
                }

                var min = Math.Min(width, height);
                return new CornerRadius(min / 2);
            }

            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Double To CornerRadius
    /// </summary>
    [MarkupExtensionReturnType(typeof(CornerRadius))]
    [ValueConversion(typeof(double), typeof(CornerRadius))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class DoubleToCornerRadiusConverter : MarkupExtensionBase, IValueConverter
    {
        public static CornerRadius FixedCornerRadius = new CornerRadius(1);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            parameter = parameter ?? 1;
            var isDouble = double.TryParse(parameter.ToString(), out double p);
            if (value is double v && isDouble)
            {
                return new CornerRadius(v * p);
            }

            return FixedCornerRadius;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CornerRadius cornerRadius)
                return cornerRadius.TopLeft;
            else
                return double.NaN;
        }
    }


    [MarkupExtensionReturnType(typeof(CornerRadius))]
    [ValueConversion(typeof(CornerRadius), typeof(CornerRadius))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class CornerRadiusToOrientationCornerRadiusConverter : MarkupExtensionBase, IValueConverter
    {
        public static CornerRadius FixedCornerRadius = new CornerRadius(1, 0, 0, 0);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CornerRadius cornerRadius && parameter is string param)
            {
                object th = param switch
                {
                    "TopLeft" => new CornerRadius(cornerRadius.TopLeft, 0, 0, 0),
                    "TopRight" => new CornerRadius(cornerRadius.TopRight, 0, 0, 0),
                    "BottomRight" => new CornerRadius(cornerRadius.BottomRight, 0, 0, 0),
                    "BottomLeft" => new CornerRadius(cornerRadius.BottomLeft, 0, 0, 0),
                    "Left" => new CornerRadius(cornerRadius.TopLeft, 0, 0, cornerRadius.BottomLeft),
                    "Right" => new CornerRadius(0, cornerRadius.TopRight, cornerRadius.BottomRight, 0),
                    "Top" => new CornerRadius(cornerRadius.TopLeft, cornerRadius.TopRight, 0, 0),
                    "Bottom" => new CornerRadius(0, 0, cornerRadius.BottomRight, cornerRadius.BottomLeft),
                    _ => throw new NotImplementedException()
                };

                return th;
            }

            return FixedCornerRadius;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
