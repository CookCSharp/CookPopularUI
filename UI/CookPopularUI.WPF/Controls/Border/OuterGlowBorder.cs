/*
 *Description: OuterGlowBorder
 *Author: Chance.zheng
 *Creat Time: 2024/2/1 9:25:45
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using CookPopularToolkit;

namespace CookPopularUI.WPF.Controls
{
    public class OuterGlowBorder : ContentControl
    {
        private GradientStop _shadowOuterStop1;
        private GradientStop _shadowOuterStop2;
        private GradientStop _shadowVertical1;
        private GradientStop _shadowVertical2;
        private GradientStop _shadowHorizontal1;
        private GradientStop _shadowHorizontal2;
        private Border _outerGlowBorder;


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The outer glow opacity.")]
        public double OuterGlowOpacity
        {
            get => (double)GetValue(OuterGlowOpacityProperty);
            set => SetValue(OuterGlowOpacityProperty, value);
        }
        public static readonly DependencyProperty OuterGlowOpacityProperty =
            DependencyProperty.Register(nameof(OuterGlowOpacity), typeof(double), typeof(OuterGlowBorder), new PropertyMetadata(ValueBoxes.Double0Box));


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The outer glow size.")]
        public double OuterGlowSize
        {
            get => (double)GetValue(OuterGlowSizeProperty);
            set => SetValue(OuterGlowSizeProperty, value);
        }
        public static readonly DependencyProperty OuterGlowSizeProperty =
            DependencyProperty.Register(nameof(OuterGlowSize), typeof(double), typeof(OuterGlowBorder), new PropertyMetadata(new PropertyChangedCallback(OnOuterGlowSizeChanged)));
        private static void OnOuterGlowSizeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is OuterGlowBorder outerGlowBorder)
            {
                outerGlowBorder.UpdateGlowSize((double)e.NewValue);
                outerGlowBorder.UpdateStops(new Size(outerGlowBorder.ActualWidth, outerGlowBorder.ActualHeight));
            }
        }


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Sets the corner radius on the border.")]
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(OuterGlowBorder), new PropertyMetadata(OnCornerRadiusChanged));
        private static void OnCornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is OuterGlowBorder outerGlowBorder)
            {
                CornerRadius shadowCornerRadius = new CornerRadius(
                    Math.Abs(outerGlowBorder.CornerRadius.TopLeft * 1.5),
                    Math.Abs(outerGlowBorder.CornerRadius.TopRight * 1.5),
                    Math.Abs(outerGlowBorder.CornerRadius.BottomRight * 1.5),
                    Math.Abs(outerGlowBorder.CornerRadius.BottomLeft * 1.5));
                outerGlowBorder.SetValue(ShadowCornerRadiusProperty, shadowCornerRadius);
            }
        }


        public CornerRadius ShadowCornerRadius => (CornerRadius)GetValue(ShadowCornerRadiusProperty);
        public static readonly DependencyProperty ShadowCornerRadiusProperty =
            DependencyProperty.Register(nameof(ShadowCornerRadius), typeof(CornerRadius), typeof(OuterGlowBorder), null);


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The outer glow color.")]
        public Brush OuterGlowBrush
        {
            get => (Brush)GetValue(OuterGlowBrushProperty);
            set => SetValue(OuterGlowBrushProperty, value);
        }
        public static readonly DependencyProperty OuterGlowBrushProperty =
            DependencyProperty.Register(nameof(OuterGlowBrush), typeof(Brush), typeof(OuterGlowBorder), new PropertyMetadata(new PropertyChangedCallback(OnOuterGlowBrushChanged)));
        private static void OnOuterGlowBrushChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependencyObject is OuterGlowBorder outerGlowBorder)
                outerGlowBorder.UpdateGlowBrush((Brush)eventArgs.NewValue);
        }


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Sets whether the content is clipped or not.")]
        public bool IsClipContent
        {
            get => (bool)GetValue(IsClipContentProperty);
            set => SetValue(IsClipContentProperty, value.BooleanBox());
        }
        public static readonly DependencyProperty IsClipContentProperty =
            DependencyProperty.Register(nameof(IsClipContent), typeof(bool), typeof(OuterGlowBorder), new PropertyMetadata(ValueBoxes.FalseBox));


        public OuterGlowBorder()
        {
            SizeChanged += (s, e) => UpdateStops(e.NewSize);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _shadowOuterStop1 = (GradientStop)GetTemplateChild("PART_ShadowOuterStop1");
            _shadowOuterStop2 = (GradientStop)GetTemplateChild("PART_ShadowOuterStop2");
            _shadowVertical1 = (GradientStop)GetTemplateChild("PART_ShadowVertical1");
            _shadowVertical2 = (GradientStop)GetTemplateChild("PART_ShadowVertical2");
            _shadowHorizontal1 = (GradientStop)GetTemplateChild("PART_ShadowHorizontal1");
            _shadowHorizontal2 = (GradientStop)GetTemplateChild("PART_ShadowHorizontal2");
            _outerGlowBorder = (Border)GetTemplateChild("PART_OuterGlowBorder");

            UpdateGlowSize(OuterGlowSize);
            UpdateGlowBrush(OuterGlowBrush);
        }

        private void UpdateGlowSize(double size)
        {
            if (_outerGlowBorder != null)
            {
                _outerGlowBorder.Margin = new Thickness(-Math.Abs(size));
            }
        }

        private void UpdateGlowBrush(Brush brush)
        {
            if (brush == null)
                return;

            var color = ((SolidColorBrush)brush).Color;

            if (_shadowVertical1 != null)
            {
                _shadowVertical1.Color = color;
            }

            if (_shadowVertical2 != null)
            {
                _shadowVertical2.Color = color;
            }

            if (_shadowOuterStop1 != null)
            {
                _shadowOuterStop1.Color = Color.FromArgb(0, color.R, color.G, color.B);
            }

            if (_shadowOuterStop2 != null)
            {
                _shadowOuterStop2.Color = Color.FromArgb(0, color.R, color.G, color.B);
            }
        }

        private void UpdateStops(Size size)
        {
            if (size.Width > 0 && size.Height > 0)
            {
                if (_shadowHorizontal1 != null)
                {
                    _shadowHorizontal1.Offset = Math.Abs(OuterGlowSize) / (size.Width + Math.Abs(OuterGlowSize) + Math.Abs(OuterGlowSize));
                }

                if (_shadowHorizontal2 != null)
                {
                    _shadowHorizontal2.Offset = 1 - (Math.Abs(OuterGlowSize) / (size.Width + Math.Abs(OuterGlowSize) + Math.Abs(OuterGlowSize)));
                }

                if (_shadowVertical1 != null)
                {
                    _shadowVertical1.Offset = Math.Abs(OuterGlowSize) / (size.Height + Math.Abs(OuterGlowSize) + Math.Abs(OuterGlowSize));
                }

                if (_shadowVertical2 != null)
                {
                    _shadowVertical2.Offset = 1 - (Math.Abs(OuterGlowSize) / (size.Height + Math.Abs(OuterGlowSize) + Math.Abs(OuterGlowSize)));
                }
            }
        }
    }
}
