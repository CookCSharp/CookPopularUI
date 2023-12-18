/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ToBooleanConverters
 * Author： Chance_写代码的厨子
 * Create Time：2021-11-06 20:06:29
 */


using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Linq;
using Binding = System.Windows.Data.Binding;

namespace CookPopularToolkit.Windows
{
    /// <summary>
    /// Bool to Re bool
    /// </summary>
    [MarkupExtensionReturnType(typeof(bool))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class BooleanToReBooleanConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
                return !b;

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }


    /// <summary>
    /// 值在于(0,100)之间，返回Ture，否则返回False
    /// </summary>
    [MarkupExtensionReturnType(typeof(bool))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class ValueBetween0And100ToBooleanConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double v)
            {
                return v > 0 && v < 100 ? true : false;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }


    [MarkupExtensionReturnType(typeof(bool))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class EqualityToBooleanConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Equals(value, parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.Equals(true) == true ? parameter : System.Windows.Data.Binding.DoNothing;
        }
    }


    [MarkupExtensionReturnType(typeof(bool))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class ValueLessThanTargetValueToBooleanConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double v && double.TryParse(parameter.ToString(), out double p))
            {
                return v < p ? true : false;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }


    [MarkupExtensionReturnType(typeof(bool))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class ValueMoreThanTargetValueToBooleanConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double v && double.TryParse(parameter.ToString(), out double p))
            {
                return v > p ? true : false;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }


    [MarkupExtensionReturnType(typeof(bool))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class EmptyOrNullToBooleanConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value?.ToString()))
                return true;
            else
                return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    [MarkupExtensionReturnType(typeof(bool))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class NotEmptyAndNullToBooleanConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!string.IsNullOrEmpty(value?.ToString()))
                return true;
            else
                return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    [MarkupExtensionReturnType(typeof(bool))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    [ValueConversion(typeof(Enum), typeof(bool))]
    public class EnumToBooleanConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return DependencyProperty.UnsetValue;

            if (Enum.IsDefined(value.GetType(), value) == false)
                return DependencyProperty.UnsetValue;

            string parameterString = parameter.ToString()!;
            object parameterValue = Enum.Parse(value.GetType(), parameterString);

            return parameterValue.Equals(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool v && v)
            {
                //var can = Enum.TryParse<object>(targetType, parameter.ToString(), out object result);
                //if (can) return result;

                return Enum.Parse(targetType, parameter.ToString());
            }
            return DependencyProperty.UnsetValue;
        }
    }


    [MarkupExtensionReturnType(typeof(bool))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class IsFirstItemConverter : MarkupExtensionBase, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length > 1 && values[0] is object item && values[1] is IList items)
            {
                return items.IndexOf(item) == 0;
            }

            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    [MarkupExtensionReturnType(typeof(bool))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class IsLastItemConverter : MarkupExtensionBase, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length > 1 && values[0] is object item && values[1] is IList items)
            {
                return items.IndexOf(item) == items.Count - 1;
            }

            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
