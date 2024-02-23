/*
 *Description: GlassBorder
 *Author: Chance.zheng
 *Creat Time: 2024/1/31 16:38:27
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

namespace CookPopularUI.WPF.Controls
{
    /// <summary>
    /// 带玻璃效果的内容控件
    /// </summary>
    public class GlassBorder : ContentControl
    {
        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The inner glow opacity.")]
        public double GlassOpacity
        {
            get => (double)GetValue(GlassOpacityProperty);
            set => SetValue(GlassOpacityProperty, value);
        }
        public static readonly DependencyProperty GlassOpacityProperty =
            DependencyProperty.Register(nameof(GlassOpacity), typeof(double), typeof(GlassBorder), new PropertyMetadata(ValueBoxes.Double1Box));


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Sets the corner radius on the border.")]
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(GlassBorder), new PropertyMetadata(OnCornerRadiusChanged));
        private static void OnCornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GlassBorder glassBorder)
            {
                var cornerRadius = (CornerRadius)e.NewValue;
                CornerRadius glassCornerRadius = new CornerRadius(Math.Max(0, cornerRadius.TopLeft - 1), Math.Max(0, cornerRadius.TopRight - 1), 0, 0);
                glassBorder.SetValue(GlassCornerRadiusProperty, glassCornerRadius);
            }
        }


        public double GlassCornerRadius => (double)GetValue(GlassCornerRadiusProperty);
        public static readonly DependencyProperty GlassCornerRadiusProperty =
            DependencyProperty.Register(nameof(GlassCornerRadius), typeof(CornerRadius), typeof(GlassBorder), null);


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Sets whether the content is clipped or not.")]
        public bool IsClipContent
        {
            get => (bool)GetValue(IsClipContentProperty);
            set => SetValue(IsClipContentProperty, value);
        }
        public static readonly DependencyProperty IsClipContentProperty =
            DependencyProperty.Register(nameof(IsClipContent), typeof(bool), typeof(GlassBorder), new PropertyMetadata(ValueBoxes.FalseBox));


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Set 0 for behind the shadow, 1 for in front.")]
        public int ContentZIndex
        {
            get => (int)GetValue(ContentZIndexProperty);
            set => SetValue(ContentZIndexProperty, value);
        }
        public static readonly DependencyProperty ContentZIndexProperty =
            DependencyProperty.Register(nameof(ContentZIndex), typeof(int), typeof(GlassBorder), new PropertyMetadata(ValueBoxes.Integer0Box));
    }
}
