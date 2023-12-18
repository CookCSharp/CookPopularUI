/*
 *Description: ScrollViewerEx
 *Author: Chance.zheng
 *Creat Time: 2023/9/28 19:20:44
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularToolkit;
using CookPopularToolkit.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows;
using ValueBoxes = CookPopularToolkit.ValueBoxes;


namespace CookPopularUI.WPF.Controls
{
    public class ScrollViewerEx : ScrollViewer
    {
        private double _totalVerticalOffset;

        private double _totalHorizontalOffset;

        private bool _isRunning;


        /// <summary>
        /// 是否响应鼠标滚轮操作
        /// </summary>
        public bool CanMouseWheel
        {
            get => (bool)GetValue(CanMouseWheelProperty);
            set => SetValue(CanMouseWheelProperty, ValueBoxes.BooleanBox(value));
        }
        /// <summary>
        /// 是否响应鼠标滚轮操作
        /// </summary>
        public static readonly DependencyProperty CanMouseWheelProperty =
            DependencyProperty.Register(nameof(CanMouseWheel), typeof(bool), typeof(ScrollViewer), new PropertyMetadata(ValueBoxes.TrueBox));

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (!CanMouseWheel) return;

            if (!IsInertiaEnabled)
            {
                if (ScrollViewerExAssistant.GetOrientation(this) == Orientation.Vertical)
                {
                    base.OnMouseWheel(e);
                }
                else
                {
                    _totalHorizontalOffset = HorizontalOffset;
                    CurrentHorizontalOffset = HorizontalOffset;
                    _totalHorizontalOffset = Math.Min(Math.Max(0, _totalHorizontalOffset - e.Delta), ScrollableWidth);
                    CurrentHorizontalOffset = _totalHorizontalOffset;
                }
                return;
            }
            e.Handled = true;

            if (ScrollViewerExAssistant.GetOrientation(this) == Orientation.Vertical)
            {
                if (!_isRunning)
                {
                    _totalVerticalOffset = VerticalOffset;
                    CurrentVerticalOffset = VerticalOffset;
                }
                _totalVerticalOffset = Math.Min(Math.Max(0, _totalVerticalOffset - e.Delta), ScrollableHeight);
                ScrollToVerticalOffsetWithAnimation(_totalVerticalOffset);
            }
            else
            {
                if (!_isRunning)
                {
                    _totalHorizontalOffset = HorizontalOffset;
                    CurrentHorizontalOffset = HorizontalOffset;
                }
                _totalHorizontalOffset = Math.Min(Math.Max(0, _totalHorizontalOffset - e.Delta), ScrollableWidth);
                ScrollToHorizontalOffsetWithAnimation(_totalHorizontalOffset);
            }
        }

        internal void ScrollToTopInternal(double milliseconds = 500)
        {
            if (!_isRunning)
            {
                _totalVerticalOffset = VerticalOffset;
                CurrentVerticalOffset = VerticalOffset;
            }
            ScrollToVerticalOffsetWithAnimation(0, milliseconds);
        }

        public void ScrollToVerticalOffsetWithAnimation(double offset, double milliseconds = 500)
        {
            var animation = AnimationHelper.CreateDoubleAnimation(offset, milliseconds);
            animation.EasingFunction = new CubicEase
            {
                EasingMode = EasingMode.EaseOut
            };
            animation.FillBehavior = FillBehavior.Stop;
            animation.Completed += (s, e1) =>
            {
                CurrentVerticalOffset = offset;
                _isRunning = false;
            };
            _isRunning = true;

            BeginAnimation(CurrentVerticalOffsetProperty, animation, HandoffBehavior.Compose);
        }

        public void ScrollToHorizontalOffsetWithAnimation(double offset, double milliseconds = 500)
        {
            var animation = AnimationHelper.CreateDoubleAnimation(offset, milliseconds);
            animation.EasingFunction = new CubicEase
            {
                EasingMode = EasingMode.EaseOut
            };
            animation.FillBehavior = FillBehavior.Stop;
            animation.Completed += (s, e1) =>
            {
                CurrentHorizontalOffset = offset;
                _isRunning = false;
            };
            _isRunning = true;

            BeginAnimation(CurrentHorizontalOffsetProperty, animation, HandoffBehavior.Compose);
        }

        protected override HitTestResult? HitTestCore(PointHitTestParameters hitTestParameters)
            => IsPenetrating ? null : base.HitTestCore(hitTestParameters);


        /// <summary>
        /// 是否支持惯性
        /// </summary>
        public bool IsInertiaEnabled
        {
            get => (bool)GetValue(IsInertiaEnabledProperty);
            set => SetValue(IsInertiaEnabledProperty, ValueBoxes.BooleanBox(value));
        }


        public static void SetIsInertiaEnabled(DependencyObject element, bool value) => element.SetValue(IsInertiaEnabledProperty, ValueBoxes.BooleanBox(value));
        public static bool GetIsInertiaEnabled(DependencyObject element) => (bool)element.GetValue(IsInertiaEnabledProperty);
        /// <summary>
        /// 是否支持惯性
        /// </summary>
        public static readonly DependencyProperty IsInertiaEnabledProperty =
            DependencyProperty.RegisterAttached("IsInertiaEnabled", typeof(bool), typeof(ScrollViewer), new PropertyMetadata(ValueBoxes.FalseBox));


        /// <summary>
        /// 控件是否可以穿透点击
        /// </summary>
        public bool IsPenetrating
        {
            get => (bool)GetValue(IsPenetratingProperty);
            set => SetValue(IsPenetratingProperty, ValueBoxes.BooleanBox(value));
        }


        public static void SetIsPenetrating(DependencyObject element, bool value) => element.SetValue(IsPenetratingProperty, ValueBoxes.BooleanBox(value));
        public static bool GetIsPenetrating(DependencyObject element) => (bool)element.GetValue(IsPenetratingProperty);
        /// <summary>
        /// 控件是否可以穿透点击
        /// </summary>
        public static readonly DependencyProperty IsPenetratingProperty =
            DependencyProperty.RegisterAttached("IsPenetrating", typeof(bool), typeof(ScrollViewer), new PropertyMetadata(ValueBoxes.FalseBox));


        /// <summary>
        /// 当前垂直滚动偏移
        /// </summary>
        internal double CurrentVerticalOffset
        {
            get => (double)GetValue(CurrentVerticalOffsetProperty);
            set => SetValue(CurrentVerticalOffsetProperty, value);
        }
        /// <summary>
        /// 当前垂直滚动偏移
        /// </summary>
        internal static readonly DependencyProperty CurrentVerticalOffsetProperty =
            DependencyProperty.Register(nameof(CurrentVerticalOffset), typeof(double), typeof(ScrollViewer), new PropertyMetadata(ValueBoxes.Double0Box, OnCurrentVerticalOffsetChanged));

        private static void OnCurrentVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ScrollViewer ctl && e.NewValue is double v)
            {
                ctl.ScrollToVerticalOffset(v);
            }
        }


        /// <summary>
        /// 当前水平滚动偏移
        /// </summary>
        internal double CurrentHorizontalOffset
        {
            get => (double)GetValue(CurrentHorizontalOffsetProperty);
            set => SetValue(CurrentHorizontalOffsetProperty, value);
        }
        /// <summary>
        /// 当前水平滚动偏移
        /// </summary>
        internal static readonly DependencyProperty CurrentHorizontalOffsetProperty =
            DependencyProperty.Register(nameof(CurrentHorizontalOffset), typeof(double), typeof(ScrollViewer), new PropertyMetadata(ValueBoxes.Double0Box, OnCurrentHorizontalOffsetChanged));

        private static void OnCurrentHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ScrollViewer ctl && e.NewValue is double v)
            {
                ctl.ScrollToHorizontalOffset(v);
            }
        }
    }
}
