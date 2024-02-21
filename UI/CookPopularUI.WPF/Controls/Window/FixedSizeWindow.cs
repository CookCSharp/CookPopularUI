/*
 *Description: FixedSizeWindow
 *Author: Chance.zheng
 *Creat Time: 2023/9/25 10:55:26
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularToolkit.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CookPopularUI.WPF.Windows
{
    /// <summary>
    /// 表示固定大小窗体
    /// </summary>
    public class FixedSizeWindow : Window
    {
        static FixedSizeWindow()
        {
            StyleProperty.OverrideMetadata(typeof(FixedSizeWindow), new FrameworkPropertyMetadata(ResourceHelper.GetResource<Style>(typeof(FixedSizeWindow))));
            CommandManager.RegisterClassCommandBinding(typeof(FixedSizeWindow), new CommandBinding(SystemCommands.CloseWindowCommand, (s, e) => SystemCommands.CloseWindow(s as Window), (s, e) => e.CanExecute = true));
        }
    }
}
