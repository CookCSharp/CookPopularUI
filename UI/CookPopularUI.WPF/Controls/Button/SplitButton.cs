/*
 *Description: SplitButton
 *Author: Chance.zheng
 *Creat Time: 2023/9/3 21:07:38
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularToolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace CookPopularUI.WPF.Controls
{
    [Localizability(LocalizationCategory.Button)]
    public class SplitButton : ButtonBase
    {
        [Bindable(true)]
        [Category("Layout")]
        [TypeConverter(typeof(LengthConverter))]
        public double MaxDropDownHeight
        {
            get => (double)GetValue(MaxDropDownHeightProperty);
            set => SetValue(MaxDropDownHeightProperty, value);
        }
        /// <summary>
        /// 提供<see cref="MaxDropDownHeight"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty MaxDropDownHeightProperty =
            DependencyProperty.Register(nameof(MaxDropDownHeight), typeof(double), typeof(SplitButton), new FrameworkPropertyMetadata(SystemParameters.PrimaryScreenHeight / 3.0, FrameworkPropertyMetadataOptions.AffectsRender));

        private static void OnVisualStatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Control)?.InvokeInternal("UpdateVisualState", new object[] { true });
        }


        [Bindable(true)]
        [Browsable(false)]
        [Category("Appearance")]
        public bool IsDropDownOpen
        {
            get => (bool)GetValue(IsDropDownOpenProperty);
            set => SetValue(IsDropDownOpenProperty, value);
        }
        /// <summary>
        /// 提供<see cref="IsDropDownOpen"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsDropDownOpenProperty =
            DependencyProperty.Register(nameof(IsDropDownOpen), typeof(bool), typeof(SplitButton), new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsDropDownOpenChanged, CoerceIsDropDownOpen));

        private static void OnIsDropDownOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SplitButton sb)
            {
                if ((bool)e.NewValue)
                {
                    sb.OnDropDownOpened(sb);
                }
                else
                {
                    sb.OnDropDownClosed(sb);
                }
            }
        }

        private static object CoerceIsDropDownOpen(DependencyObject d, object value)
        {
            if ((bool)value)
            {
                SplitButton sb = (SplitButton)d;
                if (!sb.IsLoaded)
                {
                    sb.Loaded += (s, e) =>
                    {
                        sb.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (DispatcherOperationCallback)delegate
                        {
                            sb.CoerceValue(IsDropDownOpenProperty);
                            return null;
                        }, null);
                    };
                    return ValueBoxes.FalseBox;
                }
            }

            return value;
        }


        public object DropDownContent
        {
            get => (object)GetValue(DropDownContentProperty);
            set => SetValue(DropDownContentProperty, value);
        }
        /// <summary>
        /// 提供<see cref="DropDownContent"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty DropDownContentProperty =
            DependencyProperty.Register(nameof(DropDownContent), typeof(object), typeof(SplitButton), new PropertyMetadata(default(object)));


        /// <summary>
        /// 下拉框打开事件
        /// </summary>
        [Category("Behavior")]
        public event RoutedEventHandler DropDownOpened
        {
            add => this.AddHandler(DropDownOpenedEvent, value);
            remove => this.RemoveHandler(DropDownOpenedEvent, value);
        }
        public static readonly RoutedEvent DropDownOpenedEvent =
            EventManager.RegisterRoutedEvent(nameof(DropDownOpened), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SplitButton));

        protected virtual void OnDropDownOpened(object source)
        {
            RoutedEventArgs arg = new RoutedEventArgs(DropDownOpenedEvent, source);
            this.RaiseEvent(arg);
        }


        /// <summary>
        /// 下拉框关闭事件
        /// </summary>
        [Category("Behavior")]
        public event RoutedEventHandler DropDownClosed
        {
            add => this.AddHandler(DropDownClosedEvent, value);
            remove => this.RemoveHandler(DropDownClosedEvent, value);
        }
        public static readonly RoutedEvent DropDownClosedEvent =
            EventManager.RegisterRoutedEvent(nameof(DropDownClosed), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SplitButton));

        protected virtual void OnDropDownClosed(object source)
        {
            RoutedEventArgs arg = new RoutedEventArgs(DropDownClosedEvent, source);
            this.RaiseEvent(arg);
        }
    }
}
