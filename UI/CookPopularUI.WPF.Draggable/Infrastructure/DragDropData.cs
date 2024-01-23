/*
 *Description: DragDropData
 *Author: Chance.zheng
 *Creat Time: 2024/1/22 20:09:08
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CookPopularUI.WPF.Draggable
{
    public class DragDropData
    {
        public static bool GetIsDragSource(DependencyObject obj) => (bool)obj.GetValue(IsDragSourceProperty);
        public static void SetIsDragSource(DependencyObject obj, bool value) => obj.SetValue(IsDragSourceProperty, value);
        public static readonly DependencyProperty IsDragSourceProperty =
            DependencyProperty.RegisterAttached("IsDragSource", typeof(bool), typeof(DragDropData), new PropertyMetadata(false, OnIsDragSourceChanged));

        private static void OnIsDragSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabControl tabControl)
            {

            }
        }

        public static bool GetIsDropTarget(DependencyObject obj) => (bool)obj.GetValue(IsDropTargetProperty);
        public static void SetIsDropTarget(DependencyObject obj, bool value) => obj.SetValue(IsDropTargetProperty, value);
        public static readonly DependencyProperty IsDropTargetProperty =
            DependencyProperty.RegisterAttached("IsDropTarget", typeof(bool), typeof(DragDropData), new PropertyMetadata(false, OnIsDropTargetChanged));

        private static void OnIsDropTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabControl tabControl)
            {

            }
        }

        public static string GetProviderDataFromatString(DependencyObject obj) => (string)obj.GetValue(ProviderDataFromatStringProperty);
        public static void SetProviderDataFromatString(DependencyObject obj, string value) => obj.SetValue(ProviderDataFromatStringProperty, value);
        public static readonly DependencyProperty ProviderDataFromatStringProperty =
            DependencyProperty.RegisterAttached("ProviderDataFromatString", typeof(string), typeof(DragDropData), new PropertyMetadata(default));


        public static string GetConsumerDataFormatString(DependencyObject obj) => (string)obj.GetValue(ConsumerDataFormatStringProperty);
        public static void SetConsumerDataFormatString(DependencyObject obj, string value) => obj.SetValue(ConsumerDataFormatStringProperty, value);
        public static readonly DependencyProperty ConsumerDataFormatStringProperty =
            DependencyProperty.RegisterAttached("ConsumerDataFormatString", typeof(string), typeof(DragDropData), new PropertyMetadata(default));
    }
}
