/*
 *Description: DropShadowTextBlock
 *Author: Chance.zheng
 *Creat Time: 2024/2/1 15:24:00
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
using System.Windows.Media;

namespace CookPopularUI.WPF.Controls
{
    /// <summary>
    /// 带有阴影的文本控件
    /// </summary>
    public class DropShadowTextBlock : Control
    {
        private TranslateTransform? _dropShadowTranslate;
        private SolidColorBrush? _dropShadowBrush;


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The drop shadow color.")]
        public Brush DropShadowBrush
        {
            get => (Brush)GetValue(DropShadowBrushProperty);
            set => SetValue(DropShadowBrushProperty, value);
        }
        public static readonly DependencyProperty DropShadowBrushProperty =
            DependencyProperty.Register(nameof(DropShadowBrush), typeof(Brush), typeof(DropShadowTextBlock), new PropertyMetadata(new PropertyChangedCallback(OnDropShadowBrushChanged)));
        private static void OnDropShadowBrushChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependencyObject is DropShadowTextBlock dropShadowTextBlock)
                dropShadowTextBlock.UpdateDropShadowBrush();
        }


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The drop shadow opacity.")]
        public double DropShadowOpacity
        {
            get => (double)GetValue(DropShadowOpacityProperty);
            set => SetValue(DropShadowOpacityProperty, value);
        }
        public static readonly DependencyProperty DropShadowOpacityProperty =
            DependencyProperty.Register(nameof(DropShadowOpacity), typeof(double), typeof(DropShadowTextBlock), new PropertyMetadata(new PropertyChangedCallback(OnDropShadowOpacityChanged)));
        private static void OnDropShadowOpacityChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependencyObject is DropShadowTextBlock dropShadowTextBlock)
                dropShadowTextBlock.UpdateDropShadowBrush();
        }


        [System.ComponentModel.Category("Common Properties"), System.ComponentModel.Description("The text content.")]
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(DropShadowTextBlock), null);


        [System.ComponentModel.Category("Common Properties"), System.ComponentModel.Description("The text decorations.")]
        public TextDecorationCollection TextDecorations
        {
            get => (TextDecorationCollection)GetValue(TextDecorationsProperty);
            set => SetValue(TextDecorationsProperty, value);
        }
        public static readonly DependencyProperty TextDecorationsProperty =
            DependencyProperty.Register(nameof(TextDecorations), typeof(TextDecorationCollection), typeof(DropShadowTextBlock), null);


        [System.ComponentModel.Category("Common Properties"), System.ComponentModel.Description("Whether the text wraps.")]
        public TextWrapping TextWrapping
        {
            get => (TextWrapping)GetValue(TextWrappingProperty);
            set => SetValue(TextWrappingProperty, value);
        }
        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register(nameof(TextWrapping), typeof(TextWrapping), typeof(DropShadowTextBlock), null);


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The drop shadow distance.")]
        public double DropShadowDistance
        {
            get => (double)GetValue(DropShadowDistanceProperty);
            set => SetValue(DropShadowDistanceProperty, value);
        }
        public static readonly DependencyProperty DropShadowDistanceProperty =
            DependencyProperty.Register(nameof(DropShadowDistance), typeof(double), typeof(DropShadowTextBlock), new PropertyMetadata(new PropertyChangedCallback(OnDropShadowDistanceChanged)));
        private static void OnDropShadowDistanceChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependencyObject is DropShadowTextBlock dropShadowTextBlock)
                dropShadowTextBlock.UpdateDropShadowPosition();
        }


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The drop shadow angle.")]
        public double DropShadowAngle
        {
            get => (double)GetValue(DropShadowAngleProperty);
            set => SetValue(DropShadowAngleProperty, value);
        }
        public static readonly DependencyProperty DropShadowAngleProperty =
            DependencyProperty.Register(nameof(DropShadowAngle), typeof(double), typeof(DropShadowTextBlock), new PropertyMetadata(new PropertyChangedCallback(OnDropShadowAngleChanged)));
        private static void OnDropShadowAngleChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependencyObject is DropShadowTextBlock dropShadowTextBlock)
                dropShadowTextBlock.UpdateDropShadowPosition();
        }


        public DropShadowTextBlock()
        {
            DefaultStyleKey = typeof(DropShadowTextBlock);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _dropShadowTranslate = GetTemplateChild("PART_DropShadowTranslate") as TranslateTransform;
            _dropShadowBrush = GetTemplateChild("PART_DropShadowBrush") as SolidColorBrush;

            UpdateDropShadowPosition();
            UpdateDropShadowBrush();
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

        private void UpdateDropShadowBrush()
        {
            if (_dropShadowBrush != null && DropShadowBrush != null)
            {
                _dropShadowBrush.Color = ((SolidColorBrush)DropShadowBrush).Color;
                _dropShadowBrush.Opacity = DropShadowOpacity;
            }
        }
    }
}
