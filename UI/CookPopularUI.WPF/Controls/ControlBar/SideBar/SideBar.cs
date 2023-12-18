/*
 *Description: SideBar
 *Author: Chance.zheng
 *Creat Time: 2023/9/8 10:53:23
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularToolkit;
using CookPopularToolkit.Windows;
using Microsoft.Xaml.Behaviors.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xml.Linq;
using ValueBoxes = CookPopularToolkit.ValueBoxes;

namespace CookPopularUI.WPF.Controls
{
    /// <summary>
    /// 遮罩层侧边栏
    /// </summary>
    public class SideBar : ContentControl
    {
        private const string ContentCoverBorder = "ContentCoverBorder";
        private const string ContentPresenter = "PART_Content";
        private Border _borderElement;
        private FrameworkElement _contentElement;


        /// <summary>
        /// 停靠位置
        /// </summary>
        public Dock Dock
        {
            get => (Dock)GetValue(DockProperty);
            set => SetValue(DockProperty, value);
        }
        /// <summary>
        /// 表示<see cref="Dock"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty DockProperty =
            DependencyProperty.Register(nameof(Dock), typeof(Dock), typeof(SideBar), new PropertyMetadata(Dock.Left));


        /// <summary>
        /// 是否显示侧边栏
        /// </summary>
        public bool IsShow
        {
            get => (bool)GetValue(IsShowProperty);
            set => SetValue(IsShowProperty, value.BooleanBox());
        }
        /// <summary>
        /// 提供<see cref="IsShow"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsShowProperty =
            DependencyProperty.Register(nameof(IsShow), typeof(bool), typeof(SideBar), new PropertyMetadata(ValueBoxes.FalseBox));


        public override void OnApplyTemplate()
        {
            if (_contentElement != null)
                _contentElement.MouseLeftButtonUp -= (s, e) => e.Handled = true;
            if (_borderElement != null)
                _borderElement.MouseLeftButtonUp -= (s, e) => IsShow = false;

            base.OnApplyTemplate();

            _contentElement = (ContentPresenter)GetTemplateChild(ContentPresenter);
            _borderElement = (Border)GetTemplateChild(ContentCoverBorder);

            if (_contentElement != null)
                _contentElement.MouseLeftButtonUp += (s, e) => e.Handled = true;
            if (_borderElement != null)
                _borderElement.MouseLeftButtonUp += (s, e) => IsShow = false;
        }
    }
}
