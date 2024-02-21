/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：NoneTitleBarWindow
 * Author： Chance_写代码的厨子
 * Create Time：2021-09-27 16:38:22
 */


using CookPopularToolkit.Windows;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shell;

namespace CookPopularUI.WPF.Windows
{
    /// <summary>
    /// <see cref="NoneTitleBarWindow"/>标识无固定标题的窗体
    /// </summary>
    public class NoneTitleBarWindow : NormalWindow
    {
        static NoneTitleBarWindow()
        {
            //StyleProperty.OverrideMetadata(typeof(NoneTitleBarWindow), new FrameworkPropertyMetadata(ResourceHelper.GetResource<Style>(typeof(NoneTitleBarWindow))));
        }

        public NoneTitleBarWindow()
        {
            var windowChrome = new WindowChrome
            {
                CornerRadius = default,
                GlassFrameThickness = new Thickness(-1),
                ResizeBorderThickness = new Thickness(2),
                UseAeroCaptionButtons = false,
                CaptionHeight = 0,
            };
            WindowChrome.SetWindowChrome(this, windowChrome);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (e.GetPosition(this).Y <= ClientTitleBarHeight && e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}
