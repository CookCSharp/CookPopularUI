/*
 *Description: InnerGlowBorder
 *Author: Chance.zheng
 *Creat Time: 2024/1/31 17:43:25
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using CookPopularToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CookPopularUI.WPF.Controls
{
    /// <summary>
    /// 内部发光的内容控件
    /// </summary>
    public class InnerGlowBorder : ContentControl
    {
        private Border? _leftGlow;
        private Border? _topGlow;
        private Border? _rightGlow;
        private Border? _bottomGlow;
        private GradientStop? _leftGlowStop0;
        private GradientStop? _leftGlowStop1;
        private GradientStop? _topGlowStop0;
        private GradientStop? _topGlowStop1;
        private GradientStop? _rightGlowStop0;
        private GradientStop? _rightGlowStop1;
        private GradientStop? _bottomGlowStop0;
        private GradientStop? _bottomGlowStop1;


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The inner glow opacity.")]
        public double InnerGlowOpacity
        {
            get { return (double)GetValue(InnerGlowOpacityProperty); }
            set { SetValue(InnerGlowOpacityProperty, value); }
        }
        public static readonly DependencyProperty InnerGlowOpacityProperty =
            DependencyProperty.Register(nameof(InnerGlowOpacity), typeof(double), typeof(InnerGlowBorder), null);


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The inner glow size.")]
        public Thickness InnerGlowSize
        {
            get => (Thickness)GetValue(InnerGlowSizeProperty);
            set => SetValue(InnerGlowSizeProperty, value);
        }
        public static readonly DependencyProperty InnerGlowSizeProperty =
            DependencyProperty.Register(nameof(InnerGlowSize), typeof(Thickness), typeof(InnerGlowBorder), new PropertyMetadata(new PropertyChangedCallback(OnInnerGlowSizeChanged)));
        private static void OnInnerGlowSizeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependencyObject is InnerGlowBorder innerGlowBorder)
                innerGlowBorder.UpdateGlowSize((Thickness)eventArgs.NewValue);
        }


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Sets the corner radius on the border.")]
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(InnerGlowBorder), null);


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The inner glow color.")]
        public Brush InnerGlowBrush
        {
            get => (Brush)GetValue(InnerGlowBrushProperty);
            set => SetValue(InnerGlowBrushProperty, value);
        }
        public static readonly DependencyProperty InnerGlowBrushProperty =
            DependencyProperty.Register(nameof(InnerGlowBrush), typeof(Brush), typeof(InnerGlowBorder), new PropertyMetadata(new PropertyChangedCallback(OnInnerGlowBrushChanged)));
        private static void OnInnerGlowBrushChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependencyObject is InnerGlowBorder innerGlowBorder)
                innerGlowBorder.UpdateGlowBrush((Brush)eventArgs.NewValue);
        }


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Sets whether the content is clipped or not.")]
        public bool IsClipContent
        {
            get { return (bool)GetValue(IsClipContentProperty); }
            set { SetValue(IsClipContentProperty, value.BooleanBox()); }
        }
        public static readonly DependencyProperty IsClipContentProperty =
            DependencyProperty.Register(nameof(IsClipContent), typeof(bool), typeof(InnerGlowBorder), new PropertyMetadata(ValueBoxes.FalseBox));


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Set 0 for behind the shadow, 1 for in front.")]
        public int ContentZIndex
        {
            get { return (int)GetValue(ContentZIndexProperty); }
            set { SetValue(ContentZIndexProperty, value); }
        }
        public static readonly DependencyProperty ContentZIndexProperty =
            DependencyProperty.Register(nameof(ContentZIndex), typeof(int), typeof(InnerGlowBorder), new PropertyMetadata(ValueBoxes.Integer0Box));


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _leftGlow = GetTemplateChild("PART_LeftGlow") as Border;
            _topGlow = GetTemplateChild("PART_TopGlow") as Border;
            _rightGlow = GetTemplateChild("PART_RightGlow") as Border;
            _bottomGlow = GetTemplateChild("PART_BottomGlow") as Border;

            _leftGlowStop0 = GetTemplateChild("PART_LeftGlowStop0") as GradientStop;
            _leftGlowStop1 = GetTemplateChild("PART_LeftGlowStop1") as GradientStop;
            _topGlowStop0 = GetTemplateChild("PART_TopGlowStop0") as GradientStop;
            _topGlowStop1 = GetTemplateChild("PART_TopGlowStop1") as GradientStop;
            _rightGlowStop0 = GetTemplateChild("PART_RightGlowStop0") as GradientStop;
            _rightGlowStop1 = GetTemplateChild("PART_RightGlowStop1") as GradientStop;
            _bottomGlowStop0 = GetTemplateChild("PART_BottomGlowStop0") as GradientStop;
            _bottomGlowStop1 = GetTemplateChild("PART_BottomGlowStop1") as GradientStop;

            UpdateGlowBrush(InnerGlowBrush);
            UpdateGlowSize(InnerGlowSize);
        }

        private void UpdateGlowBrush(Brush brush)
        {
            if (brush == null)
                return;

            var color = ((SolidColorBrush)brush).Color;

            if (_leftGlowStop0 != null)
            {
                _leftGlowStop0.Color = color;
            }

            if (_leftGlowStop1 != null)
            {
                _leftGlowStop1.Color = Color.FromArgb(0, color.R, color.G, color.B);
            }

            if (_topGlowStop0 != null)
            {
                _topGlowStop0.Color = color;
            }

            if (_topGlowStop1 != null)
            {
                _topGlowStop1.Color = Color.FromArgb(0, color.R, color.G, color.B);
            }

            if (_rightGlowStop0 != null)
            {
                _rightGlowStop0.Color = color;
            }

            if (_rightGlowStop1 != null)
            {
                _rightGlowStop1.Color = Color.FromArgb(0, color.R, color.G, color.B);
            }

            if (_bottomGlowStop0 != null)
            {
                _bottomGlowStop0.Color = color;
            }

            if (_bottomGlowStop1 != null)
            {
                _bottomGlowStop1.Color = Color.FromArgb(0, color.R, color.G, color.B);
            }
        }

        private void UpdateGlowSize(Thickness newGlowSize)
        {
            if (_leftGlow != null)
            {
                _leftGlow.Width = Math.Abs(newGlowSize.Left);
            }

            if (_topGlow != null)
            {
                _topGlow.Height = Math.Abs(newGlowSize.Top);
            }

            if (_rightGlow != null)
            {
                _rightGlow.Width = Math.Abs(newGlowSize.Right);
            }

            if (_bottomGlow != null)
            {
                _bottomGlow.Height = Math.Abs(newGlowSize.Bottom);
            }
        }
    }
}
