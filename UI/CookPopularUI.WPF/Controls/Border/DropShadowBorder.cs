/*
 *Description: DropShadowBorder
 *Author: Chance.zheng
 *Creat Time: 2024/1/31 14:53:15
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using CookPopularToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CookPopularUI.WPF.Controls
{
    /// <summary>
    /// 带阴影的内容控件
    /// </summary>
    public class DropShadowBorder : ContentControl
    {
        private GradientStop _shadowHorizontal1;
        private GradientStop _shadowHorizontal2;
        private GradientStop _shadowVertical1;
        private GradientStop _shadowVertical2;
        private GradientStop _shadowOuterStop1;
        private GradientStop _shadowOuterStop2;
        private TranslateTransform _dropShadowTranslate;


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("the drop shadow color.")]
        public Brush DropShadowBrush
        {
            get => (Brush)GetValue(DropShadowBrushProperty);
            set => SetValue(DropShadowBrushProperty, value);
        }
        public static readonly DependencyProperty DropShadowBrushProperty =
            DependencyProperty.Register(nameof(DropShadowBrush), typeof(Brush), typeof(DropShadowBorder), new PropertyMetadata(new PropertyChangedCallback(OnDropShadowBrushChanged)));
        private static void OnDropShadowBrushChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependencyObject is DropShadowBorder dropShadowBorder)
                dropShadowBorder.UpdateDropShadowBrush((Brush)eventArgs.NewValue);
        }


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The drop shadow opacity.")]
        public double DropShadowOpacity
        {
            get => (double)GetValue(DropShadowOpacityProperty);
            set => SetValue(DropShadowOpacityProperty, value);
        }
        public static readonly DependencyProperty DropShadowOpacityProperty =
            DependencyProperty.Register(nameof(DropShadowOpacity), typeof(double), typeof(DropShadowBorder), null);


        /// <summary>
        /// 阴影距离
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The drop shadow distance.")]
        public double DropShadowDistance
        {
            get => (double)GetValue(DropShadowDistanceProperty);
            set => SetValue(DropShadowDistanceProperty, value);
        }
        public static readonly DependencyProperty DropShadowDistanceProperty =
            DependencyProperty.Register(nameof(DropShadowDistance), typeof(double), typeof(DropShadowBorder), new PropertyMetadata(new PropertyChangedCallback(OnDropShadowDistanceChanged)));
        private static void OnDropShadowDistanceChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependencyObject is DropShadowBorder dropShadowBorder)
                dropShadowBorder.UpdateDropShadowPosition();
        }


        /// <summary>
        /// 阴影角度
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The drop shadow angle.")]
        public double DropShadowAngle
        {
            get => (double)GetValue(DropShadowAngleProperty);
            set => SetValue(DropShadowAngleProperty, value);
        }
        public static readonly DependencyProperty DropShadowAngleProperty =
            DependencyProperty.Register(nameof(DropShadowAngle), typeof(double), typeof(DropShadowBorder), new PropertyMetadata(new PropertyChangedCallback(OnDropShadowAngleChanged)));
        private static void OnDropShadowAngleChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependencyObject is DropShadowBorder dropShadowBorder)
                dropShadowBorder.UpdateDropShadowPosition();
        }


        /// <summary>
        /// 阴影扩展
        /// </summary>
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The drop shadow spread.")]
        public double DropShadowSpread
        {
            get => (double)GetValue(DropShadowSpreadProperty);
            set => SetValue(DropShadowSpreadProperty, value);
        }
        public static readonly DependencyProperty DropShadowSpreadProperty =
            DependencyProperty.Register(nameof(DropShadowSpread), typeof(double), typeof(DropShadowBorder), new PropertyMetadata(new PropertyChangedCallback(OnDropShadowSpreadChanged)));
        private static void OnDropShadowSpreadChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependencyObject is DropShadowBorder dropShadowBorder)
                dropShadowBorder.UpdateStops(new Size(dropShadowBorder.ActualWidth, dropShadowBorder.ActualHeight));
        }


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Sets the corner radius on the border.")]
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(DropShadowBorder), new PropertyMetadata(OnCornerRadiusChanged));
        private static void OnCornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DropShadowBorder dropShadowBorder)
            {
                var cornerRadius = (CornerRadius)e.NewValue;
                CornerRadius shadowCornerRadius = new CornerRadius(
                    Math.Abs(cornerRadius.TopLeft * 1.5),
                    Math.Abs(cornerRadius.TopRight * 1.5),
                    Math.Abs(cornerRadius.BottomRight * 1.5),
                    Math.Abs(cornerRadius.BottomLeft * 1.5));
                dropShadowBorder.SetValue(ShadowCornerRadiusProperty, shadowCornerRadius);
            }
        }


        public double ShadowCornerRadius => (double)GetValue(ShadowCornerRadiusProperty);
        public static readonly DependencyProperty ShadowCornerRadiusProperty =
            DependencyProperty.Register(nameof(ShadowCornerRadius), typeof(CornerRadius), typeof(DropShadowBorder), null);


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Sets whether the content is clipped or not.")]
        public bool IsClipContent
        {
            get => (bool)GetValue(IsClipContentProperty);
            set => SetValue(IsClipContentProperty, value.BooleanBox());
        }
        public static readonly DependencyProperty IsClipContentProperty =
            DependencyProperty.Register(nameof(IsClipContent), typeof(bool), typeof(DropShadowBorder), new PropertyMetadata(ValueBoxes.FalseBox));


        public DropShadowBorder()
        {
            DefaultStyleKey = typeof(DropShadowBorder);
            SizeChanged += (s, e) => UpdateStops(e.NewSize);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _dropShadowTranslate = (TranslateTransform)GetTemplateChild("PART_DropShadowTranslateTransform");
            _shadowOuterStop1 = (GradientStop)GetTemplateChild("PART_ShadowOuterStop1");
            _shadowOuterStop2 = (GradientStop)GetTemplateChild("PART_ShadowOuterStop2");
            _shadowVertical1 = (GradientStop)GetTemplateChild("PART_ShadowVertical1");
            _shadowVertical2 = (GradientStop)GetTemplateChild("PART_ShadowVertical2");
            _shadowHorizontal1 = (GradientStop)GetTemplateChild("PART_ShadowHorizontal1");
            _shadowHorizontal2 = (GradientStop)GetTemplateChild("PART_ShadowHorizontal2");

            UpdateDropShadowPosition();
            UpdateStops(new Size(ActualWidth, ActualHeight));
        }

        private void UpdateDropShadowPosition()
        {
            if (_dropShadowTranslate != null)
            {
                double x = Math.Cos(DropShadowAngle * (Math.PI / 180)) * DropShadowDistance;
                double y = Math.Tan(DropShadowAngle * (Math.PI / 180)) * x;
                Point offset = new Point(x, y);

                _dropShadowTranslate.X = offset.X;
                _dropShadowTranslate.Y = offset.Y;
            }
        }

        private void UpdateStops(Size size)
        {
            if (size.Width > 0 && size.Height > 0)
            {
                if (_shadowHorizontal1 != null)
                {
                    _shadowHorizontal1.Offset = DropShadowSpread / (size.Width + (DropShadowSpread * 2));
                }

                if (_shadowHorizontal2 != null)
                {
                    _shadowHorizontal2.Offset = 1 - (DropShadowSpread / (size.Width + (DropShadowSpread * 2)));
                }

                if (_shadowVertical1 != null)
                {
                    _shadowVertical1.Offset = DropShadowSpread / (size.Height + (DropShadowSpread * 2));
                }

                if (_shadowVertical2 != null)
                {
                    _shadowVertical2.Offset = 1 - (DropShadowSpread / (size.Height + (DropShadowSpread * 2)));
                }
            }
        }

        private void UpdateDropShadowBrush(Brush brush)
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
    }
}
