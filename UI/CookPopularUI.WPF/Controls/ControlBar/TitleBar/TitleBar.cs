/*
 *Description: TitleBar
 *Author: Chance.zheng
 *Creat Time: 2023/9/8 10:12:14
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
using System.Windows.Media;

namespace CookPopularUI.WPF.Controls
{
    /// <summary>
    /// 标题栏
    /// </summary>
    public class TitleBar : Control
    {
        public object Title
        {
            get => (object)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        /// <summary>
        /// 提供<see cref="Title"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(object), typeof(TitleBar), new PropertyMetadata(default(object)));


        public Geometry Logo
        {
            get => (Geometry)GetValue(LogoProperty);
            set => SetValue(LogoProperty, value);
        }
        /// <summary>
        /// 提供<see cref="Logo"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty LogoProperty =
            DependencyProperty.Register(nameof(Logo), typeof(Geometry), typeof(TitleBar), new PropertyMetadata(Geometry.Empty));
    }
}
