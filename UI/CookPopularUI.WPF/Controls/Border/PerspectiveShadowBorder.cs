/*
 *Description: PerspectiveShadowBorder
 *Author: Chance.zheng
 *Creat Time: 2024/2/1 11:36:35
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

namespace CookPopularUI.WPF.Controls
{
    /// <summary>
    /// 带有透视阴影的内容控件
    /// </summary>
    public class PerspectiveShadowBorder : ContentControl
    {
        private GradientStop? _shadowOuterStop1;
        private GradientStop? _shadowOuterStop2;
        private GradientStop? _shadowVertical1;
        private GradientStop? _shadowVertical2;
        private GradientStop? _shadowHorizontal1;
        private GradientStop? _shadowHorizontal2;
        private SkewTransform _skewTransform;
        private ScaleTransform _scaleTransform;


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("the Perspective shadow color.")]
        public Brush PerspectiveShadowBrush
        {
            get => (Brush)GetValue(PerspectiveShadowBrushProperty);
            set => SetValue(PerspectiveShadowBrushProperty, value);
        }
        public static readonly DependencyProperty PerspectiveShadowBrushProperty =
            DependencyProperty.Register(nameof(PerspectiveShadowBrush), typeof(Brush), typeof(PerspectiveShadowBorder), new PropertyMetadata(new PropertyChangedCallback(OnPerspectiveShadowBrushChanged)));
        private static void OnPerspectiveShadowBrushChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependencyObject is PerspectiveShadowBorder perspectiveShadowBorder)
                perspectiveShadowBorder.UpdatePerspectiveShadowBrush((Brush)eventArgs.NewValue);
        }


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The Perspective shadow opacity.")]
        public double PerspectiveShadowOpacity
        {
            get => (double)GetValue(PerspectiveShadowOpacityProperty);
            set => SetValue(PerspectiveShadowOpacityProperty, value);
        }
        public static readonly DependencyProperty PerspectiveShadowOpacityProperty =
            DependencyProperty.Register(nameof(PerspectiveShadowOpacity), typeof(double), typeof(PerspectiveShadowBorder), null);


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The Perspective shadow angle.")]
        public double PerspectiveShadowAngle
        {
            get => (double)GetValue(PerspectiveShadowAngleProperty);
            set => SetValue(PerspectiveShadowAngleProperty, Math.Max(Math.Min(value, 89), -89));
        }
        public static readonly DependencyProperty PerspectiveShadowAngleProperty =
            DependencyProperty.Register(nameof(PerspectiveShadowAngle), typeof(double), typeof(PerspectiveShadowBorder), new PropertyMetadata(new PropertyChangedCallback(OnPerspectiveShadowAngleChanged)));
        private static void OnPerspectiveShadowAngleChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependencyObject is PerspectiveShadowBorder perspectiveShadowBorder)
                perspectiveShadowBorder.UpdateShadowAngle((double)eventArgs.NewValue);
        }


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The Perspective shadow spread.")]
        public double PerspectiveShadowSpread
        {
            get => (double)GetValue(PerspectiveShadowSpreadProperty);
            set => SetValue(PerspectiveShadowSpreadProperty, value);
        }
        public static readonly DependencyProperty PerspectiveShadowSpreadProperty =
            DependencyProperty.Register(nameof(PerspectiveShadowSpread), typeof(double), typeof(PerspectiveShadowBorder), new PropertyMetadata(new PropertyChangedCallback(OnPerspectiveShadowSpreadChanged)));
        private static void OnPerspectiveShadowSpreadChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependencyObject is PerspectiveShadowBorder perspectiveShadowBorder)
                perspectiveShadowBorder.UpdateStops(new Size(perspectiveShadowBorder.ActualWidth, perspectiveShadowBorder.ActualHeight));
        }


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Sets the corner radius on the border.")]
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(PerspectiveShadowBorder), new PropertyMetadata(OnCornerRadiusChanged));
        private static void OnCornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PerspectiveShadowBorder perspectiveShadowBorder)
            {
                var cornerRadius = (CornerRadius)e.NewValue;
                CornerRadius shadowCornerRadius = new CornerRadius(
                    Math.Abs(cornerRadius.TopLeft * 1.5),
                    Math.Abs(cornerRadius.TopRight * 1.5),
                    Math.Abs(cornerRadius.BottomRight * 1.5),
                    Math.Abs(cornerRadius.BottomLeft * 1.5));
                perspectiveShadowBorder.SetValue(ShadowCornerRadiusProperty, shadowCornerRadius);
            }
        }


        public double ShadowCornerRadius => (double)GetValue(ShadowCornerRadiusProperty);
        public static readonly DependencyProperty ShadowCornerRadiusProperty =
            DependencyProperty.Register(nameof(ShadowCornerRadius), typeof(CornerRadius), typeof(PerspectiveShadowBorder), null);


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Sets whether the content is clipped or not.")]
        public bool IsClipContent
        {
            get => (bool)GetValue(IsClipContentProperty);
            set => SetValue(IsClipContentProperty, value);
        }
        public static readonly DependencyProperty IsClipContentProperty =
            DependencyProperty.Register(nameof(IsClipContent), typeof(bool), typeof(PerspectiveShadowBorder), null);


        public PerspectiveShadowBorder()
        {
            DefaultStyleKey = typeof(PerspectiveShadowBorder);
            SizeChanged += (s, e) => UpdateStops(e.NewSize);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _skewTransform = (SkewTransform)GetTemplateChild("PART_PerspectiveShadowSkewTransform");
            _scaleTransform = (ScaleTransform)GetTemplateChild("PART_PerspectiveShadowScaleTransform");
            _shadowOuterStop1 = (GradientStop)GetTemplateChild("PART_ShadowOuterStop1");
            _shadowOuterStop2 = (GradientStop)GetTemplateChild("PART_ShadowOuterStop2");
            _shadowVertical1 = (GradientStop)GetTemplateChild("PART_ShadowVertical1");
            _shadowVertical2 = (GradientStop)GetTemplateChild("PART_ShadowVertical2");
            _shadowHorizontal1 = (GradientStop)GetTemplateChild("PART_ShadowHorizontal1");
            _shadowHorizontal2 = (GradientStop)GetTemplateChild("PART_ShadowHorizontal2");

            UpdateStops(new Size(ActualWidth, ActualHeight));
            UpdateShadowAngle(PerspectiveShadowAngle);
        }

        private void UpdateStops(Size size)
        {
            if (size.Width > 0 && size.Height > 0)
            {
                if (_shadowHorizontal1 != null)
                {
                    _shadowHorizontal1.Offset = PerspectiveShadowSpread / (size.Width + (PerspectiveShadowSpread * 2));
                }

                if (_shadowHorizontal2 != null)
                {
                    _shadowHorizontal2.Offset = 1 - (PerspectiveShadowSpread / (size.Width + (PerspectiveShadowSpread * 2)));
                }

                if (_shadowVertical1 != null)
                {
                    _shadowVertical1.Offset = PerspectiveShadowSpread / (size.Height + (PerspectiveShadowSpread * 2));
                }

                if (_shadowVertical2 != null)
                {
                    _shadowVertical2.Offset = 1 - (PerspectiveShadowSpread / (size.Height + (PerspectiveShadowSpread * 2)));
                }
            }
        }

        private void UpdatePerspectiveShadowBrush(Brush brush)
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

        private void UpdateShadowAngle(double newAngle)
        {
            if (_skewTransform != null)
            {
                _skewTransform.AngleX = Math.Min(Math.Max(newAngle, -45), 45);
            }

            if (_scaleTransform != null)
            {
                _scaleTransform.ScaleY = 1 - (Math.Abs(PerspectiveShadowAngle) / 89.0);
            }
        }
    }
}
