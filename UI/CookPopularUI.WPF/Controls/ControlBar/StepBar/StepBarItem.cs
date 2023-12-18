/*
 *Description: StepBarItem
 *Author: Chance.zheng
 *Creat Time: 2023/9/8 11:09:46
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularToolkit;
using CookPopularToolkit.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace CookPopularUI.WPF.Controls
{
    /// <summary>
    /// 步骤单项
    /// </summary>
    [DefaultProperty("Index")]
    [DefaultValue("-1")]
    public class StepBarItem : SelectableItem
    {
        /// <summary>
        /// 表示步骤的类型
        /// </summary>
        public StepControlKind Kind
        {
            get => (StepControlKind)GetValue(KindProperty);
            set => SetValue(KindProperty, value);
        }
        /// <summary>
        /// 提供<see cref="Kind"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty KindProperty =
            DependencyProperty.Register(nameof(Kind), typeof(StepControlKind), typeof(StepBarItem), new PropertyMetadata(StepControlKind.Number));


        /// <summary>
        /// <see cref="Kind"/>值为<see cref="StepControlKind.Number"/>时的步骤号
        /// </summary>
        [DependsOn("Kind")]
        [Browsable(true)]
        public int Index
        {
            get => (int)GetValue(IndexProperty);
            set => SetValue(IndexProperty, value);
        }
        /// <summary>
        /// 提供<see cref="Index"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IndexProperty =
            DependencyProperty.Register(nameof(Index), typeof(int), typeof(StepBarItem), new PropertyMetadata(CookPopularToolkit.ValueBoxes.IntegerMinus1Box));


        /// <summary>
        /// <see cref="Kind"/>值为<see cref="StepControlKind.IconPattern"/>时的图标类型数据
        /// </summary>
        [DependsOn("Kind")]
        [Browsable(true)]
        public IconPatternKind IconPattern
        {
            get => (IconPatternKind)GetValue(IconPatternProperty);
            set => SetValue(IconPatternProperty, value);
        }
        /// <summary>
        /// 提供<see cref="IconPattern"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IconPatternProperty =
            DependencyProperty.Register(nameof(IconPattern), typeof(IconPatternKind), typeof(StepBarItem), new PropertyMetadata(IconPatternKind.Smile));


        /// <summary>
        /// <see cref="Kind"/>值为<see cref="StepControlKind.CustomIcon"/>时的自定义步骤标识内容
        /// </summary>
        [DependsOn("Kind")]
        [Bindable(true)]
        public Geometry CustomIcon
        {
            get => (Geometry)GetValue(CustomIconProperty);
            set => SetValue(CustomIconProperty, value);
        }
        /// <summary>
        /// 提供<see cref="CustomIcon"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty CustomIconProperty =
            DependencyProperty.Register(nameof(CustomIcon), typeof(Geometry), typeof(StepBarItem), new PropertyMetadata(Geometry.Empty));


        /// <summary>
        /// <see cref="Kind"/>值为<see cref="StepControlKind.Image"/>时的图片资源
        /// </summary>
        [DependsOn("Kind")]
        [Bindable(true)]
        public ImageSource ImageSource
        {
            get => (ImageSource)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }
        /// <summary>
        /// 提供<see cref="ImageSource"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register(nameof(ImageSource), typeof(ImageSource), typeof(StepBarItem), new PropertyMetadata(default(ImageSource)));


        /// <summary>
        /// 步骤状态
        /// </summary>
        public StepStatus Status
        {
            get => (StepStatus)GetValue(StatusProperty);
            set => SetValue(StatusProperty, value);
        }
        /// <summary>
        /// 提供<see cref="Status"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register(nameof(Status), typeof(StepStatus), typeof(StepBarItem), new PropertyMetadata(default(StepStatus)));


        /// <summary>
        /// 步骤标识大小
        /// </summary>
        public double ControlSize
        {
            get => (double)GetValue(ControlSizeProperty);
            set => SetValue(ControlSizeProperty, value);
        }
        /// <summary>
        /// 提供<see cref="ControlSize"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty ControlSizeProperty =
            DependencyProperty.Register(nameof(ControlSize), typeof(double), typeof(StepBarItem), new PropertyMetadata(25D, (s, e) =>
            {
                s.GetVisualAncestry().OfType<StepBar>().FirstOrDefault()?.InvalidateVisual();
            }));
    }
}
