/*
 *Description: RadialShadowBorder
 *Author: Chance.zheng
 *Creat Time: 2024/2/1 13:52:41
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
using System.Windows.Shapes;
using System.Windows;

namespace CookPopularUI.WPF.Controls
{
    /// <summary>
    /// 带有辐射的内容控件
    /// </summary>
    public class RadialShadowBorder : ContentControl
    {
        private GradientStop _shadowInnerStop;
        private GradientStop _shadowOuterStop;
        private TranslateTransform _shadowTranslate;
        private ScaleTransform _shadowScale;
        private Ellipse _shadow;


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The radial shadow color.")]
        public Brush RadialShadowBrush
        {
            get => (Brush)GetValue(RadialShadowBrushProperty);
            set => SetValue(RadialShadowBrushProperty, value);
        }
        public static readonly DependencyProperty RadialShadowBrushProperty =
            DependencyProperty.Register(nameof(RadialShadowBrush), typeof(Brush), typeof(RadialShadowBorder), new PropertyMetadata(new PropertyChangedCallback(OnRadialShadowBrushChanged)));
        private static void OnRadialShadowBrushChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependencyObject is RadialShadowBorder radialShadowBorder)
                radialShadowBorder.UpdateShadowBrush((Brush)eventArgs.NewValue);
        }


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The radial shadow opacity.")]
        public double RadialShadowOpacity
        {
            get => (double)GetValue(RadialShadowOpacityProperty);
            set => SetValue(RadialShadowOpacityProperty, value);
        }
        public static readonly DependencyProperty RadialShadowOpacityProperty =
            DependencyProperty.Register(nameof(RadialShadowOpacity), typeof(double), typeof(RadialShadowBorder), null);


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The radial shadow vertical offset.")]
        public double RadialShadowVerticalOffset
        {
            get => (double)GetValue(RadialShadowVerticalOffsetProperty);
            set => SetValue(RadialShadowVerticalOffsetProperty, value);
        }
        public static readonly DependencyProperty RadialShadowVerticalOffsetProperty =
            DependencyProperty.Register(nameof(RadialShadowVerticalOffset), typeof(double), typeof(RadialShadowBorder), new PropertyMetadata(new PropertyChangedCallback(OnRadialShadowVerticalOffsetChanged)));
        private static void OnRadialShadowVerticalOffsetChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependencyObject is RadialShadowBorder radialShadowBorder)
                radialShadowBorder.UpdateShadowVerticalOffset((double)eventArgs.NewValue);
        }


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The radial shadow width.")]
        public double RadialShadowWidth
        {
            get => (double)GetValue(RadialShadowWidthProperty);
            set => SetValue(RadialShadowWidthProperty, value);
        }
        public static readonly DependencyProperty RadialShadowWidthProperty =
            DependencyProperty.Register(nameof(RadialShadowWidth), typeof(double), typeof(RadialShadowBorder), new PropertyMetadata(new PropertyChangedCallback(OnRadialShadowWidthChanged)));
        private static void OnRadialShadowWidthChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependencyObject is RadialShadowBorder radialShadowBorder)
                radialShadowBorder.UpdateShadowScale((double)eventArgs.NewValue);
        }


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The radial shadow spread.")]
        public double RadialShadowSpread
        {
            get => (double)GetValue(RadialShadowSpreadProperty);
            set => SetValue(RadialShadowSpreadProperty, value);
        }
        public static readonly DependencyProperty RadialShadowSpreadProperty =
            DependencyProperty.Register(nameof(RadialShadowSpread), typeof(double), typeof(RadialShadowBorder), new PropertyMetadata(new PropertyChangedCallback(OnRadialShadowSpreadChanged)));
        private static void OnRadialShadowSpreadChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependencyObject is RadialShadowBorder radialShadowBorder)
                radialShadowBorder.UpdateStops((double)eventArgs.NewValue);
        }


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Sets the corner radius on the border.")]
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(RadialShadowBorder), null);


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Sets whether the content is clipped or not.")]
        public bool IsClipContent
        {
            get => (bool)GetValue(IsClipContentProperty);
            set => SetValue(IsClipContentProperty, value);
        }
        public static readonly DependencyProperty IsClipContentProperty =
            DependencyProperty.Register(nameof(IsClipContent), typeof(bool), typeof(RadialShadowBorder), null);


        public RadialShadowBorder()
        {
            DefaultStyleKey = typeof(RadialShadowBorder);
            SizeChanged += (s, e) => UpdateShadowSize(e.NewSize);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _shadow = (Ellipse)GetTemplateChild("PART_Shadow");
            _shadowInnerStop = (GradientStop)GetTemplateChild("PART_ShadowInnerStop");
            _shadowOuterStop = (GradientStop)GetTemplateChild("PART_ShadowOuterStop");
            _shadowTranslate = (TranslateTransform)GetTemplateChild("PART_ShadowTranslate");
            _shadowScale = (ScaleTransform)GetTemplateChild("PART_ShadowScale");

            UpdateShadowSize(new Size(ActualWidth, ActualHeight));
            UpdateStops(RadialShadowSpread);
            UpdateShadowScale(RadialShadowWidth);
            UpdateShadowVerticalOffset(RadialShadowVerticalOffset);
            UpdateShadowBrush(RadialShadowBrush);
        }

        private void UpdateShadowSize(Size newSize)
        {
            if (_shadow != null)
            {
                _shadow.Height = newSize.Height / 3;
                _shadow.Margin = new Thickness(0, 0, 0, _shadow.Height / 2);
            }
        }

        private void UpdateShadowScale(double scaleX)
        {
            if (_shadowScale != null)
            {
                _shadowScale.ScaleX = scaleX;
            }
        }

        private void UpdateStops(double spread)
        {
            if (_shadowInnerStop != null)
            {
                _shadowInnerStop.Offset = spread;
            }
        }

        private void UpdateShadowBrush(Brush brush)
        {
            if (brush == null)
                return;

            var color = ((SolidColorBrush)brush).Color;

            if (_shadowInnerStop != null)
            {
                _shadowInnerStop.Color = color;
            }

            if (_shadowOuterStop != null)
            {
                _shadowOuterStop.Color = Color.FromArgb(0, color.R, color.G, color.B);
            }
        }

        private void UpdateShadowVerticalOffset(double verticalOffset)
        {
            if (_shadowTranslate != null)
            {
                _shadowTranslate.Y = verticalOffset;
            }
        }
    }
}
