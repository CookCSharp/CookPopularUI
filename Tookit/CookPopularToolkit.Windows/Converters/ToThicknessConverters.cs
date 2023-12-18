/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ToThicknessConverters
 * Author： Chance_写代码的厨子
 * Create Time：2021-11-06 21:08:43
 */

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace CookPopularToolkit.Windows
{
    /// <summary>
    /// Double To Thickness
    /// </summary>
    [MarkupExtensionReturnType(typeof(Thickness))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    [ValueConversion(typeof(double), typeof(Thickness))]
    public class DoubleToThicknessConverter : MarkupExtensionBase, IValueConverter
    {
        public static Thickness FixedThickness = new Thickness(1);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            parameter = parameter ?? 1;
            var isDouble = double.TryParse(parameter.ToString(), out double p);
            if (value is double v && isDouble)
            {
                return new Thickness(v * p);
            }

            return FixedThickness;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Thickness thickness)
                return thickness.Left;
            else
                return double.NaN;
        }
    }


    /// <summary>
    /// Double To Thickness
    /// </summary>
    [MarkupExtensionReturnType(typeof(Thickness))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    [ValueConversion(typeof(double), typeof(Thickness))]
    public class DoubleToLeftRightThicknessConverter : MarkupExtensionBase, IValueConverter
    {
        public static Thickness FixedThickness = new Thickness(1);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            parameter = parameter ?? 1;
            var isDouble = double.TryParse(parameter.ToString(), out double p);
            if (value is double v && isDouble)
            {
                return new Thickness(v * p, 0, v * p, 0);
            }

            return FixedThickness;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Thickness thickness)
                return thickness.Left;
            else
                return double.NaN;
        }
    }


    /// <summary>
    /// Double To Thickness
    /// </summary>
    [MarkupExtensionReturnType(typeof(Thickness))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    [ValueConversion(typeof(double), typeof(Thickness))]
    public class DoubleToTopBottomThicknessConverter : MarkupExtensionBase, IValueConverter
    {
        public static Thickness FixedThickness = new Thickness(1);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            parameter = parameter ?? 1;
            var isDouble = double.TryParse(parameter.ToString(), out double p);
            if (value is double v && isDouble)
            {
                return new Thickness(0, v * p, 0, v * p);
            }

            return FixedThickness;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Thickness thickness)
                return thickness.Left;
            else
                return double.NaN;
        }
    }


    [MarkupExtensionReturnType(typeof(Thickness))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    [ValueConversion(typeof(Thickness), typeof(Thickness))]
    public class ThicknessToSideThicknessConverter : MarkupExtensionBase, IValueConverter
    {
        public static Thickness FixedThickness = new Thickness(1, 0, 0, 0);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Thickness thickness && parameter is string param)
            {
                object th = param switch
                {
                    "LeftTopRight" => new Thickness(thickness.Left, thickness.Top, thickness.Right, 0),
                    "LeftTopBottom" => new Thickness(thickness.Left, thickness.Top, 0, thickness.Bottom),
                    "LeftRightBottom" => new Thickness(thickness.Left, 0, thickness.Right, thickness.Bottom),
                    "TopRightBottom" => new Thickness(0, thickness.Top, thickness.Right, thickness.Bottom),
                    "LeftTop" => new Thickness(thickness.Left, thickness.Top, 0, 0),
                    "LeftRight" => new Thickness(thickness.Left, 0, thickness.Right, 0),
                    "LeftBottom" => new Thickness(thickness.Left, 0, 0, thickness.Bottom),
                    "TopRight" => new Thickness(0, thickness.Top, thickness.Right, 0),
                    "TopBottom" => new Thickness(0, thickness.Top, 0, thickness.Bottom),
                    "RightBottom" => new Thickness(0, 0, thickness.Right, thickness.Bottom),
                    "Left" => new Thickness(thickness.Left, 0, 0, 0),
                    "Top" => new Thickness(0, 0, thickness.Top, 0),
                    "Right" => new Thickness(0, 0, thickness.Right, 0),
                    "Bottom" => new Thickness(0, 0, 0, thickness.Bottom),
                    _ => throw new NotImplementedException()
                };

                return th;
            }

            return FixedThickness;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    [MarkupExtensionReturnType(typeof(Thickness))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    [ValueConversion(typeof(Thickness), typeof(Thickness))]
    public class ThicknessToTimesThicknessConverter : MarkupExtensionBase, IValueConverter
    {
        public static Thickness FixedThickness = new Thickness(1);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Thickness thickness && parameter is double param)
            {
                return new Thickness(thickness.Left * param, thickness.Top * param, thickness.Right * param, thickness.Bottom * param);
            }

            return FixedThickness;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
