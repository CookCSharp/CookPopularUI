/*
 *Description: SideBarEx
 *Author: Chance.zheng
 *Creat Time: 2023/10/17 17:35:03
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularToolkit;
using CookPopularToolkit.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CookPopularUI.WPF.Controls
{
    /// <summary>
    /// 无遮罩层侧边栏
    /// </summary>
    public class SideBarEx : SideBar
    {
        private FrameworkElement? _sideBarContentElement;
        private FrameworkElement? _contentElement;


        /// <summary>
        /// 侧边栏内容
        /// </summary>
        /// <remarks>
        /// <see cref="ContentControl.Content"/>是正文
        /// </remarks>
        public object SideContent
        {
            get => (object)GetValue(SideContentProperty);
            set => SetValue(SideContentProperty, value);
        }
        /// <summary>
        /// 提供<see cref="SideContent"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty SideContentProperty =
            DependencyProperty.Register(nameof(SideContent), typeof(object), typeof(SideBarEx), new PropertyMetadata(default));


        static SideBarEx()
        {
            IsShowProperty.AddOwner(typeof(SideBarEx), new PropertyMetadata(CookPopularToolkit.ValueBoxes.FalseBox, OnIsShowChanged));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _sideBarContentElement = this.GetTemplateChild("PART_SideContent") as FrameworkElement;
            _contentElement = this.GetTemplateChild("PART_Content") as FrameworkElement;
        }

        private static void OnIsShowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SideBarEx sideBarEx)
            {
                if (sideBarEx._sideBarContentElement != null)
                {
                    var scaleTransform = new ScaleTransform() { ScaleX = 0, ScaleY = 1 };
                    sideBarEx._sideBarContentElement.RenderTransform = scaleTransform;

                    DoubleAnimation _showAnimation = AnimationHelper.CreateDoubleAnimation(0, 1, 0.3);
                    DoubleAnimation _closeAnimation = AnimationHelper.CreateDoubleAnimation(1, 0, 0.3);
                    _closeAnimation.Completed += (s, e) => CheckHelper.ActionWhenFalse(sideBarEx.IsShow, () => sideBarEx._sideBarContentElement.Visibility = Visibility.Collapsed);

                    switch (sideBarEx.Dock)
                    {
                        case Dock.Left:
                            scaleTransform.ScaleX = 0;
                            scaleTransform.ScaleY = 1;
                            sideBarEx._sideBarContentElement.RenderTransformOrigin = default;
                            CheckHelper.ActionWhenBoolean(sideBarEx.IsShow,
                                () => scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, _showAnimation),
                                () => scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, _closeAnimation));
                            break;
                        case Dock.Right:
                            scaleTransform.ScaleX = 0;
                            scaleTransform.ScaleY = 1;
                            sideBarEx._sideBarContentElement.RenderTransformOrigin = new Point(1, 0);
                            CheckHelper.ActionWhenBoolean(sideBarEx.IsShow,
                                () => scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, _showAnimation),
                                () => scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, _closeAnimation));
                            break;
                        case Dock.Top:
                            scaleTransform.ScaleX = 1;
                            scaleTransform.ScaleY = 0;
                            sideBarEx._sideBarContentElement.RenderTransformOrigin = new Point(0, 0);
                            CheckHelper.ActionWhenBoolean(sideBarEx.IsShow,
                                () => scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, _showAnimation),
                                () => scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, _closeAnimation));
                            break;
                        case Dock.Bottom:
                            scaleTransform.ScaleX = 1;
                            scaleTransform.ScaleY = 0;
                            sideBarEx._sideBarContentElement.RenderTransformOrigin = new Point(0, 1);
                            CheckHelper.ActionWhenBoolean(sideBarEx.IsShow,
                                () => scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, _showAnimation),
                                () => scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, _closeAnimation));
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
