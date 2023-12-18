/*
 *Description: GroupBoxBorderConverter
 *Author: Chance.zheng
 *Creat Time: 2023/12/11 16:18:45
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
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CookPopularUI.WPF
{
    public class GroupBoxBorderConverter : IMultiValueConverter
    {
        public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Type typeFromHandle = typeof(double);
            if (parameter == null || values == null || values.Length != 4 || values[0] == null || values[1] == null || values[2] == null || !typeFromHandle.IsAssignableFrom(values[0].GetType()) || !typeFromHandle.IsAssignableFrom(values[1].GetType()) || !typeFromHandle.IsAssignableFrom(values[2].GetType()))
            {
                return DependencyProperty.UnsetValue;
            }

            Type type = parameter.GetType();
            if (!typeFromHandle.IsAssignableFrom(type) && !typeof(string).IsAssignableFrom(type))
            {
                return DependencyProperty.UnsetValue;
            }

            double pixels = (double)values[0];
            double num = (double)values[1];
            double num2 = (double)values[2];
            if (num == 0.0 || num2 == 0.0)
            {
                return null;
            }
            HorizontalAlignment horizontalAlignment = (HorizontalAlignment)values[3];

            double pixels2 = ((!(parameter is string)) ? ((double)parameter) : double.Parse((string)parameter, NumberFormatInfo.InvariantInfo));
            Grid grid = new Grid();
            grid.Width = num;
            grid.Height = num2;

            ColumnDefinition columnDefinition1 = new ColumnDefinition();
            ColumnDefinition columnDefinition2 = new ColumnDefinition();
            ColumnDefinition columnDefinition3 = new ColumnDefinition();
            ColumnDefinition columnDefinition4 = new ColumnDefinition();
            switch (horizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    columnDefinition1.Width = new GridLength(pixels2);
                    columnDefinition2.Width = new GridLength(0);
                    columnDefinition3.Width = new GridLength(pixels);
                    columnDefinition4.Width = new GridLength(1.0, GridUnitType.Star);
                    break;
                case HorizontalAlignment.Stretch or HorizontalAlignment.Center:
                    columnDefinition1.Width = new GridLength(1.0, GridUnitType.Star);
                    columnDefinition2.Width = new GridLength(0);
                    columnDefinition3.Width = new GridLength(pixels);
                    columnDefinition4.Width = new GridLength(1.0, GridUnitType.Star);
                    break;
                case HorizontalAlignment.Right:
                    columnDefinition1.Width = new GridLength(1.0, GridUnitType.Star);
                    columnDefinition2.Width = new GridLength(0);
                    columnDefinition3.Width = new GridLength(pixels);
                    columnDefinition4.Width = new GridLength(pixels2);
                    break;
                default:
                    break;
            }
            grid.ColumnDefinitions.Add(columnDefinition1);
            grid.ColumnDefinitions.Add(columnDefinition2);
            grid.ColumnDefinitions.Add(columnDefinition3);
            grid.ColumnDefinitions.Add(columnDefinition4);

            RowDefinition rowDefinition1 = new RowDefinition();
            RowDefinition rowDefinition2 = new RowDefinition();
            rowDefinition1.Height = new GridLength(num2 / 2.0);
            rowDefinition2.Height = new GridLength(1.0, GridUnitType.Star);
            grid.RowDefinitions.Add(rowDefinition1);
            grid.RowDefinitions.Add(rowDefinition2);

            Rectangle rectangle1 = new Rectangle();
            Rectangle rectangle2 = new Rectangle();
            Rectangle rectangle3 = new Rectangle();
            Rectangle rectangle4 = new Rectangle();
            rectangle1.Fill = Brushes.Black;
            rectangle2.Fill = Brushes.Black;
            rectangle3.Fill = Brushes.Black;
            rectangle4.Fill = Brushes.Black;
            Grid.SetRowSpan(rectangle1, 2);
            Grid.SetRow(rectangle1, 0);
            Grid.SetColumn(rectangle1, 0);
            Grid.SetRow(rectangle2, 1);
            Grid.SetColumn(rectangle2, 1);
            Grid.SetRow(rectangle3, 1);
            Grid.SetColumn(rectangle3, 2);
            Grid.SetRowSpan(rectangle4, 2);
            Grid.SetRow(rectangle4, 0);
            Grid.SetColumn(rectangle4, 3);
            grid.Children.Add(rectangle1);
            grid.Children.Add(rectangle2);
            grid.Children.Add(rectangle3);
            grid.Children.Add(rectangle4);

            return new VisualBrush(grid);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return [Binding.DoNothing];
        }
    }
}
