/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：NoneTitleBarWindow
 * Author： Chance_写代码的厨子
 * Create Time：2021-09-27 16:38:22
 */


using CookPopularToolkit.Windows;
using System.Windows;
using System.Windows.Input;

namespace CookPopularUI.WPF.Windows
{
    /// <summary>
    /// <see cref="NoneTitleBarWindow"/>标识无固定标题的窗体
    /// </summary>
    public class NoneTitleBarWindow : NormalWindow
    {
        static NoneTitleBarWindow()
        {
            StyleProperty.OverrideMetadata(typeof(NoneTitleBarWindow), new FrameworkPropertyMetadata(ResourceHelper.GetResource<Style>(typeof(NoneTitleBarWindow))));
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (e.GetPosition(this).Y <= ClientTitleBarHeight && e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}
