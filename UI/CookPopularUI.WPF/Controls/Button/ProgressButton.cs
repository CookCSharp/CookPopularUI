/*
 *Description: ProgressButton
 *Author: Chance.zheng
 *Creat Time: 2023/10/13 16:26:44
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace CookPopularUI.WPF.Controls
{
    public enum StyleType
    {
        Linear,
        Circular,
        Wave
    }


    public class ProgressButton : ButtonBase
    {
        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
        /// <summary>
        /// 提供<see cref="Value"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(double), typeof(ProgressButton), new PropertyMetadata(ValueBoxes.Double0Box));


        /// <summary>
        /// 表示Progress的类型
        /// </summary>
        public StyleType StyleType
        {
            get => (StyleType)GetValue(StyleTypeProperty);
            set => SetValue(StyleTypeProperty, value);
        }
        /// <summary>
        /// 提供<see cref="StyleType"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty StyleTypeProperty =
            DependencyProperty.Register(nameof(StyleType), typeof(StyleType), typeof(ProgressButton), new FrameworkPropertyMetadata(StyleType.Linear, FrameworkPropertyMetadataOptions.AffectsRender));


        /// <summary>
        /// 是否显示百分比
        /// </summary>
        public bool IsShowPercent
        {
            get => (bool)GetValue(IsShowPercentProperty);
            set => SetValue(IsShowPercentProperty, value.BooleanBox());
        }
        /// <summary>
        /// 提供<see cref="IsShowPercent"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsShowPercentProperty =
            DependencyProperty.Register(nameof(IsShowPercent), typeof(bool), typeof(ProgressButton), new PropertyMetadata(ValueBoxes.FalseBox));
    }
}
