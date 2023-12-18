/*
 *Description: TitleBarEx
 *Author: Chance.zheng
 *Creat Time: 2023/11/7 13:57:05
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
using System.Windows.Input;
using System.Windows.Media;

namespace CookPopularUI.WPF.Controls
{
    public class TitleBarEx : ContentControl
    {
        /// <summary>
        /// 标题栏左边图标
        /// </summary>
        public Geometry LeftLogo
        {
            get => (Geometry)GetValue(LeftLogoProperty);
            set => SetValue(LeftLogoProperty, value);
        }
        /// <summary>
        /// 提供<see cref="LeftLogo"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty LeftLogoProperty =
            DependencyProperty.Register(nameof(LeftLogo), typeof(Geometry), typeof(TitleBarEx), new PropertyMetadata(Geometry.Empty));


        /// <summary>
        /// 是否显示标题栏左边图标
        /// </summary>
        public bool IsShowLeftLogo
        {
            get => (bool)GetValue(IsShowLeftLogoProperty);
            set => SetValue(IsShowLeftLogoProperty, value.BooleanBox());
        }
        /// <summary>
        /// 提供<see cref="IsShowLeftLogo"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsShowLeftLogoProperty =
            DependencyProperty.Register(nameof(IsShowLeftLogo), typeof(bool), typeof(TitleBarEx), new PropertyMetadata(ValueBoxes.TrueBox));


        /// <summary>
        /// 标题栏左边图标是否可用
        /// </summary>
        public bool IsLeftLogoEnable
        {
            get => (bool)GetValue(IsLeftLogoEnableProperty);
            set => SetValue(IsLeftLogoEnableProperty, value.BooleanBox());
        }
        /// <summary>
        /// 提供<see cref="IsLeftLogoEnable"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsLeftLogoEnableProperty =
            DependencyProperty.Register(nameof(IsLeftLogoEnable), typeof(bool), typeof(TitleBarEx), new PropertyMetadata(ValueBoxes.TrueBox));


        public ICommand LeftCommand
        {
            get => (ICommand)GetValue(LeftCommandProperty);
            set => SetValue(LeftCommandProperty, value);
        }
        /// <summary>
        /// 提供<see cref="LeftCommand"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty LeftCommandProperty =
            DependencyProperty.Register(nameof(LeftCommand), typeof(ICommand), typeof(TitleBarEx), new PropertyMetadata(default(ICommand)));


        /// <summary>
        /// 标题栏右边图标
        /// </summary>
        public Geometry RightLogo
        {
            get => (Geometry)GetValue(RightLogoProperty);
            set => SetValue(RightLogoProperty, value);
        }
        /// <summary>
        /// 提供<see cref="RightLogo"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty RightLogoProperty =
            DependencyProperty.Register(nameof(RightLogo), typeof(Geometry), typeof(TitleBarEx), new PropertyMetadata(Geometry.Empty));


        /// <summary>
        /// 是否显示标题栏右边图标
        /// </summary>
        public bool IsShowRightLogo
        {
            get => (bool)GetValue(IsShowRightLogoProperty);
            set => SetValue(IsShowRightLogoProperty, value.BooleanBox());
        }
        /// <summary>
        /// 提供<see cref="IsShowRightLogo"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsShowRightLogoProperty =
            DependencyProperty.Register(nameof(IsShowRightLogo), typeof(bool), typeof(TitleBarEx), new PropertyMetadata(ValueBoxes.TrueBox));


        /// <summary>
        /// 标题栏右边图标是否可用
        /// </summary>
        public bool IsRightLogoEnable
        {
            get => (bool)GetValue(IsRightLogoEnableProperty);
            set => SetValue(IsRightLogoEnableProperty, value.BooleanBox());
        }
        /// <summary>
        /// 提供<see cref="IsRightLogoEnable"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsRightLogoEnableProperty =
            DependencyProperty.Register(nameof(IsRightLogoEnable), typeof(bool), typeof(TitleBarEx), new PropertyMetadata(ValueBoxes.TrueBox));


        public ICommand RightCommand
        {
            get => (ICommand)GetValue(RightCommandProperty);
            set => SetValue(RightCommandProperty, value);
        }
        /// <summary>
        /// 提供<see cref="RightCommand"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty RightCommandProperty =
            DependencyProperty.Register(nameof(RightCommand), typeof(ICommand), typeof(TitleBarEx), new PropertyMetadata(default(ICommand)));
    }
}
