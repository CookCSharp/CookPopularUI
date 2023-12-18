/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ScrollViewerExAssistant
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-26 16:22:43
 */


using CookPopularToolkit;
using CookPopularToolkit.Windows;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ValueBoxes = CookPopularToolkit.ValueBoxes;

namespace CookPopularUI.WPF.Controls
{
    public class ScrollViewerExAssistant
    {
        public static void SetAutoHide(DependencyObject element, bool value) => element.SetValue(AutoHideProperty, ValueBoxes.BooleanBox(value));
        public static bool GetAutoHide(DependencyObject element) => (bool)element.GetValue(AutoHideProperty);
        public static readonly DependencyProperty AutoHideProperty =
            DependencyProperty.RegisterAttached("AutoHide", typeof(bool), typeof(ScrollViewerExAssistant), new FrameworkPropertyMetadata(ValueBoxes.TrueBox, FrameworkPropertyMetadataOptions.Inherits));


        public static void SetOrientation(DependencyObject element, Orientation value) => element.SetValue(OrientationProperty, value);
        public static Orientation GetOrientation(DependencyObject element) => (Orientation)element.GetValue(OrientationProperty);
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.RegisterAttached("Orientation", typeof(Orientation), typeof(ScrollViewerExAssistant), new FrameworkPropertyMetadata(Orientation.Vertical, FrameworkPropertyMetadataOptions.Inherits, OnOrientationChanged));

        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ScrollViewer)
            {
                return;
            }

            if (d is ScrollViewer scrollViewer)
            {
                if ((Orientation)e.NewValue == Orientation.Horizontal)
                {
                    scrollViewer.PreviewMouseWheel += ScrollViewerPreviewMouseWheel;
                }
                else
                {
                    scrollViewer.PreviewMouseWheel -= ScrollViewerPreviewMouseWheel;
                }
            }

            void ScrollViewerPreviewMouseWheel(object sender, MouseWheelEventArgs args)
            {
                var scrollViewerNative = (System.Windows.Controls.ScrollViewer)sender;
                scrollViewerNative.ScrollToHorizontalOffset(Math.Min(Math.Max(0, scrollViewerNative.HorizontalOffset - args.Delta), scrollViewerNative.ScrollableWidth));

                args.Handled = true;
            }
        }


        public static void SetIsDisabled(DependencyObject element, bool value) => element.SetValue(IsDisabledProperty, ValueBoxes.BooleanBox(value));
        public static bool GetIsDisabled(DependencyObject element) => (bool)element.GetValue(IsDisabledProperty);
        public static readonly DependencyProperty IsDisabledProperty =
            DependencyProperty.RegisterAttached("IsDisabled", typeof(bool), typeof(ScrollViewerExAssistant), new PropertyMetadata(ValueBoxes.FalseBox, OnIsDisabledChanged));

        private static void OnIsDisabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement element)
            {
                if ((bool)e.NewValue)
                {
                    element.PreviewMouseWheel += ScrollViewerPreviewMouseWheel;
                }
                else
                {
                    element.PreviewMouseWheel -= ScrollViewerPreviewMouseWheel;
                }
            }

            void ScrollViewerPreviewMouseWheel(object sender, MouseWheelEventArgs args)
            {
                if (args.Handled)
                {
                    return;
                }

                args.Handled = true;

                if (((UIElement)sender).GetVisualAncestry().OfType<ScrollViewer>().FirstOrDefault() is { } scrollViewer)
                {
                    scrollViewer.RaiseEvent(new MouseWheelEventArgs(args.MouseDevice, args.Timestamp, args.Delta)
                    {
                        RoutedEvent = UIElement.MouseWheelEvent,
                        Source = sender
                    });
                }
            }
        }
    }
}