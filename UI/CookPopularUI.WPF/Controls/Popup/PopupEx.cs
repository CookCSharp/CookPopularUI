/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：PopupEx
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-27 13:11:46
 */


using CookPopularToolkit;
using CookPopularToolkit.Windows.Win32Api;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;

namespace CookPopularUI.WPF.Controls
{
    /// <summary>
    /// 提供<see cref="Popup"/>的扩展控件
    /// </summary>
    /// <remarks>可跟随父元素移动</remarks>
    public class PopupEx : Popup
    {
        /// <summary>
        /// 是否置前
        /// </summary>
        public bool IsTopMost
        {
            get { return (bool)GetValue(IsTopMostProperty); }
            set { SetValue(IsTopMostProperty, ValueBoxes.BooleanBox(value)); }
        }
        /// <summary>
        /// <see cref="IsTopMostProperty"/>提供<see cref="IsTopMost"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsTopMostProperty =
            DependencyProperty.Register("IsTopMost", typeof(bool), typeof(PopupEx),
                new PropertyMetadata(ValueBoxes.FalseBox, OnIsTopMostChanged));
        private static void OnIsTopMostChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PopupEx)?.UpdatePopup();
        }

        /// <summary>
        /// 是否跟随父元素移动
        /// </summary>
        public bool IsPositionUpdate
        {
            get { return (bool)GetValue(IsPositionUpdateProperty); }
            set { SetValue(IsPositionUpdateProperty, ValueBoxes.BooleanBox(value)); }
        }
        /// <summary>
        /// <see cref="IsPositionUpdateProperty"/>提供<see cref="IsPositionUpdate"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsPositionUpdateProperty =
            DependencyProperty.Register("IsPositionUpdate", typeof(bool), typeof(PopupEx),
                new PropertyMetadata(ValueBoxes.FalseBox, IsPositionUpdateChanged));
        private static void IsPositionUpdateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PopupEx)?.PopupEx_Loaded(d, default!);
        }

        /// <summary>
        /// 鼠标左键按下是否关闭<see cref="PopupEx"/>
        /// </summary>
        public bool IsMouseLeftButtonDownClosed
        {
            get { return (bool)GetValue(IsMouseLeftButtonDownClosedProperty); }
            set { SetValue(IsMouseLeftButtonDownClosedProperty, ValueBoxes.BooleanBox(value)); }
        }
        /// <summary>
        /// <see cref="IsMouseLeftButtonDownClosedProperty"/>提供<see cref="IsMouseLeftButtonDownClosed"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsMouseLeftButtonDownClosedProperty =
            DependencyProperty.Register("IsMouseLeftButtonDownClosed", typeof(bool), typeof(PopupEx), new PropertyMetadata(ValueBoxes.FalseBox));


        static PopupEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PopupEx), new FrameworkPropertyMetadata(typeof(PopupEx)));
        }

        public PopupEx()
        {
            this.Loaded += PopupEx_Loaded;
            this.Unloaded += PopupEx_Unloaded;
        }

        private void PopupEx_Loaded(object sender, RoutedEventArgs e)
        {
            PopupEx? pop = sender as PopupEx;
            if (pop == null) return;
            var win = Window.GetWindow(pop);
            if (win == null) return;
            win.LocationChanged -= HostWindow_PositionChanged;
            win.SizeChanged -= HostWindow_PositionChanged;
            pop.SizeChanged -= HostWindow_PositionChanged;
            win.StateChanged -= HostWindow_StateChanged;
            win.Activated -= HostWindow_Activated;
            win.Activated += HostWindow_Activated;
            win.Deactivated -= HostWindow_Deactivated;
            win.Deactivated += HostWindow_Deactivated;

            if (IsPositionUpdate)
            {
                win.LocationChanged += HostWindow_PositionChanged;
                win.SizeChanged += HostWindow_PositionChanged;
                pop.SizeChanged += HostWindow_PositionChanged;
                win.StateChanged += HostWindow_StateChanged;
            }
        }

        private void PopupEx_Unloaded(object sender, RoutedEventArgs e)
        {
            var win = Window.GetWindow(sender as FrameworkElement);
            if (PlacementTarget is FrameworkElement target)
            {
                target.SizeChanged -= HostWindow_PositionChanged;
            }
            if (win != null)
            {
                win.LocationChanged -= HostWindow_PositionChanged;
                win.SizeChanged -= HostWindow_PositionChanged;
                win.StateChanged -= HostWindow_StateChanged;
                win.Activated -= HostWindow_Activated;
                win.Deactivated -= HostWindow_Deactivated;
            }
            Unloaded -= PopupEx_Unloaded;
        }

        private void HostWindow_PositionChanged(object? sender, EventArgs e)
        {
            try
            {
                var method = typeof(Popup).GetMethod("UpdatePosition", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (IsOpen)
                    method?.Invoke(this, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void HostWindow_StateChanged(object? sender, EventArgs e)
        {
            var win = Window.GetWindow(sender as FrameworkElement);
            if (win != null && win.WindowState != WindowState.Minimized)
            {
                var target = PlacementTarget as FrameworkElement;
                var holder = target != null ? target.DataContext as AdornedElementPlaceholder : null;
                if (holder != null && holder.AdornedElement != null)
                {
                    PopupAnimation = System.Windows.Controls.Primitives.PopupAnimation.None;
                    IsOpen = false;
                    var errorTemplate = holder.AdornedElement.GetValue(Validation.ErrorTemplateProperty);
                    holder.AdornedElement.SetValue(Validation.ErrorTemplateProperty, null);
                    holder.AdornedElement.SetValue(Validation.ErrorTemplateProperty, errorTemplate);
                }
            }
        }

        private void HostWindow_Activated(object? sender, EventArgs e) => UpdatePopup(true);

        private void HostWindow_Deactivated(object? sender, EventArgs e) => UpdatePopup(false);

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (IsMouseLeftButtonDownClosed)
                IsOpen = false;

            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnOpened(EventArgs e)
        {
            UpdatePopup();

            base.OnOpened(e);
        }

        private void UpdatePopup(bool isTop = false)
        {
            if (!IsOpen) return;

            if (Child is null) return;

            var handSource = (HwndSource)PresentationSource.FromVisual(Child);
            if (handSource is null) return;

            isTop &= IsTopMost;
            var intPtr = handSource.Handle;
            if (!InteropMethods.GetWindowRect(intPtr, out InteropValues.RECT rect)) return;

            InteropMethods.SetWindowPos(intPtr, new IntPtr(isTop ? -1 : -2), rect.Left, rect.Top, (int)Width, (int)Height, InteropValues.WindowPositionFlags.TOPMOST);
        }
    }
}
