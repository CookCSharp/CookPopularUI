/*
 *Description: GlassBorder
 *Author: Chance.zheng
 *Creat Time: 2024/1/31 16:38:27
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
            DependencyProperty.Register(nameof(GlassOpacity), typeof(double), typeof(GlassBorder), null);


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Sets the corner radius on the border.")]
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set
            {
                SetValue(CornerRadiusProperty, value);

                CornerRadius glassCornerRadius = new CornerRadius(
                        Math.Max(0, value.TopLeft - 1),
                        Math.Max(0, value.TopRight - 1),
                        0,
                        0);
                SetValue(GlassCornerRadiusProperty, glassCornerRadius);
            }
        }
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(GlassBorder), null);


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
            DependencyProperty.Register(nameof(IsClipContent), typeof(bool), typeof(GlassBorder), null);


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Set 0 for behind the shadow, 1 for in front.")]
        public int ContentZIndex
        {
            get => (int)GetValue(ContentZIndexProperty);
            set => SetValue(ContentZIndexProperty, value);
        }
        public static readonly DependencyProperty ContentZIndexProperty =
            DependencyProperty.Register(nameof(ContentZIndex), typeof(int), typeof(GlassBorder), null);


        public GlassBorder()
        {
            DefaultStyleKey = typeof(GlassBorder);
        }
    }
}
